using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MySqlX.XDevAPI;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Models;
using WebEcommerce.Repository;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        //Injeção de dependência
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteRepository _clienteRepository;
        private readonly LoginCliente _loginCliente;

        public ClienteController(ILogger<ClienteController> logger, IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }

        public IActionResult Index()
        {
            return View();
        }

        //ActionResult para a tela de cadastro de cliente/endereço
        public IActionResult CadCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadCliente(Cliente cliente, Endereco endereco, Bairro bairro)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            cliente.IdUsu = _clienteRepository.Cadastrar(cliente, endereco, bairro);
            return RedirectToAction(nameof(LoginCliente));
        }

        //ActionResult para a tela de login de cliente
        public IActionResult LoginCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginCliente([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.RealizarLogin(cliente.Email, cliente.Senha);
            if (clienteDB != null && !string.IsNullOrEmpty(clienteDB.Email) && !string.IsNullOrEmpty(clienteDB.Senha))
            {
                _loginCliente.Login(clienteDB);
                return RedirectToAction(nameof(Index)); // Redireciona para a página inicial
            }
            else
            {
                ViewData["msgn_error"] = "Usuário não encontrado, verifique o email e senha, e tente novamente";
                return View();
            }
        }

        // Action para o logout do cliente
        public IActionResult Logout()
        {
            _loginCliente.Logout();  // Chama o método Logout para remover a sessão
            return RedirectToAction(nameof(Index));  // Redireciona para a página inicial
        }

        public IActionResult Cartoes()
        {
            int? idUsu = _loginCliente.GetCliente().IdUsu;
            return View(_clienteRepository.ListarCartoes(idUsu));
        }
        public IActionResult ExcluirCartao(decimal? codCartao)
        {
            int? idUsu = _loginCliente.GetCliente()?.IdUsu;

            if (!idUsu.HasValue)
            {
                return BadRequest("Usuário não encontrado.");
            }

            _clienteRepository.ExcluirCartao(idUsu, codCartao);

            return RedirectToAction(nameof(Cartoes));
        }

        public IActionResult AdicionarCartao()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdicionarCartao(Cartao cartao, string MesValidade, string AnoValidade)
        {
            if (!string.IsNullOrEmpty(MesValidade) && !string.IsNullOrEmpty(AnoValidade))
            {
                // Converte para um DateTime com o dia fixado como o último do mês
                cartao.DataValidade = new DateTime(
                    int.Parse(AnoValidade),
                    int.Parse(MesValidade),
                    DateTime.DaysInMonth(int.Parse(AnoValidade), int.Parse(MesValidade))
                );
            }

            _clienteRepository.AdicionarCartao(cartao);
            return RedirectToAction(nameof(Cartoes)); // Redireciona após o sucesso
        }



        public IActionResult AtualizarPerfil()
        {
            var cliente = _loginCliente.GetCliente(); // Obtém o cliente logado
            if (cliente == null)
            {
                return RedirectToAction(nameof(Index)); // Redireciona se não houver cliente logado
            }

            return View(cliente);
        }
        [HttpPost]
        public IActionResult AtualizarPerfil(Cliente cliente, Endereco endereco, Bairro bairro)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente); // Retorna a View com os erros de validação
            }

            var clienteLogado = _loginCliente.GetCliente();
            if (clienteLogado == null || clienteLogado.IdUsu != cliente.IdUsu)
            {
                return RedirectToAction(nameof(Index)); // Redireciona para login se algo der errado
            }

            _clienteRepository.AtualizarDados(cliente, endereco, bairro); // Atualiza os dados no banco
            _loginCliente.Login(cliente); // Atualiza os dados na sessão

            TempData["SuccessMessage"] = "Perfil atualizado com sucesso!";
            return RedirectToAction(nameof(Index)); // Redireciona para a página inicial
        }
    }
}

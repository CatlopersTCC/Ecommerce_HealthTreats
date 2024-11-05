using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        //Injeção de dependência
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;

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
            _clienteRepository.Cadastrar(cliente, endereco, bairro);
            return RedirectToAction(nameof(Index));
        }

        //ActionREsult para a tela de login de cliente
        public IActionResult LoginCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginCliente([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.RealizarLogin(cliente.Email, cliente.Senha);
            if (clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(Index)));
            }
            else
            {
                ViewData["msgn_error"] = "Usuário não encontrado, verifique o email e senha, e tente novamente";
                return View();
            }
        }
    }
}

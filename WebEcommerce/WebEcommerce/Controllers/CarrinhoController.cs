using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly LoginCliente _loginCliente;

        public CarrinhoController(ILogger<ClienteController> logger, IProdutoRepository produtoRepository, ICarrinhoRepository carrinhoRepository, LoginCliente loginCliente)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
            _carrinhoRepository = carrinhoRepository;
            _loginCliente = loginCliente;
        }


        public IActionResult Carrinho()
        {
            var carrinho = _carrinhoRepository.ListarProdutosCarrinho();
            return View(carrinho);
        }

        public IActionResult AddCarrinho(int cod)
        {
            // Chama o repositório para adicionar ao carrinho
            var produto = _produtoRepository.AddCarrinho(cod);

            // Redireciona o usuário para a tela do carrinho
            return RedirectToAction("Carrinho");
        }

        public IActionResult RemoverCarrinho(int cod)
        {
            var produto = _produtoRepository.RemoverCarrinho(cod);
            return RedirectToAction("Carrinho");
        }

        public IActionResult RegistrarCarrinho()
        {
            // Verifica o usuário logado
            var usuario = _loginCliente.GetCliente();

            if (usuario == null)
            {
                return RedirectToAction("LoginCliente", "Cliente");
            }

            // Obtém o carrinho atual
            var carrinho = _carrinhoRepository.ListarProdutosCarrinho();
            carrinho.IdUsu = usuario.IdUsu;

            if (carrinho.Produtos.Count == 0)
            {
                TempData["Erro"] = "Carrinho vazio! Adicione produtos antes de finalizar o pedido.";
                return RedirectToAction("Carrinho");
            }

            // Registra o carrinho no banco
            _carrinhoRepository.RegistrarCarrinho(carrinho);
            return RedirectToAction("Pagamento", "Pagamento");
        }
    }
}

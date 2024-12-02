using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Models.WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly LoginCliente _loginCliente;

        public PagamentoController(IPagamentoRepository pagamentoRepository, ICarrinhoRepository carrinhoRepository, LoginCliente loginCliente)
        {
            _pagamentoRepository = pagamentoRepository;
            _carrinhoRepository = carrinhoRepository;
            _loginCliente = loginCliente;
        }

        // Exibe a tela de pagamento
        public IActionResult Pagamento()
        {
            var usuario = _loginCliente.GetCliente();

            if (usuario == null)
            {
                return RedirectToAction("LoginCliente", "Cliente");
            }

            var carrinho = _carrinhoRepository.ListarProdutosCarrinho();

            if (carrinho.Produtos.Count == 0)
            {
                TempData["Erro"] = "Carrinho vazio! Adicione produtos antes de finalizar o pedido.";
                return RedirectToAction("Carrinho", "Carrinho");
            }

            return View(carrinho);
        }

        // Finaliza o pagamento
        [HttpPost]
        public IActionResult ConfirmarPagamento(string formaPag)
        {
            var usuario = _loginCliente.GetCliente();
            var carrinho = _carrinhoRepository.ObterUltimoCarrinho(usuario.IdUsu);  // Usando o método que pega o último carrinho


            // Registrar o pagamento no banco de dados
            var pagamento = new Pagamento
            {
                IdUsu = usuario.IdUsu,
                IdCarrinho = carrinho.IdCarrinho,
                FormaPag = formaPag,
                ValorTotal = carrinho.ValorTotal + carrinho.Frete, // Total com frete
                StatusPag = "Pendente" // Ou 'Aprovado' dependendo da lógica
            };

            _pagamentoRepository.RegistrarPagamento(pagamento);

            // Redirecionar para uma página de confirmação
            return RedirectToAction("PagamentoConfirmado");
        }

        public IActionResult PagamentoConfirmado()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

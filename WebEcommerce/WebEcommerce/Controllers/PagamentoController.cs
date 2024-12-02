using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Models.ViewModels;
using WebEcommerce.Models.WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly LoginCliente _loginCliente;

        public PagamentoController(IPagamentoRepository pagamentoRepository, ICarrinhoRepository carrinhoRepository, IClienteRepository clienteRepository, IProdutoRepository produtoRepository, LoginCliente loginCliente)
        {
            _pagamentoRepository = pagamentoRepository;
            _carrinhoRepository = carrinhoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
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

            // Criar a ViewModel com dados de pagamento, cliente e carrinho
            var pagamentoClienteCarrinhoViewModel = new PagamentoClienteCarrinhoViewModel
            {
                Cliente = usuario,  // Passando dados do cliente
                Carrinho = carrinho,  // Passando dados do carrinho
                Pagamento = new Pagamento
                {
                    IdUsu = usuario.IdUsu,
                    ValorTotal = carrinho.ValorTotal + carrinho.Frete,
                    FormaPag = string.Empty,  // Forma de pagamento será preenchida no POST
                    StatusPag = "Pendente"
                },
                Cartoes = _clienteRepository.ListarCartoes(usuario.IdUsu)
            };

            return View(pagamentoClienteCarrinhoViewModel);  // Passando a ViewModel para a View
        }

        // Finaliza o pagamento
        [HttpPost]
        public IActionResult ConfirmarPagamento(string formaPag)
        {
            var usuario = _loginCliente.GetCliente();
            var carrinho = _carrinhoRepository.ObterUltimoCarrinho(usuario.IdUsu);
            carrinho.IdUsu = usuario.IdUsu;

            var pagamento = new Pagamento
            {
                IdUsu = usuario.IdUsu,
                IdCarrinho = carrinho.IdCarrinho,
                FormaPag = formaPag,
                ValorTotal = carrinho.ValorTotal + carrinho.Frete,
                StatusPag = "Pendente"
            };

            _pagamentoRepository.RegistrarPagamento(pagamento);
            _produtoRepository.RemoverTodosCarrinho();

            return RedirectToAction("PagamentoConfirmado");
        }

        public IActionResult PagamentoConfirmado()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

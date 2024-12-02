using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;
using WebEcommerce.Models.ViewModels;

namespace WebEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository produtoRepository, IAvaliacaoRepository avaliacaoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
            _avaliacaoRepository = avaliacaoRepository;
        }

        public IActionResult Produtos(int? categoria)
        {
            IEnumerable<Produto> produtos;

            // Se não houver filtro, pega todos os produtos
            if (!categoria.HasValue)
            {
                produtos = _produtoRepository.ListarProdutos();
            }
            else
            {
                // Caso contrário, filtra os produtos pelas categorias selecionadas
                produtos = _produtoRepository.ListarProdutosPorCategoria(categoria);
            }

            return View(produtos);
        }
        public IActionResult Pesquisar(string termo)
        {
            IEnumerable<Produto> produtos;

            // Se o termo for vazio ou nulo, exibe todos os produtos
            if (string.IsNullOrEmpty(termo))
            {
                produtos = _produtoRepository.ListarProdutos(); // Sem categorias retorna todos
            }
            else
            {
                produtos = _produtoRepository.PesquisarProdutosPorNome(termo);
            }

            return View("Produtos", produtos); // Reutiliza a mesma View de Produtos
        }



        public IActionResult DetalhesProdutos(int cod)
        {
            Produto produto = _produtoRepository.ObterProduto(cod);
            IEnumerable<Avaliacao> avaliacoes = _avaliacaoRepository.ListarAvaliacoesPorProduto(cod);

            var viewModel = new ProdutoDetalhesViewModel
            {
                Produto = produto,
                Avaliacoes = avaliacoes
            };

            return View(viewModel);
        }
    }
}

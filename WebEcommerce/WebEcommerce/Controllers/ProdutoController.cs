using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(ILogger<ProdutoController> logger, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
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


        public IActionResult DetalhesProdutos(int cod)
        {
            var produto = _produtoRepository.ObterProduto(cod);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }
    }
}

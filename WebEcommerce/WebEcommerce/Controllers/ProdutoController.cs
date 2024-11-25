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

        public IActionResult Produtos()
        {
            return View(_produtoRepository.ListarProdutos());
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

        public IActionResult AddCarrinho() 
        {
            return View(); 
        }
    }
}

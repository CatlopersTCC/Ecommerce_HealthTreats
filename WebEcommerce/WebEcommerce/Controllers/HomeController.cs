using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoRepository _produtoRepository;

        public HomeController(ILogger<HomeController> logger, IProdutoRepository produtoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index()
        {
            var destaques = _produtoRepository.ListarProdutosDestaques() ?? new List<Produto>();
            return View(destaques);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class AdminController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        private LoginColaborador _loginColaborador;

        public AdminController(IColaboradorRepository colaboradorRepository, LoginColaborador loginColaborador)
        {
            _colaboradorRepository = colaboradorRepository;
            _loginColaborador = loginColaborador;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

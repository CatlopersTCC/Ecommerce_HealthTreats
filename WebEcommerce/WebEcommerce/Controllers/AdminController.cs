using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Claims;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class AdminController : Controller
    {
        private const string AdminUserName = "admin"; // constante para usuario administrador único
        private const string AdminPassword = "56798";

        [HttpGet]
        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(string username, string password) // significa que a operação será realizada de forma assincrona ao sistema
        {
            // Verifica credenciais fixas
            if (username == AdminUserName && password == AdminPassword)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("Role", "Administrator")
            };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminAuth"); // autentificação de identidade

                await HttpContext.SignInAsync("AdminAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home"); // Redireciona para a página inicial (mudar para redirecionar a login?)
            }

            ViewBag.ErrorMessage = "Credenciais inválidas.";
            return View();
        }

        [Authorize(AuthenticationSchemes = "AdminAuth")]
        public IActionResult AreaAdmin()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return RedirectToAction("LoginCliente");
        }
    }
}

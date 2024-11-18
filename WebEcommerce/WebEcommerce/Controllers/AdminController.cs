using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using WebEcommerce.Libraries.Login;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> LoginAdmin()
        { 
            var user = await _userManager.GetUserAsync(User);
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return View(); // Se for administrador, renderiza a view
            }

            return RedirectToAction("AccessDenied", "Home");

        }
       
    }
}

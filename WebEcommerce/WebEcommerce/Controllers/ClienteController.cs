using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Controllers
{
    public class ClienteController : Controller
    {
 
        private readonly  ILogger <ClienteController> _logger;
        private readonly IClienteRepository _clienteRepository;

        public ClienteController (ILogger<ClienteController> logger, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult CadCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadCliente(Cliente cliente)
        {
            _clienteRepository.Cadastrar(cliente);
            return View();
        }
    }
}

using Newtonsoft.Json;
using WebEcommerce.Models;

namespace WebEcommerce.Libraries.Login
{
    public class LoginCliente
    {
        private string Key = "Login.Cliente";
        private Section.Section _section;

        public LoginCliente(Section.Section section)
        {
            _section = section;
        }

        public void Login(Cliente cliente)
        {
            string clienteJSONString = JsonConvert.SerializeObject(cliente);
            _section.Cadastrar(Key, clienteJSONString);
        }

        public Cliente GetCliente()
        {
            if (_section.Existe(Key))
            {
                string clienteJSONString = _section.Consultar(Key);
                return JsonConvert.DeserializeObject<Cliente>(clienteJSONString);
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            _section.RemoverTodas();
        }
    }
}

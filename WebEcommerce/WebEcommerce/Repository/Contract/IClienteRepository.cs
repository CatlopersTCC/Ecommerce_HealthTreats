using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IClienteRepository
    {
        //CRUD

        void Cadastrar(Cliente cliente, Endereco endereco, Bairro bairro);

        Cliente RealizarLogin (string email, string senha);

        void AtualizarDados (Cliente cliente);

        void AtualizarPerfil (Cliente cliente);

        void ExcluirConta (int IdUsu);

    }
}

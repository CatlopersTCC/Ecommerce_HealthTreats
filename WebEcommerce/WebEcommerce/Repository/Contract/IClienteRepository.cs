using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IClienteRepository
    {
        //CRUD

        void Cadastrar(Cliente cliente, Endereco endereco, Bairro bairro);

        void RealizarLogin (int IdUsu);

        void AtualizarDados (Cliente cliente);

        void AtualizarPerfil (Cliente cliente);

        void ExcluirConta (int IdUsu);

    }
}

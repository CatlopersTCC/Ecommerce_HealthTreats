using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IClienteRepository
    {
        //CRUD

        int Cadastrar(Cliente cliente, Endereco endereco, Bairro bairro);

        Cliente RealizarLogin (string email, string senha);

        void AtualizarDados (Cliente cliente, Endereco endereco, Bairro bairro);

        void ExcluirConta(int IdUsu);

        //CRUD do cartão
        IEnumerable<Cartao> ListarCartoes(int? idUsu);
        void ExcluirCartao (int? idUsu, decimal? codCartao);
        void AdicionarCartao(Cartao cartao);

    }
}

using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IAvaliacaoRepository
    {
        IEnumerable<Avaliacao> ListarAvaliacoesPorProduto(int cod);
    }
}

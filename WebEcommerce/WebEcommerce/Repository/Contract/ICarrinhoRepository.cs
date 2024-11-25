using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface ICarrinhoRepository
    {
        IEnumerable<Produto> ListarProdutosCarrinho();
    }
}

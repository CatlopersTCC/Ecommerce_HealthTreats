using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface ICarrinhoRepository
    {
        public Carrinho ListarProdutosCarrinho();
    }
}

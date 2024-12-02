using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface ICarrinhoRepository
    {
        public Carrinho ListarProdutosCarrinho();
        public void RegistrarCarrinho(Carrinho carrinho);
        public Carrinho ObterUltimoCarrinho(int? idUsu);
    }
}

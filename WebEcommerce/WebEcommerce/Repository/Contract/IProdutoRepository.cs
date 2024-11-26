using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ListarProdutos();

        IEnumerable<Produto> ListarProdutosDestaques();

        Produto ObterProduto(int cod);

        Produto AddCarrinho(int Cod);

        Produto RemoverCarrinho(int Cod);
    }
}

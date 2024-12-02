using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ListarProdutos();
        IEnumerable<Produto> ListarProdutosPorCategoria(int? categoria);
        IEnumerable<Produto> ListarProdutosDestaques();
        IEnumerable<Produto> PesquisarProdutosPorNome(string nome);

        Produto ObterProduto(int cod);

        Produto AddCarrinho(int Cod);

        Produto RemoverCarrinho(int Cod);
    }
}

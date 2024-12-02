namespace WebEcommerce.Models.ViewModels
{
    public class ProdutoDetalhesViewModel
    {
        public Produto Produto { get; set; }
        public IEnumerable<Avaliacao> Avaliacoes { get; set; }
    }
}

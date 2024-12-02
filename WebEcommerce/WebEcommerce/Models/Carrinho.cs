namespace WebEcommerce.Models
{
    public class Carrinho
    {
        public int? IdCarrinho { get; set; }
        public int? IdUsu { get; set; }
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public decimal? ValorTotal { get; set; } = 0;
        public decimal? Frete { get; set; } = 35;
    }
}

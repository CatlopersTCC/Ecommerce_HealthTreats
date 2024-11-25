namespace WebEcommerce.Models
{
    public class Carrinho
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public decimal ValorTotal { get; set; }
    }
}

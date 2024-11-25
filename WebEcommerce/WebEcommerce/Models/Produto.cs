namespace WebEcommerce.Models
{
    public class Produto
    {

        public int? CodProduto { get; set; }
        public string? NomeProduto { get; set; }
        public DateTime? DataFabricacao { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal? Preco { get; set; }
        public int? QtdEstoque { get; set; }
        public string? DescCurta { get; set; }
        public string? DescDetalhada { get; set; }
        public decimal? Peso { get; set; }
        public string? Foto { get; set; }
        public bool? NoCarrinho { get; set; }
        public bool? Destaque { get; set; }
        public int? Quantidade { get; set; } = 1;
    }
}

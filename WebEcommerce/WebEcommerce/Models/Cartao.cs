namespace WebEcommerce.Models
{
    public class Cartao : Cliente
    {
        public decimal? CodCartao { get; set; }
        public int? IdUsu { get; set; }
        public string? NomeTitular { get; set; }
        public int? TipoCartao { get; set; }
        public decimal? CVV { get; set; }
        public DateTime? DataValidade { get; set; }

    }
}

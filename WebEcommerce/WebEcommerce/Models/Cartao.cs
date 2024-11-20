namespace WebEcommerce.Models
{
    public class Cartao: Cliente
    {
        public decimal CodCartao { get; set; }
        public required string NomeCartao { get; set; }

    }
}

namespace WebEcommerce.Models
{
    public class Endereco
    {
        public required string IdBairro { get; set; }
        public int CEP { get; set; }
        public required string Logradouro { get; set; }

       
    }
}

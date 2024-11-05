namespace WebEcommerce.Models
{
    public class Cliente
    {


        public decimal CEP { get; set; }

        public int NumResidencia { get; set; }

        public string? Complemento { get; set; }

        public decimal CPF { get; set; }

        public string? NomeUsu { get; set; }

        public string? NomeCompleto { get; set; }

        public string? Email { get; set; }

        public string? Senha { get; set; }

        public string? AvaliacaoMedica { get; set; }

        public string? Tel { get; set; }

        public bool NivelAcesso { get; set; }

        public DateTime DataNasc { get; set; }

        public string? Foto { get; set; }
        public string? NomeBairro { get; set; }
        public string? Logradouro { get; set; }
    }
}

namespace WebEcommerce.Models
{
    public class Cliente
    {
        private decimal cPF;

        public int IdUsu { get; set; }

        public decimal CEP { get; set; }

        public int NumResidencia { get; set; }

        public required string Complemento { get; set; }

        public decimal CPF { get => cPF; set => cPF = value; }

        public required string NomeUsu { get; set; }

        public required string NomeCompleto { get; set; }

        public required string Email { get; set; }

        public required string Senha { get; set; }

        public required string AvaliacaoMedica { get; set; }

        public required string Tel { get; set; }

        public bool NivelAcesso { get; set; }

        public DateTime DataNasc { get; set; }

        public string? Foto { get; set; }
    }
}

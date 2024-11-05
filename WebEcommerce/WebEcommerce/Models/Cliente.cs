using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Cliente
    {

        [Required (ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression (@"^\d{5}-\d{3}$", ErrorMessage = "O CEP deve conter 8 caracteres")]
        public decimal CEP { get; set; }

        [Required (ErrorMessage = "Digite um número de residência válido")]
        public required int NumResidencia { get; set; }

        public string? Complemento { get; set; }

        [Required (ErrorMessage = "CPF inválido")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite o CPF no formato padrão de 11 digítos")]
        public decimal CPF { get; set; }

        [Required (ErrorMessage = "Digite um nome de usuário válido")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "O nome de usuário deve ter entre 3 e 150 caracteres.")]

        public string NomeUsu { get; set; }

        [Required (ErrorMessage = "Digite seu nome completo (sem abreviações)")]
        [StringLength(200, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string? NomeCompleto { get; set; }


        [EmailAddress (ErrorMessage = "Digite um e-mail válido")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,20}$", 
            ErrorMessage = "A senha deve conter até 8 caracteres, entre letras maiúsculas, minúsculas, números e caracteres especiais")]
        public string Senha { get; set; }


        [Required]
        public string AvaliacaoMedica { get; set; }

        [Phone(ErrorMessage = "O número de telefone informado não é válido")]
        [RegularExpression(@"^\(\d{2}\)\s?\d{4,5}[-\s]?\d{4}$", ErrorMessage = "Formato de 11 digítos (com DDD)")]
        public string Tel { get; set; }

        [Required]
        public bool NivelAcesso { get; set; }

        [DataType(DataType.Date, ErrorMessage = "A data não é válida.")]
        [Range(18, 99, ErrorMessage = "A idade deve estar entre 18 e 99 anos.")]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])/(0[1-9]|1[0-2])/\d{4}$")]
        public DateTime DataNasc { get; set; }

        public string? Foto { get; set; }

        [Required]
        public string? NomeBairro { get; set; }

        [Required]
        public string? Logradouro { get; set; }
    }
}

using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Cliente
    {
        public int? IdUsu { get; set; }

        [Required (ErrorMessage = "Por favor, informe o CEP.")]
        public decimal? CEP { get; set; }

        [Required (ErrorMessage = "Digite um número de residência válido.")]
        public int? NumResidencia { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "O complemento deve ter entre 5 e 100 caracteres.")]
        public string? Complemento { get; set; }

        [Required (ErrorMessage = "Por favor, informe o CPF.")]
        public decimal? CPF { get; set; }

        [Required (ErrorMessage = "Por favor, informe seu nome de usuário.")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "O nome de usuário deve ter entre 8 e 150 caracteres.")]
        public string? NomeUsu { get; set; }

        [Required (ErrorMessage = "Por favor, informe seu nome completo, sem abreviações.")]
        [StringLength(200, ErrorMessage = "O nome ultrapassa 200 caracteres.")]
        public string? NomeCompleto { get; set; }

        [Required(ErrorMessage = "Por favor, informe um E-mail válido.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Por favor, informe uma senha.")]
        public string? Senha { get; set; }

        [Required(ErrorMessage = "Por favor, informe sua avaliação médica.")]
        public string? AvaliacaoMedica { get; set; }

        [Required(ErrorMessage = "Por favor, insira um número de telefone válido.")]
        [MinLength(11, ErrorMessage = "O número de telefone deve conter 11 caracteres.")]
        [Phone(ErrorMessage = "O número de telefone informado não é válido.")]
        public string? Tel { get; set; }

        [Required]
        public bool NivelAcesso { get; set; }

        [Required(ErrorMessage = "Por favor, insira uma data de nascimento válida.")]
        [DataType(DataType.Date)]
        public DateTime? DataNasc { get; set; }

        public string? Foto { get; set; }

        [Required(ErrorMessage = "Por favor, insira um bairro válido.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O bairro deve ter entre 5 e 200 caracteres.")]
        public string? NomeBairro { get; set; }

        [Required(ErrorMessage = "Por favor, insira um logradouro válido.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O logradouro deve ter entre 5 e 200 caracteres.")]
        public string? Logradouro { get; set; }
    }
}

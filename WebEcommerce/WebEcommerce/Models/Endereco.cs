using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "Por favor, informe o CEP.")]
        public string? CEP { get; set; }

        [Required(ErrorMessage = "Por favor, insira um logradouro válido.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O logradouro deve ter entre 5 e 200 caracteres.")]
        public string? Logradouro { get; set; }
    }
}

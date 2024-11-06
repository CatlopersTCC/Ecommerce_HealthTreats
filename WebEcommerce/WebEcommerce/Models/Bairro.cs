using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Bairro
    {
        [Required(ErrorMessage = "Por favor, insira um bairro válido.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O bairro deve ter entre 5 e 200 caracteres.")]
        public required string NomeBairro { get; set; }
    }
}

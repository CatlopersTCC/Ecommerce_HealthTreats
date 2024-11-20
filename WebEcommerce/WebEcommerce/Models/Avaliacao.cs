using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Avaliacao
    {
        [Required (ErrorMessage = "Insira a quantidade de estrelas!")]
        public int QtdEstrelas { get; set; }

        [StringLength(1200, MinimumLength = 50, ErrorMessage = "Seu comentario deve conter no mínimo 50 caracteres")]
        public required string Comentario { get; set; }
        public required string Imagem { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Avaliacao
    {
        public int? IdAvaliacao { get; set; }
        public int? IdUsu { get; set; }
        public string? NomeUsuario { get; set; }
        public string? NomeCompleto { get; set; }
        public int? CodProduto { get; set; }
        [Required (ErrorMessage = "Insira a quantidade de estrelas!")]
        public int? QtdEstrelas { get; set; }

        [StringLength(1200, MinimumLength = 50, ErrorMessage = "Seu comentario deve conter no mínimo 50 caracteres")]
        public string? Comentario { get; set; }
    }
}

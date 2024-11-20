using System.ComponentModel.DataAnnotations;

namespace WebEcommerce.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public required string NomeCli {  get; set; }

        public double? ValorTotal { get; set; }
        public required string DadosEntrega { get; set; }
        public DateTime DataeHoraCompra {  get; set; }
        public DateOnly EstimativaEntrega { get; set; }
        public double Frete { get; set; }

    }
}

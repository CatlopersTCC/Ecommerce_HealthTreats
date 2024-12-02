namespace WebEcommerce.Models
{
    namespace WebEcommerce.Models
    {
        public class Pagamento
        {
            public int? IdPagamento { get; set; }
            public int? IdUsu { get; set; }
            public int? IdCarrinho { get; set; }
            public string FormaPag { get; set; }
            public decimal? ValorTotal { get; set; }
            public DateTime DataHoraPag { get; set; }
            public string StatusPag { get; set; }
        }
    }

}

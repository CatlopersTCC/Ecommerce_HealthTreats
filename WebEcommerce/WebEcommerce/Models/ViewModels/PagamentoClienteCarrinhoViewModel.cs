using WebEcommerce.Models.WebEcommerce.Models;

namespace WebEcommerce.Models.ViewModels
{
    public class PagamentoClienteCarrinhoViewModel
    {
        public Pagamento Pagamento { get; set; }
        public Cliente Cliente { get; set; }
        public Carrinho Carrinho { get; set; }
        public IEnumerable<Cartao> Cartoes { get; set; } 
    }
}

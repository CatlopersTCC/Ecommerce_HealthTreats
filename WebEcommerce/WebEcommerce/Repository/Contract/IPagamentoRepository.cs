using WebEcommerce.Models.WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IPagamentoRepository
    {
        public void RegistrarPagamento(Pagamento pagamento);
    }
}

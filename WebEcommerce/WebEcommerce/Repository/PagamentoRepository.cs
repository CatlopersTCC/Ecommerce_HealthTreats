using MySql.Data.MySqlClient;
using WebEcommerce.Models.WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Repository
{
    public class PagamentoRepository(IConfiguration configuration) : IPagamentoRepository
    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        public readonly string conexaoMySQL = configuration.GetConnectionString("ConexaoMySql");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public string ConexaoMySql => conexaoMySQL;

        public void RegistrarPagamento(Pagamento pagamento)
        {
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                var query = "INSERT INTO tblPagamento (idUsu, idCarrinho, formaPag, valorTotal, statusPag) VALUES (@idUsu, @idCarrinho, @formaPag, @valorTotal, @statusPag)";
                var cmd = new MySqlCommand(query, conexao);

                cmd.Parameters.AddWithValue("@idUsu", pagamento.IdUsu);
                cmd.Parameters.AddWithValue("@idCarrinho", pagamento.IdCarrinho);
                cmd.Parameters.AddWithValue("@formaPag", pagamento.FormaPag);
                cmd.Parameters.AddWithValue("@valorTotal", pagamento.ValorTotal);
                cmd.Parameters.AddWithValue("@statusPag", pagamento.StatusPag);

                cmd.ExecuteNonQuery();
            }
        }
    }
}

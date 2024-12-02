using MySql.Data.MySqlClient;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Repository
{
    public class AvaliacaoRepository(IConfiguration configuration) : IAvaliacaoRepository
    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        public readonly string conexaoMySQL = configuration.GetConnectionString("ConexaoMySql");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public string ConexaoMySql => conexaoMySQL;

        public IEnumerable<Avaliacao> ListarAvaliacoesPorProduto(int cod)
        {
            List<Avaliacao> avaliacoes = new List<Avaliacao>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblAvaliacao a inner join tblCliente u on a.idUsu = u.idUsu where a.codProduto = @codProduto", conexao);
                cmd.Parameters.AddWithValue("@codProduto", cod);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        avaliacoes.Add(new Avaliacao
                        {
                            IdAvaliacao = Convert.ToInt32(dr["idAvaliacao"]),
                            IdUsu = Convert.ToInt32(dr["idUsu"]),
                            NomeUsuario = dr["nomeUsu"].ToString(),
                            NomeCompleto = dr["nomeCompleto"].ToString(),
                            CodProduto = Convert.ToInt32(dr["codProduto"]),
                            QtdEstrelas = Convert.ToInt32(dr["qtdEstrela"]),
                            Comentario = dr["comentario"].ToString()!
                        });
                    }
                }
                conexao.Close();
            }
            return avaliacoes;
        }
    }
}

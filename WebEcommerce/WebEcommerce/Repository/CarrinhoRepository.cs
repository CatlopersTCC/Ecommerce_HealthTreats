using WebEcommerce.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using WebEcommerce.Models;

namespace WebEcommerce.Repository
{
    public class CarrinhoRepository(IConfiguration configuration) : ICarrinhoRepository
    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        public readonly string conexaoMySQL = configuration.GetConnectionString("ConexaoMySql");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public string ConexaoMySql => conexaoMySQL;

        public IEnumerable<Produto> ListarProdutosCarrinho()
        {
            List<Produto> ProdList = new List<Produto>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select codProduto, nomeProd, precoUnitario, fotoProd from tblProduto where no_carrinho = true", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ProdList.Add(
                        new Produto
                        {
                            CodProduto = Convert.ToInt32(dr["codProduto"]),
                            NomeProduto = (string)(dr["nomeProd"]),
                            Preco = Convert.ToDecimal(dr["precoUnitario"]),
                            Foto = (string)(dr["fotoProd"])
                        }
                    );
                }
            }
            return ProdList;
        }
    }
}

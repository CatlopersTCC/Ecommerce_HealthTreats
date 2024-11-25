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

        public Carrinho ListarProdutosCarrinho()
        {
            var carrinho = new Carrinho();

            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT codProduto, nomeProd, precoUnitario, fotoProd FROM tblProduto WHERE no_carrinho = true", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    var preco = Convert.ToDecimal(dr["precoUnitario"]);
                    var produto = new Produto
                    {
                        CodProduto = Convert.ToInt32(dr["codProduto"]),
                        NomeProduto = dr["nomeProd"].ToString(),
                        Preco = preco,
                        Foto = dr["fotoProd"].ToString()
                    };
                    carrinho.Produtos.Add(produto);
                    carrinho.ValorTotal += preco; // Soma o preço total aqui
                }
            }

            return carrinho;
        }
    }
}

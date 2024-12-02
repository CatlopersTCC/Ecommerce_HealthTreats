using WebEcommerce.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using WebEcommerce.Models;
using WebEcommerce.Libraries.Login;
using Org.BouncyCastle.Asn1.Cmp;

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

                Produto produto = null;

                foreach (DataRow dr in dt.Rows)
                {
                    var preco = Convert.ToDecimal(dr["precoUnitario"]);
                    produto = new Produto
                    {
                        CodProduto = Convert.ToInt32(dr["codProduto"]),
                        NomeProduto = dr["nomeProd"].ToString(),
                        Preco = preco,
                        Foto = dr["fotoProd"].ToString()
                    };
                    carrinho.Produtos.Add(produto);
                    carrinho.ValorTotal = carrinho.ValorTotal + preco;
                }

                if (produto != null)
                {
                    carrinho.ValorTotal = carrinho.ValorTotal + carrinho.Frete;
                }
            }

            return carrinho;
        }

        public void RegistrarCarrinho(Carrinho carrinho)
        {
            using (var conexao = new MySqlConnection(ConexaoMySql))
            {
                conexao.Open();
                var query = "INSERT INTO tblCarrinhoCompras (idUsu, valorTotal, frete) VALUES (@idUsu, @valorTotal, @frete)";
                var cmd = new MySqlCommand(query, conexao);

                cmd.Parameters.AddWithValue("@idUsu", carrinho.IdUsu);
                cmd.Parameters.AddWithValue("@valorTotal", carrinho.ValorTotal);
                cmd.Parameters.AddWithValue("@frete", carrinho.Frete);

                cmd.ExecuteNonQuery();

                // Recupera o Id gerado para o carrinho
                carrinho.IdCarrinho = (int)cmd.LastInsertedId;

                // Verificar se o carrinho foi realmente inserido
                Console.WriteLine($"Carrinho inserido com idCarrinho: {carrinho.IdCarrinho}");
            }
        }

        public Carrinho ObterUltimoCarrinho(int? idUsu)
        {
            Carrinho carrinho = null;

            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();

                // Ordenando por idCarrinho de forma decrescente e pegando o primeiro (último inserido)
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblCarrinhoCompras WHERE idUsu = @idUsu ORDER BY idCarrinho DESC LIMIT 1", conexao);
                cmd.Parameters.AddWithValue("@idUsu", idUsu);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        carrinho = new Carrinho
                        {
                            IdCarrinho = Convert.ToInt32(dr["idCarrinho"]),
                            IdUsu = Convert.ToInt32(dr["idUsu"]),
                            ValorTotal = Convert.ToDecimal(dr["valorTotal"]),
                            Frete = Convert.ToDecimal(dr["frete"]),
                        };
                    }
                }
            }

            return carrinho; // Se não encontrado, carrinho será null.
        }



    }
}

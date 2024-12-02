using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Repository
{
    public class ProdutoRepository(IConfiguration configuration) : IProdutoRepository
    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        public readonly string conexaoMySQL = configuration.GetConnectionString("ConexaoMySql");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public string ConexaoMySql => conexaoMySQL;

        public IEnumerable<Produto> ListarProdutos()
        {
            List<Produto> ProdList = new List<Produto>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {                              
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select codProduto, nomeProd, precoUnitario, fotoProd, destaques from tblProduto", conexao);

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
                            Foto = (string)(dr["fotoProd"]),
                            Destaque = Convert.ToBoolean(dr["destaques"])
                        }
                    );
                }
            }
            return ProdList;
        }

        public IEnumerable<Produto> ListarProdutosPorCategoria(int? categoria)
        {
            List<Produto> ProdList = new List<Produto>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblProduto where codCategoria = @categoria", conexao);
                cmd.Parameters.AddWithValue("@categoria", categoria);

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
                            CodCategoria = Convert.ToInt32(dr["codCategoria"]),
                            NomeProduto = (string)(dr["nomeProd"]),
                            Preco = Convert.ToDecimal(dr["precoUnitario"]),
                            Foto = (string)(dr["fotoProd"]),
                            Destaque = Convert.ToBoolean(dr["destaques"])
                        }
                    );
                }
            }
            return ProdList;
        }

        public IEnumerable<Produto> ListarProdutosDestaques()
        {
            List<Produto> ProdList = new List<Produto>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select codProduto, nomeProd, precoUnitario, fotoProd, destaques from tblProduto where destaques = true", conexao);

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
                            Foto = (string)(dr["fotoProd"]),
                            Destaque = Convert.ToBoolean(dr["destaques"])
                        }
                    );
                }
            }
            return ProdList;
        }

        public IEnumerable<Produto> PesquisarProdutosPorNome(string nome)
        {
            List<Produto> ProdList = new List<Produto>();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblProduto where nomeProd LIKE @nome", conexao);
                cmd.Parameters.AddWithValue("@nome", $"%{nome}%");

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
                            CodCategoria = Convert.ToInt32(dr["codCategoria"]),
                            NomeProduto = (string)(dr["nomeProd"]),
                            Preco = Convert.ToDecimal(dr["precoUnitario"]),
                            Foto = (string)(dr["fotoProd"]),
                            Destaque = Convert.ToBoolean(dr["destaques"])
                        }
                    );
                }
            }
            return ProdList;
        }


        public Produto ObterProduto(int cod)
        {
            Produto produto = new Produto();
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tblProduto where codProduto = @cod", conexao);
                cmd.Parameters.AddWithValue("@cod", cod);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        produto = new Produto
                        {
                            CodProduto = Convert.ToInt32(dr["codProduto"]),
                            CodCategoria = Convert.ToInt32(dr["codCategoria"]),
                            NomeProduto = (string)(dr["nomeProd"]),
                            DataFabricacao = Convert.ToDateTime(dr["dataFab"]),
                            DataValidade = Convert.ToDateTime(dr["dataValidade"]),
                            Preco = Convert.ToDecimal(dr["precoUnitario"]),
                            QtdEstoque = Convert.ToInt32(dr["qtdEstoque"]),
                            DescCurta = (string)(dr["descCurta"]),
                            DescDetalhada = (string)(dr["descDetalhada"]),
                            Peso = Convert.ToDecimal(dr["peso"]),
                            Foto = (string)(dr["fotoProd"]),
                            NoCarrinho = Convert.ToBoolean(dr["no_carrinho"]),
                            Destaque = Convert.ToBoolean(dr["destaques"])
                        };
                    }
                }
                conexao.Close();
            }
            return produto;
        }

        public Produto AddCarrinho(int cod)
        {
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();

                // Comando para atualizar o banco
                MySqlCommand cmd = new MySqlCommand("update tblProduto set no_carrinho = true where codProduto = @cod", conexao);
                cmd.Parameters.AddWithValue("@cod", cod);

                // Executar o comando de atualização
                cmd.ExecuteNonQuery();

                // Retorna o objeto Produto atualizado
                return new Produto
                {
                    NoCarrinho = true // Já sabemos que foi alterado para true
                };
            }
        }

        public Produto RemoverCarrinho(int cod)
        {
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();

                // Comando para atualizar o banco
                MySqlCommand cmd = new MySqlCommand("update tblProduto set no_carrinho = false where codProduto = @cod", conexao);
                cmd.Parameters.AddWithValue("@cod", cod);

                // Executar o comando de atualização
                cmd.ExecuteNonQuery();

                // Retorna o objeto Produto atualizado
                return new Produto
                {
                    NoCarrinho = false
                };
            }
        }

        public Produto RemoverTodosCarrinho()
        {
            using (var conexao = new MySqlConnection(conexaoMySQL))
            {
                conexao.Open();

                // Comando para atualizar o banco
                MySqlCommand cmd = new MySqlCommand("update tblProduto set no_carrinho = false", conexao);

                // Executar o comando de atualização
                cmd.ExecuteNonQuery();

                // Retorna o objeto Produto atualizado
                return new Produto
                {
                    NoCarrinho = false
                };
            }
        }

    }
}

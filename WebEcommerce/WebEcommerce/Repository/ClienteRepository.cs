using MySql.Data.MySqlClient;
using System.Data;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Repository
{
    public class ClienteRepository(IConfiguration configuration) : IClienteRepository

    {
        #pragma warning disable CS8601 // Possible null reference assignment.
        public readonly string conexaoMySQL = configuration.GetConnectionString("ConexaoMySql");
        #pragma warning restore CS8601 // Possible null reference assignment.
        public string ConexaoMySql => conexaoMySQL;

        public void AtualizarDados(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void AtualizarPerfil(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        //Método para o login de cliente
        public Cliente RealizarLogin(string email, string senha)
        {
            using (var conexao = new MySqlConnection(ConexaoMySql))
            {
                conexao.Open(); // Abrindo a conexão com o banco de dados

                // Comando SQL para buscar o cliente com todos os dados
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblCliente WHERE email = @email AND senha = @senha", conexao);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = senha;

                MySqlDataReader dr = cmd.ExecuteReader(); // Executa o comando e lê os dados

                Cliente cliente = new Cliente(); // Inicializa o objeto como nulo

                // Se o leitor encontrar dados
                if (dr.Read())
                {
                    cliente = new Cliente
                    {
                        IdUsu = Convert.ToInt32(dr["idUsu"]),
                        Email = Convert.ToString(dr["email"]),
                        Senha = Convert.ToString(dr["senha"]),
                        NomeUsu = Convert.ToString(dr["nomeUsu"]),
                    };
                }

                return cliente; // Retorna o cliente completo ou nulo se não encontrado
            }
        }

        //Método para cadastro de cliente e de endereço
        public int Cadastrar(Cliente cliente, Endereco endereco, Bairro bairro)
        {
            using (var conexao = new MySqlConnection(ConexaoMySql))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call ipCliente(@email, @senha, @cpf, @cep, @logradouro, @bairro, @numResidencia, @complemento, " +
                    "@nomeUsu, @nomeCompleto, @avaliacaoMedica, @dataNasc, @tel, @foto)", conexao);

                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = cliente.Senha;
                cmd.Parameters.Add("@cpf", MySqlDbType.Decimal).Value = cliente.CPF;
                cmd.Parameters.Add("@cep", MySqlDbType.Decimal).Value = endereco.CEP;
                cmd.Parameters.Add("@logradouro", MySqlDbType.VarChar).Value = endereco.Logradouro;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = bairro.NomeBairro;
                cmd.Parameters.Add("@numResidencia", MySqlDbType.Int64).Value = cliente.NumResidencia;
                cmd.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = cliente.Complemento;
                cmd.Parameters.Add("@nomeUsu", MySqlDbType.VarChar).Value = cliente.NomeUsu;
                cmd.Parameters.Add("@nomeCompleto", MySqlDbType.VarChar).Value = cliente.NomeCompleto;
                cmd.Parameters.Add("@avaliacaoMedica", MySqlDbType.VarChar).Value = cliente.AvaliacaoMedica;
                cmd.Parameters.Add("@dataNasc", MySqlDbType.Date).Value = cliente.DataNasc;
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = cliente.Tel;
                cmd.Parameters.Add("@foto", MySqlDbType.VarChar).Value = cliente.Foto;

                cmd.ExecuteNonQuery();

                MySqlCommand cmd_id = new MySqlCommand("select LAST_INSERT_ID()", conexao);
                int id = Convert.ToInt32(cmd_id.ExecuteScalar());
                
                conexao.Close();

                return id;
            }
        }

        public void ExcluirConta(int IdUsu)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cartao> ListarCartoes(int? idUsu)
        {
            List<Cartao> Cartoes = new List<Cartao>();
            using (var conexao = new MySqlConnection(ConexaoMySql))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tblCartao where idUsu = @idUsu", conexao);
                cmd.Parameters.Add("@idUsu", MySqlDbType.Int32).Value = idUsu;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Cartoes.Add(
                        new Cartao
                        {
                            CodCartao = Convert.ToInt64(dr["codCartao"]),
                            IdUsu = idUsu,
                            NomeTitular = (string)(dr["nomeTitular"]),
                            TipoCartao = Convert.ToInt32(dr["tipoCartao"]),
                            CVV = Convert.ToDecimal(dr["CVV"]),
                            DataValidade = Convert.ToDateTime(dr["dataValidade"]),
                        }
                    );
                }
            }
            return Cartoes;
        }
    }
}

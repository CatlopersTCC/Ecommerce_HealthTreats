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
                conexao.Open(); //Abrindo a conexão com o banco de dados

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tblCliente where email = @email and senha = @senha", conexao); /*Executando o comando
                       para pegar o email e senha da tabela tblCliente*/ 

                
                //Atribuo os valores de email e senha pegos no banco de dados nas parâmetros do método
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd); //Lê os dados pegos do banco de dados
                MySqlDataReader dr; //Guarda os dados pegos do banco de dados

                Cliente cliente = new Cliente(); //Instancia um objeto da classe Cliente
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); //Guarda os dados pegos do comando executado anteriormente, e fecha a conexão e seguida

                /*Enquanto o DataReader estiver guardando os dados pegos do comando anteriormente executado, os atributos da classe Cliente receberão os valores
                 pegos e convertidos do banco*/
                while (dr.Read())
                {
                    cliente.Email = Convert.ToString(dr["email"]);
                    cliente.Senha = Convert.ToString(dr["senha"]);
                }
                return cliente;
            }
        }

        //Método para cadastro de cliente e de endereço
        public void Cadastrar(Cliente cliente, Endereco endereco, Bairro bairro)
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
                conexao.Close(); 
            }
        }

        public void ExcluirConta(int IdUsu)
        {
            throw new NotImplementedException();
        }
    }
}

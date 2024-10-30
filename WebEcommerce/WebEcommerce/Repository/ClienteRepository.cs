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

        public void RealizarLogin(int IdUsu)
        {
            throw new NotImplementedException();
        }
    }
}

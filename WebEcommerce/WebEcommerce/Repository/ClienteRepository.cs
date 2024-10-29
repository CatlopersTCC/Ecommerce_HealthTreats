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

                MySqlCommand cmdBairro = new MySqlCommand("call ipBairro (@bairro)", conexao);
                cmdBairro.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = bairro.NomeBairro;
                cmdBairro.ExecuteNonQueryAsync();

                MySqlCommand cmdEndereco = new MySqlCommand("call ipEndereco(@cep, @logradouro, @bairro)", conexao);

                cmdEndereco.Parameters.Add("@cep", MySqlDbType.Int64).Value = endereco.CEP;
                cmdEndereco.Parameters.Add("@logradouro", MySqlDbType.VarChar).Value = endereco.Logradouro;
                cmdEndereco.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = endereco.IdBairro;
                cmdEndereco.ExecuteNonQueryAsync();
                

                MySqlCommand cmd = new MySqlCommand("insert into tblCliente (idUsu, cpf, cep, numResidencia, complemento, email, NomeUsu, NomeCompleto, avaliacaoMedica, dataNasc, tel, foto, senha, nivelAcesso) " +
                    "values (default, @cpf, @cep, @numResidencia, @complemento, @email, @NomeUsu, @NomeCompleto, @avaliacaoMedica, @dataNasc, @tel, @foto, @senha, false)", conexao);

                cmd.Parameters.Add("@cpf", MySqlDbType.Int64).Value = cliente.CPF;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = endereco.CEP;
                cmd.Parameters.Add("@numResidencia", MySqlDbType.Int64).Value = cliente.NumResidencia;
                cmd.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = cliente.Complemento;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@NomeUsu", MySqlDbType.VarChar).Value = cliente.NomeUsu;
                cmd.Parameters.Add("@NomeCompleto", MySqlDbType.VarChar).Value = cliente.NomeCompleto;
                cmd.Parameters.Add("@avaliacaoMedica", MySqlDbType.VarChar).Value = cliente.AvaliacaoMedica;
                cmd.Parameters.Add("@dataNasc", MySqlDbType.Date).Value = cliente.DataNasc;
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = cliente.Tel;
                cmd.Parameters.Add("@foto", MySqlDbType.VarChar).Value = cliente.Foto;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = cliente.Senha;

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

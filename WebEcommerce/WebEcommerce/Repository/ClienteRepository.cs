using MySql.Data.MySqlClient;
using System.Data;
using WebEcommerce.Models;
using WebEcommerce.Repository.Contract;

namespace WebEcommerce.Repository
{
    public class ClienteRepository : IClienteRepository

    {
        private readonly string _conexaoMySQL;

        public ClienteRepository(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void AtualizarDados(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void AtualizarPerfil(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("insert into tblCliente (cpf, email, NomeUsu, NomeCompleto, avaliacaoMedica, dataNasc, tel, foto, senha) " +
                    "values (@cpf, @email, @NomeUsu, @NomeCompleto, @avaliacaoMedica, @dataNasc, @tel, @foto, @senha)", conexao);

                cmd.Parameters.Add("@cpf", MySqlDbType.Int16).Value = cliente.CPF;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@NomeUsu", MySqlDbType.VarChar).Value = cliente.NomeUsu;
                cmd.Parameters.Add("@NomeCompleto", MySqlDbType.VarChar).Value = cliente.NomeCompleto;
                cmd.Parameters.Add("@avaliacaoMedica", MySqlDbType.VarChar).Value = cliente.AvaliacaoMedica;
                cmd.Parameters.Add("@dataNasc", MySqlDbType.Date).Value = cliente.DataNasc;
                cmd.Parameters.Add("@@tel", MySqlDbType.VarChar).Value = cliente.Tel;
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

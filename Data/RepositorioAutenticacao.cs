using evecorpfy.Models;
using evecorpfy.Security;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data
{
    public class RepositorioAutenticacao
    {
        public Usuario Autenticar(string username, string senha)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT ID, USERNAME, SENHA_HASH, EMAIL, ROLE, ATIVO, DATA_CRIACAO
                               FROM USUARIOS 
                               WHERE USERNAME = @u AND ATIVO = 1";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@u", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var usuario = new Usuario
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                SenhaHash = reader.GetString(2),
                                Email = reader.GetString(3),
                                Role = reader.GetString(4),
                                Ativo = reader.GetBoolean(5),
                                DataCriacao = reader.GetDateTime(6)
                            };
                            if (PasswordHasher.Verify(senha, usuario.SenhaHash))
                                return usuario;
                        }
                    }
                }
            }
            return null;
        }
    }
}
using evecorpfy.Models;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data;
public class RepositorioAutenticacao
{
    public Usuario Autenticar(string username, string senha)
    {
        using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
        {
            string sql = @"SELECT ID, USERNAME, SENHA_HASH, EMAIL, ROLE, ATIVO, DATA_CRIACAO
                               FROM USUARIOS 
                               WHERE USERNAME = @u AND SENHA_HASH = @s AND ATIVO = 1";
            using (var command = new SqlCommand(sql, conectaDataBase))
            {
                command.Parameters.AddWithValue("@u", username);
                command.Parameters.AddWithValue("@s", senha);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            SenhaHash = reader.GetString(2),
                            Email = reader.GetString(3),
                            Role = reader.GetString(4),
                            Ativo = reader.GetBoolean(5),
                            DataCriacao = reader.GetDateTime(6)
                        };
                    }
                }
            }
        }
        return null;
    }
}
using evecorpfy.Models;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data;
public class RepositorioUsuario
{
    public Usuario ObterUsuarioPorId(int id)
    {
        using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
        {
            string sql = "SELECT ID, USERNAME, SENHA_HASH, EMAIL, ROLE, ATIVO, DATA_CRIACAO, FOTO_PERFIL " +
                         "FROM USUARIOS WHERE ID = @ID";
            using (var command = new SqlCommand(sql, conectaDataBase))
            {
                command.Parameters.AddWithValue("@ID", id);
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
                            DataCriacao = reader.GetDateTime(6),
                            FotoPerfil = reader.IsDBNull(7) ? null : (byte[])reader["FOTO_PERFIL"]
                        };
                    }
                }
            }
        }
        return null;
    }
    public Usuario Autenticar(string username, string senha)
    {
        using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
        {
            string sql = @"SELECT ID, USERNAME, SENHA_HASH, EMAIL, ROLE, ATIVO, DATA_CRIACAO
                               FROM USUARIOS 
                               WHERE USERNAME = @USERNAME AND SENHA_HASH = @SENHA AND ATIVO = 1";
            using (var command = new SqlCommand(sql, conectaDataBase))
            {
                command.Parameters.AddWithValue("@USERNAME", username);
                command.Parameters.AddWithValue("@SENHA", senha);
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
    public void AtualizarUsuario(Usuario usuario)
    {
        using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
        {
            string sql = @"UPDATE USUARIOS 
                       SET USERNAME=@USERNAME, SENHA_HASH=@SENHA, EMAIL=@EMAIL, 
                           ATIVO=@ATIVO, FOTO_PERFIL=@FOTO_PERFIL
                       WHERE ID=@ID";
            using (var command = new SqlCommand(sql, conectaDataBase))
            {
                command.Parameters.AddWithValue("@USERNAME", usuario.Username);
                command.Parameters.AddWithValue("@SENHA", usuario.SenhaHash);
                command.Parameters.AddWithValue("@EMAIL", usuario.Email);
                command.Parameters.AddWithValue("@ATIVO", usuario.Ativo);
                command.Parameters.AddWithValue("@ID", usuario.Id);
                var fotoParam = command.Parameters.Add("@FOTO_PERFIL", System.Data.SqlDbType.VarBinary);
                if (usuario.FotoPerfil != null && usuario.FotoPerfil.Length > 0)
                    fotoParam.Value = usuario.FotoPerfil;
                else
                    fotoParam.Value = DBNull.Value;
                command.ExecuteNonQuery();
            }
        }
    }
}
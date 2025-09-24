using evecorpfy.Models;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data
{
    public class RepositorioUsuarioParticipante
    {
        public List<TipoParticipante> ListarTipos()
        {
            var lista = new List<TipoParticipante>();
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "SELECT ID, NOME, ATIVO FROM TIPO_PARTICIPANTE WHERE ATIVO = 1";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TipoParticipante
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Ativo = reader.GetBoolean(2)
                        });
                    }
                }
            }
            return lista;
        }

        public UsuarioParticipante? ObterPorUsuario(int usuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT ID, USUARIO_ID, TIPO_PARTICIPANTE_ID, CPF, TELEFONE
                               FROM USUARIO_PARTICIPANTE
                               WHERE USUARIO_ID = @USUARIO_ID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@USUARIO_ID", usuarioId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UsuarioParticipante
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                TipoParticipanteId = reader.GetInt32(2),
                                Cpf = reader.GetString(3),
                                Telefone = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void SalvarOuAtualizar(UsuarioParticipante participante)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string checkSql = @"SELECT COUNT(*) FROM USUARIO_PARTICIPANTE WHERE USUARIO_ID = @USUARIO_ID";
                using (var checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@USUARIO_ID", participante.UsuarioId);
                    int count = (int)checkCmd.ExecuteScalar();

                    string sql;
                    if (count > 0)
                    {
                        sql = @"UPDATE USUARIO_PARTICIPANTE
                                SET TIPO_PARTICIPANTE_ID = @TIPO_ID,
                                    CPF = @CPF,
                                    TELEFONE = @TELEFONE
                                WHERE USUARIO_ID = @USUARIO_ID";
                    }
                    else
                    {
                        sql = @"INSERT INTO USUARIO_PARTICIPANTE (USUARIO_ID, TIPO_PARTICIPANTE_ID, CPF, TELEFONE)
                                VALUES (@USUARIO_ID, @TIPO_ID, @CPF, @TELEFONE)";
                    }

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@USUARIO_ID", participante.UsuarioId);
                        cmd.Parameters.AddWithValue("@TIPO_ID", participante.TipoParticipanteId);
                        cmd.Parameters.AddWithValue("@CPF", participante.Cpf);
                        cmd.Parameters.AddWithValue("@TELEFONE", participante.Telefone);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}

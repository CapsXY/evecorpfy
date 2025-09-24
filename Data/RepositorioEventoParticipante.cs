using Microsoft.Data.SqlClient;

namespace evecorpfy.Data
{
    public class RepositorioEventoParticipante
    {
        public void Inscrever(int eventoId, int usuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"INSERT INTO EVENTO_PARTICIPANTES (EVENTO_ID, USUARIO_ID)
                               VALUES (@EVENTO_ID, @USUARIO_ID)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EVENTO_ID", eventoId);
                    cmd.Parameters.AddWithValue("@USUARIO_ID", usuarioId);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627) // violação de UNIQUE
                            throw new Exception("Você já está inscrito neste evento.");
                        else
                            throw;
                    }
                }
            }
        }

        public void Sair(int eventoId, int usuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"DELETE FROM EVENTO_PARTICIPANTES
                               WHERE EVENTO_ID = @EVENTO_ID AND USUARIO_ID = @USUARIO_ID";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EVENTO_ID", eventoId);
                    cmd.Parameters.AddWithValue("@USUARIO_ID", usuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool PossuiConflito(int usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
                    SELECT COUNT(1)
                    FROM EVENTO_PARTICIPANTES ep
                    INNER JOIN EVENTOS e ON ep.EVENTO_ID = e.ID
                    WHERE ep.USUARIO_ID = @USUARIO_ID
                      AND (e.DATA_INICIO <= @DataFim AND e.DATA_FIM >= @DataInicio)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@USUARIO_ID", usuarioId);
                    cmd.Parameters.AddWithValue("@DataInicio", dataInicio);
                    cmd.Parameters.AddWithValue("@DataFim", dataFim);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using evecorpfy.Models;
namespace evecorpfy.Data
{
    public class RepositorioEventoProposta
    {
        public void InserirPropostas(List<EventoProposta> propostas)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    const string sql = @"INSERT INTO EVENTO_PROPOSTAS 
                        (EVENTO_ID, SERVICO_ID, USUARIO_ID, VALOR, DATA_PROPOSTA) 
                        VALUES (@EventoId, @ServicoId, @FornecedorUsuarioId, @Valor, @DataProposta)";
                    foreach (var proposta in propostas)
                    {
                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@EventoId", proposta.EventoId);
                            cmd.Parameters.AddWithValue("@ServicoId", proposta.ServicoId);
                            cmd.Parameters.AddWithValue("@FornecedorUsuarioId", proposta.FornecedorUsuarioId);
                            cmd.Parameters.AddWithValue("@Valor", proposta.ValorProposta);
                            cmd.Parameters.AddWithValue("@DataProposta", proposta.DataProposta);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public bool ExisteProposta(int eventoId, int FornecedorUsuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                const string sql = @"SELECT COUNT(1) 
                                     FROM EVENTO_PROPOSTAS 
                                     WHERE EVENTO_ID = @EventoId 
                                       AND USUARIO_ID = @FornecedorUsuarioId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EventoId", eventoId);
                    cmd.Parameters.AddWithValue("@FornecedorUsuarioId", FornecedorUsuarioId);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // 🔹 Excluir propostas de um fornecedor para um evento (cancelamento)
        public void ExcluirPropostas(int eventoId, int FornecedorUsuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                const string sql = @"DELETE FROM EVENTO_PROPOSTAS 
                                     WHERE EVENTO_ID = @EventoId 
                                       AND USUARIO_ID = @FornecedorUsuarioId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EventoId", eventoId);
                    cmd.Parameters.AddWithValue("@FornecedorUsuarioId", FornecedorUsuarioId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<OrcamentoView> ListarOrcamentosPorEvento(int eventoId)
        {
            var lista = new List<OrcamentoView>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var cmd = new SqlCommand(@"
        SELECT u.USERNAME, ep.VALOR, ep.DATA_PROPOSTA
        FROM EVENTO_PROPOSTAS ep
        INNER JOIN USUARIOS u ON ep.USUARIO_ID = u.ID
        WHERE ep.EVENTO_ID = @EventoId", conn))
            {
                cmd.Parameters.AddWithValue("@EventoId", eventoId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new OrcamentoView
                        {
                            FornecedorNome = reader.GetString(0),
                            Valor = reader.GetDecimal(1),
                            DataProposta = reader.GetDateTime(2)
                        });
                    }
                }
            }

            return lista;
        }

        public HashSet<int> ListarEventosComPropostaFornecedor(int fornecedorUsuarioId)
        {
            var ids = new HashSet<int>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var cmd = new SqlCommand(@"
        SELECT DISTINCT EVENTO_ID
        FROM EVENTO_PROPOSTAS
        WHERE USUARIO_ID = @FornecedorUsuarioId", conn))
            {
                cmd.Parameters.AddWithValue("@FornecedorUsuarioId", fornecedorUsuarioId);

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        ids.Add(rd.GetInt32(0));
                }
            }

            return ids;
        }
        public List<OrcamentoView> ListarOrcamentosPorServico(int eventoId, int servicoId)
        {
            var lista = new List<OrcamentoView>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var cmd = new SqlCommand(@"
                SELECT u.USERNAME, ep.VALOR, ep.DATA_PROPOSTA
                FROM EVENTO_PROPOSTAS ep
                INNER JOIN USUARIOS u ON ep.USUARIO_ID = u.ID
                WHERE ep.EVENTO_ID = @EventoId
                  AND ep.SERVICO_ID = @ServicoId", conn))
            {
                cmd.Parameters.AddWithValue("@EventoId", eventoId);
                cmd.Parameters.AddWithValue("@ServicoId", servicoId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new OrcamentoView
                        {
                            FornecedorNome = reader.GetString(0),
                            Valor = reader.GetDecimal(1),
                            DataProposta = reader.GetDateTime(2)
                        });
                    }
                }
            }

            return lista;
        }
        public List<OrcamentoView> ListarPropostasPorServico(int eventoId, int servicoId)
        {
            var lista = new List<OrcamentoView>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var cmd = new SqlCommand(@"
        SELECT u.USERNAME, ep.VALOR, ep.DATA_PROPOSTA
        FROM EVENTO_PROPOSTAS ep
        INNER JOIN USUARIOS u ON ep.USUARIO_ID = u.ID
        WHERE ep.EVENTO_ID = @EventoId AND ep.SERVICO_ID = @ServicoId", conn))
            {
                cmd.Parameters.AddWithValue("@EventoId", eventoId);
                cmd.Parameters.AddWithValue("@ServicoId", servicoId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new OrcamentoView
                        {
                            FornecedorNome = reader.GetString(0),
                            Valor = reader.GetDecimal(1),
                            DataProposta = reader.GetDateTime(2)
                        });
                    }
                }
            }

            return lista;
        }
    }
}
using Microsoft.Data.SqlClient;
using evecorpfy.Models;

namespace evecorpfy.Data
{
    public class RepositorioDashboardOrganizador
    {
        public List<ParticipanteAgendaResumo> ContarEventosPorParticipante(DateTime? inicio = null, DateTime? fim = null)
        {
            var lista = new List<ParticipanteAgendaResumo>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
                    SELECT u.USERNAME, COUNT(ep.EVENTO_ID) AS QtdeEventos
                    FROM EVENTO_PARTICIPANTES ep
                    INNER JOIN USUARIOS u ON ep.USUARIO_ID = u.ID
                    INNER JOIN EVENTOS e ON ep.EVENTO_ID = e.ID
                    WHERE 1=1";

                if (inicio.HasValue) sql += " AND e.DATA_INICIO >= @Inicio";
                if (fim.HasValue) sql += " AND e.DATA_FIM <= @Fim";
                sql += " GROUP BY u.USERNAME";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (inicio.HasValue) cmd.Parameters.AddWithValue("@Inicio", inicio.Value);
                    if (fim.HasValue) cmd.Parameters.AddWithValue("@Fim", fim.Value);

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            lista.Add(new ParticipanteAgendaResumo
                            {
                                ParticipanteNome = reader.GetString(0),
                                QtdeEventos = reader.GetInt32(1)
                            });
                        }
                }
            }
            return lista;
        }

        public List<FornecedorResumo> ContarPorFornecedor(DateTime? inicio = null, DateTime? fim = null)
        {
            var lista = new List<FornecedorResumo>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
                    SELECT u.USERNAME, SUM(ep.VALOR) AS TotalGasto
                    FROM EVENTO_PROPOSTAS ep
                    INNER JOIN USUARIOS u ON ep.USUARIO_ID = u.ID
                    INNER JOIN EVENTOS e ON ep.EVENTO_ID = e.ID
                    WHERE 1=1";

                if (inicio.HasValue) sql += " AND e.DATA_INICIO >= @Inicio";
                if (fim.HasValue) sql += " AND e.DATA_FIM <= @Fim";
                sql += " GROUP BY u.USERNAME";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (inicio.HasValue) cmd.Parameters.AddWithValue("@Inicio", inicio.Value);
                    if (fim.HasValue) cmd.Parameters.AddWithValue("@Fim", fim.Value);

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            lista.Add(new FornecedorResumo
                            {
                                FornecedorNome = reader.GetString(0),
                                TotalGasto = reader.GetDecimal(1)
                            });
                        }
                }
            }
            return lista;
        }

        public List<TipoParticipanteResumo> ContarPorTipoParticipante(DateTime? inicio = null, DateTime? fim = null)
        {
            var lista = new List<TipoParticipanteResumo>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
                    SELECT tp.NOME, COUNT(up.ID) AS Qtde
                    FROM USUARIO_PARTICIPANTE up
                    INNER JOIN TIPO_PARTICIPANTE tp ON up.TIPO_PARTICIPANTE_ID = tp.ID
                    INNER JOIN EVENTO_PARTICIPANTES ep ON up.USUARIO_ID = ep.USUARIO_ID
                    INNER JOIN EVENTOS e ON ep.EVENTO_ID = e.ID
                    WHERE 1=1";

                if (inicio.HasValue) sql += " AND e.DATA_INICIO >= @Inicio";
                if (fim.HasValue) sql += " AND e.DATA_FIM <= @Fim";
                sql += " GROUP BY tp.NOME";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (inicio.HasValue) cmd.Parameters.AddWithValue("@Inicio", inicio.Value);
                    if (fim.HasValue) cmd.Parameters.AddWithValue("@Fim", fim.Value);

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            lista.Add(new TipoParticipanteResumo
                            {
                                TipoNome = reader.GetString(0),
                                Qtde = reader.GetInt32(1)
                            });
                        }
                }
            }
            return lista;
        }

        public List<SaldoEventoResumo> ObterSaldoEventos(int organizadorId, DateTime? inicio = null, DateTime? fim = null)
        {
            var lista = new List<SaldoEventoResumo>();

            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
                    SELECT e.NOME,
                           e.ORCAMENTO_MAXIMO,
                           ISNULL(SUM(ep.VALOR),0) AS TotalPropostas
                    FROM EVENTOS e
                    LEFT JOIN EVENTO_PROPOSTAS ep ON e.ID = ep.EVENTO_ID
                    WHERE e.ORGANIZADOR_ID = @OrganizadorId";
                if (inicio.HasValue) sql += " AND e.DATA_INICIO >= @Inicio";
                if (fim.HasValue) sql += " AND e.DATA_FIM <= @Fim";
                sql += " GROUP BY e.NOME, e.ORCAMENTO_MAXIMO";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@OrganizadorId", organizadorId);
                    if (inicio.HasValue) cmd.Parameters.AddWithValue("@Inicio", inicio.Value);
                    if (fim.HasValue) cmd.Parameters.AddWithValue("@Fim", fim.Value);

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            lista.Add(new SaldoEventoResumo
                            {
                                EventoNome = reader.GetString(0),
                                OrcamentoMax = reader.GetDecimal(1),
                                TotalPropostas = reader.GetDecimal(2)
                            });
                        }
                }
            }
            return lista;
        }
    }
}

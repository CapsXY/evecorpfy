using evecorpfy.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace evecorpfy.Data
{
    public class RepositorioEvento
    {
        public void Inserir(Evento evento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            using (var transactionDataBase = conectaDataBase.BeginTransaction())
            {
                try
                {
                    string sql = @"
                        INSERT INTO EVENTOS 
                        (NOME, DATA_INICIO, DATA_FIM, CEP, LOGRADOURO, NUMERO, BAIRRO, LOCALIDADE, UF, ESTADO, OBSERVACOES, CAPACIDADE, ORCAMENTO_MAXIMO, ORGANIZADOR_ID, TIPO_EVENTO_ID, STATUS) 
                        OUTPUT INSERTED.ID
                        VALUES 
                        (@NOME, @DATA_INICIO, @DATA_FIM, @CEP, @LOGRADOURO, @NUMERO, @BAIRRO, @LOCALIDADE, @UF, @ESTADO, @OBSERVACOES, @CAPACIDADE, @ORCAMENTO_MAXIMO, @ORGANIZADOR_ID, @TIPO_EVENTO_ID, @STATUS)";

                    int novoId;
                    using (var command = new SqlCommand(sql, conectaDataBase, transactionDataBase))
                    {
                        command.Parameters.AddWithValue("@NOME", evento.Nome);
                        command.Parameters.AddWithValue("@DATA_INICIO", evento.DataInicio);
                        command.Parameters.AddWithValue("@DATA_FIM", evento.DataFim);
                        command.Parameters.AddWithValue("@CEP", evento.Cep);
                        command.Parameters.AddWithValue("@LOGRADOURO", evento.Logradouro);
                        command.Parameters.AddWithValue("@NUMERO", (object)evento.Numero ?? "S/N");
                        command.Parameters.AddWithValue("@BAIRRO", evento.Bairro);
                        command.Parameters.AddWithValue("@LOCALIDADE", evento.Localidade);
                        command.Parameters.AddWithValue("@UF", evento.Uf);
                        command.Parameters.AddWithValue("@ESTADO", evento.Estado);
                        command.Parameters.AddWithValue("@OBSERVACOES", (object)evento.Observacoes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CAPACIDADE", evento.Capacidade);
                        command.Parameters.AddWithValue("@ORCAMENTO_MAXIMO", evento.OrcamentoMaximo);
                        command.Parameters.AddWithValue("@ORGANIZADOR_ID", evento.OrganizadorId);
                        command.Parameters.AddWithValue("@TIPO_EVENTO_ID", evento.TipoEventoId);
                        var status = string.IsNullOrWhiteSpace(evento.Status) ? "EM CADASTRAMENTO" : evento.Status;
                        command.Parameters.Add("@STATUS", SqlDbType.NVarChar, 20).Value = status;
                        novoId = (int)command.ExecuteScalar();
                    }
                    // Insere serviços vinculados (se houver)
                    if (evento.ServicosIds != null && evento.ServicosIds.Count > 0)
                    {
                        InserirServicosEvento(novoId, evento.ServicosIds, conectaDataBase, transactionDataBase);
                    }

                    transactionDataBase.Commit();
                }
                catch
                {
                    transactionDataBase.Rollback();
                    throw;
                }
            }
        }
        public List<Evento> ListarTodos()
        {
            var lista = new List<Evento>();
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT ID, NOME, DATA_INICIO, DATA_FIM, CEP, LOGRADOURO, NUMERO, BAIRRO, LOCALIDADE, UF, ESTADO, OBSERVACOES, CAPACIDADE, ORCAMENTO_MAXIMO, ORGANIZADOR_ID, TIPO_EVENTO_ID, STATUS FROM EVENTOS";
                using (var command = new SqlCommand(sql, conectaDataBase))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Evento
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            DataInicio = reader.GetDateTime(2),
                            DataFim = reader.GetDateTime(3),
                            Cep = reader.GetString(4),
                            Logradouro = reader.GetString(5),
                            Numero = reader.IsDBNull(6) ? "S/N" : reader.GetString(6),
                            Bairro = reader.GetString(7),
                            Localidade = reader.GetString(8),
                            Uf = reader.GetString(9),
                            Estado = reader.GetString(10),
                            Observacoes = reader.IsDBNull(11) ? null : reader.GetString(11),
                            Capacidade = reader.GetInt32(12),
                            OrcamentoMaximo = reader.GetDecimal(13),
                            OrganizadorId = reader.GetInt32(14),
                            TipoEventoId = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                            Status = reader.GetString(16)
                        });
                    }
                }
            }
            return lista;
        }
        public List<Evento> ListarPorOrganizador(int usuarioId)
        {
            var lista = new List<Evento>();

            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT ID, NOME, DATA_INICIO, DATA_FIM, CEP, LOGRADOURO, NUMERO, BAIRRO, LOCALIDADE, UF, ESTADO, OBSERVACOES, CAPACIDADE, ORCAMENTO_MAXIMO, ORGANIZADOR_ID, TIPO_EVENTO_ID, STATUS
                               FROM EVENTOS
                               WHERE ORGANIZADOR_ID = @USUARIO_ID
                               ORDER BY DATA_INICIO DESC";

                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@USUARIO_ID", usuarioId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Evento
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                DataInicio = reader.GetDateTime(2),
                                DataFim = reader.GetDateTime(3),
                                Cep = reader.GetString(4),
                                Logradouro = reader.GetString(5),
                                Numero = reader.IsDBNull(6) ? "S/N" : reader.GetString(6),
                                Bairro = reader.GetString(7),
                                Localidade = reader.GetString(8),
                                Uf = reader.GetString(9),
                                Estado = reader.GetString(10),
                                Observacoes = reader.IsDBNull(11) ? "" : reader.GetString(11),
                                Capacidade = reader.GetInt32(12),
                                OrcamentoMaximo = reader.GetDecimal(13),
                                OrganizadorId = reader.GetInt32(14),
                                TipoEventoId = reader.IsDBNull(15) ? 0 : reader.GetInt32(15),
                                Status = reader.GetString(16)
                            });
                        }
                    }
                }
            }
            return lista;
        }
        public void Atualizar(Evento evento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            using (var transactionDataBase = conectaDataBase.BeginTransaction())
            {
                try
                {
                    string sql = @"UPDATE EVENTOS 
                                   SET NOME=@NOME, DATA_INICIO=@DATA_INICIO, DATA_FIM=@DATA_FIM, CEP=@CEP, LOGRADOURO=@LOGRADOURO, NUMERO=@NUMERO, BAIRRO=@BAIRRO, LOCALIDADE=@LOCALIDADE, UF=@UF, ESTADO=@ESTADO, OBSERVACOES=@OBSERVACOES, CAPACIDADE=@CAPACIDADE, ORCAMENTO_MAXIMO=@ORCAMENTO_MAXIMO, TIPO_EVENTO_ID=@TIPO_EVENTO_ID 
                                   WHERE ID=@ID";

                    using (var command = new SqlCommand(sql, conectaDataBase, transactionDataBase))
                    {
                        command.Parameters.AddWithValue("@ID", evento.Id);
                        command.Parameters.AddWithValue("@NOME", evento.Nome);
                        command.Parameters.AddWithValue("@DATA_INICIO", evento.DataInicio);
                        command.Parameters.AddWithValue("@DATA_FIM", evento.DataFim);
                        command.Parameters.AddWithValue("@CEP", evento.Cep);
                        command.Parameters.AddWithValue("@LOGRADOURO", evento.Logradouro);
                        command.Parameters.AddWithValue("@NUMERO", (object)evento.Numero ?? "S/N");
                        command.Parameters.AddWithValue("@BAIRRO", evento.Bairro);
                        command.Parameters.AddWithValue("@LOCALIDADE", evento.Localidade);
                        command.Parameters.AddWithValue("@UF", evento.Uf);
                        command.Parameters.AddWithValue("@ESTADO", evento.Estado);
                        command.Parameters.AddWithValue("@OBSERVACOES", (object)evento.Observacoes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CAPACIDADE", evento.Capacidade);
                        command.Parameters.AddWithValue("@ORCAMENTO_MAXIMO", evento.OrcamentoMaximo);
                        command.Parameters.AddWithValue("@TIPO_EVENTO_ID", evento.TipoEventoId);
                        command.ExecuteNonQuery();
                    }
                    // Sincroniza serviços: apaga todos e insere os atuais (se houver)
                    DeletarServicosEvento(evento.Id, conectaDataBase, transactionDataBase);
                    if (evento.ServicosIds != null && evento.ServicosIds.Count > 0)
                    {
                        InserirServicosEvento(evento.Id, evento.ServicosIds, conectaDataBase, transactionDataBase);
                    }
                    transactionDataBase.Commit();
                }
                catch
                {
                    transactionDataBase.Rollback();
                    throw;
                }
            }
        }
        private void InserirServicosEvento(int eventoId, List<int> servicosIds, SqlConnection conn, SqlTransaction tx)
        {
            if (servicosIds == null || servicosIds.Count == 0)
                return;
            const string insertSql = @"INSERT INTO EVENTO_SERVICOS (EVENTO_ID, SERVICO_ID) VALUES (@EVENTO_ID, @SERVICO_ID)";
            foreach (var servicoId in servicosIds.Distinct())
            {
                using (var cmd = new SqlCommand(insertSql, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@EVENTO_ID", eventoId);
                    cmd.Parameters.AddWithValue("@SERVICO_ID", servicoId);
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        throw new Exception($"Falha ao inserir vínculo Evento={eventoId}, Servico={servicoId}");
                    }
                }
            }
        }
        private void DeletarServicosEvento(int eventoId, SqlConnection conectaDataBase, SqlTransaction transactioDataBase)
        {
            const string deleteSql = @"DELETE FROM EVENTO_SERVICOS WHERE EVENTO_ID=@EVENTO_ID";
            using (var command = new SqlCommand(deleteSql, conectaDataBase, transactioDataBase))
            {
                command.Parameters.AddWithValue("@EVENTO_ID", eventoId);
                command.ExecuteNonQuery();
            }
        }
        public List<int> ListarServicosIdsPorEvento(int eventoId)
        {
            var ids = new List<int>();

            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT SERVICO_ID 
                       FROM EVENTO_SERVICOS 
                       WHERE EVENTO_ID = @EVENTO_ID";

                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@EVENTO_ID", eventoId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            return ids;
        }
        public List<Evento> ListarTodosComVagas()
        {
            var lista = new List<Evento>();
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"
            SELECT e.ID, e.NOME, e.DATA_INICIO, e.DATA_FIM, e.STATUS, e.CAPACIDADE,
                   (e.CAPACIDADE - COUNT(ep.ID)) AS VAGAS_DISPONIVEIS
            FROM EVENTOS e
            LEFT JOIN EVENTO_PARTICIPANTES ep ON e.ID = ep.EVENTO_ID
            GROUP BY e.ID, e.NOME, e.DATA_INICIO, e.DATA_FIM, e.STATUS, e.CAPACIDADE
            ORDER BY e.DATA_INICIO DESC";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Evento
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            DataInicio = reader.GetDateTime(2),
                            DataFim = reader.GetDateTime(3),
                            Status = reader.GetString(4),
                            Capacidade = reader.GetInt32(5),
                            VagasDisponiveis = reader.GetInt32(6)
                        });
                    }
                }
            }
            return lista;
        }
        public List<TipoServico> ListarServicosDoEvento(int eventoId)
        {
            var lista = new List<TipoServico>();
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                const string sql = @"
            SELECT ts.ID, ts.NOME, ts.ATIVO
            FROM EVENTO_SERVICOS es
            INNER JOIN TIPO_SERVICOS ts ON ts.ID = es.SERVICO_ID
            WHERE es.EVENTO_ID = @EVENTO_ID
            ORDER BY ts.NOME";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EVENTO_ID", eventoId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            lista.Add(new TipoServico
                            {
                                Id = r.GetInt32(0),
                                Nome = r.GetString(1),
                                Ativo = r.GetBoolean(2)
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
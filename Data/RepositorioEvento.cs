using evecorpfy.Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace evecorpfy.Data
{
    public class RepositorioEvento
    {
        // Método para inserir um novo evento no banco de dados
        public void Inserir(Evento evento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"INSERT INTO EVENTOS (NOME, DATA_INICIO, DATA_FIM, CEP, LOGRADOURO, NUMERO, BAIRRO, LOCALIDADE, UF, ESTADO, OBSERVACOES, CAPACIDADE, ORCAMENTO_MAXIMO, ORGANIZADOR_ID, TIPO_EVENTO_ID, STATUS) VALUES (@NOME, @DATA_INICIO, @DATA_FIM, @CEP, @LOGRADOURO, @NUMERO, @BAIRRO, @LOCALIDADE, @UF, @ESTADO, @OBSERVACOES, @CAPACIDADE, @ORCAMENTO_MAXIMO, @ORGANIZADOR_ID, @TIPO_EVENTO_ID, @STATUS)";
                using (var command = new SqlCommand(sql, conectaDataBase))
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
                    command.ExecuteNonQuery();
                }
            }
        }
        // Método para listar todos os eventos
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
            {
                string sql = @"UPDATE EVENTOS SET NOME=@NOME, DATA_INICIO=@DATA_INICIO, DATA_FIM=@DATA_FIM, CEP=@CEP, LOGRADOURO=@LOGRADOURO, NUMERO=@NUMERO, BAIRRO=@BAIRRO, LOCALIDADE=@LOCALIDADE, UF=@UF, ESTADO=@ESTADO, OBSERVACOES=@OBSERVACOES, CAPACIDADE=@CAPACIDADE, ORCAMENTO_MAXIMO=@ORCAMENTO_MAXIMO, TIPO_EVENTO_ID=@TIPO_EVENTO_ID WHERE ID=@ID";
                using (var command = new SqlCommand(sql, conectaDataBase))
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
            }
        }
    }
}

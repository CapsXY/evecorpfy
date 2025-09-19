using evecorpfy.Models;
using Microsoft.Data.SqlClient;

namespace evecorpfy.Data
{
    public class RepositorioTipoEvento
    {
        public void Inserir(TipoEvento tipoEvento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "INSERT INTO TIPO_EVENTOS (NOME, ATIVO) VALUES (@NOME, @ATIVO)";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@NOME", tipoEvento.Nome);
                    command.Parameters.AddWithValue("@ATIVO", tipoEvento.Ativo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TipoEvento> ListarTodos()
        {
            var lista = new List<TipoEvento>();
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "SELECT ID, NOME, ATIVO FROM TIPO_EVENTOS ORDER BY NOME";
                using (var command = new SqlCommand(sql, conectaDataBase))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TipoEvento
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

        public void Atualizar(TipoEvento tipoEvento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "UPDATE TIPO_EVENTOS SET NOME=@NOME, ATIVO=@ATIVO WHERE ID=@ID";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@NOME", tipoEvento.Nome);
                    command.Parameters.AddWithValue("@ATIVO", tipoEvento.Ativo);
                    command.Parameters.AddWithValue("@ID", tipoEvento.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

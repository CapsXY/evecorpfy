using Microsoft.Data.SqlClient;
using evecorpfy.Models;

namespace evecorpfy.Data
{
    public class RepositorioTipoServico
    {
        public void Inserir(TipoServico tipoServico)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "INSERT INTO TIPO_SERVICOS (NOME, ATIVO) VALUES (@NOME, @ATIVO)";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@NOME", tipoServico.Nome);
                    command.Parameters.AddWithValue("@ATIVO", tipoServico.Ativo);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TipoServico> ListarTodos()
        {
            var lista = new List<TipoServico>();
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "SELECT ID, NOME, ATIVO FROM TIPO_SERVICOS ORDER BY NOME";
                using (var command = new SqlCommand(sql, conectaDataBase))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TipoServico
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

        public void Atualizar(TipoServico tipoServico)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "UPDATE TIPO_SERVICOS SET NOME=@NOME, ATIVO=@ATIVO WHERE ID=@ID";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@NOME", tipoServico.Nome);
                    command.Parameters.AddWithValue("@ATIVO", tipoServico.Ativo);
                    command.Parameters.AddWithValue("@ID", tipoServico.Id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

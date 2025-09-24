using evecorpfy.Models;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data
{
    public class RepositorioUsuarioFornecedor
    {
        public UsuarioFornecedor? ObterPorUsuarioId(int usuarioId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"SELECT ID, USUARIO_ID, CNPJ, TELEFONE
                               FROM USUARIO_FORNECEDOR 
                               WHERE USUARIO_ID = @USUARIO_ID";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@USUARIO_ID", usuarioId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UsuarioFornecedor
                            {
                                Id = reader.GetInt32(0),
                                UsuarioId = reader.GetInt32(1),
                                Cnpj = reader.GetString(2),
                                Telefone = reader.GetString(3)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public void SalvarOuAtualizar(UsuarioFornecedor fornecedor)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string checkSql = @"SELECT COUNT(*) FROM USUARIO_FORNECEDOR WHERE USUARIO_ID = @USUARIO_ID";
                using (var checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@USUARIO_ID", fornecedor.UsuarioId);
                    int count = (int)checkCmd.ExecuteScalar();
                    string sql;
                    if (count > 0)
                    {
                        sql = @"UPDATE USUARIO_FORNECEDOR 
                                SET CNPJ = @CNPJ, TELEFONE = @TELEFONE
                                WHERE USUARIO_ID = @USUARIO_ID";
                    }
                    else
                    {
                        sql = @"INSERT INTO USUARIO_FORNECEDOR (USUARIO_ID, CNPJ, TELEFONE)
                                VALUES (@USUARIO_ID, @CNPJ, @TELEFONE)";
                    }
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@USUARIO_ID", fornecedor.UsuarioId);
                        cmd.Parameters.AddWithValue("@CNPJ", fornecedor.Cnpj);
                        cmd.Parameters.AddWithValue("@TELEFONE", fornecedor.Telefone);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
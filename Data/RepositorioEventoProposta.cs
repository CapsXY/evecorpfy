using Microsoft.Data.SqlClient;
using evecorpfy.Models;

namespace evecorpfy.Data
{
    public class RepositorioEventoProposta
    {
        //public void InserirPropostas(List<EventoProposta> propostas)
        //{
        //    using (var conn = DbConnectionFactory.GetOpenConnection())
        //    using (var tran = conn.BeginTransaction())
        //    {
        //        try
        //        {
        //            const string sql = @"INSERT INTO EVENTO_PROPOSTAS 
        //                (EVENTO_ID, SERVICO_ID, FORNECEDOR_ID, VALOR, DATA_PROPOSTA) 
        //                VALUES (@EVENTO_ID, @SERVICO_ID, @FORNECEDOR_ID, @VALOR, @DATA_PROPOSTA)";

        //            foreach (var proposta in propostas)
        //            {
        //                using (var cmd = new SqlCommand(sql, conn, tran))
        //                {
        //                    cmd.Parameters.AddWithValue("@EVENTO_ID", proposta.EventoId);
        //                    cmd.Parameters.AddWithValue("@SERVICO_ID", proposta.ServicoId);
        //                    cmd.Parameters.AddWithValue("@FORNECEDOR_ID", proposta.FornecedorId);
        //                    cmd.Parameters.AddWithValue("@VALOR", proposta.ValorProposta);
        //                    cmd.Parameters.AddWithValue("@DATA_PROPOSTA", proposta.DataProposta);

        //                    cmd.ExecuteNonQuery();
        //                }
        //            }

        //            tran.Commit();
        //        }
        //        catch
        //        {
        //            tran.Rollback();
        //            throw;
        //        }
        //    }
        //}
        //public bool ExisteProposta(int eventoId, int fornecedorId)
        //{
        //    using (var conn = DbConnectionFactory.GetOpenConnection())
        //    {
        //        const string sql = @"SELECT COUNT(1) 
        //                     FROM EVENTO_PROPOSTAS 
        //                     WHERE EVENTO_ID = @EventoId AND FORNECEDOR_ID = @FornecedorId";

        //        using (var cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@EventoId", eventoId);
        //            cmd.Parameters.AddWithValue("@FornecedorId", fornecedorId);

        //            int count = (int)cmd.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }
        //}

        // 🔹 Inserir várias propostas de uma vez
        public void InserirPropostas(List<EventoProposta> propostas)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    const string sql = @"INSERT INTO EVENTO_PROPOSTAS 
                        (EVENTO_ID, SERVICO_ID, FORNECEDOR_ID, VALOR, DATA_PROPOSTA) 
                        VALUES (@EventoId, @ServicoId, @FornecedorId, @Valor, @DataProposta)";

                    foreach (var proposta in propostas)
                    {
                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@EventoId", proposta.EventoId);
                            cmd.Parameters.AddWithValue("@ServicoId", proposta.ServicoId);
                            cmd.Parameters.AddWithValue("@FornecedorId", proposta.FornecedorId);
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

        // 🔹 Verificar se o fornecedor já enviou proposta para o evento
        public bool ExisteProposta(int eventoId, int fornecedorId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                const string sql = @"SELECT COUNT(1) 
                                     FROM EVENTO_PROPOSTAS 
                                     WHERE EVENTO_ID = @EventoId 
                                       AND FORNECEDOR_ID = @FornecedorId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EventoId", eventoId);
                    cmd.Parameters.AddWithValue("@FornecedorId", fornecedorId);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // 🔹 Excluir propostas de um fornecedor para um evento (cancelamento)
        public void ExcluirPropostas(int eventoId, int fornecedorId)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                const string sql = @"DELETE FROM EVENTO_PROPOSTAS 
                                     WHERE EVENTO_ID = @EventoId 
                                       AND FORNECEDOR_ID = @FornecedorId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EventoId", eventoId);
                    cmd.Parameters.AddWithValue("@FornecedorId", fornecedorId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public HashSet<int> ListarEventosComPropostaFornecedor(int fornecedorId)
        {
            var ids = new HashSet<int>();
            using (var conn = DbConnectionFactory.GetOpenConnection())
            using (var cmd = new SqlCommand(
                @"SELECT DISTINCT EVENTO_ID 
          FROM EVENTO_PROPOSTAS 
          WHERE FORNECEDOR_ID = @FornecedorId", conn))
            {
                cmd.Parameters.AddWithValue("@FornecedorId", fornecedorId);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        ids.Add(rd.GetInt32(0));
            }
            return ids;
        }
    }
}
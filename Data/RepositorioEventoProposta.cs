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
                        (EVENTO_ID, SERVICO_ID, FORNECEDOR_ID, VALOR, DATA_PROPOSTA) 
                        VALUES (@EVENTO_ID, @SERVICO_ID, @FORNECEDOR_ID, @VALOR, @DATA_PROPOSTA)";

                    foreach (var proposta in propostas)
                    {
                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@EVENTO_ID", proposta.EventoId);
                            cmd.Parameters.AddWithValue("@SERVICO_ID", proposta.ServicoId);
                            cmd.Parameters.AddWithValue("@FORNECEDOR_ID", proposta.FornecedorId);
                            cmd.Parameters.AddWithValue("@VALOR", proposta.ValorProposta);
                            cmd.Parameters.AddWithValue("@DATA_PROPOSTA", proposta.DataProposta);

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
    }
}

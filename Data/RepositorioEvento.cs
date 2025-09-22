using evecorpfy.Models;
using Microsoft.Data.SqlClient;
namespace evecorpfy.Data
{
    public class RepositorioEvento
    {
        public void Inserir(Evento evento)
        {
            using (var conectaDataBase = DbConnectionFactory.GetOpenConnection())
            {
                string sql = @"INSERT INTO EVENTOS 
                               (NOME, DATA_INICIO, DATA_FIM, CEP, LOGRADOURO, BAIRRO, LOCALIDADE, UF, ESTADO, CAPACIDADE, ORCAMENTO_MAXIMO, ORGANIZADOR_ID, TIPO_EVENTO_ID, STATUS)
                               VALUES (@NOME, @DATA_INICIO, @DATA_FIM, @CEP, @LOGRADOURO, @BAIRRO, @LOCALIDADE, @UF, @ESTADO, 
                                       @OBSERVACOES, @CAPACIDADE, @ORCAMENTO_MAXIMO, @ORGANIZADOR_ID, @TIPO_EVENTO_ID, @STATUS)";
                using (var command = new SqlCommand(sql, conectaDataBase))
                {
                    command.Parameters.AddWithValue("@NOME", evento.Nome);
                    command.Parameters.AddWithValue("@DATA_INICIO", evento.DataInicio);
                    command.Parameters.AddWithValue("@DATA_FIM", evento.DataFim);
                    command.Parameters.AddWithValue("@CEP", evento.Cep);
                    command.Parameters.AddWithValue("@LOGRADOURO", evento.Logradouro);
                    command.Parameters.AddWithValue("@BAIRRO", evento.Bairro);
                    command.Parameters.AddWithValue("@LOCALIDADE", evento.Localidade);
                    command.Parameters.AddWithValue("@UF", evento.Uf);
                    command.Parameters.AddWithValue("@ESTADO", evento.Estado);
                    command.Parameters.AddWithValue("@CAPACIDADE", evento.Capacidade);
                    command.Parameters.AddWithValue("@ORCAMENTO_MAXIMO", evento.OrcamentoMaximo);
                    command.Parameters.AddWithValue("@ORGANIZADOR_ID", evento.OrganizadorId);
                    command.Parameters.AddWithValue("@TIPO_EVENTO_ID", evento.TipoEventoId);
                    command.Parameters.AddWithValue("@STATUS", evento.Status);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void PreencherCamposEndereco(string enderecoCompleto)
        {
            try
            {
                // Exemplo: "Praça da Sé, 123 - Sé - São Paulo/SP - CEP:01001000"

                // Primeiro pega o CEP
                var cepPart = enderecoCompleto.Split("CEP:")[1];
                TextBoxCEP.Text = cepPart.Trim();

                // Remove o trecho do CEP
                var semCep = enderecoCompleto.Split("- CEP:")[0];

                // Quebra as partes principais
                var partes = semCep.Split('-');
                // [0] "Praça da Sé, 123 "
                // [1] " Sé "
                // [2] " São Paulo/SP "

                // Logradouro e número
                var logradouroNum = partes[0].Split(',');
                TextBoxLogradouro.Text = logradouroNum[0].Trim();
                TextBoxNumero.Text = logradouroNum.Length > 1 ? logradouroNum[1].Trim() : "";

                // Bairro
                TextBoxBairro.Text = partes.Length > 1 ? partes[1].Trim() : "";

                // Cidade / UF
                if (partes.Length > 2)
                {
                    var cidadeUf = partes[2].Split('/');
                    TextBoxCidade.Text = cidadeUf[0].Trim();
                    TextBoxUF.Text = cidadeUf.Length > 1 ? cidadeUf[1].Trim() : "";
                }
            }
            catch
            {
                // fallback caso o formato esteja estranho
                TextBoxEndereco.Text = enderecoCompleto;
            }
        }

        public Evento ObterPorId(int id)
        {
            using (var conn = DbConnectionFactory.GetOpenConnection())
            {
                string sql = "SELECT ID, NOME, DATA_INICIO, DATA_FIM, CEP, ENDERECO, CAPACIDADE, ORCAMENTO_MAXIMO, TIPO_EVENTO_ID, STATUS FROM EVENTOS WHERE ID=@ID";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var evento = new Evento
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                DataInicio = reader.GetDateTime(2),
                                DataFim = reader.GetDateTime(3),
                                Cep = reader.GetString(4),
                                Endereco = reader.GetString(5),
                                Capacidade = reader.GetInt32(6),
                                OrcamentoMaximo = reader.GetDecimal(7),
                                TipoEventoId = reader.GetInt32(8),
                                Status = reader.GetString(9)
                            };

                            // Quebrando o endereço para os campos
                            PreencherCamposEndereco(evento.Endereco);

                            return evento;
                        }
                    }
                }
            }
            return null;
        }

    }
}

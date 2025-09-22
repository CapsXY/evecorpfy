using evecorpfy.Data;
using evecorpfy.Models;
using evecorpfy.Services;
using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace evecorpfy.ViewsOrganizador
{
    /// <summary>
    /// Interação lógica para CadastroEventos.xam
    /// </summary>
    public partial class CadastroEventos : UserControl
    {
        public CadastroEventos()
        {
            InitializeComponent();
        }
        private void TextBoxCEPPrevisualizar(object sender, TextCompositionEventArgs e)
        {
            // Permitir apenas dígitos
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void TextBoxCEPIdentador(object sender, TextChangedEventArgs e)
        {
            var textCep = TextBoxCEP.Text.Replace("-", "").Trim();

            if (textCep.Length > 5)
                TextBoxCEP.Text = textCep.Insert(5, "-");

            TextBoxCEP.CaretIndex = TextBoxCEP.Text.Length;
        }
        private async void ButtonBuscarCEP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = new ViaCepService();
                var cep = TextBoxCEP.Text.Replace("-", "").Trim(); // remove o hífen
                var endereco = await service.BuscarCepAsync(cep);

                // pega o número do textbox manual
                endereco.Numero = TextBoxNumero.Text.Trim();

                if (endereco != null && endereco.Cep != null && endereco.Logradouro != null)
                {
                    // concatena o número ao logradouro
                    TextBoxEndereco.Text = $"{endereco.Logradouro}, {endereco.Numero} - {endereco.Bairro} - {endereco.Localidade}/{endereco.Uf} - CEP:{endereco.Cep}"; ;
                }
                else
                {
                    MessageBox.Show("CEP não encontrado!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar CEP: {ex.Message}");
            }
        }
        private void ButtonCadastrarEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var enderecoCompleto = $"{TextBoxLogradouro.Text.Trim()}, {TextBoxNumero.Text.Trim()} - " +
                       $"{TextBoxBairro.Text.Trim()} - {TextBoxCidade.Text.Trim()}/{TextBoxUF.Text.Trim()} - CEP:{TextBoxCEP.Text.Replace("-", "").Trim()}";

                var evento = new Evento
                {
                    Nome = TextBoxNomeEvento.Text.Trim(),
                    DataInicio = DatePickerInicio.SelectedDate.Value,
                    DataFim = DatePickerFim.SelectedDate.Value,
                    Cep = TextBoxCEP.Text.Replace("-", "").Trim(),
                    Endereco = enderecoCompleto,
                    Capacidade = int.Parse(TextBoxCapacidade.Text),
                    OrcamentoMaximo = decimal.Parse(TextBoxOrcamento.Text),
                    OrganizadorId = Sessao.UsuarioId,
                    TipoEventoId = (int)ComboBoxTipoEvento.SelectedValue,
                    Status = "EM ANDAMENTO"
                };


                var repo = new RepositorioEvento();
                repo.Inserir(evento);

                MessageBox.Show("Evento cadastrado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar evento: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

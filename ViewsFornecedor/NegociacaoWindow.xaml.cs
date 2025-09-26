using evecorpfy.Data;
using evecorpfy.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace evecorpfy.ViewsFornecedor
{
    public partial class NegociacaoWindow : Window
    {
        private readonly int eventoId;
        private static readonly Regex _regex = new Regex("[^0-9,]+");
        private static readonly decimal ValorMaximo = 999999999.99m;
        private readonly decimal orcamentoMaximo;
        private List<EventoProposta> servicos;
        public NegociacaoWindow(int eventoId, decimal orcamentoMaximo)
        {
            InitializeComponent();
            this.eventoId = eventoId;
            this.orcamentoMaximo = orcamentoMaximo;
            CarregarServicos();
        }
        private void CarregarServicos()
        {
            var repo = new RepositorioEvento();
            var servicosEvento = repo.ListarServicosDoEvento(eventoId);
            servicos = servicosEvento.Select(s => new EventoProposta
            {
                ServicoId = s.Id,
                EventoId = eventoId,
                FornecedorUsuarioId = Sessao.UsuarioId,
                NomeServico = s.Nome,
                ValorProposta = 0,
                DataProposta = DateTime.Now
            }).ToList();
            DataGridServicos.ItemsSource = servicos;
            AtualizarOrcamentoRestante();
        }
        private void AtualizarOrcamentoRestante()
        {
            decimal totalPropostas = servicos.Sum(s => s.ValorProposta);
            decimal restante = orcamentoMaximo - totalPropostas;
            TextBlockOrcamentoRestante.Text = restante.ToString("C");
            TextBlockOrcamentoRestante.Foreground =
                restante < 0 ? System.Windows.Media.Brushes.Red
                             : System.Windows.Media.Brushes.Black;
        }
        private void EnviarOrcamento_Click(object sender, RoutedEventArgs e)
        {
            if (!Sessao.FornecedorId.HasValue)
            {
                MessageBox.Show("Fornecedor não identificado na sessão.",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            decimal soma = servicos.Sum(s => s.ValorProposta);
            if (soma > orcamentoMaximo)
            {
                MessageBox.Show("O valor total das propostas ultrapassa o orçamento máximo do evento!",
                                "Erro de Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var repo = new RepositorioEventoProposta();
                foreach (var s in servicos)
                {
                    s.EventoId = eventoId;
                    s.FornecedorUsuarioId = Sessao.UsuarioId;
                    s.DataProposta = DateTime.Now;
                }
                repo.InserirPropostas(servicos);
                MessageBox.Show("Proposta enviada com sucesso!",
                                "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar propostas: {ex.Message}",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void DataGridServicos_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(AtualizarOrcamentoRestante),
                                   System.Windows.Threading.DispatcherPriority.Background);
        }
        private void ValorProposta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }
        private void ValorProposta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox txt && !string.IsNullOrWhiteSpace(txt.Text))
            {
                if (decimal.TryParse(txt.Text, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal valor))
                {
                    if (valor > ValorMaximo)
                        valor = ValorMaximo;
                    txt.Text = valor.ToString("N2", new CultureInfo("pt-BR"));
                }
                else
                {
                    txt.Text = "0,00";
                }
            }
        }
        private void DataGridServicos_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var dataGrid = sender as DataGrid;
                dataGrid.BeginEdit();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}

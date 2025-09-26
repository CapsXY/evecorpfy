using System.Windows;
using evecorpfy.Data;
using evecorpfy.Models;
namespace evecorpfy.ViewsOrganizador
{
    public partial class NegociacaoWindow : Window
    {
        private Evento _evento;
        public NegociacaoWindow(Evento evento)
        {
            InitializeComponent();
            _evento = evento;

            TxtEvento.Text = $"Evento: {evento.Nome}";
            CarregarServicos();
        }
        private void CarregarServicos()
        {
            var repo = new RepositorioEvento();
            var servicosIds = repo.ListarServicosIdsPorEvento(_evento.Id);
            var repoServicos = new RepositorioTipoServico();
            var servicos = repoServicos.ListarTodos()
                                       .Where(s => servicosIds.Contains(s.Id))
                                       .ToList();
            DataGridServicos.ItemsSource = servicos;
            var repoPropostas = new RepositorioEventoProposta();
            var propostas = repoPropostas.ListarOrcamentosPorEvento(_evento.Id);

            decimal total = propostas.Sum(p => p.Valor);
            TxtTotal.Content = $"Total Orçamento: {total:C}";
        }
        private void VerOrcamentos_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is int servicoId)
            {
                var servico = ((List<TipoServico>)DataGridServicos.ItemsSource)
                                .FirstOrDefault(s => s.Id == servicoId);

                if (servico != null)
                {
                    // Abre a janela de orçamentos já com dados reais
                    var win = new OrcamentoWindow(servico, _evento);
                    win.Owner = this;
                    win.ShowDialog();
                }
            }
        }
        private void AtualizarTotalOrcamento()
        {
            var itens = DataGridServicos.ItemsSource as List<OrcamentoView>;
            if (itens != null && itens.Count > 0)
            {
                decimal total = itens.Sum(x => x.Valor);
                TxtTotal.Content = $"Total Orçamento: {total:C}";
            }
            else
            {
                TxtTotal.Content = "Total Orçamento: R$ 0,00";
            }
        }

        private void Aceitar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Orçamento ACEITO com sucesso!", "Confirmação",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Orçamento CANCELADO.", "Aviso",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

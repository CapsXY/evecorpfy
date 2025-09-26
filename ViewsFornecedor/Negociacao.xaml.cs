using evecorpfy.Data;
using evecorpfy.Models;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy.ViewsFornecedor
{
    public partial class Negociacao : UserControl
    {
        public Negociacao()
        {
            InitializeComponent();
            CarregarEventos();
            Loaded += (_, __) => CarregarEventos();
            IsVisibleChanged += (s, e) =>
            {
                if (IsVisible) CarregarEventos();
            };
        }
        private void CarregarEventos()
        {
            if (!Sessao.FornecedorId.HasValue)
            {
                MessageBox.Show("Fornecedor não encontrado para este usuário.",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                DataGridEventos.ItemsSource = null;
                return;
            }
            var repoEvento = new RepositorioEvento();
            var eventos = repoEvento.ListarTodos()
                                    .Where(e => e.Status == "EM CADASTRAMENTO")
                                    .ToList();
            var repoPropostas = new RepositorioEventoProposta();
            var eventosComProposta = repoPropostas.ListarEventosComPropostaFornecedor(Sessao.UsuarioId);
            foreach (var ev in eventos)
                ev.TemOrcamento = eventosComProposta.Contains(ev.Id);
            DataGridEventos.ItemsSource = eventos;
            DataGridEventos.Items.Refresh();
        }
        private void NegociarOuCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Evento evento)
            {
                if (evento.TemOrcamento)
                {
                    var repo = new RepositorioEventoProposta();
                    repo.ExcluirPropostas(evento.Id, Sessao.UsuarioId);
                    evento.TemOrcamento = false;
                    MessageBox.Show($"Orçamento cancelado para {evento.Nome}", "Cancelado",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var win = new NegociacaoWindow(evento.Id, evento.OrcamentoMaximo);
                    bool? ok = win.ShowDialog();
                    if (ok == true)
                        evento.TemOrcamento = true;
                }
                DataGridEventos.Items.Refresh();
            }
        }
    }
}

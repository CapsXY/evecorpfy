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
            // Aqui você ainda pode precisar de RepositorioTipoServico
            var repoServicos = new RepositorioTipoServico();
            var servicos = repoServicos.ListarTodos()
                                       .Where(s => servicosIds.Contains(s.Id))
                                       .ToList();
            DataGridServicos.ItemsSource = servicos;
        }
        private void VerOrcamentos_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is int servicoId)
            {
                var servico = ((List<TipoServico>)DataGridServicos.ItemsSource)
                                .FirstOrDefault(s => s.Id == servicoId);
                if (servico != null)
                {
                    var win = new OrcamentoWindow(servico, _evento);
                    win.Owner = this;
                    win.ShowDialog();
                }
            }
        }
    }
}

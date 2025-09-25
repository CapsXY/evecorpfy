using evecorpfy.Data;
using evecorpfy.Models;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy.ViewsOrganizador
{
    /// <summary>
    /// Interação lógica para GestaoEventos.xam
    /// </summary>
    public partial class GestaoEventos : UserControl
    {
        public GestaoEventos()
        {
            InitializeComponent();
            this.Loaded += (s, e) => CarregarEventosGrid();
        }
        private void CarregarEventosGrid()
        {
            var repoEvento = new RepositorioEvento();
            var eventos = repoEvento.ListarPorOrganizador(Sessao.UsuarioId);

            var repoParticipantes = new RepositorioEventoParticipante();

            foreach (var ev in eventos)
            {
                ev.QuantidadeParticipantes = repoParticipantes.ContarParticipantes(ev.Id);
            }

            DataGridEventos.ItemsSource = eventos;
        }

        private void Negociacao_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Evento evento)
            {
                var win = new NegociacaoWindow(evento);
                win.Owner = Window.GetWindow(this); // seta a janela atual como "pai"
                win.ShowDialog(); // abre como popup modal
            }
        }
        private void Participantes_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int eventoId)
            {
                var repo = new RepositorioEventoParticipante();
                var participantes = repo.ListarPorEvento(eventoId);

                if (participantes.Count == 0)
                {
                    MessageBox.Show("Nenhum participante encontrado para este evento.",
                                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Abre a nova janela
                    var win = new ParticipantesWindow(participantes);
                    win.ShowDialog();
                }
            }
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                MessageBox.Show($"Botão Cancelar clicado para o evento ID={btn.Tag}",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}

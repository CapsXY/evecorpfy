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
            var repo = new RepositorioEvento();
            DataGridEventos.ItemsSource = repo.ListarPorOrganizador(Sessao.UsuarioId);
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
            if (sender is Button btn)
            {
                MessageBox.Show($"Botão Participantes clicado para o evento ID={btn.Tag}",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
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

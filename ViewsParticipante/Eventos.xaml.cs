using evecorpfy.Data;
using evecorpfy.Models;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy.ViewsParticipante
{
    /// <summary>
    /// Interação lógica para Eventos.xam
    /// </summary>
    public partial class Eventos : UserControl
    {
        public Eventos()
        {
            InitializeComponent();
            CarregarEventosDisponiveis();
        }
        private void CarregarEventosDisponiveis()
        {
            var repo = new RepositorioEvento();
            DataGridEventos.ItemsSource = repo.ListarTodos();
        }
        private void Inscrever_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Evento evento)
            {
                var repo = new RepositorioEventoParticipante();
                if (repo.PossuiConflito(Sessao.UsuarioId, evento.DataInicio, evento.DataFim))
                {
                    MessageBox.Show("Você já está inscrito em um evento nesse período!",
                                    "Conflito de Datas", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    repo.Inscrever(evento.Id, Sessao.UsuarioId);
                    MessageBox.Show($"Inscrição confirmada no evento: {evento.Nome}",
                                    "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);

                    CarregarEventosDisponiveis(); // 🔥 Atualiza a grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Evento evento)
            {
                var repo = new RepositorioEventoParticipante();
                repo.Sair(evento.Id, Sessao.UsuarioId);

                MessageBox.Show($"Você saiu do evento: {evento.Nome}",
                                "Cancelamento", MessageBoxButton.OK, MessageBoxImage.Information);

                CarregarEventosDisponiveis(); // 🔥 Atualiza a grid
            }
        }
    }
}

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
        }
        private void CarregarEventos()
        {
            var repo = new RepositorioEvento();
            var eventos = repo.ListarTodos()
                              .Where(e => e.Status == "EM CADASTRAMENTO")
                              .ToList();
            DataGridEventos.ItemsSource = eventos;
        }
        private void Negociar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Evento evento)
            {
                var repoFornecedor = new RepositorioUsuarioFornecedor();
                var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);

                if (fornecedor == null)
                {
                    MessageBox.Show("Fornecedor não encontrado para este usuário.",
                                    "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var janela = new NegociacaoWindow(evento.Id, evento.OrcamentoMaximo, fornecedor.Id);
                bool? result = janela.ShowDialog();

                if (result == true)
                {
                    // Só marca como enviado se clicou em "Enviar Orçamento"
                    evento.TemOrcamento = true;
                }

                DataGridEventos.Items.Refresh();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Evento evento)
            {
                MessageBox.Show($"Cancelando proposta do evento {evento.Nome}");
                evento.TemOrcamento = false; // volta para negociar
                DataGridEventos.Items.Refresh();
            }
        }

    }
}
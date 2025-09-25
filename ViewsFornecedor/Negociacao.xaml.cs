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
            // 1) Descobre o fornecedor logado
            var repoFornecedor = new RepositorioUsuarioFornecedor();
            var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);
            if (fornecedor == null)
            {
                MessageBox.Show("Fornecedor não encontrado para este usuário.",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                DataGridEventos.ItemsSource = null;
                return;
            }

            // 2) Lista os eventos em cadastramento
            var repoEvento = new RepositorioEvento();
            var eventos = repoEvento.ListarTodos()
                                    .Where(e => e.Status == "EM CADASTRAMENTO")
                                    .ToList();

            // 3) Marca quais eventos já têm proposta deste fornecedor
            var repoPropostas = new RepositorioEventoProposta();
            var eventosComProposta = repoPropostas.ListarEventosComPropostaFornecedor(fornecedor.Id);

            foreach (var ev in eventos)
                ev.TemOrcamento = eventosComProposta.Contains(ev.Id); // <- precisa da prop TemOrcamento no Evento

            DataGridEventos.ItemsSource = eventos;
            DataGridEventos.Items.Refresh();
        }
        //private void CarregarEventos()
        //{
        //    var repo = new RepositorioEvento();
        //    var eventos = repo.ListarTodos()
        //                      .Where(e => e.Status == "EM CADASTRAMENTO")
        //                      .ToList();
        //    DataGridEventos.ItemsSource = eventos;
        //}
        //private void Negociar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is Evento evento)
        //    {
        //        var repoFornecedor = new RepositorioUsuarioFornecedor();
        //        var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);

        //        if (fornecedor == null)
        //        {
        //            MessageBox.Show("Fornecedor não encontrado para este usuário.",
        //                            "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        var janela = new NegociacaoWindow(evento.Id, evento.OrcamentoMaximo, fornecedor.Id);
        //        bool? result = janela.ShowDialog();

        //        if (result == true)
        //        {
        //            // Só marca como enviado se clicou em "Enviar Orçamento"
        //            evento.TemOrcamento = true;
        //        }

        //        DataGridEventos.Items.Refresh();
        //    }
        //}
        //private void Negociar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is Evento evento)
        //    {
        //        var repoFornecedor = new RepositorioUsuarioFornecedor();
        //        var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);

        //        if (fornecedor == null)
        //        {
        //            MessageBox.Show("Fornecedor não encontrado para este usuário.",
        //                            "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        var janela = new NegociacaoWindow(evento.Id, evento.OrcamentoMaximo, fornecedor.Id);
        //        bool? result = janela.ShowDialog();

        //if (result == true)
        //{
        //    // ✅ Checa no banco se existem propostas desse fornecedor para esse evento
        //    var repoProposta = new RepositorioEventoProposta();
        //    bool jaTemOrcamento = repoProposta.ExisteProposta(evento.Id, fornecedor.Id);

        //    evento.TemOrcamento = jaTemOrcamento;
        //}
        //        if (result == true)
        //        {
        //            var repoProposta = new RepositorioEventoProposta();
        //            evento.TemOrcamento = repoProposta.ExisteProposta(evento.Id, fornecedor.Id);
        //        }


        //        DataGridEventos.Items.Refresh();
        //    }
        //}


        //private void Cancelar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is Evento evento)
        //    {
        //        MessageBox.Show($"Cancelando proposta do evento {evento.Nome}");
        //        evento.TemOrcamento = false; // volta para negociar
        //        DataGridEventos.Items.Refresh();
        //    }
        //}
        //private void Cancelar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button btn && btn.Tag is Evento evento)
        //    {
        //        var repoFornecedor = new RepositorioUsuarioFornecedor();
        //        var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);

        //        if (fornecedor == null) return;

        //        var repoProposta = new RepositorioEventoProposta();
        //        repoProposta.ExcluirPropostas(evento.Id, fornecedor.Id);

        //        evento.TemOrcamento = false;
        //        DataGridEventos.Items.Refresh();

        //        MessageBox.Show($"Proposta cancelada para o evento {evento.Nome}.",
        //                        "Cancelamento", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

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

                var win = new NegociacaoWindow(evento.Id, evento.OrcamentoMaximo, fornecedor.Id);
                bool? ok = win.ShowDialog();

                // Só recarrega se realmente enviou (DialogResult = true no Enviar)
                if (ok == true) CarregarEventos();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Evento evento)
            {
                var repoFornecedor = new RepositorioUsuarioFornecedor();
                var fornecedor = repoFornecedor.ObterPorUsuarioId(Sessao.UsuarioId);
                if (fornecedor == null) return;

                var repoPropostas = new RepositorioEventoProposta();
                repoPropostas.ExcluirPropostas(evento.Id, fornecedor.Id);

                // Recarrega estado do grid após cancelar
                CarregarEventos();
            }
        }
    }
}
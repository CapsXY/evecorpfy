using evecorpfy.Data;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy.ViewsAdministrador
{
    /// <summary>
    /// Interação lógica para TipoEvento.xaml
    /// </summary>
    public partial class TipoEvento : UserControl
    {
        private readonly RepositorioTipoEvento repo = new RepositorioTipoEvento();
        private Models.TipoEvento eventoSelecionado;
        public TipoEvento()
        {
            InitializeComponent();
            CarregarTipos();
        }
        private void CarregarTipos()
        {
            ComboBoxPesquisarEvento.ItemsSource = repo.ListarTodos();
            ComboBoxPesquisarEvento.DisplayMemberPath = "Nome";
            ComboBoxPesquisarEvento.SelectedValuePath = "Id";
            ComboBoxPesquisarEvento.SelectedIndex = -1;
            TextBoxNomeEvento.Clear();
            eventoSelecionado = null;
        }
        private void ButtonCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxNomeEvento.Text))
            {
                MessageBox.Show("Digite o nome do tipo de evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxNomeEvento.Text.Trim().Length < 3)
            {
                MessageBox.Show("O nome do tipo de evento deve ter pelo menos 3 caracteres.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var nomeDigitado = TextBoxNomeEvento.Text.Trim();
            try
            {
                if (eventoSelecionado == null)
                {
                    if (repo.NomeExiste(nomeDigitado))
                    {
                        MessageBox.Show("Já existe um tipo de evento com esse nome!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var tipo = new Models.TipoEvento
                    {
                        Nome = nomeDigitado,
                        Ativo = true
                    };
                    repo.Inserir(tipo);
                    MessageBox.Show("Tipo de evento cadastrado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    eventoSelecionado.Nome = nomeDigitado;
                    repo.Atualizar(eventoSelecionado);
                    MessageBox.Show("Tipo de evento atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                CarregarTipos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (eventoSelecionado != null)
            {
                if (MessageBox.Show($"Deseja realmente inativar o evento '{eventoSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    eventoSelecionado.Ativo = false;
                    repo.Atualizar(eventoSelecionado);
                    MessageBox.Show("Evento inativado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarTipos();
                }
            }
            else
            {
                MessageBox.Show("Selecione um evento na lista para inativar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ComboBoxPesquisarEvento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxPesquisarEvento.SelectedItem is Models.TipoEvento selecionado)
            {
                eventoSelecionado = selecionado;
                TextBoxNomeEvento.Text = selecionado.Nome;
            }
        }
        private void ButtonReativar_Click(object sender, RoutedEventArgs e)
        {
            if (eventoSelecionado != null)
            {
                if (MessageBox.Show($"Deseja realmente reativar o evento '{eventoSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    eventoSelecionado.Ativo = true;
                    repo.Atualizar(eventoSelecionado);
                    MessageBox.Show("Evento reativado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarTipos();
                }
            }
            else
            {
                MessageBox.Show("Selecione um evento na lista para reativar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
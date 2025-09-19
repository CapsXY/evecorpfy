using evecorpfy.Data;
using System.Windows;
using System.Windows.Controls;

namespace evecorpfy.ViewsAdministrador
{
    /// <summary>
    /// Interação lógica para TipoServico.xaml
    /// </summary>
    public partial class TipoServico : UserControl
    {
        private readonly RepositorioTipoServico repo = new RepositorioTipoServico();
        private Models.TipoServico servicoSelecionado;

        public TipoServico()
        {
            InitializeComponent();
            CarregarTipos();
        }

        private void CarregarTipos()
        {
            ComboBoxPesquisarServico.ItemsSource = repo.ListarTodos();
            ComboBoxPesquisarServico.DisplayMemberPath = "Nome";
            ComboBoxPesquisarServico.SelectedValuePath = "Id";
            ComboBoxPesquisarServico.SelectedIndex = -1;

            TextBoxNomeServico.Clear();
            servicoSelecionado = null;
        }

        private void ButtonCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxNomeServico.Text))
            {
                MessageBox.Show("Digite o nome do tipo de serviço.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (TextBoxNomeServico.Text.Trim().Length < 3)
            {
                MessageBox.Show("O nome do tipo de serviço deve ter pelo menos 3 caracteres.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (servicoSelecionado == null)
            {
                // Inserir novo
                var tipo = new Models.TipoServico
                {
                    Nome = TextBoxNomeServico.Text.Trim(),
                    Ativo = true
                };

                repo.Inserir(tipo);
                MessageBox.Show("Tipo de serviço cadastrado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Atualizar existente
                servicoSelecionado.Nome = TextBoxNomeServico.Text.Trim();
                repo.Atualizar(servicoSelecionado);
                MessageBox.Show("Tipo de serviço atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            CarregarTipos();
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (servicoSelecionado != null)
            {
                if (MessageBox.Show($"Deseja realmente inativar o serviço '{servicoSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    servicoSelecionado.Ativo = false;
                    repo.Atualizar(servicoSelecionado);
                    MessageBox.Show("Serviço inativado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarTipos();
                }
            }
            else
            {
                MessageBox.Show("Selecione um serviço na lista para inativar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ComboBoxPesquisarServico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxPesquisarServico.SelectedItem is Models.TipoServico selecionado)
            {
                servicoSelecionado = selecionado;
                TextBoxNomeServico.Text = selecionado.Nome;
            }
        }

        private void ButtonReativar_Click(object sender, RoutedEventArgs e)
        {
            if (servicoSelecionado != null)
            {
                if (MessageBox.Show($"Deseja realmente reativar o serviço '{servicoSelecionado.Nome}'?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    servicoSelecionado.Ativo = true;
                    repo.Atualizar(servicoSelecionado);
                    MessageBox.Show("Serviço reativado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
                    CarregarTipos();
                }
            }
            else
            {
                MessageBox.Show("Selecione um serviço na lista para reativar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

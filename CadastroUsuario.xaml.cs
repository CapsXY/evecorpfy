using evecorpfy.Data;
using evecorpfy.Models;
using evecorpfy.Security;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy
{
    public partial class CadastroUsuario : Window
    {
        public CadastroUsuario()
        {
            InitializeComponent();
        }
        private async void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Password) ||
                comboTipoUsuario.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var novoUsuario = new Usuario
            {
                Username = txtUsuario.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                SenhaHash = PasswordHasher.Hash(txtSenha.Password),
                Role = (comboTipoUsuario.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Ativo = true,
                DataCriacao = DateTime.Now
            };
            try
            {
                await RepositorioUsuario.Inserir(novoUsuario);
                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao cadastrar usuário: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
using evecorpfy.Data;
using evecorpfy.Models;
using evecorpfy.Security;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
namespace evecorpfy.ViewsFornecedor
{
    /// <summary>
    /// Interação lógica para PerfilFornecedor.xaml
    /// </summary>
    public partial class PerfilFornecedor : UserControl
    {
        private Usuario? usuario;
        private UsuarioFornecedor? usuarioFornecedor;
        public PerfilFornecedor(int usuarioId)
        {
            InitializeComponent();
            CarregarUsuario(usuarioId);
        }
        private void CarregarUsuario(int id)
        {
            var repo = new RepositorioUsuario();
            usuario = repo.ObterUsuarioPorId(id);
            if (usuario != null)
            {
                TextBlockNomeUsuario.Text = usuario.Username;
                TextBoxNomeUsuario.Text = usuario.Username;
                TextBoxEmail.Text = usuario.Email;
                PasswordboxSenha.Password = "";
                LabelTipo.Content = usuario.Role;
                CheckboxHabilitado.IsChecked = usuario.Ativo;
                if (usuario.FotoPerfil != null)
                    ImagemPerfil.ImageSource = ByteArrayToImage(usuario.FotoPerfil);
                var repoFornecedor = new RepositorioUsuarioFornecedor();
                usuarioFornecedor = repoFornecedor.ObterPorUsuarioId(id);
                if (usuarioFornecedor != null)
                {
                    TextBoxCNPJ.Text = usuarioFornecedor.Cnpj;
                    TextBoxTelefone.Text = usuarioFornecedor.Telefone;
                }
            }
            else
            {
                MessageBox.Show("Usuário não encontrado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EllipseFoto_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Selecione uma foto de perfil",
                    Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (dialog.ShowDialog() == true)
                {
                    var img = new BitmapImage(new Uri(dialog.FileName));
                    ImagemPerfil.ImageSource = img;
                    usuario.FotoPerfil = File.ReadAllBytes(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private BitmapImage ByteArrayToImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                return img;
            }
        }
        private void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new RepositorioUsuario();
                usuario.Username = TextBoxNomeUsuario.Text.Trim();
                usuario.Email = TextBoxEmail.Text.Trim();
                usuario.Ativo = CheckboxHabilitado.IsChecked ?? false;
                if (!string.IsNullOrWhiteSpace(PasswordboxSenha.Password))
                    usuario.SenhaHash = PasswordHasher.Hash(PasswordboxSenha.Password.Trim());
                repo.AtualizarUsuario(usuario);
                string cnpj = TextBoxCNPJ.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}/\d{4}\-\d{2}$"))
                {
                    MessageBox.Show("CNPJ inválido! Use o formato 00.000.000/0000-00.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string telefone = TextBoxTelefone.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(telefone, @"^\(\d{2}\)\d{4,5}\-\d{4}$"))
                {
                    MessageBox.Show("Telefone inválido! Use o formato (00)00000-0000.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var repoFornecedor = new RepositorioUsuarioFornecedor();
                var fornecedor = new UsuarioFornecedor
                {
                    UsuarioId = usuario.Id,
                    Cnpj = cnpj,
                    Telefone = telefone
                };
                repoFornecedor.SalvarOuAtualizar(fornecedor);
                MessageBox.Show("Perfil atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar alterações: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TextBoxCNPJ_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void TextBoxCNPJ_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = TextBoxCNPJ.Text.Replace(".", "").Replace("/", "").Replace("-", "");
            if (txt.Length > 14) txt = txt.Substring(0, 14);
            if (txt.Length >= 12)
                TextBoxCNPJ.Text = $"{txt.Substring(0, 2)}.{txt.Substring(2, 3)}.{txt.Substring(5, 3)}/{txt.Substring(8, 4)}-{txt.Substring(12)}";
            else if (txt.Length >= 8)
                TextBoxCNPJ.Text = $"{txt.Substring(0, 2)}.{txt.Substring(2, 3)}.{txt.Substring(5, 3)}/{txt.Substring(8)}";
            else if (txt.Length >= 5)
                TextBoxCNPJ.Text = $"{txt.Substring(0, 2)}.{txt.Substring(2, 3)}.{txt.Substring(5)}";
            else if (txt.Length >= 2)
                TextBoxCNPJ.Text = $"{txt.Substring(0, 2)}.{txt.Substring(2)}";
            else
                TextBoxCNPJ.Text = txt;
            TextBoxCNPJ.CaretIndex = TextBoxCNPJ.Text.Length;
        }
        private void TextBoxTelefone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void TextBoxTelefone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = TextBoxTelefone.Text.Replace("(", "").Replace(")", "").Replace("-", "");
            if (txt.Length > 11) txt = txt.Substring(0, 11);
            if (txt.Length >= 7)
                TextBoxTelefone.Text = $"({txt.Substring(0, 2)}){txt.Substring(2, 5)}-{txt.Substring(7)}";
            else if (txt.Length >= 6)
                TextBoxTelefone.Text = $"({txt.Substring(0, 2)}){txt.Substring(2, 4)}-{txt.Substring(6)}";
            else if (txt.Length > 2)
                TextBoxTelefone.Text = $"({txt.Substring(0, 2)}){txt.Substring(2)}";
            else if (txt.Length > 0)
                TextBoxTelefone.Text = $"({txt}";
            TextBoxTelefone.CaretIndex = TextBoxTelefone.Text.Length;
        }
    }
}
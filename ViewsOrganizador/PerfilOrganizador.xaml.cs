using evecorpfy.Data;
using evecorpfy.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
namespace evecorpfy.ViewsOrganizador
{
    /// <summary>
    /// Interação lógica para PerfilOrganizador.xam
    /// </summary>
    public partial class PerfilOrganizador : UserControl
    {
        private Usuario? usuario;
        public PerfilOrganizador(int usuarioId)
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
                {
                    ImagemPerfil.ImageSource = ByteArrayToImage(usuario.FotoPerfil);
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
        private void ButtonConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new RepositorioUsuario();
                usuario.Username = TextBoxNomeUsuario.Text.Trim();
                usuario.Email = TextBoxEmail.Text.Trim();
                usuario.Ativo = CheckboxHabilitado.IsChecked ?? false;
                if (!string.IsNullOrWhiteSpace(PasswordboxSenha.Password))
                {
                    usuario.SenhaHash = PasswordboxSenha.Password.Trim();
                }
                repo.AtualizarUsuario(usuario);
                MessageBox.Show("Perfil atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar alterações: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
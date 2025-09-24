using evecorpfy.Data;
using evecorpfy.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
namespace evecorpfy.ViewsParticipante
{
    /// <summary>
    /// Interação lógica para PerfilParticipante.xaml
    /// </summary>
    public partial class PerfilParticipante : UserControl
    {
        private Usuario? usuario;
        private UsuarioParticipante? usuarioParticipante;
        public PerfilParticipante(int usuarioId)
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
                var repoTipo = new RepositorioUsuarioParticipante();
                var tipos = repoTipo.ListarTipos();
                ComboBoxCategoria.ItemsSource = tipos;
                ComboBoxCategoria.DisplayMemberPath = "Nome";
                ComboBoxCategoria.SelectedValuePath = "Id";
                usuarioParticipante = repoTipo.ObterPorUsuario(id);
                if (usuarioParticipante != null)
                {
                    ComboBoxCategoria.SelectedValue = usuarioParticipante.TipoParticipanteId;
                    TextBoxCPF.Text = usuarioParticipante.Cpf;
                    TextBoxTelefone.Text = usuarioParticipante.Telefone;
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
                {
                    usuario.SenhaHash = PasswordboxSenha.Password.Trim();
                }
                repo.AtualizarUsuario(usuario);
                string cpf = TextBoxCPF.Text.Trim();
                string telefone = TextBoxTelefone.Text.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$"))
                {
                    MessageBox.Show("CPF inválido! Use o formato 000.000.000-00.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(telefone, @"^\(\d{2}\)\d{4,5}\-\d{4}$"))
                {
                    MessageBox.Show("Telefone inválido! Use o formato (00)00000-0000.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (ComboBoxCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Selecione a categoria do participante.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var repoTipo = new RepositorioUsuarioParticipante();
                var participante = new UsuarioParticipante
                {
                    UsuarioId = usuario.Id,
                    TipoParticipanteId = (int)ComboBoxCategoria.SelectedValue,
                    Cpf = cpf,
                    Telefone = telefone
                };
                repoTipo.SalvarOuAtualizar(participante);
                MessageBox.Show("Perfil atualizado com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar alterações: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TextBoxCPF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }
        private void TextBoxCPF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = TextBoxCPF.Text.Replace(".", "").Replace("-", "");
            if (txt.Length > 11) txt = txt.Substring(0, 11);
            if (txt.Length >= 9)
                TextBoxCPF.Text = $"{txt.Substring(0, 3)}.{txt.Substring(3, 3)}.{txt.Substring(6, 3)}-{txt.Substring(9)}";
            else if (txt.Length >= 6)
                TextBoxCPF.Text = $"{txt.Substring(0, 3)}.{txt.Substring(3, 3)}.{txt.Substring(6)}";
            else if (txt.Length >= 3)
                TextBoxCPF.Text = $"{txt.Substring(0, 3)}.{txt.Substring(3)}";
            else
                TextBoxCPF.Text = txt;
            TextBoxCPF.CaretIndex = TextBoxCPF.Text.Length;
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
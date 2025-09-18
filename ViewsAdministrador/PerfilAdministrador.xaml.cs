using evecorpfy.Data;
using evecorpfy.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace evecorpfy.ViewsAdministrador
{
    /// <summary>
    /// Interação lógica para PerfilAdministrador.xaml
    /// </summary>
    public partial class PerfilAdministrador : UserControl
    {
        private Usuario usuario;

        // Construtor que recebe o ID do usuário logado
        public PerfilAdministrador(int usuarioId)
        {
            InitializeComponent();
            CarregarUsuario(usuarioId);
        }

        // Método para carregar os dados do usuário
        private void CarregarUsuario(int id)
        {
            var repo = new RepositorioUsuario();
            usuario = repo.ObterUsuarioPorId(id);

            if (usuario != null)
            {
                TextBlockNomeUsuario.Text = usuario.Username;
                TextBoxNomeUsuario.Text = usuario.Username;
                TextBoxEmail.Text = usuario.Email;
                PasswordboxSenha.Password = ""; // não mostramos a senha real
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

        // Evento: alterar foto de perfil
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
                    // Atualizar imagem visual
                    var img = new BitmapImage(new Uri(dialog.FileName));
                    ImagemPerfil.ImageSource = img;

                    // Salvar em memória para persistir depois
                    usuario.FotoPerfil = File.ReadAllBytes(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento: confirmar alterações
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
                    // Aqui o ideal é salvar como hash!
                }

                repo.AtualizarUsuario(usuario);

                MessageBox.Show("Perfil atualizado com sucesso!", "Confirmação",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar alterações: {ex.Message}", "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Converter byte[] → BitmapImage
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

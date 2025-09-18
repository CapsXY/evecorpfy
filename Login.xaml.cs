using System;
using System.Windows;
using evecorpfy.Data;
using evecorpfy.Models;

namespace evecorpfy
{
    /// <summary>
    /// Interação lógica para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonAcessar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = TextBoxUsuario.Text.Trim();
            string senha = PasswordboxSenha.Password.Trim();

            // 🔹 Validação básica
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha usuário e senha!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var repo = new RepositorioUsuario();
                var user = repo.Autenticar(usuario, senha);

                if (user != null)
                {
                    if (!user.Ativo)
                    {
                        MessageBox.Show("Usuário está desabilitado.", "Acesso negado", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // 🔹 Salvar dados na sessão
                    Sessao.UsuarioId = user.Id;
                    Sessao.Username = user.Username;
                    Sessao.Role = user.Role;

                    // 🔹 Mensagem opcional de boas-vindas
                    MessageBox.Show($"Bem-vindo, {user.Username}!", "Login realizado", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 🔹 Abrir menu principal
                    var menu = new MenuAdministrador();
                    menu.Show();

                    // 🔹 Fechar tela de login
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar banco: {ex.Message}", "Erro crítico", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

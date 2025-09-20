using System;
using System.Windows;
using evecorpfy.Data.Repositorios;
using evecorpfy.Utils;

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
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha usuário e senha!", "Aviso",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        MessageBox.Show("Usuário está desabilitado.", "Acesso negado",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // guarda sessão
                    Sessao.UsuarioId = user.Id;
                    Sessao.Username = user.Username;
                    Sessao.Role = user.Role;
                    MessageBox.Show($"Bem-vindo, {user.Username}!", "Login realizado", MessageBoxButton.OK, MessageBoxImage.Information);

                    // decide qual menu abrir de acordo com a ROLE
                    Window menu = null;
                    switch (user.Role.ToLower())
                    {
                        case "administrador":
                            menu = new MenuAdministrador();
                            break;
                        case "fornecedor":
                            menu = new MenuFornecedor();
                            break;
                        case "organizador":
                            menu = new MenuOrganizador();
                            break;
                        case "participante":
                            menu = new MenuParticipante();
                            break;
                        default:
                            MessageBox.Show("Perfil de usuário não reconhecido!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }
                    menu.Show();
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

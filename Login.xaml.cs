using System.Windows;
using evecorpfy.Data;
namespace evecorpfy
{
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
                var repo = new RepositorioAutenticacao();
                var user = repo.Autenticar(usuario, senha);
                if (user != null)
                {
                    if (!user.Ativo)
                    {
                        MessageBox.Show("Usuário está desabilitado.", "Acesso negado",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Sessao.UsuarioId = user.Id;
                    Sessao.Username = user.Username;
                    Sessao.Role = user.Role;
                    Sessao.FornecedorId = null;
                    MessageBox.Show($"Bem-vindo, {user.Username}!",
                                    "Login realizado",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                    Window menu = null;
                    switch (user.Role.ToLower())
                    {
                        case "administrador":
                            menu = new MenuAdministrador();
                            break;
                        case "fornecedor":
                            var repoFornecedor = new RepositorioUsuarioFornecedor();
                            var fornecedor = repoFornecedor.ObterPorUsuarioId(user.Id);
                            if (fornecedor != null)
                                Sessao.FornecedorId = fornecedor.Id;
                            menu = new MenuFornecedor();
                            break;
                        case "organizador":
                            menu = new MenuOrganizador();
                            break;
                        case "participante":
                            menu = new MenuParticipante();
                            break;
                        default:
                            MessageBox.Show("Perfil de usuário não reconhecido!",
                                            "Erro",
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                            return;
                    }
                    menu.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha inválidos!",
                                    "Erro",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao acessar banco: {ex.Message}",
                                "Erro crítico",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        private void BtnCadastrese_Click(object sender, RoutedEventArgs e)
        {
            var cadastro = new CadastroUsuario();
            bool? resultado = cadastro.ShowDialog();
            if (resultado == true)
            {
                MessageBox.Show("Usuário cadastrado com sucesso! Agora você já pode fazer login.",
                                "Sucesso",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
    }
}

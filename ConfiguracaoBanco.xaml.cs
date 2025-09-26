using System.Data.SqlClient;
using System.Windows;
namespace evecorpfy.Views
{
    public partial class ConfiguracaoBanco : Window
    {
        public ConfiguracaoBanco()
        {
            InitializeComponent();
            var config = ConfigManager.Carregar();
            if (config != null)
            {
                txtServidor.Text = config.Servidor;
                txtBanco.Text = config.Banco;
                txtUsuario.Text = config.Usuario;
                txtSenha.Password = config.Senha;
            }
            else
            {
                txtServidor.Text = @".\";
                txtBanco.Text = "EVECORPFY";
            }
        }
        private void BtnTestar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connStr = $"Server={txtServidor.Text};Database={txtBanco.Text};User Id={txtUsuario.Text};Password={txtSenha.Password};TrustServerCertificate=True;";
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Conexão bem-sucedida!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var config = new ConfiguracaoConexao
            {
                Servidor = txtServidor.Text,
                Banco = txtBanco.Text,
                Usuario = txtUsuario.Text,
                Senha = txtSenha.Password
            };

            ConfigManager.Salvar(config);
            MessageBox.Show("Configuração salva com sucesso!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
    }
}
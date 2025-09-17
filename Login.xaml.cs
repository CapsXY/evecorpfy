using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonAcessar_Click(object sender, RoutedEventArgs e)
        {
            {
                string usuario = TextBoxUsuario.Text;
                string senha = PasswordBoxSenha.Password;

                // Exemplo de validação simples
                if (usuario == "admin" && senha == "123")
                {
                    MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Aqui você pode abrir a tela principal do sistema:
                    MenuAdministrador main = new MenuAdministrador();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha inválidos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

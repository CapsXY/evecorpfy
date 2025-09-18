using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using evecorpfy.ViewsAdministrador;

namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para MenuAdministrador.xaml
    /// </summary>
    public partial class MenuAdministrador : Window
    {
        public MenuAdministrador()
        {
            InitializeComponent();

            // Tela inicial ao abrir
            ContentArea.Content = new DashboardAdministrador();
        }

        private void GridLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Volta sempre para o dashboard
            ContentArea.Content = new DashboardAdministrador();
        }

        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Carrega o perfil do usuário logado
            ContentArea.Content = new PerfilAdministrador(Sessao.UsuarioId);
        }

        private void GridButtonTipoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new TipoEvento();
        }

        private void GridButtonTipoServico_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new TipoServico();
        }

        private void GridButtonGestaoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new GestaoEventos();
        }

        // 🔹 Métodos auxiliares para hover (evitam repetição de código)
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = Brushes.Transparent;
        }
    }
}

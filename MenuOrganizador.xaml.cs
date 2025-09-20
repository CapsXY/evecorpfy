using evecorpfy.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para MenuOrganizador.xaml
    /// </summary>
    public partial class MenuOrganizador : Window
    {
        // Efeito hover nos botões do menu
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
        private void EfeitoTrocaTela(UserControl novoUC)
        {
            novoUC.RenderTransform = new TranslateTransform();
            novoUC.Opacity = 0;
            ContentArea.Content = novoUC;
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            var slide = new DoubleAnimation(100, 0, TimeSpan.FromMilliseconds(300))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            novoUC.BeginAnimation(OpacityProperty, fade);
            novoUC.RenderTransform.BeginAnimation(TranslateTransform.XProperty, slide);
        }
        public MenuOrganizador()
        {
            InitializeComponent();
            //EfeitoTrocaTela(new DashboardOrganizador());
        }
        private void GridLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //EfeitoTrocaTela(new DashboardOrganizador());
        }
        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //EfeitoTrocaTela(new PerfilOrganizador(Sessao.UsuarioId));
        }
        private void GridButtonTipoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // EfeitoTrocaTela(new TipoEvento());
        }
        private void GridButtonTipoServico_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //EfeitoTrocaTela(new TipoServico());
        }
        private void GridButtonGestaoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //EfeitoTrocaTela(new GestaoEventos());
        }
        private void GridButtonSair_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}

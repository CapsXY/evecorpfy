using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using evecorpfy.ViewsAdministrador;
namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para MenuAdministrador.xaml
    /// </summary>
    public partial class MenuAdministrador : Window
    {
        // Efeito hover nos botões do menu ao entrar com o mouse
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }
        // Efeito hover nos botões do menu ao sair com o mouse
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = Brushes.Transparent;
        }
        // Animação de transição entre telas
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
        // Construtor do MenuAdministrador
        public MenuAdministrador()
        {
            InitializeComponent();
            EfeitoTrocaTela(new TelaPrincipal());
        }
        // Navegação entre telas ao clicar na logo do menu
        private void GridLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new TelaPrincipal());
        }
        // Navegação entre telas ao clicar no botão de Perfil do menu
        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new PerfilAdministrador(Sessao.UsuarioId));
        }
        // Navegação entre telas ao clicar no botão de Tipo de Eventos do menu
        private void GridButtonTipoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new TipoEvento());
        }
        // Navegação entre telas ao clicar no botão de Tipo de Serviços do menu
        private void GridButtonTipoServico_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new TipoServico());
        }
        // Navegação entre telas ao clicar no botão de Gestão de Eventos do menu
        private void GridButtonGestaoEventos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new GestaoEventos());
        }
        // Navegação entre telas ao clicar no botão de Sair do menu
        private void GridButtonSair_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var login = new Login();
            login.Show();
            this.Close();
        }
    }
}

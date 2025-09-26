using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using evecorpfy.ViewsOrganizador;
namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para MenuOrganizador.xaml
    /// </summary>
    public partial class MenuOrganizador : Window
    {
        // Efeito hover nos botões do menu ao entrar com o mouse
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECBB80"));
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
        // Construtor do MenuOrganizador
        public MenuOrganizador()
        {
            InitializeComponent();
            EfeitoTrocaTela(new DashboardOrganizador());
        }
        // Navegação entre telas ao clicar na logo do menu
        private void GridLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new DashboardOrganizador());
        }
        // Navegação entre telas ao clicar no botão de Perfil do menu
        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new PerfilOrganizador(Sessao.UsuarioId));
        }
        // Navegação entre telas ao clicar no botão de Cadastro de Eventos do menu
        private void GridButtonCadastroEventos_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new CadastroEventos());
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
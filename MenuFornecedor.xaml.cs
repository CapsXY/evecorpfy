using evecorpfy.ViewsFornecedor;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace evecorpfy
{
    /// <summary>
    /// Lógica interna para MenuFornecedor.xaml
    /// </summary>
    public partial class MenuFornecedor : Window
    {
        // Efeito hover nos botões do menu ao entrar com o mouse
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is System.Windows.Controls.Grid grid)
                grid.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF48F274"));
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
        public MenuFornecedor()
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
            EfeitoTrocaTela(new PerfilFornecedor(Sessao.UsuarioId));
        }
        // Navegação entre telas ao clicar no botão de Negociação do menu
        private void GridButtonNegociacao_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EfeitoTrocaTela(new Negociacao());
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
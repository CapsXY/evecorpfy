using evecorpfy.Data;
using evecorpfy.ViewsAdministrador;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        }
        private void GridLogo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is MenuAdministrador mainWindow)
            {
                mainWindow.ContentArea.Content = null;
            }
        }
        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new PerfilAdministrador();
        }
        private void GridButtonPerfil_MouseEnter(object sender, MouseEventArgs e)
        {
            GridButtonPerfil.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }
        private void GridButtonPerfil_MouseLeave(object sender, MouseEventArgs e)
        {
            GridButtonPerfil.Background = Brushes.Transparent;
        }
        private void GridButtonTipoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new TipoEvento();
        }
        private void GridButtonTipoEvento_MouseEnter(object sender, MouseEventArgs e)
        {
            GridButtonTipoEvento.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }
        private void GridButtonTipoEvento_MouseLeave(object sender, MouseEventArgs e)
        {
            GridButtonTipoEvento.Background = Brushes.Transparent;
        }
        private void GridButtonTipoServico_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new TipoServico();
        }
        private void GridButtonTipoServico_MouseEnter(object sender, MouseEventArgs e)
        {
            GridButtonTipoServico.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }
        private void GridButtonTipoServico_MouseLeave(object sender, MouseEventArgs e)
        {
            GridButtonTipoServico.Background = Brushes.Transparent;
        }
        private void GridButtonGestaoEvento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContentArea.Content = new GestaoEventos();
        }
        private void GridButtonGestaoEvento_MouseEnter(object sender, MouseEventArgs e)
        {
            GridButtonGestaoEvento.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }
        private void GridButtonGestaoEvento_MouseLeave(object sender, MouseEventArgs e)
        {
            GridButtonGestaoEvento.Background = Brushes.Transparent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = DbConnectionFactory.GetOpenConnection())
                {
                    MessageBox.Show("✅ Conexão bem-sucedida com o banco de dados!",
                                    "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao conectar: {ex.Message}",
                                "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

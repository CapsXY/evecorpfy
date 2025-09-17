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
        private void GridButtonPerfil_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Sua lógica de clique aqui, por exemplo:
            MessageBox.Show("Perfil do usuário clicado!");
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
            // Sua lógica de clique aqui, por exemplo:
            MessageBox.Show("Perfil do usuário clicado!");
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
            // Sua lógica de clique aqui, por exemplo:
            MessageBox.Show("Perfil do usuário clicado!");
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
            // Sua lógica de clique aqui, por exemplo:
            MessageBox.Show("Perfil do usuário clicado!");
        }

        private void GridButtonGestaoEvento_MouseEnter(object sender, MouseEventArgs e)
        {
            GridButtonGestaoEvento.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#91D8FF"));
        }

        private void GridButtonGestaoEvento_MouseLeave(object sender, MouseEventArgs e)
        {
            GridButtonGestaoEvento.Background = Brushes.Transparent;
        }
    }
}

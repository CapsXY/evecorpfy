using evecorpfy.Data;
using System.Windows;
using System.Windows.Controls;
namespace evecorpfy.ViewsAdministrador
{
    /// <summary>
    /// Interação lógica para DashboardAdministrador.xam
    /// </summary>
    public partial class DashboardAdministrador : UserControl
    {
        public DashboardAdministrador()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = DbConnectionFactory.GetOpenConnection())
                {
                    MessageBox.Show("✅ Conexão bem-sucedida com o banco de dados!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Erro ao conectar: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

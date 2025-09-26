using System.Windows;
using evecorpfy.Views;
namespace evecorpfy
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var config = ConfigManager.Carregar();
            if (config == null)
            {
                var janelaConfig = new ConfiguracaoBanco();
                if (janelaConfig.ShowDialog() != true)
                {
                    Shutdown();
                    return;
                }
            }
            var login = new Login();
            login.Show();
        }
    }
}
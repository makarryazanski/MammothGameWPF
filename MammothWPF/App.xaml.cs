using System.Windows;
using MammothWPF.Views;

namespace MammothWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindow()
            };
            mainWindow.Show();
        }
    }
}

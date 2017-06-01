using System.Windows;

namespace Automation.UI.Shell.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new NewBootStrapper();
            bootstrapper.Run();
        }
    }
}
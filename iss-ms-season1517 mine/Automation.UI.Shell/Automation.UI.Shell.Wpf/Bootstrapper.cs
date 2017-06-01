using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Prism.Mef;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Automation.UI.Shell.Wpf
{
    public class Bootstrapper : MefBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            //register WPF project to get Shell view
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog((typeof(Bootstrapper)).Assembly));

            //register infrastructure project to get TestRunnerControl
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(TestRunnerControl).Assembly));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            string modulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            return new DirectoryModuleCatalog() { ModulePath = modulePath };
        }
    }
}
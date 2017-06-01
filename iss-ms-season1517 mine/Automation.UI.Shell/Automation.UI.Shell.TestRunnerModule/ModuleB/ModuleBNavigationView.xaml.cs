using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Microsoft.Practices.Unity;
using Prism.Regions;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Automation.UI.Shell.TestRunnerModule.ModuleB
{
    /// <summary>
    /// Interaction logic for ModuleBNavigationView.xaml
    /// </summary>
    public partial class ModuleBNavigationView : UserControl
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private static Uri MainModuleBViewUri = new Uri(ModuleBView.ViewName, UriKind.Relative);

        public ModuleBNavigationView(IRegionManager regionManager, IUnityContainer container)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.container = container;
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            //resolve the viewModel so that it can link datacontext to View
            var viewModel = this.container.Resolve<ModuleBViewModel>();
            this.regionManager.RequestNavigate(RegionNames.ContentRegion, MainModuleBViewUri);
            e.Handled = false;
        }
    }
}
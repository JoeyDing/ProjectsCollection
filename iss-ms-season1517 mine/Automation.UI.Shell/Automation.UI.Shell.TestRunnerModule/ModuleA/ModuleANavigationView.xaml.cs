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

namespace Automation.UI.Shell.TestRunnerModule.ModuleA
{
    /// <summary>
    /// Interaction logic for ModuleANavigationView.xaml
    /// </summary>
    public partial class ModuleANavigationView : UserControl
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private static Uri MainModuleAViewUri = new Uri(ModuleAView.ViewName, UriKind.Relative);

        public ModuleANavigationView(IRegionManager regionManager, IUnityContainer container)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.container = container;
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            //resolve the viewModel so that it can link datacontext to View
            var viewModel = this.container.Resolve<ModuleAViewModel>();
            this.regionManager.RequestNavigate(RegionNames.ContentRegion, MainModuleAViewUri);
        }
    }
}
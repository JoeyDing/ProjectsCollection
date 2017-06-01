using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Microsoft.Practices.Unity;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Sfb.UI.Shell.Module
{
    /// <summary>
    /// Interaction logic for SfBAutomationNavigationView.xaml
    /// </summary>
    public partial class SfBAutomationNavigationView : UserControl
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private static Uri SfBContentViewUri = new Uri("SfbAutomationContentView", UriKind.Relative);

        public SfBAutomationNavigationView(IRegionManager regionManager, IUnityContainer container)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.container = container;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.container.Resolve<SfbAutomationContentViewModel>();
            this.regionManager.RequestNavigate(RegionNames.ContentRegion, SfBContentViewUri);
            e.Handled = false;
        }
    }
}
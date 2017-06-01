using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkypeIntlPortfolio.Silverlight.Views
{
    public partial class FabricOnboardingsView : Page
    {
        public FabricOnboardingsView()
        {
            InitializeComponent();
            this.DataContext = new FabricOnboardingsViewModel();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
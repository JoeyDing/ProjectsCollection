using Automation.UI.Shell.Wpf.Infrastructure.ConfigPopUp;
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
    /// Interaction logic for ModuleBView.xaml
    /// </summary>
    public partial class ModuleBView : UserControl
    {
        public const string ViewName = "ModuleBView";

        public ModuleBView()
        {
            InitializeComponent();
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "emailConfigReflection.xml");
            ConfigPopUpControl popup = new ConfigPopUpControl(typeof(Wpf.Infrastructure.EmailSettingReflection), fileName);
            popup.Show();
            popup.Height = 500;
            popup.Width = 500;

            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
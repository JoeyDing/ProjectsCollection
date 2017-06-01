using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
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
    /// Interaction logic for TestModuleView.xaml
    /// </summary>
    public partial class ModuleAView : UserControl, IModuleAView
    {
        public const string ViewName = "ModuleAView";

        public ModuleAView()
        {
            InitializeComponent();
        }

        public TestRunnerControl RunnerControl
        {
            get
            {
                return this.testRunnerControl;
            }
        }
    }
}
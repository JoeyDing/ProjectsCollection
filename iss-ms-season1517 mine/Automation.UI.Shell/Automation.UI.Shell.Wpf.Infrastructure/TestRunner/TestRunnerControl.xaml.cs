using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public partial class TestRunnerControl
    {
        public TestRunnerControl()
        {
            InitializeComponent();
            this.AdditionalContentDataContext = "";
        }

        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(TestRunnerControl),
              new PropertyMetadata(null));

        public object AdditionalContentDataContext
        {
            get { return (object)GetValue(AdditionalContentDataContextProperty); }
            set
            {
                SetValue(AdditionalContentDataContextProperty, value);
            }
        }

        public static readonly DependencyProperty AdditionalContentDataContextProperty =
            DependencyProperty.Register("AdditionalContentDataContext", typeof(object), typeof(TestRunnerControl),
              new PropertyMetadata(null));

        public RadGridView GridView
        {
            get
            {
                return this.grid_History;
            }
        }

        private void log_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.log.ScrollToEnd();
        }
    }
}
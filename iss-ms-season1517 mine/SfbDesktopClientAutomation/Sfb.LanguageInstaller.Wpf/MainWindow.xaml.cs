using Sfb.LanguageInstaller.Wpf.Interface;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Data;

namespace Sfb.LanguageInstaller.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICanExpand
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.ExpandService = this;
        }

        public void ExpandHistory()
        {
            this.grid_History.Dispatcher.Invoke(() =>
            {
                var groups = this.grid_History.Items.Groups;
                var groupToExpand = groups.Cast<IGroup>().FirstOrDefault();
                if (groupToExpand != null)
                {
                    //expand the group
                    this.grid_History.ExpandGroup(groupToExpand);

                    //scroll to last item in the group
                    this.grid_History.ScrollIntoView(groupToExpand.Items.Cast<object>().Last());
                }
            });
        }
    }
}
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
using VsoApi.Report.Wpf.Services;

namespace VsoApi.Report.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            var findLinksService = new FindLinkService();
            var findEsService = new FindEnablingSpecificationService();
            var findTaskService = new FindTaskService(findLinksService, findEsService);
            this.DataContext = this.viewModel = new MainWindowViewModel(findTaskService);
        }

        private void radbutton_export_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ExportToExcel(this.radgridview_result);
        }

        private void radbutton_search_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.Search(this.textbox_esIDs.Text);
        }
    }
}
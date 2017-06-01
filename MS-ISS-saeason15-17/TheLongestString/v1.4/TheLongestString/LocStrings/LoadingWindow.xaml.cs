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
using System.Windows.Shapes;

namespace TheLongestString
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private string _lclFolderLocation = null;
        private string _fileSearchPattern = null;

        public LoadingWindow()
        {
            InitializeComponent();

            _lclFolderLocation = Properties.Settings.Default.LclFolder;
            _fileSearchPattern = Properties.Settings.Default.FileSearchPattern;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var renderFontName = Properties.Settings.Default.FontToRender;
            var renderFontSize = Properties.Settings.Default.FontSizeToRender;

            var t = LocalizationsViewModel.CreateAsync(renderFontName, renderFontSize, _lclFolderLocation, _fileSearchPattern);
            t.Start();
            await t;

            var viewModel = t.Result;
            var mainWindow = new MainWindow(viewModel);
            mainWindow.Show();
            this.Close();
        }
    }
}

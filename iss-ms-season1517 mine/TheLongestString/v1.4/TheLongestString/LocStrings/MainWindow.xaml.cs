using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace TheLongestString
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LocalizationsViewModel _viewModel;
        private bool saving = false;
        private SourceCultureWindow _sourceCultureWindow = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(LocalizationsViewModel viewModel)
            : this()
        {
            _viewModel = viewModel;

            this.DataContext = _viewModel;
        }

        void collectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CollectionChanged " + e.Action);

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetStatusBar();
        }

        private void SourceDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_sourceCultureWindow == null || !_sourceCultureWindow.IsVisible)
            {
                _sourceCultureWindow = new SourceCultureWindow(_viewModel);
            }

            _sourceCultureWindow.Show();
            _sourceCultureWindow.Activate();
        }

        private void ExportToCSV(System.Windows.Threading.Dispatcher uiDispatcher, string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("Id,en-US string,Longest culture(s)");
                
                for (int i = 0; i < _viewModel.Ids.Count(); ++i)
                {
                    uiDispatcher.BeginInvoke(
                        new Action<string>(SetStatusBar), String.Format("Scanning ID: {0} of {1}...", i + 1, _viewModel.Ids.Count())
                        );

                    _viewModel.SelectedLocIdIndex = i;
                    
                    var longestCultures =
                        from l in _viewModel.Localizations
                        where l.IsMaxWidth
                        select l.Dictionary.CultureName;

                    var longestCulturesString = String.Join("|", longestCultures);

                    writer.WriteLine(
                        "{0},\"{1}\",{2}",
                        _viewModel.Ids.ElementAt(i),
                        _viewModel.SourceDictionary.Strings[_viewModel.Ids.ElementAt(i)],
                        longestCulturesString
                        );
                }
            }

            uiDispatcher.BeginInvoke(
                new Action<string>(SetStatusBar), String.Format("Saved CSV to {0}.", filePath)
                );
            
            System.Threading.Thread.Sleep(3000);

            uiDispatcher.BeginInvoke(
                new Action<string>(SetStatusBar), String.Empty
                );
        }

        private void SetStatusBar(string text = null)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = String.Format("Loaded files from: {0}.", _viewModel.FolderPath);
            }

            try
            {
                StatusBar.Text = text;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private void SaveLongestStringsButton_Click(object sender, RoutedEventArgs e)
        {
            saving = true;

            var saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.FileName = "Longest Strings";
            saveFile.DefaultExt = ".csv";
            saveFile.Filter = "Comma-separated file (.csv)|*.csv";
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var success = saveFile.ShowDialog();

            if (success != null && success == true)
            {
                var csvTask = new Task(() => ExportToCSV(Dispatcher, saveFile.FileName));
                csvTask.Start();
                csvTask.ContinueWith((precedent) => 
                {
                    saving = false;
                });
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!saving && e.AddedItems != null && e.AddedItems.Count > 0 )
            {
                var item = e.AddedItems[0];
                ids_lb.ScrollIntoView(item);
            }
            
        }
    }
}

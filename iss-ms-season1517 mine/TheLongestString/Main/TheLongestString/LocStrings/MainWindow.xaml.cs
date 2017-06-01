using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using TheLongestString.Model;

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
            this._viewModel.cultureFonts = CultureFontUtil.LoadCultureFontsFromConfig("font.config");
        }

        private void collectionView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CollectionChanged " + e.Action);

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetStatusBar();
            this.cbFontFamily.SelectedValue = _viewModel.SelectedFontName;
            int fontSize = (int)_viewModel.RenderFontSize;
            for (int i = 0; i < this.cbFontSize.Items.Count; i++)
            {
                int ft = int.Parse(this.cbFontSize.Items[i].ToString());
                if (fontSize == ft)
                {
                    this.cbFontSize.SelectedIndex = i;
                }
            }
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

        private async void ExportToCSV(System.Windows.Threading.Dispatcher uiDispatcher, string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Id,en-US string,Longest culture, Translation, Width, Number of Characters");
                for (int i = 0; i < _viewModel.Ids.Count(); ++i)
                {
                    uiDispatcher.BeginInvoke(
                        new Action<string>(SetStatusBar), String.Format("Scanning ID: {0} of {1}...", i + 1, _viewModel.Ids.Count())
                        );

                    await _viewModel.SetSelectedIndex(i);
                    var x = _viewModel.Localizations.Items.ToList();
                    var longestCultures =
                    from l in x
                    where l.IsMaxWidth
                    select new { l.Dictionary.CultureName, l.String, l.Width, l.CharacterCount };
                    foreach (var item in longestCultures)
                    {
                        writer.WriteLine(
                        "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"",
                        _viewModel.Ids.ElementAt(i),
                        //print the corresponding translated string
                        _viewModel.SourceDictionary == null ? "" : _viewModel.SourceDictionary.Strings[_viewModel.Ids.ElementAt(i)].String,
                        item.CultureName,
                        item.String,
                        item.Width,
                        item.CharacterCount
                        );
                    }
                    writer.WriteLine();
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
            if (!saving && e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0];
                ids_lb.ScrollIntoView(item);

                CultureFontUtil.SaveFontConfigureToFile("font.config", this._viewModel.cultureFonts);
            }
        }

        private void BindingFontInformation()
        {
            if (this._viewModel.Localizations == null) return;
            foreach (var vm in this._viewModel.Localizations.Items)
            {
                if (vm.IsSelected == true)
                {
                    vm.FontFamilyName = this.cbFontFamily.SelectedValue.ToString();
                    vm.FontFamily = new System.Windows.Media.FontFamily(vm.FontFamilyName);
                    vm.FontSize = double.Parse(this.cbFontSize.SelectedValue.ToString());
                    Typeface tf = new Typeface(vm.FontFamilyName);
                    vm.Width = LocString.CalculateTextWidth(vm, tf, vm.FontSize.Value);
                    vm.Height = LocString.CalculateTextHeight(vm, tf, vm.FontSize.Value);

                    this._viewModel.cultureFonts = CultureFontUtil.UpdateCultureFontsFromLocString(this._viewModel.cultureFonts, vm);
                }
            }
            this._viewModel.RefreshLocalizations();
            CultureFontUtil.SaveFontConfigureToFile("font.config", this._viewModel.cultureFonts);
        }

        private void cbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingFontInformation();
        }

        private void cbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingFontInformation();
        }

        private void cbxAll_Checked(object sender, RoutedEventArgs e)
        {
            if (this._viewModel.Localizations == null) return;
            foreach (var vm in this._viewModel.Localizations.Items)
            {
                vm.IsSelected = true;
            }
        }

        private void cbxAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this._viewModel.Localizations == null) return;
            foreach (var vm in this._viewModel.Localizations.Items)
            {
                vm.IsSelected = false;
            }
        }

        private void DataGrid_Selected(object sender, RoutedEventArgs e)
        {
            if (this._viewModel.Localizations == null) return;
            foreach (var vm in this._viewModel.Localizations.Items)
            {
                vm.IsSelected = false;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selRows = this.DataGrid.SelectedItems;
            foreach (var row in selRows)
            {
                LocString vm = (LocString)row;
                vm.IsSelected = true;
            }
        }

        private void MenuItem_Save_As_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog().Value == true)
            {
                string fileName = dialog.FileName;
                CultureFontUtil.SaveFontConfigureToFile(fileName, this._viewModel.cultureFonts);
            }
        }

        private void MenuItem_Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filename = dialog.FileName;
                this._viewModel.cultureFonts = CultureFontUtil.LoadCultureFontsFromConfig(filename);
                if (this._viewModel.Localizations != null)
                {
                    CultureFontUtil.UpdateLocStringCollectionFromCultureFont(ref this._viewModel.cultureFonts, this._viewModel.Localizations, this._viewModel.defaultFontName, this._viewModel.defaultFontSize);
                }
            }
        }

        private void CommandBinding_SaveAs(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog().Value == true)
            {
                string fileName = dialog.FileName;
                CultureFontUtil.SaveFontConfigureToFile(fileName, this._viewModel.cultureFonts);
            }
        }

        private void ColumnItem_Checked(object sender, RoutedEventArgs e)
        {
            if (DataGrid != null)
            {
                string selectedItemText = ((MenuItem)sender).Header.ToString();
                foreach (DataGridColumn column in DataGrid.Columns)
                {
                    if (column.Header != null && column.Header.ToString() == selectedItemText)
                    {
                        column.Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
        }

        private void ColumnItem_Unchecked(object sender, RoutedEventArgs e)
        {
            string selectedItemText = ((MenuItem)sender).Header.ToString();
            foreach (DataGridColumn column in DataGrid.Columns)
            {
                if (column.Header != null && column.Header.ToString() == selectedItemText)
                {
                    column.Visibility = Visibility.Hidden;
                    break;
                }
            }
        }
    }
}
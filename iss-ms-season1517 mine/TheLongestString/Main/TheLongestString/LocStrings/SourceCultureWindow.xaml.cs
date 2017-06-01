using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SourceCultureWindow.xaml
    /// </summary>
    public partial class SourceCultureWindow : Window
    {
        private LocalizationsViewModel viewModel;

        public SourceCultureWindow()
        {
            InitializeComponent();
        }

        public SourceCultureWindow(LocalizationsViewModel viewModel)
            : this()
        {
            this.DataContext = this.viewModel = viewModel;

            if (viewModel.SourceDictionary != null)
                this.Title = String.Format("{0} Strings", viewModel.SourceDictionary.CultureName);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilterTextBox.Focus();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;

            if (!String.IsNullOrEmpty(FilterTextBox.Text))
            {
                var needle = FilterTextBox.Text;

                if (e.Item is KeyValuePair<string, LocString>)
                {
                    var pair = (KeyValuePair<string, LocString>)e.Item;

                    if (!String.IsNullOrEmpty(pair.Value.String))
                    {
                        if (pair.Value.String.IndexOf(needle, StringComparison.InvariantCultureIgnoreCase) < 0 &&
                            pair.Key.IndexOf(needle, StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            e.Accepted = false;
                        }
                    }
                }
            }
        }

        private void FilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var s = this.Resources["SourceStrings"] as CollectionViewSource;

            if (s.View != null)
                s.View.Refresh();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow)
            {
                var row = sender as DataGridRow;
                var data = (KeyValuePair<string, LocString>)row.DataContext;
                var index = this.viewModel.Ids.ToList().IndexOf(data.Key);
                this.viewModel.SelectedLocIdIndex = index;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Automation.UI.Shell.Wpf.Infrastructure.StringListControl
{
    /// <summary>
    /// Interaction logic for ListBoxControl.xaml
    /// </summary>
    public partial class ListBoxControl : UserControl
    {
        public ListBoxControl()
        {
            InitializeComponent();
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            var selectItem = ((Button)sender).DataContext.ToString();

            var observableList = new ObservableCollection<string>();
            foreach (string item in xRadListBox.ItemsSource)
            {
                observableList.Add(item);
            }

            observableList.Remove(selectItem);
            xRadListBox.ItemsSource = observableList;
        }

        private void RadButton_Click_1(object sender, RoutedEventArgs e)
        {
            var insertItem = newItem.Text;
            if (!string.IsNullOrWhiteSpace(insertItem))
            {
                var observableList = new ObservableCollection<string>();
                foreach (string item in xRadListBox.ItemsSource)
                {
                    observableList.Add(item);
                }

                observableList.Add(insertItem);
                xRadListBox.ItemsSource = observableList;
                newItem.Text = string.Empty;
            }
        }
    }
}
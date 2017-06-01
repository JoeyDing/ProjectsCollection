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

using TheLongestString.LocFileProvider;
using WinForms = System.Windows.Forms;

namespace TheLongestString
{
    /// <summary>
    /// Interaction logic for LaunchWindow.xaml
    /// </summary>
    public partial class LaunchWindow : Window
    {
        public LaunchWindow()
        {
            InitializeComponent();

            // populate the font combobox
            var fonts =
                from font in System.Drawing.FontFamily.Families
                select font.Name;
            FontComboBox.ItemsSource = fonts;

            // set initial values
            FolderTextBox.Text = Properties.Settings.Default.LcxFolder;
            FileSearchPatternComboBox.ItemsSource = new List<string>
            {
                LcxLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b),
                ResxLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b),
                XmlLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b),
                JSLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b),
                StringsLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b),
                JsonLocDocument.AllowedExtensions.Aggregate((a, b) => a + ", " + b)
            };
            FileSearchPatternComboBox.SelectedIndex = 0;

            // initial font value
            if (fonts.Contains(Properties.Settings.Default.FontToRender))
            {
                FontComboBox.SelectedItem = fonts.First(s => s.Equals(Properties.Settings.Default.FontToRender, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                FontComboBox.SelectedItem = fonts.First(s => s.Equals("Segoe UI", StringComparison.InvariantCultureIgnoreCase));
            }

            // initial font size value
            var defaultFontSizes = new List<double>()
            {
                8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
            };

            if (!defaultFontSizes.Contains(Properties.Settings.Default.FontSizeToRender))
            {
                defaultFontSizes.Add(Properties.Settings.Default.FontSizeToRender);
            }
            defaultFontSizes.Sort();

            FontSizeComboBox.ItemsSource = defaultFontSizes;
            FontSizeComboBox.SelectedItem = Properties.Settings.Default.FontSizeToRender;
        }

        private void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderSelectDialog();
            dialog.Title = "Select a folder that contains Resource files";

            if (dialog.ShowDialog())
            {
                FolderTextBox.Text = dialog.SelectedPath;
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // write settings
            Properties.Settings.Default.LcxFolder = FolderTextBox.Text.Trim(new char[] { '"' });
            Properties.Settings.Default.FileSearchPattern = FileSearchPatternComboBox.Text;
            Properties.Settings.Default.FontToRender = FontComboBox.SelectedItem.ToString();
            Properties.Settings.Default.FontSizeToRender = (double)FontSizeComboBox.SelectedItem;
            Properties.Settings.Default.Save();

            // create loading screen with correct config vars
            var loadingWindow = new LoadingWindow();
            loadingWindow.Show();

            this.Close();
        }
    }
}
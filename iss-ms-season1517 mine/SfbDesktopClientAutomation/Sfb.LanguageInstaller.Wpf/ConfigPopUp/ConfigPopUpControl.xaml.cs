using Sfb.LanguageInstaller.Wpf.StringListControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sfb.LanguageInstaller.Wpf.ConfigPopUp
{
    /// <summary>
    /// Interaction logic for ConfigPopUpControl.xaml
    /// </summary>
    public partial class ConfigPopUpControl : Window
    {
        private readonly Type _configXmlType;
        private readonly string _fileName;
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public ConfigPopUpControl()
        {
        }

        public ConfigPopUpControl(Type configXmlType, string fileName)
        {
            _configXmlType = configXmlType;
            _fileName = fileName;
            InitializeComponent();

            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            //reflection
            var deserializeService = new DeserializeService();
            var configxmlObject = deserializeService.Deserialize(fileName, configXmlType);

            int rowIndex = 0;

            LoadControlsRecurrsively(dynamicGrid, configxmlObject, ref rowIndex);

            //button
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            GenerateControlBasedOnType<Button>(dynamicGrid, "", rowIndex + 1, 1);
        }

        private void LoadControlsRecurrsively(Grid dynamicGrid, object parentObject, ref int rowIndex)
        {
            var parentType = parentObject.GetType();
            var childrenProperties = parentType.GetProperties();
            foreach (var childPropertyInfo in childrenProperties)
            {
                string childPropertyName = childPropertyInfo.Name;
                var childPropertyType = childPropertyInfo.PropertyType;

                if (childPropertyType == typeof(string))
                {
                    var value = childPropertyInfo.GetValue(parentObject);
                    string textBoxName = LoadControls<TextBox>(dynamicGrid, rowIndex, childPropertyName);
                    //load
                    ((TextBox)this.FindName(textBoxName)).Text = value.ToString();
                    dict.Add(childPropertyName, textBoxName);
                    rowIndex++;
                }
                else if (childPropertyType == typeof(List<string>))
                {
                    var list = (List<string>)childPropertyInfo.GetValue(parentObject);
                    var observerList = new ObservableCollection<string>();
                    foreach (var item in list)
                    {
                        observerList.Add(item);
                    }

                    string listBoxControlName = LoadControlsForListBox(dynamicGrid, rowIndex, childPropertyName, observerList);
                    //load
                    //((ListBoxControl)this.FindName(listBoxControlName)).xRadListBox.ItemsSource = list;
                    dict.Add(childPropertyName, listBoxControlName);
                    rowIndex++;
                }
                else if (childPropertyType == typeof(bool))
                {
                    var value = (bool)childPropertyInfo.GetValue(parentObject);
                    string checkBoxName = LoadControls<CheckBox>(dynamicGrid, rowIndex, childPropertyName);
                    //load
                    ((CheckBox)this.FindName(checkBoxName)).IsChecked = value;
                    dict.Add(childPropertyName, checkBoxName);
                    rowIndex++;
                }
                else if (childPropertyType.IsEnum)
                {
                    var enumArry = Enum.GetValues(childPropertyType);
                    var value = (int)childPropertyInfo.GetValue(parentObject);
                    string comboBoxName = LoadControlsForComboBox(dynamicGrid, rowIndex, childPropertyName, enumArry);
                    //load
                    ((ComboBox)this.FindName(comboBoxName)).SelectedIndex = value;
                    dict.Add(childPropertyName, comboBoxName);
                    rowIndex++;
                }
                else if (childPropertyType != typeof(ValueType))
                {
                    var childObject = childPropertyInfo.GetValue(parentObject);
                    LoadControlsRecurrsively(dynamicGrid, childObject, ref rowIndex);
                }
            }
        }

        private string LoadControls<T>(Grid dynamicGrid, int rowIndex, string childPropertyName) where T : Control
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            GenerateControlBasedOnType<Telerik.Windows.Controls.Label>(dynamicGrid, childPropertyName, rowIndex, 0);
            //dynamicGrid.RowDefinitions.Add(new RowDefinition());
            string textBoxName = Regex.Replace("ConfigControl_" + childPropertyName + rowIndex, @"\s+", "");
            GenerateControlBasedOnType<T>(dynamicGrid, textBoxName, rowIndex, 1);
            return textBoxName;
        }

        private string LoadControlsForComboBox(Grid dynamicGrid, int rowIndex, string childPropertyName, Array array)
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            GenerateControlBasedOnType<Telerik.Windows.Controls.Label>(dynamicGrid, childPropertyName, rowIndex, 0);
            //dynamicGrid.RowDefinitions.Add(new RowDefinition());
            string textBoxName = Regex.Replace("ConfigControl_" + childPropertyName + rowIndex, @"\s+", "");
            GenerateControlBasedOnTypeForComboBox(dynamicGrid, textBoxName, rowIndex, 1, array);
            return textBoxName;
        }

        private string LoadControlsForListBox(Grid dynamicGrid, int rowIndex, string childPropertyName, ObservableCollection<string> list)
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            //rd.Height = new GridLength(1, GridUnitType.Star);
            //dynamicGrid.RowDefinitions.Add(rd);

            GenerateControlBasedOnType<Telerik.Windows.Controls.Label>(dynamicGrid, childPropertyName, rowIndex, 0);

            string textBoxName = Regex.Replace("ConfigControl_" + childPropertyName + rowIndex, @"\s+", "");
            GenerateControlBasedOnTypeForListBox(dynamicGrid, textBoxName, rowIndex, 1, list);
            return textBoxName;
        }

        public void GenerateControlBasedOnType<T>(Grid dynamicGrid, string Name, int rowIndex, int columnIndex) where T : Control
        {
            if (typeof(T) == typeof(Telerik.Windows.Controls.Label))
            {
                var controlToSetup = new Telerik.Windows.Controls.Label();
                controlToSetup.HorizontalAlignment = HorizontalAlignment.Left;
                controlToSetup.VerticalAlignment = VerticalAlignment.Top;
                controlToSetup.Name = "ConfigLable";
                controlToSetup.Height = 30;
                controlToSetup.Width = 90;
                controlToSetup.Content = Name;
                controlToSetup.FontWeight = FontWeights.Bold;
                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(TextBox))
            {
                var controlToSetup = new TextBox();
                controlToSetup.HorizontalAlignment = HorizontalAlignment.Left;
                controlToSetup.VerticalAlignment = VerticalAlignment.Top;
                controlToSetup.Name = Name;
                controlToSetup.Height = 20;
                controlToSetup.Width = 180;
                this.RegisterName(Name, controlToSetup);

                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(CheckBox))
            {
                var controlToSetup = new CheckBox();
                controlToSetup.HorizontalAlignment = HorizontalAlignment.Left;
                controlToSetup.VerticalAlignment = VerticalAlignment.Top;
                controlToSetup.Name = Name;
                controlToSetup.Height = 20;
                controlToSetup.Width = 180;
                this.RegisterName(Name, controlToSetup);

                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(Button))
            {
                var saveButton = new Button();
                saveButton.Click += SaveButton_Click;
                saveButton.Content = "Save";
                //saveButton.Height = 35;
                saveButton.Width = 70;
                //saveButton.Margin = new Thickness(10, 10, 10, 10);
                //saveButton.Padding = new Thickness(10, 10, 10, 10);

                Grid.SetRow(saveButton, rowIndex);
                Grid.SetColumn(saveButton, columnIndex);

                dynamicGrid.Children.Add(saveButton);
            }
        }

        public void GenerateControlBasedOnTypeForComboBox(Grid dynamicGrid, string Name, int rowIndex, int columnIndex, Array array)
        {
            var controlToSetup = new ComboBox();
            controlToSetup.HorizontalAlignment = HorizontalAlignment.Left;
            controlToSetup.VerticalAlignment = VerticalAlignment.Top;
            controlToSetup.ItemsSource = array;
            controlToSetup.Name = Name;
            controlToSetup.Height = 20;
            controlToSetup.Width = 180;
            this.RegisterName(Name, controlToSetup);

            Grid.SetRow(controlToSetup, rowIndex);
            Grid.SetColumn(controlToSetup, columnIndex);
            dynamicGrid.Children.Add(controlToSetup);
        }

        public void GenerateControlBasedOnTypeForListBox(Grid dynamicGrid, string Name, int rowIndex, int columnIndex, ObservableCollection<string> list)
        {
            var controlToSetup = new ListBoxControl();
            controlToSetup.HorizontalAlignment = HorizontalAlignment.Left;
            controlToSetup.VerticalAlignment = VerticalAlignment.Top;
            controlToSetup.xRadListBox.ItemsSource = list;
            controlToSetup.Name = Name;

            //controlToSetup.VerticalAlignment = VerticalAlignment.Stretch;

            this.RegisterName(Name, controlToSetup);

            Grid.SetRow(controlToSetup, rowIndex);
            Grid.SetColumn(controlToSetup, columnIndex);
            dynamicGrid.Children.Add(controlToSetup);
        }

        private void RecurrToLoadProperty(object parentObject, Dictionary<string, string> dict)
        {
            var parentType = parentObject.GetType();
            var childrenProperties = parentType.GetProperties();

            foreach (var childPropertyInfo in childrenProperties)
            {
                var childPropertyType = childPropertyInfo.PropertyType;
                var childPropertyName = childPropertyInfo.Name;

                if (childPropertyType == typeof(List<string>))
                {
                    var valList = ReadFromListBox(dict, childPropertyName);
                    if (valList.Any())
                        childPropertyInfo.SetValue(parentObject, valList);
                }
                else if (childPropertyType == typeof(string))
                {
                    var val = ReadFromTextBox(dict, childPropertyName);
                    childPropertyInfo.SetValue(parentObject, val);
                }
                else if (childPropertyType == typeof(bool))
                {
                    bool? bl = ReadFromCheckBox(dict, childPropertyName);
                    childPropertyInfo.SetValue(parentObject, bl);
                }
                else if (childPropertyType.IsEnum)
                {
                    object selectedItem = ReadFromComboBox(dict, childPropertyName);
                    childPropertyInfo.SetValue(parentObject, selectedItem);
                }
                else if (childPropertyType != typeof(ValueType))
                {
                    var childObject = Activator.CreateInstance(childPropertyInfo.PropertyType);
                    RecurrToLoadProperty(childObject, dict);
                    childPropertyInfo.SetValue(parentObject, childObject);
                }
            }
        }

        private List<string> ReadFromListBox(Dictionary<string, string> dict, string childPropertyName)
        {
            var textBoxName = dict[childPropertyName];
            var list = new List<string>();

            var s = ((ListBoxControl)this.FindName(textBoxName)).xRadListBox.ItemsSource as ObservableCollection<string>;
            list = s.ToList();

            return list;
        }

        private string ReadFromTextBox(Dictionary<string, string> dict, string childPropertyName)
        {
            var textBoxName = dict[childPropertyName];
            //if (!string.IsNullOrWhiteSpace(((TextBox)this.FindName(textBoxName)).Text))
            string val = ((TextBox)this.FindName(textBoxName)).Text;
            return val;
        }

        private bool? ReadFromCheckBox(Dictionary<string, string> dict, string childPropertyName)
        {
            var checkBoxName = dict[childPropertyName];
            return ((CheckBox)this.FindName(checkBoxName)).IsChecked;
        }

        private object ReadFromComboBox(Dictionary<string, string> dict, string childPropertyName)
        {
            var comboBoxName = dict[childPropertyName];
            return ((ComboBox)this.FindName(comboBoxName)).SelectedItem;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var configObject = Activator.CreateInstance(_configXmlType);
            RecurrToLoadProperty(configObject, dict);
            //Deserialize

            var serializeService = new SerializeService();
            serializeService.Serialize(_fileName, configObject);

            this.Close();
        }
    }
}
using Automation.UI.Shell.Wpf.Infrastructure.ConfigPopUp;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Automation.UI.Shell.Wpf.Infrastructure
{
    public class LoadDictForPopupService : ILoadDictForPopup
    {
        private Window ui;

        public LoadDictForPopupService(Window _ui)
        {
            ui = _ui;
            NameScope.SetNameScope(ui, new NameScope());
        }

        public void LoadControlsRecurrsively(Grid dynamicGrid, object parentObject, ref int rowIndex, Dictionary<string, List<string>> dict)
        {
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

                        ((TextBox)ui.FindName(textBoxName)).Text = value.ToString();
                        dict.Add(childPropertyName, new List<string>() { textBoxName });
                        rowIndex++;
                    }
                    else if (childPropertyType == typeof(List<string>))
                    {
                        var list = (List<string>)childPropertyInfo.GetValue(parentObject);
                        var textBoxNameList = new List<string>();
                        foreach (var value in list)
                        {
                            string textBoxName = LoadControls<TextBox>(dynamicGrid, rowIndex, childPropertyName);
                            //load
                            ((TextBox)ui.FindName(textBoxName)).Text = value;
                            textBoxNameList.Add(textBoxName);
                            rowIndex++;
                        }
                        dict.Add(childPropertyName, textBoxNameList);
                    }
                    else if (childPropertyType == typeof(bool))
                    {
                        var value = (bool)childPropertyInfo.GetValue(parentObject);
                        string checkBoxName = LoadControls<CheckBox>(dynamicGrid, rowIndex, childPropertyName);
                        //load
                        ((CheckBox)ui.FindName(checkBoxName)).IsChecked = value;
                        dict.Add(childPropertyName, new List<string>() { checkBoxName });
                        rowIndex++;
                    }
                    else if (childPropertyType.IsEnum)
                    {
                        var enumArry = Enum.GetValues(childPropertyType);
                        var value = (int)childPropertyInfo.GetValue(parentObject);
                        string comboBox = LoadControlsForComboBox(dynamicGrid, rowIndex, childPropertyName, enumArry);
                        //load
                        ((ComboBox)ui.FindName(comboBox)).SelectedIndex = value;
                        dict.Add(childPropertyName, new List<string>() { comboBox });
                        rowIndex++;
                    }
                    else if (childPropertyType != typeof(ValueType))
                    {
                        var childObject = childPropertyInfo.GetValue(parentObject);
                        LoadControlsRecurrsively(dynamicGrid, childObject, ref rowIndex, dict);
                    }
                }
            }
        }

        private string LoadControls<T>(Grid dynamicGrid, int rowIndex, string childPropertyName) where T : Control
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition());
            GenerateControlBasedOnType<Telerik.Windows.Controls.Label>(dynamicGrid, childPropertyName, rowIndex, 0);
            dynamicGrid.RowDefinitions.Add(new RowDefinition());
            string textBoxName = Regex.Replace("ConfigControl_" + childPropertyName + rowIndex, @"\s+", "");
            GenerateControlBasedOnType<T>(dynamicGrid, textBoxName, rowIndex, 1);
            return textBoxName;
        }

        private string LoadControlsForComboBox(Grid dynamicGrid, int rowIndex, string childPropertyName, Array array)
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition());
            GenerateControlBasedOnType<Telerik.Windows.Controls.Label>(dynamicGrid, childPropertyName, rowIndex, 0);
            dynamicGrid.RowDefinitions.Add(new RowDefinition());
            string textBoxName = Regex.Replace("ConfigControl_" + childPropertyName + rowIndex, @"\s+", "");
            GenerateControlBasedOnTypeForComboBox(dynamicGrid, textBoxName, rowIndex, 1, array);
            return textBoxName;
        }

        public void GenerateControlBasedOnType<T>(Grid dynamicGrid, string Name, int rowIndex, int columnIndex) where T : Control
        {
            if (typeof(T) == typeof(Telerik.Windows.Controls.Label))
            {
                var controlToSetup = new Telerik.Windows.Controls.Label();
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
                controlToSetup.Name = Name;
                controlToSetup.Height = 20;
                controlToSetup.Width = 180;
                ui.RegisterName(Name, controlToSetup);

                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(CheckBox))
            {
                var controlToSetup = new CheckBox();
                controlToSetup.Name = Name;
                controlToSetup.Height = 20;
                controlToSetup.Width = 180;
                ui.RegisterName(Name, controlToSetup);

                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(ComboBox))
            {
                var controlToSetup = new ComboBox();

                controlToSetup.Name = Name;
                controlToSetup.Height = 20;
                controlToSetup.Width = 180;
                ui.RegisterName(Name, controlToSetup);

                Grid.SetRow(controlToSetup, rowIndex);
                Grid.SetColumn(controlToSetup, columnIndex);
                dynamicGrid.Children.Add(controlToSetup);
            }
            else if (typeof(T) == typeof(Button))
            {
                var saveButton = new Button();
                //saveButton.Click += SaveButton_Click;
                saveButton.Content = "Save";
                //saveButton.Height = 35;
                saveButton.Width = 70;
                //saveButton.Margin = new Thickness(10, 10, 10, 10);
                //saveButton.Padding = new Thickness(10, 10, 10, 10);

                Grid.SetRow(saveButton, rowIndex);
                Grid.SetColumn(saveButton, columnIndex);

                dynamicGrid.Children.Add(saveButton);
            }
            ui.Content = dynamicGrid;
        }

        public void GenerateControlBasedOnTypeForComboBox(Grid dynamicGrid, string Name, int rowIndex, int columnIndex, Array array)
        {
            var controlToSetup = new ComboBox();
            controlToSetup.ItemsSource = array;
            //foreach (var item in array)
            //{
            //    controlToSetup.Items.Add(item);
            //}
            controlToSetup.Name = Name;
            controlToSetup.Height = 20;
            controlToSetup.Width = 180;
            ui.RegisterName(Name, controlToSetup);

            Grid.SetRow(controlToSetup, rowIndex);
            Grid.SetColumn(controlToSetup, columnIndex);
            dynamicGrid.Children.Add(controlToSetup);

            ui.Content = dynamicGrid;
        }
    }
}
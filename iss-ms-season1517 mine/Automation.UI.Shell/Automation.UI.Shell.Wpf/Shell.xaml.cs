using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Automation.UI.Shell.Wpf
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window
    {
        private bool loaded;

        public Shell()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();

            //add a custom event handler to listen to inner items click handler
            tabsRegion.AddHandler(Button.ClickEvent, new RoutedEventHandler(tabsRegion_OnChildClick), true);

            //add a handler to listen new added items
            ((INotifyCollectionChanged)this.tabsRegion.Items).CollectionChanged += Shell_CollectionChanged;
        }

        private void tabsRegion_OnChildClick(object source, RoutedEventArgs args)
        {
            var parent = GetParentOfType<RadListBoxItem>((DependencyObject)args.OriginalSource);
            if (parent != null)
            {
                //trigger selection changed event
                parent.IsSelected = true;
            }
        }

        private void Shell_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!loaded && e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.tabsRegion.Items.Count > 0)
                {
                    var loadedNav = ((UserControl)this.tabsRegion.Items[0]).Content as DependencyObject;
                    var button = GetChildrenOfType<ButtonBase>(loadedNav).FirstOrDefault();
                    if (button != null)
                    {
                        this.loaded = true;

                        //raise click event
                        button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

                        //make item selected
                        var parent = GetParentOfType<RadListBoxItem>(button);
                        if (parent != null)
                        {
                            //trigger selection changed event
                            parent.IsSelected = true;
                        }

                        //usubscribe
                           ((INotifyCollectionChanged)this.tabsRegion.Items).CollectionChanged -= Shell_CollectionChanged;
                    }
                }
            }
        }

        public static T GetParentOfType<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return GetParentOfType<T>(parentObject);
        }

        public static IEnumerable<T> GetChildrenOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                if (typeof(T).IsAssignableFrom(depObj.GetType()))
                    yield return (T)depObj;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && typeof(T).IsAssignableFrom(child.GetType()))
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in GetChildrenOfType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Mvp
{
    public sealed class ViewHelper
    {
        private static readonly ViewHelper instance = new ViewHelper();

        private ViewHelper()
        {
        }

        public static ViewHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #region RadListBox

        public IReadOnlyList<CheckableItem> RadListBox_GetCheckable(RadListBox radListBox)
        {
            var result = new List<CheckableItem>();
            foreach (RadListBoxItem item in radListBox.Items)
            {
                result.Add(new CheckableItem { Value = item.Value, Text = item.Text, IsChecked = item.Checked });
            }
            return result.AsReadOnly();
        }

        public IReadOnlyList<SelectableItem> RadListBox_GetSelectable(RadListBox radListBox)
        {
            var result = new List<SelectableItem>();
            foreach (RadListBoxItem item in radListBox.Items)
            {
                result.Add(new SelectableItem { Value = item.Value, Text = item.Text, IsSelected = item.Selected });
            }
            return result.AsReadOnly();
        }

        public void RadListBox_SetCheckable(RadListBox radListBox, IEnumerable<CheckableItem> checkableItems)
        {
            radListBox.Items.Clear();
            foreach (var item in checkableItems)
            {
                radListBox.Items.Add(new Telerik.Web.UI.RadListBoxItem { Text = item.Text, Value = item.Value, Checked = item.IsChecked });
            }
        }

        public void RadListBox_SetSelectable(RadListBox radListBox, IEnumerable<SelectableItem> checkableItems)
        {
            radListBox.Items.Clear();
            foreach (var item in checkableItems)
            {
                radListBox.Items.Add(new Telerik.Web.UI.RadListBoxItem { Text = item.Text, Value = item.Value, Selected = item.IsSelected });
            }
        }

        #endregion RadListBox

        #region RadComboBox

        public IReadOnlyList<SelectableItem> RadComboBox_GetSelectable(RadComboBox radComboBox)
        {
            var result = new List<SelectableItem>();
            foreach (RadComboBoxItem item in radComboBox.Items)
            {
                result.Add(new SelectableItem { Value = item.Value, Text = item.Text, IsSelected = item.Selected });
            }
            return result.AsReadOnly();
        }

        public void RadComboBox_SetSelectable(RadComboBox radComboBox, IEnumerable<SelectableItem> checkableItems)
        {
            radComboBox.Items.Clear();
            foreach (var item in checkableItems)
            {
                radComboBox.Items.Add(new Telerik.Web.UI.RadComboBoxItem { Text = item.Text, Value = item.Value, Selected = item.IsSelected });
            }
        }

        #endregion RadComboBox
    }
}
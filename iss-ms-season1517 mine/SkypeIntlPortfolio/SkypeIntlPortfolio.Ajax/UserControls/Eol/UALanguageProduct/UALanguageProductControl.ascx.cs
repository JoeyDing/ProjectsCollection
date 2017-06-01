using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol.UALanguageProduct
{
    public partial class UALanguageProductControl : UserControl, IUAlanguageProductBridge
    {
        public event Func<string, int, int, List<UALanguage_Base>> GetUALanguageUnderPrudctByType;

        public event Action<int, int> onDeleteFiles;

        public event EventHandler<List<BasicLanguageList>> onGetBasicLanguage;

        public event Action<bool, List<UALanguage_Base>> onInsertLanguage;

        public event Action<bool, UALanguage> onUpdateLanguage;

        public event Func<string, int> GetTotalCountOfUALanguageUnderPrudctByType;

        public event Func<string, int, int, int, List<ResourceFile>> GetFilesUnderLanguage;

        public event Func<string, int, int> GetTotalCountOfFilesUnderLanguage;

        public event Func<List<ResourceFile>> GetFilesListUnderProductForInsert;

        public event Action<int> DeleteUALanguage;

        public event Func<string, List<BasicLanguageList_Base>> GetFilteredLanguageListForProductLevel;

        public event Func<List<Products_New>> GetAllProducts;

        public event Action<string, int, string> InsertFromAnotherProject;

        public event Action<string, string> MoveEolFromOneTypeToAnother;

        protected void RadGrid_UALanguage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGrid_UALanguage.DataSource = Enumerable.Range(0, 1).Select(c => new { Name = "" }).ToList();
        }

        protected void RadGrid_UALanguage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                string name = e.Item.OwnerTableView.Name;

                var item = (GridEditableItem)e.Item;

                if (name == "ReleasedDetails" || name == "PlannedDetails" || name == "BlockedDetails" || name == "ReleasedDetails_Files" || name == "PlannedDetails_Files" || name == "BlockedDetails_Files")
                {
                    GridEditFormItem editForm = (GridEditFormItem)e.Item;
                    editForm.EditFormCell.CssClass = "centerPopUpModal";

                    //var labelNoLanguageChecked = (Label)item.FindControl("lable_NoLanguageChecked");
                    //labelNoLanguageChecked.Visible = false;
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        var pannel_add = (Panel)item.FindControl("pannel_add");
                        pannel_add.Visible = true;

                        //the two lines of code are used to center the rad grid popup window

                        // insert item
                        var radListBox_Language = (RadListBox)item.FindControl("radListBox_Language");
                        radListBox_Language.Visible = true;

                        radListBox_Language.DataSource = this.GetFilteredLanguageListForProductLevel(name);
                        radListBox_Language.DataTextField = "LanguageIDPlusName";
                        radListBox_Language.DataValueField = "LanguageKey";
                        radListBox_Language.DataBind();

                        var radListBox_File = (RadListBox)item.FindControl("radListBox_File");
                        radListBox_File.Visible = true;

                        if (this.GetFilesListUnderProductForInsert != null)
                        {
                            radListBox_File.DataSource = this.GetFilesListUnderProductForInsert();
                            radListBox_File.DataTextField = "Source_File_Location";
                            radListBox_File.DataValueField = "FileKey";
                            radListBox_File.DataBind();
                        }
                        if (name != "BlockedDetails")
                        {
                            var radCombobox_Project = (RadComboBox)item.FindControl("radCombobox_Project");
                            if (this.GetAllProducts != null)
                            {
                                radCombobox_Project.DataSource = this.GetAllProducts();
                                radCombobox_Project.DataTextField = "Product_Name";
                                radCombobox_Project.DataValueField = "ProductKey";
                                radCombobox_Project.DataBind();
                            }
                        }
                    }
                    else
                    {
                        var pannel_edit = (Panel)item.FindControl("pannel_edit");
                        pannel_edit.Visible = true;
                        // edit item
                        if (name != "BlockedDetails" && name != "BlockedDetails_Files")
                        {
                            var radDatePicker = (RadDatePicker)item.FindControl("raddatepicker_ReleaseDate_Edit");
                            var radComboBoxQuality = (RadComboBox)item.FindControl("radcombobox_Quality_Edit");
                            DateTime resultReleaseDate;
                            if (item["Release_Date"] != null && DateTime.TryParse(item["Release_Date"].Text, out resultReleaseDate))
                                radDatePicker.SelectedDate = resultReleaseDate;
                            Dictionary<string, string> qualityDict = new Dictionary<string, string> { { "Auto", "1" }, { "HT", "2" }, { "MT", "3" } };
                            radComboBoxQuality.SelectedValue = qualityDict[item["QualityName"].Text];
                        }
                        else
                        {
                            var radTexBox = (RadTextBox)item.FindControl("radtextbox_BlockedReason_Edit");
                            radTexBox.Text = item["Blocked_Reason"].Text != "&nbsp;" ? item["Blocked_Reason"].Text : "";
                        }
                    }
                }
                else if (name == "masterTable")
                {
                    var comboList = new List<ComboBoxObject>();
                    comboList.Add(new ComboBoxObject() { Key = "Released", Value = "Released Language" });
                    comboList.Add(new ComboBoxObject() { Key = "Planned", Value = "Planned Language" });
                    comboList.Add(new ComboBoxObject() { Key = "Blocked", Value = "Blocked Language" });

                    //Move From A to B
                    var radcombobox_From = (RadComboBox)item.FindControl("radcombobox_From");
                    radcombobox_From.DataSource = comboList;
                    radcombobox_From.DataTextField = "Value";
                    radcombobox_From.DataValueField = "Key";
                    radcombobox_From.SelectedIndex = 1;
                    radcombobox_From.DataBind();

                    var radcombobox_To = (RadComboBox)item.FindControl("radcombobox_To");
                    radcombobox_To.DataSource = comboList;
                    radcombobox_To.DataTextField = "Value";
                    radcombobox_To.DataValueField = "Key";
                    radcombobox_To.SelectedIndex = 0;
                    radcombobox_To.DataBind();
                }
            }
        }

        private class ComboBoxObject
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        protected void RadGrid_UALanguage_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            string UALanguageType = "";
            string detailTableName = e.DetailTableView.Name;
            if (detailTableName == "ReleasedDetails" || detailTableName == "PlannedDetails" || detailTableName == "BlockedDetails")
            {
                switch (detailTableName)
                {
                    case "ReleasedDetails":
                        UALanguageType = "Released";
                        break;

                    case "PlannedDetails":
                        UALanguageType = "Planned";
                        break;

                    case "BlockedDetails":
                        UALanguageType = "Blocked";
                        break;
                }

                if (this.GetTotalCountOfUALanguageUnderPrudctByType != null)
                    e.DetailTableView.VirtualItemCount = this.GetTotalCountOfUALanguageUnderPrudctByType(UALanguageType);

                int currentPageIndex = e.DetailTableView.CurrentPageIndex;
                int detailTablePageSize = e.DetailTableView.PageSize;

                if (this.GetUALanguageUnderPrudctByType != null)
                    e.DetailTableView.DataSource = this.GetUALanguageUnderPrudctByType(UALanguageType, currentPageIndex, detailTablePageSize);
            }
            else if (detailTableName == "ReleasedDetails_Files" || detailTableName == "PlannedDetails_Files" || detailTableName == "BlockedDetails_Files")
            {
                var data = (GridEditableItem)e.DetailTableView.ParentItem;

                int UALanguageKey = int.Parse(data.GetDataKeyValue("UALanguagesKey").ToString());
                switch (detailTableName)
                {
                    case "ReleasedDetails_Files":
                        UALanguageType = "Released";
                        break;

                    case "PlannedDetails_Files":
                        UALanguageType = "Planned";
                        break;

                    case "BlockedDetails_Files":
                        UALanguageType = "Blocked";
                        break;
                }

                if (this.GetTotalCountOfFilesUnderLanguage != null)
                    e.DetailTableView.VirtualItemCount = this.GetTotalCountOfFilesUnderLanguage(UALanguageType, UALanguageKey);

                int currentPageIndex = e.DetailTableView.CurrentPageIndex;
                int detailTablePageSize = e.DetailTableView.PageSize;

                if (this.GetFilesUnderLanguage != null)
                    e.DetailTableView.DataSource = this.GetFilesUnderLanguage(UALanguageType, UALanguageKey, currentPageIndex, detailTablePageSize);
            }
        }

        protected void RadGrid_UALanguage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid grid = (sender as RadGrid);
            grid.MasterTableView.ClearEditItems();
            e.Item.OwnerTableView.IsItemInserted = false;

            string name = e.Item.OwnerTableView.Name;
            if ((name == "ReleasedDetails" || name == "PlannedDetails" || name == "BlockedDetails" || name == "ReleasedDetails_Files" || name == "PlannedDetails_Files" || name == "BlockedDetails_Files") && e.Item != null)
            {
                if (e.CommandName == "Edit")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string languageIDPlusName = item["LanguageIDPlusName"].Text;

                    string languageType = "";
                    switch (name)
                    {
                        case "ReleasedDetails":
                            languageType = "Released Languages";
                            break;

                        case "PlannedDetails":
                            languageType = "Planned Languages";
                            break;

                        case "BlockedDetails":
                            languageType = "Blocked Languages";
                            break;
                    }
                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} : {1}", languageType, languageIDPlusName);

                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Width = Unit.Pixel(500);
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Height = Unit.Pixel(100);
                }
                else if (e.CommandName == "InitInsert")
                {
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Width = Unit.Pixel(1400);
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Height = Unit.Pixel(500);
                }

                if (e.CommandName == "PerformInsert" && this.onInsertLanguage != null)
                {
                    var editableItem = (GridEditableItem)e.Item;
                    if (name == "ReleasedDetails" || name == "PlannedDetails")
                    {
                        var radMultiPageUALanguage = (RadMultiPage)editableItem.FindControl("radMultiPageUALanguage");
                        if (radMultiPageUALanguage.SelectedIndex == 0)
                        {
                            InsertFromLanguageSelection(name, editableItem);
                        }
                        else
                        {
                            var radCombobox_Project = (RadComboBox)editableItem.FindControl("radCombobox_Project");
                            var radComboBox_UaOrUi = (RadComboBox)editableItem.FindControl("radCombobox_UAorUI");

                            string ualanguageType = "";
                            switch (name)
                            {
                                case "ReleasedDetails":
                                    ualanguageType = "Released";
                                    break;

                                case "PlannedDetails":
                                    ualanguageType = "Planned";
                                    break;
                            }
                            string uaOruI = radComboBox_UaOrUi.SelectedValue;

                            int productKeyToCopyFrom = int.Parse(radCombobox_Project.SelectedValue);
                            this.InsertFromAnotherProject(uaOruI, productKeyToCopyFrom, ualanguageType);
                        }
                    }
                    else if (name == "BlockedDetails")
                        InsertFromLanguageSelection(name, editableItem);
                }
                else if (e.CommandName == "Delete")
                {
                    var editableItem = (GridEditableItem)e.Item;

                    if (name == "ReleasedDetails_Files" || name == "PlannedDetails_Files" || name == "BlockedDetails_Files" && this.onDeleteFiles != null)
                    {
                        int fileKey = int.Parse(editableItem.GetDataKeyValue("FileKey").ToString());
                        int UALanguageKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("UALanguagesKey").ToString());

                        this.onDeleteFiles(UALanguageKey, fileKey);
                    }
                    else if (name == "ReleasedDetails" || name == "PlannedDetails" || name == "BlockedDetails" && this.DeleteUALanguage != null)
                    {
                        int UALanguageKey = int.Parse(editableItem.GetDataKeyValue("UALanguagesKey").ToString());
                        this.DeleteUALanguage(UALanguageKey);
                    }
                }
                else if (e.CommandName == "Update" && this.onUpdateLanguage != null)
                {
                    var editableItem = (GridEditableItem)e.Item;

                    int UAlanguageKey = int.Parse(editableItem.GetDataKeyValue("UALanguagesKey").ToString());
                    var item = new UALanguage();
                    bool ifModifyQualityID = false;
                    if (name != "BlockedDetails" && name != "BlockedDetails_Files")
                    {
                        var radDatePicker = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate_Edit");
                        var radComboBoxQuality = (RadComboBox)editableItem.FindControl("radcombobox_Quality_Edit");

                        if (radDatePicker.SelectedDate.HasValue)
                            item.Release_Date = radDatePicker.SelectedDate.Value;
                        item.QualityID = int.Parse(radComboBoxQuality.SelectedValue);
                        ifModifyQualityID = true;
                    }
                    else
                    {
                        var radTextBox = (RadTextBox)editableItem.FindControl("radtextbox_BlockedReason_Edit");
                        var blockedReason = radTextBox.Text;
                        item.Blocked_Reason = blockedReason;
                    }

                    item.UALanguagesKey = UAlanguageKey;

                    this.onUpdateLanguage(ifModifyQualityID, item);
                }
            }
            else if (name == "masterTable")
            {
                if (e.CommandName == "PerformInsert")
                {
                    var editableItem = (GridEditableItem)e.Item;
                    var radcombobox_From = (RadComboBox)editableItem.FindControl("radcombobox_From");
                    var radcombobox_To = (RadComboBox)editableItem.FindControl("radcombobox_To");
                    string fromType = radcombobox_From.SelectedValue;
                    string toType = radcombobox_To.SelectedValue;
                    this.MoveEolFromOneTypeToAnother(fromType, toType);
                }
            }
        }

        private void InsertFromLanguageSelection(string name, GridEditableItem editableItem)
        {
            GridDataItem dataItem = editableItem.OwnerTableView.ParentItem;

            var radListBox_Language = (RadListBox)editableItem.FindControl("radListBox_Language");
            var radListBox_File = (RadListBox)editableItem.FindControl("radListBox_File");
            if (!radListBox_Language.CheckedItems.Any())
            {
                throw new Exception();
            }
            else
            {
                var UAList = new List<UALanguage_Base>();
                if (radListBox_File.CheckedItems.Any())
                {
                    foreach (var file in radListBox_File.CheckedItems)
                    {
                        int fileKey = int.Parse(file.Value);
                        LoadInsertListForUALanguage(name, editableItem, radListBox_Language, UAList, fileKey);
                    }

                    this.onInsertLanguage(true, UAList);
                }
                else
                {
                    LoadInsertListForUALanguage(name, editableItem, radListBox_Language, UAList, 0);
                    this.onInsertLanguage(false, UAList);
                }
            }
        }

        private static void LoadInsertListForUALanguage(string name, GridEditableItem editableItem, RadListBox radListBox_Language, List<UALanguage_Base> UAList, int fileKey)
        {
            foreach (var language in radListBox_Language.CheckedItems)
            {
                var item = new UALanguage_Base();

                switch (name)
                {
                    case "ReleasedDetails":
                        item.Language_Released = true;
                        item.Language_Planned = false;
                        item.Language_Blocked = false;

                        var datePickerReleased = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate");
                        var comboboxQualityReleased = (RadComboBox)editableItem.FindControl("radcombobox_Quality");
                        var releasedDateReleased = datePickerReleased.SelectedDate.Value;
                        var qualityIDReleased = int.Parse(comboboxQualityReleased.SelectedValue);
                        item.Release_Date = releasedDateReleased;
                        item.QualityID = qualityIDReleased;
                        item.Blocked_Reason = null;
                        break;

                    case "PlannedDetails":
                        item.Language_Released = false;
                        item.Language_Planned = true;
                        item.Language_Blocked = false;

                        var datePickerPlanned = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate");
                        var comboboxQualityPlanned = (RadComboBox)editableItem.FindControl("radcombobox_Quality");
                        var releasedDatePlanned = datePickerPlanned.SelectedDate.Value;
                        int qualityIDPlanned = int.Parse(comboboxQualityPlanned.SelectedValue);
                        item.Release_Date = releasedDatePlanned;
                        item.QualityID = qualityIDPlanned;
                        item.Blocked_Reason = null;
                        break;

                    case "BlockedDetails":
                        item.Language_Released = false;
                        item.Language_Planned = false;
                        item.Language_Blocked = true;

                        var radTextBox = (RadTextBox)editableItem.FindControl("radtextbox_BlockedReason");
                        var blockedReason = radTextBox.Text;
                        item.Blocked_Reason = blockedReason;
                        item.Release_Date = null;
                        item.QualityID = 2;
                        break;
                }

                item.LanguageKey = int.Parse(language.Value);
                if (fileKey != 0)
                    item.FileKey = fileKey;

                UAList.Add(item);
            }
        }
    }
}
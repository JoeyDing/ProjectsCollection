using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Model.Mock;
using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public partial class UILanguageControl : UserControl, IUILanguageBridge
    {
        public event EventHandler<UILanguage> onUpdateLanguage;

        public event EventHandler<List<UILanguage_Base>> onInsertLanguage;

        public event Func<int, int, List<GetUILanguageFileOfProductByPage_Result>> onGetUILanguageFileOfProduct;

        public event EventHandler<List<BasicLanguageList>> onGetBasicLanguage;

        public event EventHandler<int> onDeleteLanguage;

        public event Func<int> onGetTotalRecord;

        public event Func<int, string, int, int, List<UILanguage_Base>> onGetUILanguagePerFile;

        public event Func<int, string, int> onGetTotalRecordPerFile;

        public event Func<int, UILanguage> GetUILanguageByUiLanguageKey;

        public event Func<int, List<BasicLanguageList>> GetFilteredLanguageListForFileLevel;

        public List<GetUILanguageFileOfProductByPage_Result> UILanguageFileOfProductByPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.UILanguageFileOfProductByPage == null)
                this.UILanguageFileOfProductByPage = new List<GetUILanguageFileOfProductByPage_Result>();
            if (this.BasicLanguageList == null)
                this.BasicLanguageList = new List<BasicLanguageList>();
            if (!this.IsPostBack && this.Visible)
            {
                int currentPage = this.RadGrid_UILanguage.CurrentPageIndex;
                int pageSize = this.RadGrid_UILanguage.MasterTableView.PageSize;
                if (this.onGetUILanguageFileOfProduct != null)
                    UILanguageFileOfProductByPage = this.onGetUILanguageFileOfProduct(currentPage * pageSize, (currentPage + 1) * pageSize);

                if (this.onGetBasicLanguage != null)
                    this.onGetBasicLanguage(sender, this.BasicLanguageList);
            }
        }

        protected void RadGrid_UILanguage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //1.get files list

            if (this.onGetTotalRecord != null)
                this.RadGrid_UILanguage.VirtualItemCount = this.onGetTotalRecord();
            if (this.UILanguageFileOfProductByPage == null)
                this.UILanguageFileOfProductByPage = new List<GetUILanguageFileOfProductByPage_Result>();

            int currentPage = this.RadGrid_UILanguage.CurrentPageIndex;
            int pageSize = this.RadGrid_UILanguage.MasterTableView.PageSize;

            if (this.onGetUILanguageFileOfProduct != null)
                UILanguageFileOfProductByPage = this.onGetUILanguageFileOfProduct(currentPage * pageSize, (currentPage + 1) * pageSize);
            //2.get files' details
            var uiFiles = this.UILanguageFileOfProductByPage.Select(u => new { File_Name = u.File_Name, FileKey = u.FileKey }).Distinct().ToList();
            this.RadGrid_UILanguage.DataSource = uiFiles;
        }

        protected void RadGrid_UILanguage_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            //e.DetailTableView.CurrentPageIndex = 0;
            string uiLanguageType = "";
            string detailTableName = e.DetailTableView.Name;
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            int fileKey = int.Parse(dataItem.GetDataKeyValue("FileKey").ToString());
            switch (detailTableName)
            {
                case "ReleasedDetails":
                    uiLanguageType = "Released";
                    break;

                case "PlannedDetails":
                    uiLanguageType = "Planned";
                    break;

                case "BlockedDetails":
                    uiLanguageType = "Blocked";
                    break;
            }

            if (this.onGetTotalRecordPerFile != null)
                e.DetailTableView.VirtualItemCount = this.onGetTotalRecordPerFile(fileKey, uiLanguageType);

            int currentPageIndex = e.DetailTableView.CurrentPageIndex;
            int detailTablePageSize = e.DetailTableView.PageSize;

            if (this.onGetUILanguagePerFile != null)
                e.DetailTableView.DataSource = this.onGetUILanguagePerFile(fileKey, uiLanguageType, currentPageIndex, detailTablePageSize);
        }

        protected void RadGrid_UILanguage_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid grid = (sender as RadGrid);
            grid.MasterTableView.ClearEditItems();
            e.Item.OwnerTableView.IsItemInserted = false;
            string name = e.Item.OwnerTableView.Name;
            if ((name == "ReleasedDetails" || name == "PlannedDetails" || name == "BlockedDetails") && e.Item != null)
            {
                if (e.CommandName == "Edit")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
                    string fileName = parentItem["File_Name"].Text;
                    string languageName = item["LanguageName"].Text;

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

                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} : {1} \ {2}", languageType, fileName, languageName);
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Width = Unit.Pixel(500);
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Height = Unit.Pixel(80);
                    //e.Item.OwnerTableView.Rebind();
                }
                else if (e.CommandName == "InitInsert")
                {
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Width = Unit.Pixel(1000);
                    e.Item.OwnerTableView.EditFormSettings.PopUpSettings.Height = Unit.Pixel(450);
                    //e.Item.OwnerTableView.Rebind();
                }

                if (e.CommandName == "PerformInsert" && this.onInsertLanguage != null)
                {
                    var editableItem = (GridEditableItem)e.Item;
                    var labelNoLanguageChecked = (Label)editableItem.FindControl("lable_NoLanguageChecked");
                    //labelNoLanguageChecked.Visible = false;
                    GridDataItem dataItem = editableItem.OwnerTableView.ParentItem;
                    int fileKey = int.Parse(dataItem.GetDataKeyValue("FileKey").ToString());

                    var radListBox = (RadListBox)editableItem.FindControl("radListBox_Language");
                    if (!radListBox.CheckedItems.Any())
                    {
                        throw new Exception();
                    }
                    else
                    {
                        var uiList = new List<UILanguage_Base>();
                        foreach (var language in radListBox.CheckedItems)
                        {
                            var item = new UILanguage_Base();

                            switch (name)
                            {
                                case "ReleasedDetails":
                                    item.Language_Released = true;
                                    item.Language_Planned = false;
                                    item.Language_Blocked = false;

                                    var datePickerReleased = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate_Insert");
                                    var releasedDateReleased = datePickerReleased.SelectedDate.Value;
                                    item.Release_Date = releasedDateReleased;
                                    item.Blocked_Reason = null;
                                    break;

                                case "PlannedDetails":
                                    item.Language_Released = false;
                                    item.Language_Planned = true;
                                    item.Language_Blocked = false;

                                    var datePickerPlanned = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate_Insert");
                                    var releasedDatePlanned = datePickerPlanned.SelectedDate.Value;
                                    item.Release_Date = releasedDatePlanned;
                                    item.Blocked_Reason = null;
                                    break;

                                case "BlockedDetails":
                                    item.Language_Released = false;
                                    item.Language_Planned = false;
                                    item.Language_Blocked = true;

                                    var radTextBox = (RadTextBox)editableItem.FindControl("radtextbox_BlockedReason_Insert");
                                    var blockedReason = radTextBox.Text;
                                    item.Blocked_Reason = blockedReason;
                                    item.Release_Date = null;
                                    break;
                            }

                            item.LanguageKey = int.Parse(language.Value);
                            item.FileKey = fileKey;
                            uiList.Add(item);
                        }
                        this.onInsertLanguage(sender, uiList);
                    }
                }
                else if (e.CommandName == "Delete" && this.onDeleteLanguage != null)
                {
                    var editableItem = (GridEditableItem)e.Item;
                    int uiLanguageKey = int.Parse(editableItem.GetDataKeyValue("UILanguagesKey").ToString());
                    this.onDeleteLanguage(sender, uiLanguageKey);
                }
                else if (e.CommandName == "Update" && this.onUpdateLanguage != null)
                {
                    var editableItem = (GridEditableItem)e.Item;
                    int uilanguageKey = int.Parse(editableItem.GetDataKeyValue("UILanguagesKey").ToString());
                    var item = new UILanguage();

                    if (name != "BlockedDetails")
                    {
                        var radDatePicker = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseDate");
                        var releaseDate = radDatePicker.SelectedDate.Value;
                        item.Release_Date = releaseDate;
                    }
                    else
                    {
                        var radTextBox = (RadTextBox)editableItem.FindControl("radtextbox_BlockedReason");
                        var blockedReason = radTextBox.Text;
                        item.Blocked_Reason = blockedReason;
                    }

                    item.UILanguagesKey = uilanguageKey;

                    if (this.onUpdateLanguage != null)
                        this.onUpdateLanguage(sender, item);
                }
            }
        }

        protected void RadGrid_UILanguage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                string name = e.Item.OwnerTableView.Name;

                var item = (GridEditableItem)e.Item;

                if (name == "ReleasedDetails" || name == "PlannedDetails" || name == "BlockedDetails")
                {
                    GridEditFormItem editForm = (GridEditFormItem)e.Item;
                    editForm.EditFormCell.CssClass = "centerPopUpModal";

                    //var labelNoLanguageChecked = (Label)item.FindControl("lable_NoLanguageChecked");
                    //labelNoLanguageChecked.Visible = false;
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem && GetFilteredLanguageListForFileLevel != null)
                    {
                        //the two lines of code are used to center the rad grid popup window

                        // insert item
                        var radListBox_Language = (RadListBox)item.FindControl("radListBox_Language");
                        var pannel_Insert = (Panel)item.FindControl("pannel_Insert");
                        pannel_Insert.Visible = true;

                        var parentItem = e.Item.OwnerTableView.ParentItem;
                        int fileKey = int.Parse(parentItem.GetDataKeyValue("FileKey").ToString());
                        radListBox_Language.DataSource = this.GetFilteredLanguageListForFileLevel(fileKey);
                        radListBox_Language.DataTextField = "Language";
                        radListBox_Language.DataValueField = "LanguageKey";
                        radListBox_Language.DataBind();
                    }
                    else
                    {
                        // edit item
                        var pannel_Update = (Panel)item.FindControl("pannel_Update");
                        pannel_Update.Visible = true;

                        if (name != "BlockedDetails")
                        {
                            var radDatePicker = (RadDatePicker)item.FindControl("raddatepicker_ReleaseDate");
                            radDatePicker.SelectedDate = DateTime.Parse(item["Release_Date"].Text);
                        }
                        else
                        {
                            var radTexBox = (RadTextBox)item.FindControl("radtextbox_BlockedReason");
                            radTexBox.Text = item["Blocked_Reason"].Text;
                        }
                    }
                }
            }
        }

        public List<BasicLanguageList> BasicLanguageList
        {
            get
            {
                return this.ViewState["BasicLanguageList"] as List<BasicLanguageList>;
            }
            set
            {
                this.ViewState["BasicLanguageList"] = value;
            }
        }
    }
}
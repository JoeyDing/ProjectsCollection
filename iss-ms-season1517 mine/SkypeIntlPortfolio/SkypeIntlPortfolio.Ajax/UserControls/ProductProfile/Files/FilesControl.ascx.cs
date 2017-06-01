using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Files
{
    public partial class FilesControl : System.Web.UI.UserControl, IFilesView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public event Func<List<UIResourceFile>> GetFilesData;

        public event Action<UIResourceFile> UpdateFile;

        public event Func<UIResourceFile, int> AddNewFile;

        public event Action<int> DeleteFile;

        public event EventHandler OnClickNext;

        public Dictionary<int, UIResourceFile> ResourceFiles
        {
            get
            {
                return this.ViewState["ResourceFiles"] as Dictionary<int, UIResourceFile>;
            }
            set
            {
                this.ViewState["ResourceFiles"] = value;
            }
        }

        public List<SelectableItem> FabricTenants
        {
            get
            {
                return this.ViewState["FabricTenants"] as List<SelectableItem>;
            }

            set
            {
                this.ViewState["FabricTenants"] = value;
            }
        }

        protected void radgrid_files_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            //reload data from db only on page refresh
            if (!this.IsPostBack && this.GetFilesData != null)
            {
                this.ResourceFiles = this.GetFilesData().ToDictionary(c => c.FileKey, c => c);
            }

            if (this.ResourceFiles != null)
                this.radgrid_files.DataSource = this.ResourceFiles.Values;
        }

        protected void radgrid_files_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var newFile = this.ExtractFileFromGridRow(editableItem);
            if (this.AddNewFile != null)
            {
                var test = this.AddNewFile(newFile);
                this.ResourceFiles.Add(newFile.FileKey, newFile);
            }
        }

        protected void radgrid_files_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var fileKey = (int)editableItem.GetDataKeyValue("FileKey");
            var updatedFile = this.ExtractFileFromGridRow(editableItem);
            if (this.UpdateFile != null)
            {
                updatedFile.FileKey = fileKey;
                this.UpdateFile(updatedFile);
                this.ResourceFiles[fileKey] = updatedFile;
            }
        }

        protected void radgrid_files_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var fileKey = (int)editableItem.GetDataKeyValue("FileKey");
            if (this.DeleteFile != null)
            {
                this.DeleteFile(fileKey);
                this.ResourceFiles.Remove(fileKey);
            }
        }

        private UIResourceFile ExtractFileFromGridRow(GridEditableItem editableItem)
        {
            var result = new UIResourceFile();

            var values = new Dictionary<object, object>();
            editableItem.ExtractValues(values);

            result.LCG_File_Location = (string)values["LCG_File_Location"];
            result.Source_File_Location = (string)values["Source_File_Location"];
            var textBoxParserID = (TextBox)editableItem.FindControl("TextBox_ParseID");
            result.ParserID = Convert.ToInt32(textBoxParserID.Text);
            result.RepoBranch = (string)values["repoBranch"];
            result.RepoType = (string)values["repoType"];
            result.RepoURL = (string)values["RepoURL"];

            result.File_Name = (string)values["File_Name"];
            result.File_Type = (string)values["File_Type"];
            result.Fabric_Project = (string)values["Fabric_Project"];
            result.FabricTenants = this.FabricTenants.ToList();

            var rbl = (RadioButtonList)editableItem["templatecolumn_FabricTenant"].FindControl("radiobuttonlist_fabricTenants");
            if (rbl.SelectedValue != null)
            {
                foreach (var tenant in result.FabricTenants)
                {
                    if (tenant.Value.ToString() == rbl.SelectedValue)
                    {
                        tenant.IsSelected = true;
                        result.SelectedFabricTenant = tenant.Text;
                    }
                    else
                        tenant.IsSelected = false;
                }
            }
            return result;
        }

        protected void radgrid_files_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                var data = e.Item.DataItem as UIResourceFile;
                //DataItem is set to "InsertionObject" in case of "insert"
                if (data != null)
                {
                    GridEditFormItem item = (GridEditFormItem)e.Item;
                    RadioButtonList rbl = (RadioButtonList)item["templatecolumn_FabricTenant"].FindControl("radiobuttonlist_fabricTenants");
                    rbl.ClearSelection();
                    if (data.FabricTenants.Any(c => c.IsSelected))
                    {
                        rbl.SelectedValue = data.FabricTenants.First(c => c.IsSelected).Value;
                    }
                }
            }
        }

        protected void radgrid_files_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //switch between edit and insert modes
            RadGrid grid = (sender as RadGrid);
            //set your form values
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                grid.MasterTableView.ClearEditItems();
                //Add new" button clicked
                e.Canceled = true;
                //Prepare an IDictionary with the predefined values
                var newValues = new System.Collections.Specialized.ListDictionary();
                newValues["ParserID"] = default(int?);
                newValues["FabricTenants"] = this.FabricTenants.ToList();
                //Insert the item and rebind
                e.Item.OwnerTableView.InsertItem(newValues);
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }
        }

        protected void RadButton_tab_file_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
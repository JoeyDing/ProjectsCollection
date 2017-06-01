using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ImportCommandLine;
using CopyCommandLine;
using Telerik.Web.UI;
using SkypeIntlPortfolio.Ajax.Core.Service;
using System.Configuration;
using System.Text;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ReviewBuild
{
    public partial class ReviewBuildView : Page, IReviewBuildView
    {
        public event Func<BuildRequest, string, string, Tuple<CopyModel, ImportModel>> LoadCopyAndImport;

        public event Func<string, BuildRequest> GetRequestBuildFromDb;

        public event Action<BuildRequest, bool> UpdateRequestBuildInDb;

        public List<FileNode> FilePathObject
        {
            get { return ViewState["FilePathObject"] as List<FileNode>; }
            set { ViewState["FilePathObject"] = value; }
        }

        public CopyModel Copy
        {
            get
            {
                return this.ViewState["Copy"] as CopyModel;
            }
            set
            {
                this.ViewState["Copy"] = value;
            }
        }

        public ImportModel Import
        {
            get
            {
                return this.ViewState["Import"] as ImportModel;
            }
            set
            {
                this.ViewState["Import"] = value;
            }
        }

        public Tuple<CopyModel, ImportModel> TupleResult
        {
            get
            {
                return this.ViewState["TupleResult"] as Tuple<CopyModel, ImportModel>;
            }
            set
            {
                this.ViewState["TupleResult"] = value;
            }
        }

        public BuildRequest RequestOfBuild
        {
            get
            {
                return this.ViewState["Request"] as BuildRequest;
            }
            set
            {
                this.ViewState["Request"] = value;
            }
        }

        public bool IsLyncServer
        {
            get
            {
                return (bool)this.ViewState["IsLyncServer"];
            }
            set
            {
                this.ViewState["IsLyncServer"] = value;
            }
        }

        public string SkypeConfigPath
        {
            get { return ViewState["SkypeConfigPath"] as string; }
            set { ViewState["SkypeConfigPath"] = value; }
        }
        public string LyncServerConfigPath
        {
            get { return ViewState["LyncServerConfigPath"] as string; }
            set { ViewState["LyncServerConfigPath"] = value; }
        }
        private BuildVersionValidator buildValidator = new BuildVersionValidator();

        protected void Page_Load(object sender, EventArgs e)
        {
            var presenter = new ReviewBuildPresenter(this);
            if (!this.IsPostBack)
            {
                this.SkypeConfigPath = ConfigurationManager.AppSettings["CopyImport:XmlPathForSkypeLctBuild"];
                this.LyncServerConfigPath = ConfigurationManager.AppSettings["CopyImport:XmlPathForSkypeLctBuild"];
                if (LoadCopyAndImport != null && GetRequestBuildFromDb != null)
                {
                    string requestID = base.Request.QueryString["RequestID"].ToString();
                    this.RequestOfBuild = this.GetRequestBuildFromDb(requestID);
                    this.TupleResult = LoadCopyAndImport(RequestOfBuild, this.SkypeConfigPath, this.LyncServerConfigPath);
                    this.Copy = TupleResult.Item1;
                    this.Import = TupleResult.Item2;
                    this.Copy.CopyJobName = this.GetCopyJobNameByBuildType(this.Copy.BuildType);
                    this.Import.ImportJobName = this.GetImportJobNameByBuildType(this.Import.BuildType);

                    FilePathObject = this.Import.FileList;
                    if (FilePathObject == null)
                    {
                        FilePathObject = new List<FileNode>();
                        panel_ModifyXml.Visible = false;
                    }

                    label_BuildType.Text = RequestOfBuild.BuildType;
                    IsLyncServer = RequestOfBuild.BuildType == "lync server" ? true : false;
                    if (IsLyncServer)
                    {
                        panel_Tenant.Visible = true;
                        panel_SkypeXml.Visible = true;
                    }
                    else
                    {
                        panel_Tenant.Visible = false;
                        panel_SkypeXml.Visible = true;

                    }
                    if (IsLyncServer)
                    {
                        HashSet<string> componentList = new HashSet<string>(RequestOfBuild.LyncServerComponentList.Split(','));
                        foreach (RadListBoxItem item in radListBox_LyncServerComponentList.Items)
                        {
                            if (componentList.Contains(item.Text))
                                item.Checked = true;
                        }
                        radComboBox_Tenant.SelectedValue = Copy.Tenant;
                    }
                    label_SkypeLctFilePaths.Text = string.Join(@"<br />", RequestOfBuild.SkypeLctFilePaths.Split(','));

                    radTextBox_BuildToKeep.Text = Copy.NumbersOfBuildsToKeep.ToString();
                    radTextBox_SourcePath.Text = Copy.SourcePath;
                    radTextBox_DestinationPath.Text = Copy.DestinationPath;
                    radTextBox_FromVersion.Text = Copy.FromBuildVersion;
                    radTextBox_BranchName.Text = RequestOfBuild.BranchName != null ? RequestOfBuild.BranchName : Copy.ProductName;
                    radTextBox_ProductName.Text = Copy.ProductName;

                    label_CopyCmdLine.Text = Copy.CommandLine;
                    label_ImportCmdLine.Text = Import.CommandLine;
                    label_DWImportCmdLine.Text = Import.DWCommandLine;
                    radTextBox_SkypeXml.Text = Import.xmlContent;
                    if (HasErrorMessage())
                        return;
                }
            }

            BindCopyAndImportWithControls();
        }

        private bool HasErrorMessage()
        {
            label_StartSuccess.Visible = false;
            label_BuildVersionFormat.Visible = false;
            label_ErrorMsg.Visible = false;
            label_ErrorMsgBottom.Visible = false;
            radButton_CreateBranchID.Visible = false;
            label_AtLeastOneTenantOrComponent.Visible = false;
            label_lctFilePathValidation.Visible = false;

            if (this.Import.BranchID == 0)
            {
                radButton_CreateBranchID.Visible = true;
                radTextBox_BranchID.Text = string.Empty;
            }
            else
                radTextBox_BranchID.Text = Import.BranchID.ToString();

            if (!string.IsNullOrWhiteSpace(this.Import.ErrorMessage))
            {
                label_ErrorMsg.Visible = true;
                label_ErrorMsgBottom.Visible = true;
                label_ErrorMsg.Text = this.Import.ErrorMessage;
                label_ErrorMsgBottom.Text = this.Import.ErrorMessage;
                return true;
            }

            if (!string.IsNullOrWhiteSpace(this.Copy.ErrorMessage))
            {
                label_ErrorMsg.Visible = true;
                label_ErrorMsgBottom.Visible = true;
                label_ErrorMsg.Text = this.Copy.ErrorMessage;
                label_ErrorMsgBottom.Text = this.Copy.ErrorMessage;
                return true;
            }

            string buildVersion = radTextBox_FromVersion.Text;

            if (!buildValidator.IsBuildVersionValid(buildVersion, IsLyncServer))
            {
                label_BuildVersionFormat.Visible = true;
                label_BuildVersionFormat.Text = IsLyncServer ? "Build Version can not be empty and the format shuold be like 12.0.3.24" : "Build Version can not be empty and the format shuold be like 20170108_104334";
                return true;
            }

            if (IsLyncServer && !radListBox_LyncServerComponentList.CheckedItems.Any() && radComboBox_Tenant.Text == "none")
            {
                label_AtLeastOneTenantOrComponent.Visible = true;
                return true;
            }

            if (!IsLyncServer && !FilePathObject.Any())
            {
                label_lctFilePathValidation.Visible = true;
                label_lctFilePathValidation.Text = "Please add at least one lct File!";
                return true;
            }

            return false;
        }

        protected void radButton_CreateBranchID_Click(object sender, EventArgs e)
        {
            int createdBranchID = ImportCommandLineUtils.InsertBranchIDIfNotExisted(label_BuildType.Text, radTextBox_ProductName.Text);
            radTextBox_BranchID.Text = createdBranchID.ToString();

            //bind branchID here to make sure branchID is inserted
            this.Import.BranchID = int.Parse(radTextBox_BranchID.Text);
            RequestOfBuild.BranchID = createdBranchID;
            RequestOfBuild.ProductName = radTextBox_ProductName.Text;
            if (this.UpdateRequestBuildInDb != null)
                UpdateRequestBuildInDb(this.RequestOfBuild, true);
            label_ErrorMsg.Visible = false;
            label_ErrorMsgBottom.Visible = false;
            radButton_CreateBranchID.Visible = false;
            RefreshImportAndCopy();

            radTextBox_SkypeXml.Text = Import.xmlContent;
            label_CopyCmdLine.Text = Copy.CommandLine;
            label_ImportCmdLine.Text = Import.CommandLine;
            label_DWImportCmdLine.Text = Import.DWCommandLine;
        }

        protected void radButton_Start_Click(object sender, EventArgs e)
        {
            //Refresh import and copy based on current use input
            RefreshImportAndCopy();

            //check all the possible error msg
            if (HasErrorMessage())
                return;

            //refresh cmd Line
            label_CopyCmdLine.Text = Copy.CommandLine;
            label_ImportCmdLine.Text = Import.CommandLine;
            label_DWImportCmdLine.Text = Import.DWCommandLine;

            //Update buildRequest table in db
            string tenant = radComboBox_Tenant.Text;
            var componentCheckedItems = radListBox_LyncServerComponentList.CheckedItems;
            var componentTotalItems = radListBox_LyncServerComponentList.Items;

            this.RequestOfBuild.BuildToKeep = int.Parse(radTextBox_BuildToKeep.Text);
            this.RequestOfBuild.BranchID = int.Parse(radTextBox_BranchID.Text);
            this.RequestOfBuild.ProductName = radTextBox_ProductName.Text;
            this.RequestOfBuild.SourcePath = radTextBox_SourcePath.Text;
            this.RequestOfBuild.DestinationPath = radTextBox_DestinationPath.Text;
            this.RequestOfBuild.FromBuildVersion = radTextBox_FromVersion.Text;
            this.RequestOfBuild.BranchName = (radTextBox_ProductName.Text.ToLower() != radTextBox_BranchName.Text.ToLower()) ? radTextBox_BranchName.Text : null;
            this.RequestOfBuild.Tenant = IsLyncServer ? (tenant != "none" ? tenant : string.Empty) : null;
            this.RequestOfBuild.LyncServerComponentList = IsLyncServer ?
                (componentCheckedItems.Any() ? string.Join(",", componentCheckedItems.Select(c => c.Text)) : string.Empty)
                : null;
            this.RequestOfBuild.IsComponentListAll = !IsLyncServer ? (bool?)null :
                (!componentCheckedItems.Any() && tenant != "none")
                || (componentCheckedItems.Any() && componentCheckedItems.Count() == componentTotalItems.Count());

            if (this.UpdateRequestBuildInDb != null)
                UpdateRequestBuildInDb(this.RequestOfBuild, false);
            List<string> successMsg = new List<string>();
            //save skype config
            if (radListBox_StartOptions.Items[0].Checked == true)
            {
                ImportCommandLineUtils.SaveConfigForSkypeLctAndLyncServer(this.Import);
                successMsg.Add(string.Format("Skype config has been saved to {0} !", this.SkypeConfigPath));
            }

            //add Sql job and PD
            if (radListBox_StartOptions.Items[1].Checked == true)
            {
                JobInfoForCopy copyJob = CopyCommandLineUtils.AddCopyStep(this.Copy);
                JobInfoForImport importJob = ImportCommandLineUtils.AddImportStep(this.Import);

                successMsg.Add(string.Format("{0} has been added to sql job {1}!", copyJob.CopyStepName, copyJob.JobName));
                successMsg.Add(string.Format("{0} has been added to sql job {1}!", importJob.ImportStepName, importJob.JobName));
                successMsg.Add(string.Format("{0} has been added to sql job {1}!", importJob.ImportDWStepName, importJob.JobName));
            }
            if (radListBox_StartOptions.Items[2].Checked == true)
            {
                successMsg.Add("PD has been generated!");
            }
            label_StartSuccess.Text = string.Join(@"<br />", successMsg);
            label_StartSuccess.Visible = true;

            radButton_Start.Enabled = false;
            radListBox_StartOptions.Enabled = false;
        }

        private void RefreshImportAndCopy()
        {
            CopyCommandLineUtils.GetCopyInfo(this.Copy);
            ImportCommandLineUtils.GetImportInfo(this.Import);
        }

        private void BindCopyAndImportWithControls()
        {
            if (FilePathObject != null)
            {
                panel_ModifyXml.Visible = true;
            }
            string tenant = radComboBox_Tenant.Text;
            var componentCheckedItems = radListBox_LyncServerComponentList.CheckedItems;
            var componentTotalItems = radListBox_LyncServerComponentList.Items;

            this.Copy.BuildType = label_BuildType.Text;
            this.Copy.DestinationPath = radTextBox_DestinationPath.Text;
            this.Copy.FromBuildVersion = radTextBox_FromVersion.Text;
            this.Copy.ProductName = radTextBox_ProductName.Text;
            this.Copy.SourcePath = radTextBox_SourcePath.Text;
            this.Copy.Tenant = IsLyncServer ? (tenant != "none" ? tenant : string.Empty) : null;
            this.Copy.LyncServerComponentList = IsLyncServer ? radListBox_LyncServerComponentList.CheckedItems.Select(c => c.Text).ToList() : null;
            this.Copy.CommandLine = label_CopyCmdLine.Text;
            this.Copy.IsComponetListAll = !IsLyncServer ? false :
                (!componentCheckedItems.Any() && tenant != "none")
                || (componentCheckedItems.Any() && componentCheckedItems.Count() == componentTotalItems.Count());
            this.Copy.NumbersOfBuildsToKeep = int.Parse(radTextBox_BuildToKeep.Text);

            this.Import.FileList = FilePathObject;
            this.Import.BuildType = label_BuildType.Text;
            this.Import.DestinationPath = radTextBox_DestinationPath.Text;
            this.Import.FromBuildVersion = radTextBox_FromVersion.Text;
            this.Import.ProductName = radTextBox_ProductName.Text;
            this.Import.xmlContent = radTextBox_SkypeXml.Text;
            this.Import.Tenant = IsLyncServer ? (tenant != "none" ? tenant : string.Empty) : null;
            this.Import.LyncServerComponentList = IsLyncServer ? radListBox_LyncServerComponentList.CheckedItems.Select(c => c.Text).ToList() : null;
            this.Import.CommandLine = label_ImportCmdLine.Text;
            this.Import.DWCommandLine = label_DWImportCmdLine.Text;
            this.Import.IsComponetListAll = !IsLyncServer ? false :
                (!componentCheckedItems.Any() && tenant != "none")
                || (componentCheckedItems.Any() && componentCheckedItems.Count() == componentTotalItems.Count());
        }

        protected void radButton_refreshCmdLine_Click(object sender, EventArgs e)
        {
            RefreshImportAndCopy();

            if (HasErrorMessage())
                return;

            label_CopyCmdLine.Text = Copy.CommandLine;
            label_ImportCmdLine.Text = Import.CommandLine;
            label_DWImportCmdLine.Text = Import.DWCommandLine;
        }

        protected void radListBox_StartOptions_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            if (radListBox_StartOptions.CheckedItems.Any())
                radButton_Start.Enabled = true;
            else
                radButton_Start.Enabled = false;
        }

        protected void radTextBox_ProductName_TextChanged(object sender, EventArgs e)
        {
            RefreshImportAndCopy();
            HasErrorMessage();
        }

        protected void radButton_RefreshSkypeXml_Click(object sender, EventArgs e)
        {
            RefreshImportAndCopy();

            if (HasErrorMessage())
                return;
            radTextBox_SkypeXml.Text = Import.xmlContent;
        }

        protected void radGrid_SkypeXml_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var editableItem = ((GridEditableItem)e.Item);
                var hash = new Dictionary<object, object>();
                editableItem.ExtractValues(hash);
                var itemToDelete = FilePathObject.First(c => c.Name == hash["Name"].ToString());
                FilePathObject.Remove(itemToDelete);
            }
        }

        protected void radGrid_SkypeXml_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            radGrid_SkypeXml.DataSource = FilePathObject;
            Import.FileList = FilePathObject;
            RefreshImportAndCopy();
            radTextBox_SkypeXml.Text = Import.xmlContent;
        }

        protected void radGrid_SkypeXml_InsertCommand(object sender, GridCommandEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                label_lctFilePathValidation.Visible = false;
                var editableItem = ((GridEditableItem)e.Item);
                var hash = new Dictionary<object, object>();
                editableItem.ExtractValues(hash);
                string lctFilePath = hash["Name"].ToString();
                if (string.IsNullOrWhiteSpace(lctFilePath))
                {
                    label_lctFilePathValidation.Visible = true;
                    label_lctFilePathValidation.Text = "lct file path can not be empty!";
                    return;
                }
                if (FilePathObject.Any(c => c.Name == lctFilePath))
                {
                    label_lctFilePathValidation.Visible = true;
                    label_lctFilePathValidation.Text = "lct file path aleady exists!";
                    return;
                }

                FilePathObject.Add(new FileNode
                {
                    Name = hash["Name"].ToString(),
                    LcgFileName = hash["LcgFileName"].ToString(),
                    Override = hash["Override"].ToString()
                });
            }
        }

        protected void radGrid_SkypeXml_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                label_lctFilePathValidation.Visible = false;
                var editableItem = ((GridEditableItem)e.Item);
                var hash = new Dictionary<object, object>();
                string oldLctFilePath = editableItem.SavedOldValues["Name"].ToString();
                var FileToUpdate = FilePathObject.First(c => c.Name == oldLctFilePath);

                editableItem.ExtractValues(hash);
                string newLctFilePath = hash["Name"].ToString();
                if (string.IsNullOrWhiteSpace(newLctFilePath))
                {
                    label_lctFilePathValidation.Visible = true;
                    label_lctFilePathValidation.Text = "lct file path can not be empty!";
                    return;
                }
                if (newLctFilePath != oldLctFilePath && FilePathObject.Any(c => c.Name == newLctFilePath))
                {
                    label_lctFilePathValidation.Visible = true;
                    label_lctFilePathValidation.Text = "lct file path can not be changed to an existing file path!";
                    return;
                }
                FileToUpdate.Name = hash["Name"].ToString();
                FileToUpdate.LcgFileName = hash["LcgFileName"].ToString();
                FileToUpdate.Override = hash["Override"].ToString();
            }
        }

        private string GetCopyJobNameByBuildType(string BuildType)
        {
            BuildType = BuildType.ToLower();
            switch (BuildType)
            {
                case ("lync server"): return ConfigurationManager.AppSettings["CopyImport:CopyJobLyncserver"];
                case ("skype lct builds"): return ConfigurationManager.AppSettings["CopyImport:CopyJobSkype"];
                default: return "TestJobForTestingKillJobToll"; //Just add them to a test job when error happens as default
            }
        }

        private string GetImportJobNameByBuildType(string BuildType)
        {
            BuildType = BuildType.ToLower();
            switch (BuildType)
            {
                case ("skype lct builds"): return ConfigurationManager.AppSettings["CopyImport:ImportJobSkype"];
                case ("lync server"): return ConfigurationManager.AppSettings["CopyImport:ImportJobLyncserver"];
                default: return "";
            }
        }
    }
}
using CopyCommandLine;
using ImportCommandLine;
using SkypeIntlPortfolio.Ajax.Core.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.RequestBuild
{
    public partial class RequestBuildView : Page, IRequestBuildView
    {
        public event Action<BuildRequest> SaveBuildRequestToDb;

        public List<LctFilePathObject> FilePathObject
        {
            get { return ViewState["filePathObject"] as List<LctFilePathObject>; }
            set { ViewState["filePathObject"] = value; }
        }

        private BuildVersionValidator buildValidator = new BuildVersionValidator();

        private SourcePathValidator sourcePathValidator = new SourcePathValidator();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                radioButtonList_BuildType.SelectedIndex = 0;
                string defaultPath = @"\\skype_drop\FS_SKYPE_TLL\LCT\[Product Folder Name]";
                radTextBox_SourcePath.Text = defaultPath;
                radTextBox_LctFilePath.Text = defaultPath;
                label_PathNotice.Text = @"Fill in the information in [Product Folder Name]";
                label_BuildVersionNotice.Text = @"The format shuold be like 20170108_104334"; ;

                FilePathObject = new List<LctFilePathObject>();
                radGrid_LctFilePaths.DataSource = FilePathObject;
                radGrid_LctFilePaths.DataBind();
            }

            var presenter = new RequestBuildPresenter(this);
        }

        protected void radioButtonList_BuildType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetUserInput();

            if (radioButtonList_BuildType.SelectedIndex == 0)
            {
                panel_SkypeLctFilePath.Visible = true;
                panel_LyncServerComponentList.Visible = false;
                panel_Tenant.Visible = false;
            }
            else
            {
                panel_SkypeLctFilePath.Visible = true;
                panel_LyncServerComponentList.Visible = true;
                panel_Tenant.Visible = true;
            }
        }

        private void ResetUserInput()
        {
            label_AtLeastOneTenantOrComponent.Visible = false;
            label_SubmitSuccess.Visible = false;
            label_BuildVersionFormat.Visible = false;
            label_lctFilePathValidation.Visible = false;

            radTextBox_FromVersion.Text = "";

            radTextBox_ProductName.Text = "";
            radComboBox_Tenant.SelectedIndex = 0;
            radTextBox_LctFilePath.Text = "";

            foreach (var item in radListBox_LyncServerComponentList.CheckedItems)
            {
                item.Checked = false;
            }
            string defaultPath = "";
            string pathNotice = "";
            string buildVersionNotice = "";
            if (radioButtonList_BuildType.SelectedIndex == 0)
            {
                defaultPath = @"\\skype_drop\FS_SKYPE_TLL\LCT\[Product Folder Name]";
                pathNotice = @"Fill in the information in [Product Folder Name]";
                buildVersionNotice = @"The format shuold be like 20170108_104334";
            }
            else
            {
                defaultPath = @"\\sbsrel\Prerelease\lcs\[Major Minor Prefix]\[Product Folder Name]";
                pathNotice = @"Fill in the information in [Major Minor Prefix] and [Product Folder Name]";
                buildVersionNotice = @"The format shuold be like 12.0.3.24";
            }

            label_PathNotice.Text = pathNotice;
            label_BuildVersionNotice.Text = buildVersionNotice;
            radTextBox_SourcePath.Text = defaultPath;
            radTextBox_LctFilePath.Text = defaultPath;
        }

        protected void radButton_Submit_Click(object sender, EventArgs e)
        {
            label_AtLeastOneTenantOrComponent.Visible = false;
            label_SubmitSuccess.Visible = false;
            label_BuildVersionFormat.Visible = false;
            label_lctFilePathValidation.Visible = false;
            label_SourcePathNotValid.Visible = false;

            bool isLyncServer = radioButtonList_BuildType.SelectedIndex == 1;
            string tenant = radComboBox_Tenant.Text;
            var componentCheckedItems = radListBox_LyncServerComponentList.CheckedItems;
            var componentTotalItems = radListBox_LyncServerComponentList.Items;
            string buildVersion = radTextBox_FromVersion.Text;

            if (!sourcePathValidator.IsValid(radTextBox_SourcePath.Text, isLyncServer))
            {
                label_SourcePathNotValid.Visible = true;
                label_SourcePathNotValid.Text = "Please check the source path format and replace the placeholder with product content!";
                return;
            }

            if (!buildValidator.IsBuildVersionValid(buildVersion, isLyncServer))
            {
                label_BuildVersionFormat.Visible = true;
                label_BuildVersionFormat.Text = isLyncServer ? "Build Version can not be empty and the format shuold be like 12.0.3.24" : "Build Version can not be empty and the format shuold be like 20170108_104334";
                return;
            }

            if (!isLyncServer && !FilePathObject.Any())
            {
                label_lctFilePathValidation.Visible = true;
                label_lctFilePathValidation.Text = "Please add at least one lct file";
                return;
            }

            if (!isLyncServer && FilePathObject.Any())
            {
                foreach (string filePath in FilePathObject.Select(c => c.LctFilePath))
                {
                    if (!filePath.Trim().ToLower().StartsWith(radTextBox_SourcePath.Text.Trim().ToLower()))
                    {
                        label_lctFilePathValidation.Visible = true;
                        label_lctFilePathValidation.Text = "Some of the files in the list are not in the source path folder!";
                        return;
                    }
                }
            }

            if (isLyncServer && !componentCheckedItems.Any() && tenant == "none")
            {
                label_AtLeastOneTenantOrComponent.Visible = true;
                return;
            }

            string requestID = Guid.NewGuid().ToString();
            BuildRequest request = new BuildRequest
            {
                RequestID = requestID,
                BuildType = radioButtonList_BuildType.Text,
                SourcePath = radTextBox_SourcePath.Text,
                FromBuildVersion = buildVersion,
                ProductName = radTextBox_ProductName.Text,
                Tenant = isLyncServer ? (tenant != "none" ? tenant : string.Empty) : null,
                SkypeLctFilePaths = !isLyncServer ? string.Join(",", FilePathObject.Select(c => c.LctFilePath)) : string.Join(",", FilePathObject.Select(c => c.LctFilePath)),
                LyncServerComponentList = isLyncServer ? (componentCheckedItems.Any() ? string.Join(",", componentCheckedItems.Select(c => c.Text)) : string.Empty) : null,
                IsComponentListAll = !isLyncServer ? (bool?)null :
                (!componentCheckedItems.Any() && tenant != "none")
                || (componentCheckedItems.Any() && componentCheckedItems.Count() == componentTotalItems.Count()),
                BuildToKeep = 5
            };

            if (SaveBuildRequestToDb != null)
            {
                SaveBuildRequestToDb(request);

                //sending email
                string emailServer = ConfigurationManager.AppSettings["EmailServer"].ToString();
                string emailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                var emailTo = new List<string>();
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EmailTo"]))
                    emailTo.AddRange(ConfigurationManager.AppSettings["EmailTo"].Split(new char[] { ';' }));
                var ccTo = new List<string>();
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CcTo"]))
                    ccTo.AddRange(ConfigurationManager.AppSettings["CcTo"].Split(new char[] { ';' }));
                var sendEmailService = new SendEmailService(emailServer, emailFrom, emailTo, ccTo);
                sendEmailService.EmailSubject = "Build Request";
                StringBuilder sb = new StringBuilder();
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string reviewUrl = url.Replace(@"RequestBuild/RequestBuildView.aspx", string.Format(@"ReviewBuild/ReviewBuildView.aspx?RequestID={0}", request.RequestID));
                sb.AppendLine("<table border=\"1\">");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Review Page url for this request</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", reviewUrl));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Request ID</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", request.RequestID));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Build Type</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", request.BuildType));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Source Path</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", request.SourcePath));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td>From Build Version</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", request.FromBuildVersion));
                sb.AppendLine("</tr>");

                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Product Name</td>");
                sb.AppendLine(string.Format("<td>{0}</td>", request.ProductName));
                sb.AppendLine("</tr>");

                if (!string.IsNullOrWhiteSpace(request.Tenant))
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td>Tenant</td>");
                    sb.AppendLine(string.Format("<td>{0}</td>", request.Tenant));
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</table>");

                sendEmailService.EmailBody = sb.ToString();

                if (!string.IsNullOrWhiteSpace(sendEmailService.EmailBody))
                {
                    sendEmailService.SendEmail();
                    label_SubmitSuccess.Visible = true;
                }
                radButton_Submit.Enabled = false;
            }
        }

        protected void radTextBox_SourcePath_TextChanged(object sender, EventArgs e)
        {
            radTextBox_LctFilePath.Text = radTextBox_SourcePath.Text;
        }

        protected void radButton_AddLctFilePath_Click(object sender, EventArgs e)
        {
            label_lctFilePathValidation.Visible = false;

            string newFilePath = radTextBox_LctFilePath.Text;
            if (string.IsNullOrWhiteSpace(newFilePath))
            {
                label_lctFilePathValidation.Visible = true;
                label_lctFilePathValidation.Text = "File path can not be empty";
                return;
            }
            if (FilePathObject.Any(c => c.LctFilePath == newFilePath))
            {
                label_lctFilePathValidation.Visible = true;
                label_lctFilePathValidation.Text = "File with this path already exists";
                return;
            }
            var itemToAdd = new LctFilePathObject { LctFilePath = newFilePath };
            FilePathObject.Add(itemToAdd);

            radGrid_LctFilePaths.DataSource = FilePathObject;
            radGrid_LctFilePaths.DataBind();


        }

        protected void radGird_LctFilePaths_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var item = (GridEditableItem)e.Item;
                var itemToDelete = FilePathObject.First(c => c.LctFilePath == item["LctFilePath"].Text);
                FilePathObject.Remove(itemToDelete);
                radGrid_LctFilePaths.DataSource = FilePathObject;
                radGrid_LctFilePaths.DataBind();
            }
        }
    }
}
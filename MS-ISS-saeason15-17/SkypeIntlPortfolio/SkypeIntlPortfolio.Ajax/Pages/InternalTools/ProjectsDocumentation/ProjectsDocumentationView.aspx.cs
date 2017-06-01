using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ProjectsDocumentation
{
    public partial class ProjectsDocumentationView : System.Web.UI.Page, IProjectDocumentationView
    {
        private string _currentRepository;

        private string _currentFilePath;

        private string _currentBranch;

        public string Url
        {
            get
            {
                string path = this.Page.Request.Url.GetLeftPart(UriPartial.Path);
                return path;
            }
        }

        public event Func<Dictionary<string, List<ProjectDocInfo>>> GetAvailableProjectsDocs;

        public event Func<string, string, string, string> GetMdFileContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            var presenter = new ProjectsDocumentationPresenter(this);
            if (!this.IsPostBack && this.GetAvailableProjectsDocs != null)
            {
                var projectsData = this.GetAvailableProjectsDocs();
                this.LoadAvailableProjects(projectsData);

                if (!string.IsNullOrEmpty(Request.QueryString["repository"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["filePath"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["branch"]))
                {
                    this._currentRepository = Request.QueryString["repository"];
                    this._currentFilePath = Request.QueryString["filePath"];
                    this._currentBranch = Request.QueryString["branch"];
                    if (this.GetMdFileContent != null)
                    {
                        var content = this.GetMdFileContent(this._currentRepository, this._currentFilePath, this._currentBranch);
                        this.mdContent.Text = content;
                        this.mdContent.DataBind();
                    }
                }
            }
        }

        private void LoadAvailableProjects(Dictionary<string, List<ProjectDocInfo>> data)
        {
            if (data == null)
                return;

            foreach (var projectInfoGroup in data)
            {
                var panelbarItem = new RadPanelItem();
                this.radpanelbar_projectsDocInfoRoot.Items.Add(panelbarItem);
                var innerPanelBar = panelbarItem.FindControl("radpanelbar_projectsDocInfoChild") as RadPanelBar;

                var nodeItem = innerPanelBar.Items[0];
                var label = nodeItem.Header.FindControl("label_repositoryName") as Label;
                label.Text = projectInfoGroup.Key;

                var repeater_projects = nodeItem.FindControl("repeater_projects") as Repeater;
                repeater_projects.DataSource = projectInfoGroup.Value;
                repeater_projects.DataBind();
            }
        }
    }
}
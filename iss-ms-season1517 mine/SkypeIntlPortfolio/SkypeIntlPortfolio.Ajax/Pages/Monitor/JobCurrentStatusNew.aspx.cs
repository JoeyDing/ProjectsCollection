using SkypeIntlMonitoring.Data;
using SkypeIntlPortfolio.Ajax.Model.Monitor;
using SkypeIntlPortfolio.Ajax.UserControls.JobHistory;
using SkypeIntlPortfolio.Ajax.UserControls.JobStatus;
using SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger;
using SkypeIntlPortfolio.Ajax.UserControls.RunningJobs;
using SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor
{
    public partial class JobCurrentStatusNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ToolRelationshipsPresenter trP = new ToolRelationshipsPresenter(this.custom_toolRelationshipsControl);
            JobStatusPresenter jsP = new JobStatusPresenter(this.custom_jobStatusControl);
            JobHistoryPresenter jhP = new JobHistoryPresenter(this.custom_jobHistoryControl);
            RunningJobsPresenter rjp = new RunningJobsPresenter(this.custom_runningJobsControl);
            //after url redirection tab name will be reassigned.
            if (!IsPostBack)
            {
                this.SelectTabFromQueryString();
            }
        }

        protected void RadTabStripJobStatusNew_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            string currentTabName = "";
            switch (this.RadTabStripJobStatusNew.SelectedIndex)
            {
                case 0:
                    currentTabName = "LastRunRecords";
                    break;

                case 1:
                    currentTabName = "JobStatus";
                    break;

                case 2:
                    currentTabName = "JobHistory";
                    break;

                case 3:
                    currentTabName = "ToolRelationships";
                    break;
            }
            //redirect to the new generated url
            string urlToNavigate = "~/Pages/Monitor/JobCurrentStatusNew.aspx?";
            urlToNavigate += "Tab=" + currentTabName;

            Response.Redirect(urlToNavigate, false);
        }

        protected void SelectTabFromQueryString()
        {
            string currentTabName = Request.QueryString["Tab"];
            //get current tab name
            if (!string.IsNullOrEmpty(currentTabName))
            {
                switch (currentTabName)
                {
                    case "LastRunRecords":
                        this.RadTabStripJobStatusNew.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 0;
                        break;

                    case "JobStatus":
                        this.RadTabStripJobStatusNew.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 1;
                        break;

                    case "JobHistory":
                        this.RadTabStripJobStatusNew.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 2;
                        break;

                    case "ToolRelationships":
                        this.RadTabStripJobStatusNew.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 3;
                        break;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools
{
    public partial class ProjectBuildInfo : System.Web.UI.Page
    {
        public Dictionary<string, string> PDNamesAndLinks { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            using (var context = new SkypeIntlMonitoringEntities())
            {
                //grouping data by the project id
                var filteredBuildInfo = context.vw_ProjectBuildInfo.GroupBy(b => b.ProjectID);

                //create new radpanelitem for each of buildinfo group
                foreach (var buildInfoGroup in filteredBuildInfo)
                {
                    string buildInfoGroupName = context.vw_ProjectFamilyInfo.FirstOrDefault(x => x.ProjectID == buildInfoGroup.Key).DisplayName;
                    GeneratepProjectBuildInfoTable(buildInfoGroupName, buildInfoGroup.ToList());
                }
            }
        }

        protected void GeneratepProjectBuildInfoTable(string projectName, List<vw_ProjectBuildInfo> buildInfoGroup)
        {
            RadPanelBar radpanelbar_projectBuildInfoRoot = this.radpanelbar_projectBuildInfoRoot;
            RadPanelItem projectBuildInfoItem = new RadPanelItem();
            radpanelbar_projectBuildInfoRoot.Items.Add(projectBuildInfoItem);
            var radpanelbar_projectBuildInfoChild = projectBuildInfoItem.FindControl("radpanelbar_projectBuildInfoChild") as RadPanelBar;
            var currentRadGrid = radpanelbar_projectBuildInfoChild.Items[0].FindControl("radGrid_projectBuild") as RadGrid;
            var currentProjectLabel = radpanelbar_projectBuildInfoChild.Items[0].Header.FindControl("label_projectName") as Label;
            currentProjectLabel.Text = projectName;

            DataTable dt = new DataTable();
            dt.Columns.Add("Branch ID");
            dt.Columns.Add("Project ID");
            dt.Columns.Add("Build Version");
            dt.Columns.Add("Sync Date");
            dt.Columns.Add("Branch Identity");
            dt.Columns.Add("PD Link");
            PDNamesAndLinks = new Dictionary<string, string>();
            for (int i = 0; i < buildInfoGroup.Count; i++)
            {
                if (buildInfoGroup[i].IsVisible)
                {
                    dt.Rows.Add(buildInfoGroup[i].BranchID, buildInfoGroup[i].ProjectID, buildInfoGroup[i].BuildVersion, buildInfoGroup[i].SyncDate, buildInfoGroup[i].BranchIdentity, buildInfoGroup[i].PDName);
                    if (!String.IsNullOrEmpty(buildInfoGroup[i].PDName) && !PDNamesAndLinks.ContainsKey(buildInfoGroup[i].PDName))
                    {
                        PDNamesAndLinks.Add(buildInfoGroup[i].PDName, buildInfoGroup[i].PDLinks);
                    }
                }
            }
            currentRadGrid.DataSource = dt;
            currentRadGrid.DataBind();
        }

        /// <summary>
        /// this methond will be executed after one previous DataBind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void radGrid_projectBuild_ItemDataBound(object sender, GridItemEventArgs e)
        {
            string PDName = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "PD Link"));
            GridDataItem item = e.Item as GridDataItem;
            if (e.Item is GridDataItem)
            {
                HyperLink pdLink = (HyperLink)item["PDLink"].Controls[0];
                if (pdLink.Text == PDName)
                {
                    if (PDNamesAndLinks.ContainsKey(PDName))
                    {
                        pdLink.NavigateUrl = PDNamesAndLinks[PDName];
                    }
                }
            }
        }
    }
}
using SkypeIntlMonitoring.Data;
using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new SkypeIntlMonitoringContext())
            {
                var day = DateTime.Now.AddDays(-1);
                this.PieChart1.DataSource = context.SqlJobStepIterations
                       .Where(i => i.SqlJobStepID == 0 && i.RunDateTime > day)
                       .GroupBy(i => i.JobRunStatus.Description)
                       .Select(g => new { Status = g.Key, Total = g.Count() })
                       .ToList();

                this.gridview_JobsIterationsFailed.DataSource = context.SqlJobStepIterations
                   .Where(c => c.SqlJobStepID == 0)
                   .OrderByDescending(c => c.SqlJobStepIterationID)
                   .Take(5)
                   .Select(c => new SqlJobIterationsViewModel
                   {
                       SqlJobStepIterationID = c.SqlJobStepIterationID,
                       JobName = c.SqlJobStep.SqlJob.Name,
                       StepName = c.SqlJobStep.Name,
                       RunDateTime = c.RunDateTime,
                       Outcome_Message = c.Outcome_Message,
                       Status = c.JobRunStatus.Description
                   }).
                   ToList();

                List<Tool> tools = context.Tools.OrderBy(j => j.Name).ToList();
                Tool tool1 = tools.FirstOrDefault();
                this.gridview_JobsLog.DataSource = context.LogFiles.Where(j => j.ToolID == tool1.ToolID).OrderBy(j => j.FullPath).ToList();
                this.gridview_JobsLog.DataBind();
            }
            this.gridview_JobsIterationsFailed.DataBind();
        }

        protected void gridview_JobsIterationsFailed_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                if (dataItem["Status"].Text == "Succeeded")
                {
                    dataItem["Status"].ForeColor = Color.Green;
                }
                else if (dataItem["Status"].Text == "Failed")
                {
                    dataItem["Status"].ForeColor = Color.Red;
                    dataItem.Font.Bold = true;
                }
            }
        }
    }
}
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
    public partial class JobCurrentStatus : System.Web.UI.Page
    {
        public const string CURRENT_JOB_STATUS_DATA = "CURRENT_JOB_STATUS_DATA";
        public const string CURRENT_JOB_ITERATION_FAILED_DATA = "CURRENT_JOB_ITERATION_FAILED_DATA";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var day = DateTime.Now.AddDays(-1);
                using (var context = new SkypeIntlMonitoringContext())
                {
                    //job iterations total by status
                    this.PieChart1.DataSource = context.SqlJobStepIterations
                        .Where(i => i.SqlJobStepID == 0 && i.RunDateTime > day)
                        .GroupBy(i => i.JobRunStatus.Description)
                        .Select(g => new { Status = g.Key, Total = g.Count() })
                        .ToList();

                    this.PieChart1.DataBind();

                    //job iterations average duration by job
                    this.RadHtmlChart2.DataSource = context.SqlJobStepIterations
                        .Where(i => i.SqlJobStepID == 0 && i.RunDateTime > day)
                        .GroupBy(i => i.SqlJobStep.SqlJob.Name)
                        .Select(g => new { JobName = g.Key, AverageTime = Math.Round(g.Average(x => x.RunDurationMinutes).Value, 2) })
                        .ToList()
                        .OrderBy(x => x.AverageTime);
                    this.RadHtmlChart2.DataBind();

                    this.ViewState[CURRENT_JOB_STATUS_DATA] = context.SqlJobs.Select(c => new SqlJobsViewModel { Name = c.Name, Last_Run_DateTime = c.Last_Run_DateTime, Last_Outcome_Message = c.Last_Outcome_Message, Status = c.JobRunStatus.Description }).ToList();
                    this.gridview_Jobs.DataSource = this.ViewState[CURRENT_JOB_STATUS_DATA];
                    this.gridview_Jobs.DataBind();

                    this.ViewState[CURRENT_JOB_ITERATION_FAILED_DATA] = context.SqlJobStepIterations
                    .Where(c => c.SqlJobStepID == 0 && (c.Job_Run_Status_ID == 0))
                    .OrderByDescending(c => c.RunDateTime)
                    .Take(50)
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
                    this.gridview_JobsFailed.DataSource = this.ViewState[CURRENT_JOB_ITERATION_FAILED_DATA];
                    this.gridview_JobsFailed.DataBind();
                }
            }
        }

        #region GridViewJobStatus

        protected void gridview_Jobs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridview_Jobs.DataSource = this.ViewState[CURRENT_JOB_STATUS_DATA];
        }

        protected void gridview_Jobs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (dataItem["Status"].Text == "Succeeded")
                {
                    dataItem.BackColor = Color.LightGreen;
                    dataItem.Font.Bold = true;
                }

                else if (dataItem["Status"].Text == "Failed")
                {
                    dataItem.BackColor = Color.Red;
                    dataItem.ForeColor = Color.WhiteSmoke;
                    dataItem.Font.Bold = true;
                }
            }
        }

        #endregion GridViewJobStatus

        #region GridViewJobFailed

        protected void gridview_JobsFailed_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridview_JobsFailed.DataSource = this.ViewState[CURRENT_JOB_ITERATION_FAILED_DATA];
        }

        protected void gridview_JobsFailed_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                if (dataItem["Status"].Text == "Succeeded")
                {
                    dataItem["Status"].ForeColor = Color.Green;
                    //dataItem.Font.Bold = true;
                }
                else if (dataItem["Status"].Text == "Failed")
                {
                    //dataItem.BackColor = Color.PaleVioletRed;
                    dataItem["Status"].ForeColor = Color.Red;
                    dataItem.Font.Bold = true;
                }
            }
        }

        protected void gridview_JobsFailed_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;

            switch (e.DetailTableView.Name)
            {
                case "Steps":
                    {
                        int iterationID = int.Parse(dataItem.GetDataKeyValue("SqlJobStepIterationID").ToString());
                        using (var context = new SkypeIntlMonitoringContext())
                        {
                            var data = context.SqlJobStepIterations
                                .Where(i => i.ParentIterationID == iterationID)
                                .OrderBy(i => i.SqlJobStepIterationID)
                                .Select(c => new
                                {
                                    StepName = c.SqlJobStep.Name,
                                    c.RunDateTime,
                                    c.Outcome_Message,
                                    Status = c.JobRunStatus.Description
                                })
                                .ToList();

                            e.DetailTableView.DataSource = data;
                        }
                        break;
                    }
            }
        }

        #endregion GridViewJobFailed


    }
}
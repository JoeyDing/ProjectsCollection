using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.JobStatus
{
    public partial class JobStatusControl : System.Web.UI.UserControl, IJobStatusView
    {
        public event Func<DateTime, DateTime, List<New_SqlJobModel>> GetNewJobTree;

        public event Func<List<New_SqlJobModel>, DateTime, DateTime, List<New_SqlJobModel>> GetRefreshedNewJobTree;

        public event Func<string, DateTime, DateTime, List<New_SqlJobRecordModel>> GetJobRecords;

        public event Func<string, int, DateTime, List<New_SqlJobStepModel>> GetStepStatus;

        //JobList to store JobList. Then I shouldn't connnet to DB everytime when I want to see the JobList>
        //But if the selected Date Rang is changed, then the JobList will be Changed.
        public List<New_SqlJobModel> JobList
        {
            get
            {
                return ViewState["JobList"] as List<New_SqlJobModel>;
            }
            set { ViewState["JobList"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //When the first time need to bound the source. Only shows the Jobs run in 24 hours
        protected void RadGrid_JobStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (JobList == null)
            {
                JobList = this.GetNewJobTree(DateTime.Now.AddDays(-1), DateTime.Now);
            }
            RadGrid_JobStatus.DataSource = JobList;
        }

        //There are three level for the telerik guid table. So there need to judge which level should be use.
        protected void RadGrid_JobStatus_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            DateTime startTime = this.GetStartTime();
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "JobRunRecord":
                    {
                        string jobName = dataItem.GetDataKeyValue("JobName").ToString();
                        e.DetailTableView.DataSource = this.GetJobRecords(jobName, startTime, DateTime.Now);
                        break;
                    }
                case "StepRecordInJobRecord":
                    {
                        int jobInstanceID = (int)dataItem.GetDataKeyValue("JobInstanceID");
                        var jobRunDateTime = Convert.ToDateTime(dataItem["JobRunDateTime"].Text);
                        string jobName = dataItem.OwnerTableView.ParentItem["JobName"].Text;
                        var stepDetail = this.GetStepStatus(jobName, jobInstanceID, jobRunDateTime);
                        e.DetailTableView.DataSource = stepDetail;
                        break;
                    }
            }
        }

        //Judge start time by users selecting in comboBox
        protected DateTime GetStartTime()
        {
            int periodSelect = this.RadComboBoxPeriodFilter.SelectedIndex;

            switch (periodSelect)
            {
                case 0:
                    return DateTime.Now.AddDays(-1);

                case 1:
                    return DateTime.Now.AddDays(-2);

                case 2:
                    return DateTime.Now.AddDays(-7);

                default:
                    return DateTime.Now.AddDays(-1);
            }
        }

        //status image choose method
        protected void RadGrid_JobStatus_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ToolDetails")
            {
                //Get the instance of the right type
                GridDataItem dataBoundItem = e.Item as GridDataItem;

                //if(dataBoundItem.GetDataKeyValue("ID").ToString() == "you Compared Text") // you can also use datakey also
                switch (dataBoundItem["JobStatus"].Text)
                {
                    case "0":

                        RadBinaryImage binaryImage1 = dataBoundItem["Image"].FindControl("RadBinaryImage1") as RadBinaryImage;
                        binaryImage1.ImageUrl = @"~\Images\Red.jpg";
                        break;

                    case "1":
                        RadBinaryImage binaryImage2 = dataBoundItem["Image"].FindControl("RadBinaryImage1") as RadBinaryImage;
                        binaryImage2.ImageUrl = @"~\Images\Green.jpg";
                        break;

                    case "5":
                        RadBinaryImage binaryImage3 = dataBoundItem["Image"].FindControl("RadBinaryImage1") as RadBinaryImage;
                        binaryImage3.ImageUrl = @"~\Images\Yellow.jpg";
                        break;
                }
            }
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "JobRunRecord")
            {
                //Get the instance of the right type
                GridDataItem dataBoundItemForJobRecord = e.Item as GridDataItem;
                switch (dataBoundItemForJobRecord["JobRunStatus"].Text)
                {
                    case "0":

                        RadBinaryImage binaryImage1 = dataBoundItemForJobRecord["JobRecordImage"].FindControl("RadBinaryJobRecordImage1") as RadBinaryImage;
                        binaryImage1.ImageUrl = @"~\Images\Red.jpg";
                        break;

                    case "1":
                        RadBinaryImage binaryImage2 = dataBoundItemForJobRecord["JobRecordImage"].FindControl("RadBinaryJobRecordImage1") as RadBinaryImage;
                        binaryImage2.ImageUrl = @"~\Images\Green.jpg";
                        break;

                    default:
                        RadBinaryImage binaryImage3 = dataBoundItemForJobRecord["JobRecordImage"].FindControl("RadBinaryJobRecordImage1") as RadBinaryImage;
                        binaryImage3.ImageUrl = @"~\Images\Yellow.jpg";
                        break;
                }
            }
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "StepRecordInJobRecord")
            {
                //Get the instance of the right type
                GridDataItem dataBoundItemForJobRecord = e.Item as GridDataItem;
                switch (dataBoundItemForJobRecord["StepRunStatus"].Text)
                {
                    case "0":

                        RadBinaryImage binaryImage1 = dataBoundItemForJobRecord["JobStepImage"].FindControl("RadBinaryJobStepImage1") as RadBinaryImage;
                        binaryImage1.ImageUrl = @"~\Images\Red.jpg";
                        break;

                    case "1":
                        RadBinaryImage binaryImage2 = dataBoundItemForJobRecord["JobStepImage"].FindControl("RadBinaryJobStepImage1") as RadBinaryImage;
                        binaryImage2.ImageUrl = @"~\Images\Green.jpg";
                        break;

                    case "5":
                        RadBinaryImage binaryImage4 = dataBoundItemForJobRecord["JobStepImage"].FindControl("RadBinaryJobStepImage1") as RadBinaryImage;
                        binaryImage4.ImageUrl = @"~\Images\Yellow.jpg";
                        break;

                    default:
                        RadBinaryImage binaryImage3 = dataBoundItemForJobRecord["JobStepImage"].FindControl("RadBinaryJobStepImage1") as RadBinaryImage;
                        binaryImage3.ImageUrl = @"~\Images\Yellow.jpg";
                        break;
                }
            }
        }

        protected void RadComboBoxStatusFilter_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //based on what user selected , method from presenter'll be invoked
            int selectedItem = this.RadComboBoxStatusFilter.SelectedIndex;

            this.RadGrid_JobStatus.DataSource = this.GetFilteredStatusData(selectedItem);
            this.RadGrid_JobStatus.Rebind();
        }

        protected List<New_SqlJobModel> GetFilteredStatusData(int selectId)
        {
            if (selectId == 0) return JobList;
            int filterStatus = selectId;
            if (filterStatus == 3) filterStatus = 5;
            if (filterStatus == 2) filterStatus = 0;
            List<New_SqlJobModel> filterJobList = new List<New_SqlJobModel>();
            foreach (var job in JobList)
            {
                if (job.JobStatus == filterStatus)
                    filterJobList.Add(job);
            }
            return filterJobList;
        }

        protected void RadComboBoxPeriodFilter_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int selectPeriod = this.RadComboBoxPeriodFilter.SelectedIndex;
            switch (selectPeriod)
            {
                case 0:
                    JobList = this.GetNewJobTree(DateTime.Now.AddDays(-1), DateTime.Now);
                    break;

                case 1:
                    JobList = this.GetNewJobTree(DateTime.Now.AddDays(-2), DateTime.Now);
                    break;

                case 2:
                    JobList = this.GetNewJobTree(DateTime.Now.AddDays(-7), DateTime.Now);
                    break;
            }
            RadGrid_JobStatus.DataSource = JobList;
            this.RadComboBoxStatusFilter.SelectedIndex = 0;
            this.TextOfTitle.Text = "SQL Job Status In " + this.RadComboBoxPeriodFilter.SelectedItem.Text;
            this.RadGrid_JobStatus.Rebind();
        }

        protected void RadGrid_JobStatus_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            GridTableView tableView = e.Item.OwnerTableView;
            string sortExpression = e.SortExpression;
            if (sortExpression == "JobName" || sortExpression == "JobLatestRunTime" || sortExpression == "JobLatestRunFinishTime")
            {
                GridSortExpression expression = new GridSortExpression();
                if (tableView.SortExpressions.Count == 0)
                {
                    expression.SortOrder = GridSortOrder.Descending;
                }
                else if (tableView.SortExpressions[0].SortOrder == GridSortOrder.Descending)
                {
                    expression.SortOrder = GridSortOrder.Ascending;
                }
                else if (tableView.SortExpressions[0].SortOrder == GridSortOrder.Ascending)
                {
                    expression.SortOrder = GridSortOrder.None;
                }
                tableView.SortExpressions.AddSortExpression(expression);
                tableView.Rebind();
            }
        }

        protected void RadButton_refresh_Click(object sender, EventArgs e)
        {
            JobList = this.GetNewJobTree(DateTime.Now.AddDays(-1), DateTime.Now);
            RadGrid_JobStatus.DataSource = JobList;
            this.RadGrid_JobStatus.Rebind();
        }
    }
}
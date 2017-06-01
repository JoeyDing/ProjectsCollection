using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.JobHistory
{
    public partial class JobHistoryControl : System.Web.UI.UserControl, IJobHistoryView
    {
        public event Func<DateTime, DateTime, List<New_SqlJobModel>> GetNewJobTree;

        public event Func<string, DateTime, DateTime, List<New_SqlJobRecordModel>> GetJobRecords;

        public event Func<string, int, DateTime, List<New_SqlJobStepModel>> GetStepStatus;

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

        protected void RadGrid_JobStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (this.RadDatePickerStartDate.SelectedDate == null)
            { this.RadDatePickerStartDate.SelectedDate = DateTime.Now.AddDays(-7); }
            if (this.RadDatePickerEndDate.SelectedDate == null)
            { this.RadDatePickerEndDate.SelectedDate = DateTime.Now; }
            DateTime startTime = (DateTime)this.RadDatePickerStartDate.SelectedDate;
            DateTime endTime = (DateTime)this.RadDatePickerEndDate.SelectedDate;
            var jobList = this.GetNewJobTree(startTime, endTime);
            RadGrid_JobStatus.DataSource = jobList; ;
        }

        protected void RadGrid_JobStatus_DetailTableDataBind(object sender, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            if (this.RadDatePickerStartDate.SelectedDate == null)
            { this.RadDatePickerStartDate.SelectedDate = DateTime.Now.AddDays(-7); }
            if (this.RadDatePickerEndDate.SelectedDate == null)
            { this.RadDatePickerEndDate.SelectedDate = DateTime.Now; }
            DateTime startTime = (DateTime)this.RadDatePickerStartDate.SelectedDate;
            DateTime endTime = (DateTime)this.RadDatePickerEndDate.SelectedDate;
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "JobRunRecord":
                    {
                        string jobName = dataItem.GetDataKeyValue("JobName").ToString();
                        e.DetailTableView.DataSource = this.GetJobRecords(jobName, startTime, endTime);
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

        protected void RadGrid_JobStatus_ItemDataBound(object sender, GridItemEventArgs e)
        {
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
            //string selectedItem = e.Text;
            //if (this.GetFilteredStatusData != null)
            //{
            //    this.RadGrid_JobStatus.DataSource = this.GetFilteredStatusData(selectedItem);
            //    this.RadGrid_JobStatus.Rebind();
            //}
        }

        protected void DateFilter_Click(object sender, EventArgs e)
        {
            if (this.RadDatePickerStartDate.SelectedDate == null)
            { this.RadDatePickerStartDate.SelectedDate = Convert.ToDateTime("1900-1-1"); }
            if (this.RadDatePickerEndDate.SelectedDate == null)
            { this.RadDatePickerEndDate.SelectedDate = DateTime.Now; }
            DateTime startTime = (DateTime)this.RadDatePickerStartDate.SelectedDate;
            DateTime endTime = (DateTime)this.RadDatePickerEndDate.SelectedDate;
            JobList = this.GetNewJobTree(startTime, endTime);
            RadGrid_JobStatus.DataSource = JobList;
            //this.RadComboBoxStatusFilter.SelectedIndex = 0;
            this.RadGrid_JobStatus.Rebind();
        }
    }
}
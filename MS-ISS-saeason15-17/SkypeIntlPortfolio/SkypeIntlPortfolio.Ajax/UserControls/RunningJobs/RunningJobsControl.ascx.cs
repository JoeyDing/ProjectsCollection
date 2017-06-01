using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.RunningJobs
{
    public partial class RunningJobsControl : UserControl,IRunningJobsView
    {
        public event Func<DateTime, List<NewRunningJobModel>> GetRunningJobs;
        public event Func<int, List<New_RuningStepsInPeriodModel>> GetRuningStepsInPeriod;
        public event Func<DateTime> GetServerTime;
        public event Func<string, List<New_LiveJobStepModel>> GetJobLiveStepStatus;
        public static DateTime ServerTime = DateTime.Now;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Refresh server time
            ServerTime = this.GetServerTime();
            this.GettingServerTime.Text = ServerTime.ToString();
        }

        protected void RadGrid_RunningJobs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<NewRunningJobModel>  RunningJobList = this.GetRunningJobs(ServerTime);
            RadGrid_RunningJobs.DataSource = RunningJobList;
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            List<NewRunningJobModel>  RunningJobList = this.GetRunningJobs(ServerTime);
            RadGrid_RunningJobs.DataSource = RunningJobList;
            this.RadGrid_RunningJobs.Rebind();
            int selectedItem = this.RadComboBoxPeriodFilter1.SelectedIndex;
            List<New_RuningStepsInPeriodModel> RuningStepsInPeriod = this.GetRuningStepsInPeriod(selectedItem);
            RadGrid_RunedStepInPeriod.DataSource = RuningStepsInPeriod;
            this.RadGrid_RunedStepInPeriod.Rebind();
        }
        protected void RadGrid_RunedStepInPeriod_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            int selectedPeriod = this.RadComboBoxPeriodFilter1.SelectedIndex;
            List<New_RuningStepsInPeriodModel> RuningStepsInPeriod = this.GetRuningStepsInPeriod(selectedPeriod);
            RadGrid_RunedStepInPeriod.DataSource = RuningStepsInPeriod;
        }

        protected void RadGrid_RunRecordsInPeroid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;

                switch (dataBoundItem["StepRunStatus"].Text)
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
                    default:
                        RadBinaryImage binaryImage4 = dataBoundItem["Image"].FindControl("RadBinaryImage1") as RadBinaryImage;
                        binaryImage4.ImageUrl = @"~\Images\Yellow.jpg";
                        break;
                }
            }
        }
        protected void RadComboBoxPeriodFilter_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.TextOfTitle.Text = "Last Run SQL Jobs In The " + RadComboBoxPeriodFilter1.Text;
            //based on what user selected , method from presenter'll be invoked
            int selectedItem = this.RadComboBoxPeriodFilter1.SelectedIndex;
            List<New_RuningStepsInPeriodModel> RuningStepsInPeriod = this.GetRuningStepsInPeriod(selectedItem);
            RadGrid_RunedStepInPeriod.DataSource = RuningStepsInPeriod;
            this.RadGrid_RunedStepInPeriod.Rebind();
        }

        protected void RadGrid_LiveStatus_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            if (e.DetailTableView.Name == "FinishedStepsInRunningJob")
            {
                string jobName = e.DetailTableView.ParentItem.GetDataKeyValue("JobName").ToString();
                e.DetailTableView.DataSource = this.GetJobLiveStepStatus(jobName);
            }
        }
    }
}
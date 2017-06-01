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
    public partial class JobDetails : System.Web.UI.Page
    {
        public const string CURRENT_JOB_ITERATION_LIST_DATA = "CURRENT_JOB_ITERATION_LIST_DATA";
        public static SkypeIntlMonitoringContext dbContext = new SkypeIntlMonitoringContext();
        public static List<SqlJobStepIteration> stepHistory = new List<SqlJobStepIteration>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                using (var context = new SkypeIntlMonitoringContext())
                {
                    this.radCombo_jobs.DataSource = context.SqlJobs.Select(c => new { JobName = c.Name, c.SqlJobID }).OrderBy(j=>j.JobName).ToList();
                    this.radCombo_jobs.DataBind();



                    this.radlistbox_sjobteps.DataSource = context.SqlJobs.First().SqlJobSteps.Select(c => new { JobStepName = c.Name, JobStepID = c.SqlJobStepID });
                    this.radlistbox_sjobteps.DataBind();
                }

                foreach (RadListBoxItem item in this.radlistbox_sjobteps.Items)
                {
                    item.Checked = true;
                }
                //radButton_update_Click(null, null);

                //grid job list
                UpdateGridData();
                this.gridview_JobsIterations.DataSource = this.ViewState[CURRENT_JOB_ITERATION_LIST_DATA];
                this.gridview_JobsIterations.DataBind();
                var now = DateTime.Now;
                var date = new DateTime(now.Year,now.Month,now.Day,0,0,0);
                date = date.AddDays(-1);
                stepHistory= dbContext.SqlJobStepIterations.Where(i => i.RunDateTime > date && i.Job_Run_Status_ID != 4).OrderBy(i => i.RunDateTime).ToList();

            }
        }


        protected void radCombo_jobs_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            using (var context = new SkypeIntlMonitoringContext())
            {
                var jobID = Guid.Parse(e.Value);
                this.radlistbox_sjobteps.DataSource = context.SqlJobs.First(c => c.SqlJobID == jobID).SqlJobSteps.Select(c => new { JobStepName = c.Name, JobStepID = c.SqlJobStepID });
                this.radlistbox_sjobteps.DataBind();
            }

            foreach (RadListBoxItem item in this.radlistbox_sjobteps.Items)
            {
                item.Checked = true;
            }

            UpdateGridData();
          
            this.gridview_JobsIterations.DataSource = this.gridview_JobsIterations.DataSource = this.ViewState[CURRENT_JOB_ITERATION_LIST_DATA];
            this.gridview_JobsIterations.DataBind();
            UpdateChart();
        }

        private void AddTimeSeries1()
        {
            List<SqlJobStepIteration> data = null;
            using (var context = new SkypeIntlMonitoringContext())
            {
                var date = DateTime.Now.AddDays(-1);
                data = context.SqlJobStepIterations.Where(i => i.RunDateTime > date && i.Job_Run_Status_ID != 4).OrderBy(i => i.RunDateTime).ToList();
            }

            var checkItems = this.radlistbox_sjobteps.CheckedItems.ToList();
            this.radchart_stepsDuration.PlotArea.Series.Clear();
            this.radchart_stepsDuration.PlotArea.XAxis.Items.Clear();

            for (int i = 0; i < checkItems.Count; i++)
            {
                var item = checkItems[i];
                var lineSerie = new LineSeries();
                lineSerie.Name = item.Text;
                lineSerie.LineAppearance.LineStyle = Telerik.Web.UI.HtmlChart.Enums.ExtendedLineStyle.Step;
                var stepID = int.Parse(item.Value);
                var jobID = Guid.Parse(this.radCombo_jobs.SelectedValue);
                var allIterations = data.Where(j => j.SqlJobStepID == stepID && j.SqlJobID == jobID);
                var iterations = allIterations.Skip(allIterations.Count() < 15 ? 0 : allIterations.Count() - 15).ToList();
                foreach (var iteration in iterations)
                {
                    if (i == 0)
                    {
                        this.radchart_stepsDuration.PlotArea.XAxis.Items.Add(iteration.RunDateTime.Value.ToShortTimeString());

                    }
                    lineSerie.Items.Add(Convert.ToDecimal(iteration.RunDurationMinutes));

                }
                this.radchart_stepsDuration.PlotArea.Series.Add(lineSerie);
            }
        }
        private List<SqlJobStepIteration> GetIteration(Guid jobID, int stepID)
        {
                return stepHistory.Where(j => j.SqlJobStepID == stepID && j.SqlJobID == jobID).ToList();
                //var iterations = allIterations.Skip(allIterations.Count() < 15 ? 0 : allIterations.Count() - 15).ToList();
           
        }
        private void AddTimeSeries2()
        {
            this.radchart_stepsDuration.PlotArea.Series.Clear();
            this.radchart_stepsDuration.PlotArea.XAxis.Items.Clear();
            this.radchart_stepsDuration.Transitions = false;

           
            Dictionary<string,List<SqlJobStepIteration>> info = new Dictionary<string, List<SqlJobStepIteration>>();
            var checkItems = this.radlistbox_sjobteps.CheckedItems.ToList();

            HashSet<DateTime> timeline = new HashSet<DateTime>(); 
            for (int i = 0; i < checkItems.Count; i++)
            {
                var item = checkItems[i];
                var stepID = int.Parse(item.Value);
                var jobID = Guid.Parse(this.radCombo_jobs.SelectedValue);
                List<SqlJobStepIteration> iter= GetIteration(jobID, stepID);
                foreach (SqlJobStepIteration it in iter)
                {
                    timeline.Add(it.RunDateTime.Value);
                }
                info[item.Text] = iter;
            }
            DateTime now = DateTime.Now;
            DateTime start_dt = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(-1);
            DateTime end_dt= new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            if (timeline.Count > 0)
            {
                start_dt = timeline.Min();
                end_dt = timeline.Max();
            }
            start_dt = start_dt.AddHours(-1);
            end_dt = end_dt.AddHours(1);
            int diff_h = (end_dt-start_dt).Hours;
            
            for (int i=0;i<diff_h*60; i++)
            {
                DateTime cdt = start_dt.AddMinutes(i);
                this.radchart_stepsDuration.PlotArea.XAxis.Items.Add(cdt.ToShortTimeString());
            }
             this.radchart_stepsDuration.PlotArea.XAxis.LabelsAppearance.Step = 10;
            this.radchart_stepsDuration.PlotArea.XAxis.LabelsAppearance.RotationAngle = 60;

            foreach (KeyValuePair<String,List<SqlJobStepIteration>> pair in info)
            {
                var serie = new AreaSeries();
                serie.Name = pair.Key;
                serie.LineAppearance.LineStyle = Telerik.Web.UI.HtmlChart.Enums.ExtendedLineStyle.Step;
                serie.LabelsAppearance.Visible = false;
                serie.MarkersAppearance.Visible = false;
                List<SqlJobStepIteration> iterations = pair.Value;
                Dictionary<DateTime, int> jobtimeline = new Dictionary<DateTime, int>();
                foreach (var iteration in iterations)
                {
                    DateTime bdt = iteration.RunDateTime.Value;
                    int duration = iteration.RunDurationMinutes.Value == 0 ? 1 : iteration.RunDurationMinutes.Value;
                    DateTime edt = bdt.AddMinutes(duration);
                    for (int i = 0; i < diff_h * 60; i++)
                    {
                        DateTime cdt = start_dt.AddMinutes(i);
                        if (cdt >= bdt && cdt <= edt&& (!jobtimeline.ContainsKey(cdt)||jobtimeline[cdt]==0))
                        {
                            jobtimeline[cdt] = duration;  
                        }
                        else if(!jobtimeline.ContainsKey(cdt)) {
                            jobtimeline[cdt] = 0;
                        }
                    }
                  
                }
                foreach (KeyValuePair<DateTime, int> tl in jobtimeline)
                {
                    serie.Items.Add(tl.Value);
                }
                this.radchart_stepsDuration.PlotArea.Series.Add(serie);
                
            }
        }


        private void UpdateChart()
        {
             AddTimeSeries2();
        }
     
        #region RadGrid_JobList

        private void UpdateGridData()
        {
            /*using (var context = new SkypeIntlMonitoringContext())
            {
                var currentJobSelected = Guid.Parse(this.radCombo_jobs.SelectedValue);
                this.ViewState[CURRENT_JOB_ITERATION_LIST_DATA] = context.SqlJobStepIterations
                   .Where(c => c.SqlJobID == currentJobSelected && c.SqlJobStepID == 0)
                   .OrderByDescending(c => c.SqlJobStepIterationID)
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
            }*/
            List<RadListBoxItem> checkItems = this.radlistbox_sjobteps.CheckedItems.ToList();
            List<int> stepIds = new List<int>();
            foreach (RadListBoxItem item in checkItems)
            {
                stepIds.Add(int.Parse(item.Value));
            }
            var jobID = Guid.Parse(this.radCombo_jobs.SelectedValue);

            var job = stepHistory.Where(h => h.SqlJobID.Equals(jobID) && stepIds.Contains(h.SqlJobStepID)).ToList();
            List<SqlJobIterationsViewModel> vm = 
                  job
                   .Select(c => new SqlJobIterationsViewModel
                   {
                       SqlJobStepIterationID = c.SqlJobStepIterationID,
                       JobName = c.SqlJobStep.SqlJob.Name,
                       StepName = c.SqlJobStep.Name,
                       RunDateTime = c.RunDateTime,
                       Outcome_Message = c.Outcome_Message,
                       Status = c.JobRunStatus.Description,
                       RunDateHour= c.RunDateTime.Value.Hour,
                       Duration=MinuteToHour( c.RunDurationMinutes.Value)
                   }).
                   ToList();

            this.ViewState[CURRENT_JOB_ITERATION_LIST_DATA] = vm.OrderByDescending(v=>v.RunDateTime).ToList();
        }

        private string SecondToHour(int seconds)
        {
            string rs = "";
            int hour = seconds / 3600;
            if (hour > 0)
            {
                rs += hour + " hr ";
            }
            int minute = (seconds- hour * 3600)/60;
            if (minute > 0)
            {
                rs += minute + " m";
            }
            seconds = seconds - hour * 3600 - minute * 60;
            if (seconds > 0)
            {
                rs += seconds + " s";
            }
            return rs;
        }

        private string MinuteToHour(int minutes)
        {
            string rs = "";
            int hour = minutes / 60;
            if (hour > 0)
            {
                rs += hour + " hr ";
            }
            minutes = minutes - hour * 60;
            if (minutes > 0)
            {
                rs += minutes + " m";
            }

            if (hour == 0 && minutes == 0)
            {
                rs = "<1 m";
            }
            return rs;
        }

        public void gridview_JobsIterations_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
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

        public void gridview_JobsIterationsNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridview_JobsIterations.DataSource = this.ViewState[CURRENT_JOB_ITERATION_LIST_DATA];
        }

        public void gridview_JobsIterations_ItemDataBound(object sender, GridItemEventArgs e)
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


        #endregion RadGrid_JobList

        protected void radlistbox_sjobteps_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            UpdateChart();
            UpdateGridData();
        }
    }
}
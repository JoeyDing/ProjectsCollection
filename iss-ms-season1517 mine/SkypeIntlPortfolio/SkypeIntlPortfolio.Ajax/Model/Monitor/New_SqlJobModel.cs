using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Monitor
{
    [Serializable]
    public class New_SqlJobModel
    {
        public string JobName { get; set; }
        public DateTime JobLatestRunTime { get; set; }

        public int JobStatus { get; set; }
        public int JobLatestDurationRunTime { get; set; }

        public DateTime JobLatestRunFinishTime { get; set; }

        public TimeSpan JobLatestDurationRunTimeSpan
        { get { return TimeSpan.FromSeconds(JobLatestDurationRunTime); } }

        public List<New_SqlJobRecordModel> JobRecords { get; set; }
    }

    [Serializable]
    public class New_SqlJobRecordModel
    {
        public int JobInstanceID { get; set; }
        public int JobRunStatus { get; set; }
        public DateTime JobRunDateTime { get; set; }
        public int JobDurationRunTime { get; set; }

        public TimeSpan JobDurationRunTimeSpan
        { get { return TimeSpan.FromSeconds(JobDurationRunTime); } }

        public string JobOutMessage { get; set; }
        public List<New_SqlJobStepModel> JobRecords { get; set; }
    }

    [Serializable]
    public class New_SqlJobStepModel
    {
        public int StepRunStatus { get; set; }
        public string JobStepName { get; set; }
        public int JobStepID { get; set; }
        public DateTime RunDateTime { get; set; }
        public int RunDurationTime { get; set; }

        public TimeSpan RunDurationTimeSpan
        { get { return TimeSpan.FromSeconds(RunDurationTime); } }

        public string Failed_Outcome_Message { get; set; }
    }
}
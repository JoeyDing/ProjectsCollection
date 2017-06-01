using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Monitor
{

    [Serializable]
    public class NewRunningJobModel
    {
        public string JobName { get; set; }
        public DateTime JobStartTime { get; set; }
        public int JobRunningTime { get; set; }
        public int CurrentStepId { get; set; }
        public string CurrentStepName { get; set; }
        
            public TimeSpan JobRunningTimeSpan
        { get { return TimeSpan.FromSeconds(JobRunningTime); } }

    }
    [Serializable]
    public class New_LiveJobStepModel
    {
        public string JobStepName { get; set; }
        public int JobStepID { get; set; }
        public DateTime StartTime { get; set; }
        public int RunDurationTime { get; set; }
        public TimeSpan RunDurationTimeSpan
        { get { return TimeSpan.FromSeconds(RunDurationTime); } }

    }
}
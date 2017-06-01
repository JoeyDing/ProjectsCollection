using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Monitor
{
    //This model are used in Running page 
    [Serializable]
    public class New_RuningStepsInPeriodModel
    {
        public int InstanceId { get; set; }
        public string JobName { get; set; }
        public int StepRunStatus { get; set; }
        public string JobStepName { get; set; }
        public int JobStepID { get; set; }
        public DateTime RunDateTime { get; set; }
        public int RunDurationTimeSeconds { get; set; }
        public TimeSpan RunDurationTimeSecondsSpan
        { get { return TimeSpan.FromSeconds(RunDurationTimeSeconds); }
}
public DateTime RunEndTime { get; set; }
        public string StatusText
        {
            get
            {
                switch(StepRunStatus)
                {
                    case (0): return "Failed";
                    case (1): return "Succeed";
                    case (5): return "Unknow";
                    default: return "Unknow";
                }
            }
        }
    }
}
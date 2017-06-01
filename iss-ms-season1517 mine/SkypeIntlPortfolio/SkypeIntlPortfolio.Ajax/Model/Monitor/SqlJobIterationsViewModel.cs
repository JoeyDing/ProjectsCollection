using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Monitor
{
    [Serializable]
    public class SqlJobIterationsViewModel
    {
        public int SqlJobStepIterationID { get; set; }

        public string JobName { get; set; }

        public string StepName { get; set; }

        public string Status { get; set; }

        public DateTime? RunDateTime { get; set; }

        public string Outcome_Message { get; set; }

        public int RunDateHour { get; set; }
        
        public string Duration { get; set; }
    }
}
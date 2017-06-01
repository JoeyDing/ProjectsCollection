using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Monitor
{
    [Serializable]
    public class SqlJobsViewModel
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public DateTime? Last_Run_DateTime { get; set; }

        public string Last_Outcome_Message { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public class BugInfo
    {
        public int Bug_ID { get; set; }
        public string Bug_Status { get; set; }
        public string Project_Name { get; set; }
        public string Project_Area_Path { get; set; }
        public string Project_Iteration_Path { get; set; }
        public string Bug_Title { get; set; }
        public string Bug_Assigned_To { get; set; }
        public string Bug_Reason { get; set; }
        public string Bug_Created_By { get; set; }
        public string Bug_Priority { get; set; }
        public string Bug_Severity { get; set; }
        public string Bug_Issue_Type { get; set; }
        public string Bug_World_Readiness_Impact { get; set; }
        public string Bug_Localization_Impact { get; set; }
        public Nullable<System.DateTime> Bug_Created_Date { get; set; }
        public Nullable<System.DateTime> Bug_Closed_Date { get; set; }
        public Nullable<System.DateTime> Bug_Resolved_Date { get; set; }
    }
}
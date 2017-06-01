using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.VsoWorkItem
{
    public class WorkItem
    {
        public int ID;
        public string Title;
        public string AreaPath;
        public string IterationPath;
        public string WorkItemType;
        public List<string> Tags;
        public DateTime DueDate;
        public DateTime LocStartDate;
        public string Family;
        public string ProductName;
        public DateTime ChangedDate;
        public string AssignedTo;
        public string State;
        public string Url;
    }
}
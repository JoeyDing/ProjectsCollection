using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.VsoWorkItem
{
    public class WorkItemAreaPathComparer : IEqualityComparer<WorkItem>
    {
        public bool Equals(WorkItem x, WorkItem y)
        {
            return x.AreaPath.Trim().ToLower() == y.AreaPath.Trim().ToLower();
        }

        public int GetHashCode(WorkItem obj)
        {
            var code = obj.AreaPath.Trim().ToLower().GetHashCode();
            return code;
        }
    }
}
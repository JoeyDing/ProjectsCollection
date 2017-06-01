using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public class LinkComparer : IEqualityComparer<WorkItemLink>
    {
        public bool Equals(WorkItemLink x, WorkItemLink y)
        {
            return x.Source_ID == y.Source_ID
                && x.Target_ID == y.Target_ID
                && x.Changed_Date == y.Changed_Date
                && x.Link_Type == y.Link_Type
                && x.Is_Active == y.Is_Active;
        }

        public int GetHashCode(WorkItemLink obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.Source_ID.GetHashCode();
            hash = hash * 31 + obj.Target_ID.GetHashCode();
            hash = hash * 31 + obj.Changed_Date.GetHashCode();
            hash = hash * 31 + obj.Link_Type.GetHashCode();
            hash = hash * 31 + obj.Is_Active.GetHashCode();
            return hash;
        }
    }
}
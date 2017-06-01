using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public class CoreBugComparer : IEqualityComparer<CoreBugInfo>
    {
        public bool Equals(CoreBugInfo x, CoreBugInfo y)
        {
            return x.Loc_Bug_ID == y.Loc_Bug_ID && x.Core_Bug_ID == y.Core_Bug_ID;
        }

        public int GetHashCode(CoreBugInfo obj)
        {
            int hash = 13;
            hash = hash * 21 + obj.Loc_Bug_ID.GetHashCode();
            hash = hash * 21 + obj.Core_Bug_ID.GetHashCode();
            return hash;
        }
    }
}
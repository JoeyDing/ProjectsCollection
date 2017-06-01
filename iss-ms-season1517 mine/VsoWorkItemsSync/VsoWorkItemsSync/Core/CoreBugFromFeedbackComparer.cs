using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public class CoreBugFromFeedbackComparer : IEqualityComparer<CoreBugInfosFromFeedback>
    {
        public bool Equals(CoreBugInfosFromFeedback x, CoreBugInfosFromFeedback y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(CoreBugInfosFromFeedback obj)
        {
            int hash = 13;
            hash = hash * 21 + obj.ID.GetHashCode();
            return hash;
        }
    }
}
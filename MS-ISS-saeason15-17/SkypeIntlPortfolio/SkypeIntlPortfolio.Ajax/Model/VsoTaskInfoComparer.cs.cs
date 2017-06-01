using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class VsoTaskInfoComparer : IEqualityComparer<VsoTaskInfo>
    {
        public bool Equals(VsoTaskInfo x, VsoTaskInfo y)
        {
            return x.EpicID == y.EpicID
                && x.Vsotag == y.Vsotag;
        }

        public int GetHashCode(VsoTaskInfo obj)
        {
            int hash = 5;
            hash = hash * 3 + obj.EpicID.GetHashCode();
            hash = hash * 3 + obj.Vsotag.GetHashCode();
            return hash;
        }
    }
}
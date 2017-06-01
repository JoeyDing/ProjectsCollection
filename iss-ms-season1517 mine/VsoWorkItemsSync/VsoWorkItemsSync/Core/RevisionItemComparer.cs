using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync;

namespace VsoWorkItemsSync.Core
{
    public class RevisionItemComparer<T> : IEqualityComparer<T> where T : IRevisionItem
    {
        public bool Equals(T x, T y)
        {
            return x.ID == y.ID && x.Rev == y.Rev;
        }

        public int GetHashCode(T obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.ID.GetHashCode();
            hash = hash * 31 + obj.Rev.GetHashCode();
            return hash;
        }
    }
}
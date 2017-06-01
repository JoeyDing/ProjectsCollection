using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.WorkItemsProvider;

namespace VsoWorkItemsSync.Core
{
    public class TestSuiteInfoComparer : IEqualityComparer<TestSuiteInfo>
    {
        public bool Equals(TestSuiteInfo x, TestSuiteInfo y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(TestSuiteInfo obj)
        {
            int hash = obj.ID.GetHashCode();
            return hash;
        }
    }
}
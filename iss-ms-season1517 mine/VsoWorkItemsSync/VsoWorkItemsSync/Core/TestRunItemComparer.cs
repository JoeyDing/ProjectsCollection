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
    public class TestRunItemComparer : IEqualityComparer<TestRunResult>
    {
        public bool Equals(TestRunResult x, TestRunResult y)
        {
            return x.Test_Run_ID == y.Test_Run_ID && x.Test_Case_ID == y.Test_Case_ID;
        }

        public int GetHashCode(TestRunResult obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.Test_Run_ID.GetHashCode();
            hash = hash * 31 + obj.Test_Case_ID.GetHashCode();
            return hash;
        }
    }
}
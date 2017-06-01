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
    public class TestRunItemComparer1 : IEqualityComparer<TestRunResult>
    {
        public bool Equals(TestRunResult x, TestRunResult y)
        {
            return x.Test_Run_ID == y.Test_Run_ID;
        }

        public int GetHashCode(TestRunResult obj)
        {
            int hash = obj.Test_Run_ID.GetHashCode();
            return hash;
        }
    }
}
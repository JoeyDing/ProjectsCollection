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
    public class testSuiteTestCaseMappingComparer : IEqualityComparer<TestSuiteTestCaseMapping>
    {
        public bool Equals(TestSuiteTestCaseMapping x, TestSuiteTestCaseMapping y)
        {
            return x.Test_Plan_ID == y.Test_Plan_ID && x.Test_Suite_ID == y.Test_Suite_ID && x.Test_Case_ID == y.Test_Case_ID;
        }

        public int GetHashCode(TestSuiteTestCaseMapping obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.Test_Plan_ID.GetHashCode();
            hash = hash * 31 + obj.Test_Suite_ID.GetHashCode();
            hash = hash * 31 + obj.Test_Case_ID.GetHashCode();
            return hash;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    public partial class TestSuiteInfo
    {
        public int ID { get; set; }
        public int Rev { get; set; }
        public string Test_Suite_Audit { get; set; }
        public string Query_Text { get; set; }
        public string Test_Suite_Type { get; set; }

        public string TestCaseIds { get; set; }
    }
}
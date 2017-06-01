using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Tests
{
    [TestClass()]
    public class MonitorDataManagementTests
    {
        [TestMethod()]
        public void TestGetNewJobTree()
        {
            var list = MonitorDataManagement.GetNewJobTree(DateTime.Now.AddDays(-1), DateTime.Now);
        }
    }
}
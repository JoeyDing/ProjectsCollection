using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Tests
{
    [TestClass]
    public class TestBuildVersion
    {
        [TestMethod]
        public void Test_IsBuildVersionValid_Skype()
        {
            BuildVersionValidator validatorService = new BuildVersionValidator();
            string buildVersion = "00000000_000000";

            bool result = validatorService.IsBuildVersionValid(buildVersion, false);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_IsBuildVersionValid_LyncServer()
        {
            BuildVersionValidator validatorService = new BuildVersionValidator();
            string buildVersion = "000.0.00.0";

            bool result = validatorService.IsBuildVersionValid(buildVersion, true);
            Assert.IsTrue(result);
        }
    }
}
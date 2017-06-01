using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Tests
{
    [TestClass]
    public class TestVsoApi
    {
        [TestMethod]
        public void Test_GetTestPlan()
        {
            string token = ConfigurationManager.AppSettings["VsoPrivateKey"];
            VsoContext context = new VsoContext("skype", token);
            var json = context.GetListOfTestSuitesByPlanID("LOCALIZATION", 339422);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax
{
    [TestClass]
    public class TestGetSuitesInTestPlan
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
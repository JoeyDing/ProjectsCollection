using ExtensibleVSOAPI.Lib.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixPanelDataExtraction.Api.Data.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace ExtensibleVSOAPI.Tests.UnitTests
{
    [TestClass]
    public class Test_GetVSOData
    {
        [TestMethod]
        public void ExecutePostRequest_Test()
        {
            //Arrange
            VSOContextService vsoContextService = new VSOContextService();

            //Act
            //var request = vsoContextService.ExecutePostRequest(
            //    "application/json",
            //    string.Format("{0}/{1}&{2}", vsoContextService.vsoUrl, string.Format("{0}{1}", "DefaultCollection/_apis/wit/attachments?filename=", fileName), "api-version=1.0"),
            //    content);
            //JObject json = JObject.Parse(request.Result);
            DateTime dateTime = new DateTime(2016, 01, 01);
            vsoContextService.GetVsoWorkItemByDate("LOCALIZATION", TaskTypes.Task, dateTime);
            //var reqeust = vsoContextService.GetProjects();
            //Assert
        }
    }
}
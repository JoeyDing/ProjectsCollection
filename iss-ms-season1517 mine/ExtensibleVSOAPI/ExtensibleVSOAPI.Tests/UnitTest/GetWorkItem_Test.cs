using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleDataExtraction.Lib.Services;
using ExtensibleVSOAPI;
using ExtensibleVSOAPI.Services;

using ExtensibleVSOAPI.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VsoApi.Rest;

namespace ExtensibleVSOAPI.Tests
{
    [TestClass]
    public class GetWorkItem_Test
    {
        [TestMethod]
        public void GetWorkItem()
        {
            //arrange
            VSOContextService VsoContextService = new VSOContextService();
            DateTime datetTime = new DateTime(2017, 01, 01);

            //act
            var result = VsoContextService.GetAllWorkItemRevisionsFromDate("LOCALIZATION", TaskTypes.EnablingSpecification, datetTime);
            string value = result["values"].ToString();
            bool systemId = value.Contains("\"System.Id\": 433121");
            bool systemAreaPath = value.Contains("\"System.AreaPath\": \"LOCALIZATION\"");

            //Assert
            Assert.IsTrue(systemId);
            Assert.IsTrue(systemAreaPath);
        }

        [TestMethod]
        public void GetWorkItem_FieldsNotNull()
        {
            //arrange
            VSOContextService VsoContextService = new VSOContextService();
            DateTime datetTime = new DateTime(2017, 01, 01);
            string[] fields = {
                "System.Id",
                "System.AreaId",
                "System.AreaPath",
                "System.TeamProject"
            };

            //act
            var result = VsoContextService.GetAllWorkItemRevisionsFromDate("LOCALIZATION", TaskTypes.EnablingSpecification, datetTime, fields);
            string value = result["values"].ToString();
            bool systemId = value.Contains("\"System.Id\": 433121");
            bool systemAreaPath = value.Contains("\"System.AreaPath\": \"LOCALIZATION\"");

            //Assert
            Assert.IsTrue(systemId);
            Assert.IsTrue(systemAreaPath);
        }

        [TestMethod]
        public void GetWorkItem_isLastBatch_False()
        {
            //arrange
            VSOContextService VsoContextService = new VSOContextService();
            DateTime datetTime = new DateTime(2015, 01, 01);

            //act
            var result = VsoContextService.GetAllWorkItemRevisionsFromDate("LOCALIZATION", TaskTypes.EnablingSpecification, datetTime);
            string value = result["values"].ToString();
            bool systemId = value.Contains("\"System.Id\": 433121");
            bool systemAreaPath = value.Contains("\"System.AreaPath\": \"LOCALIZATION\"");

            //Assert
            Assert.IsTrue(systemId);
            Assert.IsTrue(systemAreaPath);
        }

        [TestMethod]
        public void GetWorkItemsFromConfig()
        {
            //Arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");

            //act
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            string systemId = extensibleItems.Items.Items[0].Mapping.Fields[0].JsonFieldName;
            string systemAreaId = extensibleItems.Items.Items[0].Mapping.Fields[1].JsonFieldName;

            //Assert
            Assert.AreEqual("System.Id", systemId);
            Assert.AreEqual("System.AreaId", systemAreaId);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsTask()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Task");

            //Assert
            Assert.AreEqual(result, TaskTypes.Task);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsBug()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Bug");

            //Assert
            Assert.AreEqual(result, TaskTypes.Bug);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsEnablingSpecification()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Enabling Specification");

            //Assert
            Assert.AreEqual(result, TaskTypes.EnablingSpecification);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsEpic()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Epic");

            //Assert
            Assert.AreEqual(result, TaskTypes.Epic);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsTestCase()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Test Case");

            //Assert
            Assert.AreEqual(result, TaskTypes.TestCase);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsTestPlan()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Test Plan");

            //Assert
            Assert.AreEqual(result, TaskTypes.TestPlan);
        }

        [TestMethod]
        public void ParseService_Test_ParamIsTestSuite()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
            var items = extensibleItems.Items.Items;
            ParseService parseService = null;
            foreach (var item in items)
            {
                parseService = new ParseService(item);
            }

            //act
            var result = parseService.ReturnTaskType("Test Suite");

            //Assert
            Assert.AreEqual(result, TaskTypes.TestSuite);
        }

        [TestMethod]
        public void ParseService_OneTypeOneDate()
        {
            //Arrange
            ExtensibleItem extensibleItem = new ExtensibleItem();

            extensibleItem.JsonEndPoint = new JsonEndPoint();

            extensibleItem.JsonEndPoint.CustomLib = new CustomLib();

            extensibleItem.JsonEndPoint.CustomLib.Params = new Params();

            List<Param> fetchParamsCollection = new List<Param>();

            extensibleItem.JsonEndPoint.CustomLib.Params.ParamsCollection = new List<Param>();

            fetchParamsCollection = extensibleItem.JsonEndPoint.CustomLib.Params.ParamsCollection;

            fetchParamsCollection.Add(new Param { Key = "Type", Value = "ENABLING Specification" });
            fetchParamsCollection.Add(new Param { Key = "FromDate", Value = System.DateTime.Today.ToString() });

            //Act
            ParseService parseService = new ParseService(extensibleItem);
            FetchParamsInfo fetchParamInfo = (FetchParamsInfo)parseService.Parse();

            //Assert
            Assert.AreEqual(fetchParamInfo.TaskType.Value, "Enabling Specification");
            Assert.IsTrue(fetchParamInfo.DateTime.Value != null);
        }

        [ExpectedException(typeof(Exception))]
        public void ParseService_TwoTypesOneDate()
        {
            //Arrange
            ExtensibleItem extensibleItem = new ExtensibleItem();

            extensibleItem.JsonEndPoint = new JsonEndPoint();

            extensibleItem.JsonEndPoint.CustomLib = new CustomLib();

            extensibleItem.JsonEndPoint.CustomLib.Params = new Params();

            List<Param> fetchParamsCollection = new List<Param>();

            extensibleItem.JsonEndPoint.CustomLib.Params.ParamsCollection = new List<Param>();

            fetchParamsCollection = extensibleItem.JsonEndPoint.CustomLib.Params.ParamsCollection;

            fetchParamsCollection.Add(new Param { Key = "Type", Value = "ENABLING Specification" });
            fetchParamsCollection.Add(new Param { Key = "Type", Value = "hello" });

            //Act
            ParseService parseService = new ParseService(extensibleItem);
            FetchParamsInfo fetchParamInfo = (FetchParamsInfo)parseService.Parse();

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        public void ConnectionString_ExtensibleItem_null()
        {
            //Arrange
            ConfigurationSerializerService service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_ExtensibleItem_ConnectionStringIsNull.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            context.StartProcess(configPath);

            //Assert
            Assert.IsNull(connectionString);
        }

        [TestMethod]
        public void ConnectionString_GlobalConfig_Null()
        {
            //Arrange
            ConfigurationSerializerService service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_ExtensibleItem_ConnectionStringIsNull.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            context.StartProcess(configPath);

            //Assert
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "VSOAPI.log");
            string logInfo = File.ReadAllText(logFilePath);
            Assert.IsTrue(logInfo.Contains("cannot be null or empty"));
        }
    }
}
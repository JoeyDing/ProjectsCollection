using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleVSOAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleVSOAPI.Tests.IntegratedTest
{
    [TestClass]
    public class IntegratedTest_FetchData
    {
        [TestMethod]
        public void FetchDataTest_FetchIncrementalData_TypeTask()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items;

            string sqlTableName = items.Items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items.Items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();

            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            bool tableEmpty = !tableExists ? true : extensibleDbEntity.Database.SqlQuery<int>(
         string.Format(@"SELECT count(*) FROM {0}", sqlTableName)).ToList()[0] == 0;

            string[] fields = items.Items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create table and insert few data
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));
            }
            else if (tableEmpty)
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);
            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            string levelstring = jsonObject["values"][0]["fields"]["System.AreaLevel1"].ToString();
            Assert.AreEqual("LOCALIZATION", levelstring);
        }

        [TestMethod]
        public void FetchDataTest_FetchIncrementalData_TypeEpic()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Epic";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();

            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            bool tableEmpty = !tableExists ? true : extensibleDbEntity.Database.SqlQuery<int>(
         string.Format(@"SELECT count(*) FROM {0}", sqlTableName)).ToList()[0] == 0;

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create table and insert few data
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));
            }
            else if (tableEmpty)
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);
            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            string levelstring = jsonObject["values"][0]["fields"]["System.AreaLevel1"].ToString();
            Assert.AreEqual("LOCALIZATION", levelstring);
        }

        [TestMethod]
        public void FetchDataTest_FetchIncrementalData_TypeES()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Enabling Specification";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();

            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            bool tableEmpty = !tableExists ? true : extensibleDbEntity.Database.SqlQuery<int>(
         string.Format(@"SELECT count(*) FROM {0}", sqlTableName)).ToList()[0] == 0;

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create table and insert few data
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));
            }
            else if (tableEmpty)
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);
            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            string levelstring = jsonObject["values"][0]["fields"]["System.AreaLevel1"].ToString();
            Assert.AreEqual("LOCALIZATION", levelstring);
        }

        [TestMethod]
        public void FetchDataTest_FetchIncrementalData_TypeBug()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Bug";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();

            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            bool tableEmpty = !tableExists ? true : extensibleDbEntity.Database.SqlQuery<int>(
         string.Format(@"SELECT count(*) FROM {0}", sqlTableName)).ToList()[0] == 0;

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create table and insert few data
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));
            }
            else if (tableEmpty)
                extensibleDbEntity.Database.ExecuteSqlCommand(String.Format("Insert into {0}({1}) Values ('{2}')", sqlTableName, fields[0].ToString(), "TestingValue"));

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);
            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            string levelstring = jsonObject["values"][0]["fields"]["System.AreaLevel1"].ToString();
            Assert.AreEqual("LOCALIZATION", levelstring);
        }

        [TestMethod]
        public void FetchDataTest_FetchFullData_TypeTask()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create empty table
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
            }
            else
            {
                //truncate the table
                sqlCreateService.EmptyTable(sqlTableName, extensibleDbEntity);
            }

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);

            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            int valuesNumber = jsonObject["values"].ToList().Count;
            Assert.IsTrue(valuesNumber > 0);
        }

        [TestMethod]
        public void FetchDataTest_FetchFullData_TypeEpic()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Epic";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create empty table
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
            }
            else
            {
                //truncate the table
                sqlCreateService.EmptyTable(sqlTableName, extensibleDbEntity);
            }

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);

            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            int valuesNumber = jsonObject["values"].ToList().Count;
            Assert.IsTrue(valuesNumber > 0);
        }

        [TestMethod]
        public void FetchDataTest_FetchFullData_TypeES()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Enabling Specification";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create empty table
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
            }
            else
            {
                //truncate the table
                sqlCreateService.EmptyTable(sqlTableName, extensibleDbEntity);
            }

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);

            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            int valuesNumber = jsonObject["values"].ToList().Count;
            Assert.IsTrue(valuesNumber > 0);
        }

        [TestMethod]
        public void FetchDataTest_FetchFullData_TypeBug()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            FetchService fetchService = new FetchService();
            var service = new ConfigurationSerializerService();
            var extensibleItems = service.GetExtensibleConfigFromConfig(configPath);

            var items = extensibleItems.Items.Items;
            items[0].JsonEndPoint.CustomLib.Params.ParamsCollection[0].Value = "Bug";

            string sqlTableName = items[0].Mapping.SqlTableName;

            ExtensibleDbEntity extensibleDbEntity = new ExtensibleDbEntity(items[0].ConnectionString);

            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            bool tableExists = sqlReadService.TableExists(sqlTableName, extensibleDbEntity);

            string[] fields = items[0].Mapping.Fields.Select(c => c.SqlColumnName).ToArray();

            if (!tableExists)
            {
                //create empty table
                sqlCreateService.CreateTable(sqlTableName, fields, extensibleDbEntity);
            }
            else
            {
                //truncate the table
                sqlCreateService.EmptyTable(sqlTableName, extensibleDbEntity);
            }

            //Act
            string vsoworkitemsJsonString = fetchService.FetchData(extensibleItems.Items.Items[0], extensibleDbEntity);

            //Assert
            JObject jsonObject = JObject.Parse(vsoworkitemsJsonString);
            int valuesNumber = jsonObject["values"].ToList().Count;
            Assert.IsTrue(valuesNumber > 0);
        }
    }
}
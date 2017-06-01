using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleVSOAPI.Tests.EndToEndTest
{
    [TestClass]
    public class E2ETest
    {
        private ConfigurationSerializerService service = new ConfigurationSerializerService();

        [TestMethod]
        public void E2E_Type_Task()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_Type_Task.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            context.StartProcess(configPath);

            //Assert
            List<string> types = this.AssertionDBCheck("SystemWorkItemType", connectionString);
            Assert.AreEqual("Task", types[0]);
        }

        [TestMethod]
        public void E2E_Type_Epic()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_Type_Epic.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            context.StartProcess(configPath);

            //Assert
            List<string> types = this.AssertionDBCheck("SystemWorkItemType", connectionString);
            Assert.AreEqual("Epic", types[0]);
        }

        [TestMethod]
        public void E2E_Type_Exception()
        {
            //Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_Type_Exception.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            context.StartProcess(configPath);

            //Assert
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "VSOAPI.log");
            string logInfo = File.ReadAllText(logFilePath);
            Assert.IsTrue(logInfo.Contains("Value of Type is incorrect"));
        }

        [TestMethod]
        public void E2E_SaveType_Full()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_SaveType_Full.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);

            //Act
            foreach (var item in extensibleConfig.Items.Items)
            {
                string saveType = item.Mapping.SaveType.ToString();
                if (saveType == "Full")
                    context.logger.LogException(new Exception("Value of SaveType must be Incremental"));
            }

            //Assert
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "VSOAPI.log");
            string logInfo = File.ReadAllText(logFilePath);
            Assert.IsTrue(logInfo.Contains("Value of SaveType must be Incremental"));
        }

        public List<string> AssertionDBCheck(string columnName, string connectionString)
        {
            List<string> types = new List<string>();
            using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                using (SqlCommand command = new SqlCommand("Select " + columnName + " FROM CountryLanguageCount", dbConnection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    string columnValue;
                    while (reader.Read())
                    {
                        columnValue = reader[columnName].ToString();
                        types.Add(columnValue);
                    }
                }
            }
            return types;
        }

        [TestMethod]
        public void E2E_CleanOnSchemaChange_true()
        {
            //Arrange
            string configPath_fourColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
            string configPath_sixColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_CleanOnSchemaChange_true.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath_fourColumns);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);
            var dbContext = new ExtensibleDbEntity(connectionString);

            //Act
            //Drop the table
            dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('dbo.CountryLanguageCount', 'U') IS NOT NULL DROP TABLE CountryLanguageCount;");
            //Create table of six columns
            context.StartProcess(configPath_sixColumns);
            //Decrease the number of columns to four
            context.StartProcess(configPath_fourColumns);

            //Assert
            //get the number of columns
            var numberOfColumns = dbContext.Database.SqlQuery<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CountryLanguageCount'").ToList();
            Assert.AreEqual(5, numberOfColumns.Count());
        }

        [TestMethod]
        public void E2E_CleanOnSchemaChange_false()
        {
            //Arrange
            string configPath_fourColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
            string configPath_threeColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InputFiles", "Config_CleanOnSchemaChange_false.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath_fourColumns);
            string logFile = extensibleConfig.GlobalConfigItem.LogName;
            string connectionString = extensibleConfig.Items.Items[0].ConnectionString;
            ExtensibleContext context = new ExtensibleContext(logFile);
            var dbContext = new ExtensibleDbEntity(connectionString);

            //Act
            //Drop the table
            dbContext.Database.ExecuteSqlCommand("IF OBJECT_ID('dbo.CountryLanguageCount', 'U') IS NOT NULL DROP TABLE CountryLanguageCount;");
            //Create table of six columns
            context.StartProcess(configPath_fourColumns);
            //Decrease the number of columns to four
            context.StartProcess(configPath_threeColumns);

            //Assert
            //get the number of columns
            var numberOfColumns = dbContext.Database.SqlQuery<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CountryLanguageCount'").ToList();
            Assert.AreEqual(5, numberOfColumns.Count());
        }
    }
}
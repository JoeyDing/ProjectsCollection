using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using System.IO;
using ExtensibleDataExtraction.Lib.Data.Sql;

namespace ExtensibleDataExtraction.Lib.Tests.EndToEndTests
{
    [TestClass]
    public class ArgusMissing
    {
        [TestMethod]
        public void ETE_ArgusMissing_NoConnectionString()
        {
            //arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\ArgusMissing_NoConnectionString.xml");

            //act
            string logName = "ete_ArgusMissing_NoConnectionString";
            ExtensibleContext extensibleContext = new ExtensibleContext(logName);
            extensibleContext.StartProcess(configPath);

            //assert
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(@"logs\{0}", logName));

            string log = System.IO.File.ReadAllText(logPath);
            Assert.IsTrue(log.Contains("Exception"));
            Assert.IsTrue(log.Contains("Connection string must be included in globalConfig or extensible item"));
        }

        [TestMethod]
        public void ETE_ArgusMissing_NoJsonQueryUrl()
        {
            //arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\Config_ArgusMissing_NoConStr_NoSqlUrl.xml");

            //act
            string logName = "ete_ArgusMissing_NoJsonQueryUrl";
            ExtensibleContext extensibleContext = new ExtensibleContext(logName);
            extensibleContext.StartProcess(configPath);

            //assert
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(@"logs\{0}", logName));

            string log = System.IO.File.ReadAllText(logPath);
            Assert.IsTrue(log.Contains("Exception"));
            Assert.IsTrue(log.Contains("JsonQueryUrl or CustomLib cannot be null or empty"));
        }

        [TestMethod]
        public void ETE_ArgusMissing_ContainsCustomLibAndJsonQueryUrl()
        {
            //arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\Config_ArgusMissing_NoConStr_NoSqlUrl.xml");

            //act
            string logName = "ete_ArgusMissing_ContainsCustomLibAndJsonQueryUrl";
            ExtensibleContext extensibleContext = new ExtensibleContext(logName);
            extensibleContext.StartProcess(configPath);

            //assert
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(@"logs\{0}", logName));

            string log = System.IO.File.ReadAllText(logPath);
            Assert.IsTrue(log.Contains("Exception"));
            throw new ArgumentException(@"JsonQueryUrl && CustomLib cannot be used together, pick one.");
        }

        [TestMethod]
        public void ETE_ArgusMissing_NoIdentity()
        {
            //arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\Config_ArgusMissing_NoIdentity.xml");

            //act
            string logName = "ete_ArgusMissing_NoIdentity";
            ExtensibleContext extensibleContext = new ExtensibleContext(logName);
            extensibleContext.StartProcess(configPath);

            //assert
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, String.Format(@"logs\{0}", logName));
            string log = System.IO.File.ReadAllText(logPath);
            Assert.IsTrue(log.Contains("Exception"));
            Assert.IsTrue(log.Contains("No identify field found: At least one Identify field should be specified."));
        }

        [TestMethod]
        public void E2E_CleanOnSchemaChange_true()
        {
            //Arrange
            string connectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";
            string configPath_fourColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\Config_OnSchemaChange.xml");
            string configPath_threeColumns = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EndToEndTests\UserInputFiles\Config_OnSchemaChange_True.xml");
            ExtensibleContext context = new ExtensibleContext("ExtensibleDataExtraction.log");
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
            var numberOfColumns = dbContext.Database.SqlQuery<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CountryLanguageCount'");
            Assert.AreEqual(5, numberOfColumns);
        }
    }
}
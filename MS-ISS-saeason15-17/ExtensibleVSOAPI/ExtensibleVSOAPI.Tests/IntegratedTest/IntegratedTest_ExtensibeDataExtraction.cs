using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleVSOAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleVSOAPI.Tests
{
    [TestClass]
    public class IntegratedTest_ExtensibeDataExtraction
    {
        private static ConfigurationSerializerService service = new ConfigurationSerializerService();
        private static string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
        private static ExtensibleConfig extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
        private List<ExtensibleItem> items = extensibleItems.Items.Items;

        [TestMethod]
        public void StartProcess_Test()
        {
            //Arrange
            var context = new ExtensibleContext("VSOAPI.log");
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            List<string> countries = new List<string>();

            this.CommonAct(context, countries);
            if (countries.Count == 0)
                this.CommonAct(context, countries);

            //Assert
            Assert.AreEqual("37136", countries[0]);
            Assert.AreEqual("37461", countries[124]);
        }

        public void CommonAct(ExtensibleContext context, List<string> countries)
        {
            //Act
            context.StartProcess(configPath);
            var dbConnection = new ExtensibleDbEntity(items[0].ConnectionString);
            using (SqlCommand command = new SqlCommand("Select SystemAreaId FROM CountryLanguageCount", dbConnection.Database.Connection as SqlConnection))
            {
                if (dbConnection.Database.Connection.State == ConnectionState.Closed)
                    dbConnection.Database.Connection.Open();
                command.ExecuteNonQuery();

                SqlDataReader reader = command.ExecuteReader();

                string columnValue;

                while (reader.Read())
                {
                    columnValue = reader["SystemAreaId"].ToString();
                    countries.Add(columnValue);
                }
            }
        }

        [TestMethod]
        public void Application_Test()
        {
            //Act
            Application.Main(new string[] { "Hello" });
        }
    }
}
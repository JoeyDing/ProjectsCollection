using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VsoWorkItemsSync.Core.Configuration;

namespace VsoWorkItemsSync.Tests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod]
        public void Configuration_LoadConfiguration_Sucess()
        {
            //setup
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\Success.xml");
            var loader = new ConfigurationLoader();

            //Execute
            var config = loader.Load(configPath);

            //Assert
            Assert.IsNotNull(config);
            //global info
            Assert.AreEqual(config.DbConnectionString, "DbConnectionString");
            Assert.AreEqual(config.VsoRootAccount, "VsoRootAccount");
            Assert.AreEqual(config.VsoProjectName, "VsoProjectName");
            //table mapping info
            Assert.IsTrue(config.Mappings.Length == 2);
            Assert.AreEqual(config.Mappings[0].DbTableName, "DbTableName1");
            Assert.AreEqual(config.Mappings[0].VsoWorkItemName, "VsoWorkItemName1");
            //fields
            Assert.IsTrue(config.Mappings[0].Fields.Length == 2);
            Assert.AreEqual(config.Mappings[0].Fields[0].DataType, "DataType");
            Assert.AreEqual(config.Mappings[0].Fields[0].DbField, "DbField11");
            Assert.AreEqual(config.Mappings[0].Fields[0].VsoField, "VsoField11");
        }
    }
}
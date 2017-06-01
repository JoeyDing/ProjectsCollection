using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Core.Configuration;
using VsoWorkItemsSync.Core.WorkItem;

namespace VsoWorkItemsSync.Tests
{
    [TestClass]
    public class MappingTest
    {
        [TestMethod]
        public void Mapping_ValidateMapping_Success()
        {
            //prepare
            var configMapping = new ConfigurationMappings()
            {
                DbConnectionString = "configMapping",
                VsoProjectName = "VsoProjectName",
                VsoRootAccount = "VsoRootAccount",
                Mappings = new VsoDbMapping[]
                {
                    new VsoDbMapping()
                    {
                        DbTableName = "Epics",
                        VsoWorkItemName = "Epic",
                        Fields = new VsoDbField[]
                        {
                            new VsoDbField {
                                DataType = "string",
                                DbField = "Title",
                                VsoField = "Title"
                            }
                        }
                    }
                }
            };

            //execute
            var errors = new List<string>();
            var result = new WorkItemProvider(configMapping, configMapping.Mappings[0]).ValidateVsoDbMapping(out errors);

            //assert
            string error = errors.Any() ? errors.Aggregate((a, b) => a + ',' + b) : "";
            Assert.IsTrue(result, error);
            Assert.IsFalse(errors.Any(), error);
        }

        [TestMethod]
        public void Mapping_ValidateMapping_Fail_NoDbTableName_VsoWorkItemName()
        {
            //prepare
            var configMapping = new ConfigurationMappings()
            {
                DbConnectionString = "configMapping",
                VsoProjectName = "VsoProjectName",
                VsoRootAccount = "VsoRootAccount",
                Mappings = new VsoDbMapping[]
                {
                    new VsoDbMapping()
                    {
                        DbTableName = "",
                        VsoWorkItemName = "",
                        Fields = new VsoDbField[]
                        {
                            new VsoDbField {
                                DataType = "string",
                                DbField = "Title",
                                VsoField = "Title"
                            }
                        }
                    }
                }
            };

            //execute
            var errors = new List<string>();
            var result = new WorkItemProvider(configMapping, configMapping.Mappings[0]).ValidateVsoDbMapping(out errors);

            //assert
            Assert.IsFalse(result);
            Assert.AreEqual(2, errors.Count);
        }

        [TestMethod]
        public void Mapping_ValidateMapping_Fail_Field_No_DataType_DbField_VsoField()
        {
            //prepare
            var configMapping = new ConfigurationMappings()
            {
                DbConnectionString = "configMapping",
                VsoProjectName = "VsoProjectName",
                VsoRootAccount = "VsoRootAccount",
                Mappings = new VsoDbMapping[]
                {
                    new VsoDbMapping()
                    {
                        DbTableName = "Epics",
                        VsoWorkItemName = "Epic",
                        Fields = new VsoDbField[]
                        {
                            new VsoDbField {
                                DataType = "",
                                DbField = "",
                                VsoField = ""
                            }
                        }
                    }
                }
            };

            //execute
            var errors = new List<string>();
            var result = new WorkItemProvider(configMapping, configMapping.Mappings[0]).ValidateVsoDbMapping(out errors);

            //assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, errors.Count);
        }
    }
}
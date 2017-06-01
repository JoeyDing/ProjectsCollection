using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Core.Configuration;
using VsoWorkItemsSync.Core.Exception;
using VsoWorkItemsSync.Core.WorkItem;

namespace VsoWorkItemsSync.Tests
{
    [TestClass]
    public class SyncTest
    {
        private static string _vsoPrivateKey;
        private static string _vsoRootAccount;
        private static string _vsoProject;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _vsoPrivateKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            _vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];
            _vsoProject = "LOCALIZATION";
        }

        [TestMethod]
        public void Sync_SyncData_Success()
        {
            //prepare
            var configMapping = new ConfigurationMappings()
            {
                DbConnectionString = "configMapping",
                VsoProjectName = _vsoProject,
                VsoRootAccount = _vsoRootAccount,
                VsoPrivateKey = _vsoPrivateKey,
                Mappings = new VsoDbMapping[]
                {
                    new VsoDbMapping()
                    {
                        DbTableName = "Epics",
                        VsoWorkItemName = "Epic",
                        Fields = new VsoDbField[]
                        {
                            new VsoDbField
                            {
                                DataType = "string",
                                DbField = "Title",
                                VsoField = "System.Title"
                            }
                        }
                    }
                }
            };

            try
            {
                //execute
                var errors = new List<string>();
                new WorkItemProvider(configMapping, configMapping.Mappings[0]).SyncData();
            }
            catch (Exception e)
            {
                if (e is SyncException)
                {
                    var syncEx = e as SyncException;
                    //assert
                    string error = syncEx.InnerExceptionsList.Aggregate((a, b) => a + ',' + b);
                    Assert.Fail(error);
                }
                else
                {
                    Assert.Fail(e.ToString());
                }
            }
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Processing;
using ExtensibleDataExtraction.Lib.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Tests.IntegrationTests
{
    [TestClass]
    public class Int_SaveToDatabase
    {
        [TestMethod]
        public void Int_SaveToDatabase_InsertNewRows()
        {
            //arrange
            string logName = "Int_SaveToDatabase_InsertNewRows";
            var context = new ExtensibleContext(logName);
            var mapping = new Mapping
            {
                SqlTableName = "Country1",
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country", IsIdentity=false},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language", IsIdentity=false},
                                    new Field {JsonFieldName = "Test3", SqlColumnName = "Test3", IsIdentity=true},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model", IsIdentity=false},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount", IsIdentity=false},
                                },
                SaveType = DataSaveType.SaveType.Full
            };

            var dataSet = new ExtensibleDataSet()
            {
                Mapping = mapping,
                Rows = new List<ExtensibleDataRow>() {
                    new ExtensibleDataRow{ Values = new List<string>{"France", "fr-fr", "null","null", "1"}},
                    new ExtensibleDataRow{ Values = new List<string>{"US", "en-us", "null", "null", "2"}}
                }
            };

            List<string> countriesList = new List<string>();
            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";
            //act
            context.SaveToDatabase(dbConnectionString, dataSet);

            //assert
            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * FROM Country1", dbContext.Database.Connection as SqlConnection))
                {
                    dbContext.Database.Connection.Open();
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        countriesList.Add(reader["Country"].ToString());
                    }
                    reader.Close();
                }
                Assert.AreEqual(countriesList[0], "France");
                Assert.AreEqual(countriesList[1], "US");
            }
        }

        [TestMethod]
        public void Int_SaveToDatabase_FullProcessInsertNewRows()
        {
            //arrange
            string logName = "Int_SaveToDatabase_InsertNewRows";
            var context = new ExtensibleContext(logName);
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.POST,
                JsonQueryUrl = "https://mixpanel.com/api/2.0/jql",
                HeaderContent = @"application/json",
                Token = "5e7e771ff845e78fc7d2f942b15800ff",
                ResultTemplate = "values",
                ResultItemTemplate = "fields",

                FormContent = new List<BodyFormContent>{
                                    new BodyFormContent{
                                        Key = "script",
                                    Value = @"function main() {
                                              return Events({
                                                from_date: '2016-10-10',
                                                to_date:   '2016-10-10'
                                              })
                                              .filter(function(event) {
                                                return event.name == 'End Session' || event.name == 'Send Message' || 'Change App Settings' || event.name == 'Create New Event' || event.name == 'Open Attachments' || event.name == 'Add Member' || event.name == 'Start DM' || event.name == 'Start Group';
                                              })
                                              // for totals
                                              // .groupBy([""properties.mp_country_code"", ""properties.Language"", ""properties.$model""], mixpanel.reducer.count())
                                              // for uniques
                                              .groupByUser([""properties.mp_country_code"", ""properties.Language"", ""properties.$model""], mixpanel.reducer.count())
                                              .groupBy([function(item) { return item.key[1]; }, function(item) { return item.key[2]; }, function(item) { return item.key[3]; }], mixpanel.reducer.count())
                                              .map(function(item) {
                                                return {
                                                  'Country': item.key[0],
                                                  'Language': item.key[1],
                                                  'Model': item.key[2],
                                                  'UniqueCount': item.value,
                                                }
                                              })
                                            }"}
                }
            };
            extensibleItem.Mapping = new Mapping
            {
                SqlTableName = "Country",
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country",IsIdentity=true},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language",IsIdentity=true},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model",IsIdentity=true},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount", IsIdentity=true},
                                },
                SaveType = DataSaveType.SaveType.Full
            };

            extensibleItem.ConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";

            //act
            var jsonData = context.RetrieveJsonData(extensibleItem, "");
            var dataSet = context.ParseJsonData(jsonData, extensibleItem);
            context.SaveToDatabase(extensibleItem.ConnectionString, dataSet);

            //assert
            using (var dbContext = new ExtensibleDbEntity(extensibleItem.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * FROM Country", dbContext.Database.Connection as SqlConnection))
                {
                    dbContext.Database.Connection.Open();
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> countriesList = new List<string>();
                    while (reader.Read())
                    {
                        countriesList.Add(reader["Country"].ToString());
                    }
                    reader.Close();
                    Assert.IsTrue(countriesList.Count != 0);
                }
            }
        }

        /// <summary>
        /// db contains 1 more record than the json data
        /// </summary>
        [TestMethod]
        public void Int_SaveToDatabase_FullProcessFromConfig_IncrementalInsert()
        {
            //arrange
            var service = new ConfigurationSerializerService();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UnitTests\Configuration\Files\Config_IncrementalInsert.xml");

            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            string logName = "Int_SaveToDatabase_InsertNewRows";
            var context = new ExtensibleContext(logName);

            //act
            //var jsonData = context.RetrieveJsonData(extensibleItem);

            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json.json");
            string jsonString;
            using (StreamReader sr = new StreamReader(jsonFilePath))
            {
                jsonString = sr.ReadToEnd();
            }

            var dataSet = context.ParseJsonData(jsonString, null);
            context.SaveToDatabase(extensibleConfig.GlobalConfigItem.ConnectionString, dataSet);

            //assert
            Assert.IsNotNull(jsonString);
        }

        [TestMethod]
        public void StartProcess_Test()
        {
            //Arrange
            string logName = "StartProcess_Test";
            var context = new ExtensibleContext(logName);
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UnitTests\Configuration\Files\Config.xml");

            //Act
            context.StartProcess(configPath);
        }
    }
}
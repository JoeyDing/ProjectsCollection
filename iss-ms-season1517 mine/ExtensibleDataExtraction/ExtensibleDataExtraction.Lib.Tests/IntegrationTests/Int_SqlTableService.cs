using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Processing;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleDataExtraction.Lib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Tests.IntegrationTests
{
    [TestClass]
    public class Int_SqlTableService
    {
        [TestMethod]
        public void Int_CreateMissingColumns_CreateColumInDb()
        {
            //arrange
            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";
            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                bool tableExist = sqlReadService.TableExists("Country3", dbContext);
                if (tableExist)
                {
                    //drop the table

                    using (SqlCommand command = new SqlCommand("DROP TABLE " + "Country3", dbContext.Database.Connection as SqlConnection))
                    {
                        dbContext.Database.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                var mapping = new Mapping
                {
                    SqlTableName = "Country3",
                    Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"}
                                },
                    SaveType = DataSaveType.SaveType.Full
                };

                var dataSet = new ExtensibleDataSet()
                {
                    Mapping = mapping,
                    Rows = new List<ExtensibleDataRow>() {
                    new ExtensibleDataRow{ Values = new List<string>{"France", "fr-fr"}},
                    new ExtensibleDataRow{ Values = new List<string>{"US", "en-us"}}
                }
                };

                sqlCreateService.CreateTable(mapping.SqlTableName, mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                var fields = new string[] { "Test1", "Test2" };

                //act
                sqlCreateService.CreateMissingColumns(mapping.SqlTableName, fields, dbContext);

                //assert
                using (SqlCommand command = new SqlCommand("Select * FROM Country3", dbContext.Database.Connection as SqlConnection))
                {
                    if (dbContext.Database.Connection.State == ConnectionState.Closed)
                    {
                        dbContext.Database.Connection.Open();
                    }

                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();

                    List<string> columnNames = new List<string>();
                    foreach (DataRow row in reader.GetSchemaTable().Rows)
                    {
                        columnNames.Add(row["ColumnName"].ToString());
                    }

                    Assert.IsTrue(columnNames.Contains("Test1"));

                    Assert.IsTrue(columnNames.Contains("Test2"));
                }
            }
        }

        [TestMethod]
        public void CreateTable()
        {
            //arrange
            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";

            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                bool tableExist = sqlReadService.TableExists("Country1", dbContext);
                if (tableExist)
                {
                    using (SqlCommand command = new SqlCommand("DROP TABLE " + "Country1", dbContext.Database.Connection as SqlConnection))
                    {
                        dbContext.Database.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                var mapping = new Mapping
                {
                    SqlTableName = "Country1",
                    Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Test3", SqlColumnName = "Test3"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
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

                //act
                sqlCreateService.CreateTable(mapping.SqlTableName, mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                //assert
                Assert.AreEqual(true, sqlReadService.TableExists(dataSet.Mapping.SqlTableName, dbContext));
            }
        }

        [TestMethod]
        public void RemoveDataOnlyInDB()
        {
            //arrange
            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();
            SqlWriteService sqlWriteService = new SqlWriteService();

            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";

            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                bool tableExist = sqlReadService.TableExists("Country1", dbContext);
                if (tableExist)
                {
                    //drop the table
                    string sqlCommand1 = "DROP TABLE Country1";
                    dbContext.Database.ExecuteSqlCommand(sqlCommand1);
                }

                //config xml file
                var mapping = new Mapping
                {
                    SqlTableName = "Country1",
                    Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country",IsIdentity=false},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language",IsIdentity=false},
                                    new Field {JsonFieldName = "Test3", SqlColumnName = "Test3", IsIdentity=true},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model",IsIdentity=false},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount",IsIdentity=false},
                                },
                    SaveType = DataSaveType.SaveType.Full
                };

                var dataSet = new ExtensibleDataSet()
                {
                    Mapping = mapping,
                    Rows = new List<ExtensibleDataRow>() {
                    new ExtensibleDataRow{ Values = new List<string>{"France", "fr-fr", "null","null", "1"}},
                }
                };

                //create table and insert data into the table
                sqlCreateService.CreateTable(mapping.SqlTableName, mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                string sqlCommand2 = "INSERT INTO Country1(Country, Language, Test3, Model,[UniqueCount]) VALUES (@country, @language, @test3, @model,@unic)";

                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@country", "France")
                    ,new SqlParameter("@language", "fr-fr")
                    ,new SqlParameter("@test3", "null")
                    ,new SqlParameter("@model", "null")
                    ,new SqlParameter("@unic", "1")
                };
                dbContext.Database.ExecuteSqlCommand(sqlCommand2, parameters);

                HashSet<string[]> dataOnlyInDB = new HashSet<string[]>();
                foreach (var x in dataSet.Rows)
                {
                    dataOnlyInDB.Add(x.Values.ToArray());
                }

                //act
                sqlWriteService.RemoveDataOnlyInDB(mapping.Fields.Select(X => X.SqlColumnName).ToArray(), dataOnlyInDB, mapping.SqlTableName, mapping.Fields.Where(x => x.IsIdentity == true).Select(x => x.SqlColumnName).ToArray(), dbContext);

                //assert
                using (SqlCommand command = new SqlCommand("Select * FROM Country1", dbContext.Database.Connection as SqlConnection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    Assert.IsFalse(reader.HasRows);
                }
            }
        }

        [TestMethod]
        public void EmptyTable()
        {
            //arrange
            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();

            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";

            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                bool tableExist = sqlReadService.TableExists("Country1", dbContext);
                if (tableExist)
                {
                    //drop the table

                    using (SqlCommand command = new SqlCommand("DROP TABLE " + "Country1", dbContext.Database.Connection as SqlConnection))
                    {
                        dbContext.Database.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                var mapping = new Mapping
                {
                    SqlTableName = "Country1",
                    Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Test3", SqlColumnName = "Test3"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
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
                sqlCreateService.CreateTable(mapping.SqlTableName, mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                //act
                sqlCreateService.EmptyTable(dataSet.Mapping.SqlTableName, dbContext);

                //assert
                using (SqlCommand command = new SqlCommand("Select * FROM Country1", dbContext.Database.Connection as SqlConnection))
                {
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    Assert.IsFalse(reader.HasRows);
                }
            }
        }

        [TestMethod]
        public void UpdateData()
        {
            //arrange
            SqlReadService sqlReadService = new SqlReadService();
            SqlCreateService sqlCreateService = new SqlCreateService();
            SqlWriteService sqlWriteService = new SqlWriteService();

            string dbConnectionString = "Server=skypeintl;Database=Test_ExtensibleDataExtraction;integrated security=True;MultipleActiveResultSets=True;App=ExtensibleDbContext;";

            using (var dbContext = new ExtensibleDbEntity(dbConnectionString))
            {
                bool tableExist = sqlReadService.TableExists("Country1", dbContext);
                if (tableExist)
                {
                    //drop the table

                    using (SqlCommand command = new SqlCommand("DROP TABLE " + "Country1", dbContext.Database.Connection as SqlConnection))
                    {
                        dbContext.Database.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                var mapping = new Mapping
                {
                    SqlTableName = "Country1",
                    Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country", IsIdentity=true},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language",IsIdentity=true},
                                    new Field {JsonFieldName = "Test3", SqlColumnName = "Test3",IsIdentity=true},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model",IsIdentity=true},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount",IsIdentity=true},
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

                sqlCreateService.CreateTable(mapping.SqlTableName, mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                //act
                string[] fields = dataSet.Mapping.Fields.Select(c => c.SqlColumnName).ToArray();
                string[][] values = dataSet.Rows.Select(x => x.Values.ToArray()).ToArray();
                string logname = "UpdateData.log";
                Logger logger = new Logger(logname);
                sqlWriteService.UpdateData(mapping.SqlTableName, fields, values, mapping.Fields.Where(x => x.IsIdentity == true).Select(x => x.SqlColumnName).ToArray(), dbContext, logger);

                //assert
                using (SqlCommand command = new SqlCommand("Select Country FROM Country1", dbContext.Database.Connection as SqlConnection))
                {
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();

                    string columnValue;

                    List<string> countries = new List<string>();

                    while (reader.Read())
                    {
                        columnValue = reader["Country"].ToString();
                        countries.Add(columnValue);
                    }
                    Assert.AreEqual("France", countries[0]);
                    Assert.AreEqual("US", countries[1]);
                }
            }
        }
    }
}
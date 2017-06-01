using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Processing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Tests.UnitTests
{
    [TestClass]
    public class Unit_ParseJsonData
    {
        [TestMethod]
        public void Unit_ParseJsonData_JsonList_NoTemplate_DirectArray()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            string jsonData = @"[{""Country"":null,""Language"":null,""Model"":null,""UniqueCount"":21323},
                                 {""Country"":null,""Language"":null,""Model"":""InFocus M2_3G"",""UniqueCount"":1}]";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = "",
                ResultItemTemplate = "",
            };
            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };
            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            var result = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            //it will throw exception cos there is no ResultTemplate
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unit_ParseJsonData_JsonList_ResultTemplate_NoChildTemplate()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            string jsonData = @"{
                                   values: [{""Country"":null,""Language"":null,""Model"":null,""UniqueCount"":21323},
                                     {""Country"":null,""Language"":null,""Model"":""InFocus M2_3G"",""UniqueCount"":1}]
                                }";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = "values",
                ResultItemTemplate = "",
            };
            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };
            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            var result = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate()
        {
            //arrange
            string logName = "Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem.log";

            var context = new ExtensibleContext(logName);
            string jsonData = @"{
                                  root1:{
                                       root2:{
                                       values: [
                                        { childRoot1: {""Country"":null,""Language"":null,""Model"":null,""UniqueCount"":21323}},
                                        { childRoot1: {""Country"":null,""Language"":null,""Model"":""InFocus M2_3G"",""UniqueCount"":1}}
                                                ]
                                             }
                                     }
                                }";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = @"root1\\root2\\values",
                ResultItemTemplate = "childRoot1",
            };
            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };
            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            ExtensibleDataSet extensibleDataSet = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            Assert.AreEqual("21323", extensibleDataSet.Rows[0].Values[3]);
            Assert.AreEqual("1", extensibleDataSet.Rows[1].Values[3]);
        }

        [TestMethod]
        public void Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleNestedChildTemplate()
        {
            //arrange
            string logName = "Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem.log";

            var context = new ExtensibleContext(logName);

            string jsonData = @"{
                                  root1:{
                                       root2:{
                                       values: [
                                        { childRoot1:
                                            { childRoot2: {
                                                childRoot3: {""Country"":null,""Language"":null,""Model"":null,""UniqueCount"":21324},
                                                ""Country"":null,""Language"":null,""Model"":null,""UniqueCount"":21323},
                                                ""Country"":null,""Language"":null,""Model"":""InFocus M2_3G"",""UniqueCount"":1}}
                                                ]
                                             }
                                     }
                                }";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = @"root1\\root2\\values",
                ResultItemTemplate = @"childRoot1\\childRoot2\\childRoot3",
            };
            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };
            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            ExtensibleDataSet extensibleDataSet = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            //Assert.IsNotNull(result);
            Assert.AreEqual("21324", extensibleDataSet.Rows[0].Values[3]);
        }

        [TestMethod]
        public void Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem()
        {
            //arrange
            string logName = "Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem.log";

            var context = new ExtensibleContext(logName);
            string jsonData = @"{
                                  root1:{
                                       root2:{
                                       values: [
                                        {
                                            ""Country"":null,
                                            childRoot1: { ""Language"":null,""Model"":""InFocus M2_3G"",""UniqueCount"":21323}
                                                }
                                                ]
                                             }
                                        }
                                }";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = @"root1\\root2\\values",
                ResultItemTemplate = "childRoot1",
            };

            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };

            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            ExtensibleDataSet extensibleDataSet = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            Assert.AreEqual("21323", extensibleDataSet.Rows[0].Values[3]);
        }

        [TestMethod]
        public void Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem_All()
        {
            //arrange
            string logName = "Unit_ParseJsonData_JsonList_MultipleResultTemplate_MultipleChildTemplate_NoItem_All.log";
            var context = new ExtensibleContext(logName);
            string jsonData = @"{
                                  root1:{
                                       root2:{
                                       values: [
                                        {
                                            ""Country"":null,
                                            ""Language"":null,
                                            ""Model"":""InFocus M2_3G"",
                                            ""UniqueCount"":21323,
                                            childRoot1: { }
                                                }
                                                ]
                                             }
                                        }
                                }";

            JsonEndPoint endpoint = new JsonEndPoint
            {
                ResultTemplate = @"root1\\root2\\values",
                ResultItemTemplate = "childRoot1",
            };

            var mapping = new Mapping
            {
                Fields = new List<Field>
                                {
                                    new Field {JsonFieldName = "Country", SqlColumnName = "Country"},
                                    new Field {JsonFieldName = "Language", SqlColumnName = "Language"},
                                    new Field {JsonFieldName = "Model", SqlColumnName = "Model"},
                                    new Field {JsonFieldName = "UniqueCount", SqlColumnName = "UniqueCount"},
                                }
            };

            var extensibleItem = new ExtensibleItem
            {
                JsonEndPoint = endpoint,
                Mapping = mapping,
            };

            //act
            ExtensibleDataSet extensibleDataSet = context.ParseJsonData(jsonData, extensibleItem);

            //assert
            Assert.AreEqual("21323", extensibleDataSet.Rows[0].Values[3]);
        }
    }
}
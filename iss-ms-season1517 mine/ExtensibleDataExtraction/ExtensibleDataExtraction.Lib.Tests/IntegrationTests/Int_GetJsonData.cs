using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ExtensibleDataExtraction.Lib.Tests.IntegrationTests
{
    [TestClass]
    public class Int_GetJsonData
    {
        [TestMethod]
        public void Int_GetJsonData_Get_MixPanelEventByName_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.GET,
                JsonQueryUrl = "https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_apis/build/builds?api-version=2.0",
                HeaderContent = @"application/json",
                Token = "67y5535ggcnsbvq2yay343pf63oxbsv5jgdpedra4q4g4457y7ua"
            };
            //act
            string jsonStr = context.RetrieveJsonData(extensibleItem, "");

            //assert
            JObject jsonData = JObject.Parse(jsonStr);
            List<string> namesList = new List<string>();
            foreach (var field in jsonData["value"])
            {
                namesList.Add(field["definition"]["project"]["name"].ToString());
            }
            Assert.IsTrue(namesList.Contains("LOCALIZATION"));
        }

        [TestMethod]
        public void Int_GetJsonData_Get_TeamProjectsList_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.GET,
                JsonQueryUrl = "https://skype.visualstudio.com/DefaultCollection/_apis/projects?api-version=1.0",
                HeaderContent = @"application/json",
                Token = "67y5535ggcnsbvq2yay343pf63oxbsv5jgdpedra4q4g4457y7ua"
            };
            //act
            var jsonDataStr = context.RetrieveJsonData(extensibleItem, "");

            //assert
            JObject json = JObject.Parse(jsonDataStr);
            List<string> namesList = new List<string>();
            foreach (var field in json["value"])
            {
                namesList.Add(field["name"].ToString());
            }

            Assert.IsTrue(namesList.Contains("LOCALIZATION"));
        }

        [TestMethod]
        public void Int_GetJsonData_Post_MixPanelEventByName_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.POST,
                JsonQueryUrl = "https://mixpanel.com/api/2.0/jql",
                HeaderContent = @"application/json",
                Token = "5e7e771ff845e78fc7d2f942b15800ff",
                FormContent = new List<BodyFormContent>{
                                    new BodyFormContent{
                                        Key = "script",
                                        Value = @"function main() {return Events({from_date: '2016-04-25',to_date:   '2016-04-26'}).groupBy([""name""], mixpanel.reducer.count());}"
                                    }
                }
            };
            //act
            var result = context.RetrieveJsonData(extensibleItem, "");

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Int_GetJsonData_MixPanelUserDistinctID_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.POST,
                JsonQueryUrl = "https://mixpanel.com/api/2.0/jql",
                HeaderContent = @"application/json",
                Token = "5e7e771ff845e78fc7d2f942b15800ff",
                FormContent = new List<BodyFormContent>{
                                    new BodyFormContent{
                                        Key = "script",
                                    Value = @"function main() {
                                              return People()
                                              .filter(function(user) {
                                                return user.distinct_id=='0QW/fOHlGVD0u9bExGNNjCfWZaE1hrUHSuyQs9SOAQE='
                                              })
                                            }"}
                }
            };
            //act
            var result = context.RetrieveJsonData(extensibleItem, "");

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Int_GetJsonData_MixPanelUniqueUserByCountryLangModel_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.POST,
                JsonQueryUrl = "https://mixpanel.com/api/2.0/jql",
                HeaderContent = @"application/json",
                Token = "5e7e771ff845e78fc7d2f942b15800ff",
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
            //act
            var result = context.RetrieveJsonData(extensibleItem, "");

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Int_GetJsonDataViaCustomLib_MixPanelEventByName_ReturnJsonString()
        {
            //arrange
            var context = new ExtensibleContext("logInfo.log");
            ExtensibleItem extensibleItem = new ExtensibleItem();
            extensibleItem.JsonEndPoint = new JsonEndPoint
            {
                RequestType = RequestType.POST,
                CustomLib = new CustomLib
                {
                    DllName = "ExtensibleDataExtraction.Lib.Tests",
                    FetchClass = "ExtensibleDataExtraction.Lib.Tests.IntegrationTests.CustomLibImplementation.FetchTest",
                },
            };
            //act
            var jsonStr = context.RetrieveJsonData(extensibleItem, "");

            //assert
            JObject jsonData = JObject.Parse(jsonStr);
            List<string> systemIDs = new List<string>();
            foreach (var jsonField in jsonData["values"])
            {
                systemIDs.Add(jsonField["fields"]["System.Id"].ToString());
            }

            Assert.IsNotNull(systemIDs.Contains("13901"));
            Assert.IsNotNull(systemIDs.Contains("13903"));
            Assert.IsNotNull(systemIDs.Contains("402990"));
        }
    }
}
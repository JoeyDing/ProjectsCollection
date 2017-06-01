using CiFxMappingParser;
using CiFxMappingParser.Writer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParserUnitTestProject
{
    [TestClass]
    public class UT_Serializer
    {
        [TestMethod]
        public void UT_Serializer_SourceMappingSerializer_Serialize()
        {
            //arrange
            MappingContent sourceMappingContent = new MappingContent();
            MappingContentItem mappingContentItem = new MappingContentItem();
            mappingContentItem.Text = "about title";
            sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };
            var serializer = new SourceMappingSerializer();
            //act
            string result = serializer.Serialize(sourceMappingContent);

            //assert
            var r = @"{
             ""mk_aboutTitle"":{
                 ""text"": ""about title""
             }
         }";
            var rJson = JObject.Parse(r).ToString();
            Assert.AreEqual(rJson, result);
        }
    }
}
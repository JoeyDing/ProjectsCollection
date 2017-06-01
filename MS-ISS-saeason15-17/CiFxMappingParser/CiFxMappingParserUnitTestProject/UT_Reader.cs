using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiFxMappingParser.Reader;
using System.IO;
using CiFxMappingParser;

namespace CiFxMappingParserUnitTestProject
{
    [TestClass]
    public class UT_Reader
    {
        [TestMethod]
        public void UT_Reader_SourceMappingReader_Read()
        {
            //arrange
            var reader = new SourceMappingReader();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reader\sourceMapping.json");

            //act
            MappingContent result = reader.Read(path);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(2, result.Items.Count);

            Assert.AreEqual(result.Items["mk_aboutText"].Text, "about title");
            Assert.AreEqual(result.Items["mk_aboutText"].Accessibility_id, null);
            Assert.AreEqual(result.Items["mk_aboutText"].Id, null);

            Assert.AreEqual(result.Items["main_password"].Text, "Password");
            Assert.AreEqual(result.Items["main_password"].Id, "i0118");
            Assert.AreEqual(result.Items["main_password"].Accessibility_id, "Password1");
        }

        [TestMethod]
        public void UT_Reader_ResourceReader_Read()
        {
            //arrange
            var reader = new ResourceReader();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reader\resource_en-us.json");
            string culture = "en-us";

            //act
            ResourceContent result = reader.Read(path, culture);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual("en-us", result.Culture);
            Assert.AreEqual(2, result.Content.Count);

            Assert.AreEqual(2, result.Content["AboutPanel"].Count);
            Assert.AreEqual("About Skype Preview", result.Content["AboutPanel"]["AboutTitle"]);
            Assert.AreEqual("This string is the title of About panel.", result.Content["AboutPanel"]["_AboutTitle.comment"]);

            Assert.AreEqual(1, result.Content["AccessibilityLabel"].Count);
            Assert.AreEqual("an audio message", result.Content["AccessibilityLabel"]["Audio"]);
            ;
        }

        [TestMethod]
        public void UT_Reader_MappingToLocIdReader_Read()
        {
            //arrange
            var reader = new UIMappingToLocIdReader();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Reader\mappingToLocId.json");

            //act
            UIMappingToLocIdContent result = reader.Read(path);

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Mapping);
            Assert.IsNotNull(result.Mapping["mappId1"]);
            Assert.IsNotNull(result.Mapping["mappId1"]["text"]);
            Assert.AreEqual(".*value.*", result.Mapping["mappId1"]["text"].MappingSource);
            Assert.AreEqual(MatchedBy.Regex, result.Mapping["mappId1"]["text"].MatchedBy);
            Assert.IsNotNull(result.Mapping["mappId1"]["text"].ResourceIds);
            Assert.AreEqual("value1", result.Mapping["mappId1"]["text"].ResourceIds[0].Value);
            Assert.AreEqual("value2", result.Mapping["mappId1"]["text"].ResourceIds[1].Value);

            Assert.IsNotNull(result.Mapping["mappId2"]);
            Assert.AreEqual("world", result.Mapping["mappId2"]["text"].MappingSource);
            Assert.AreEqual("world", result.Mapping["mappId2"]["text"].ResourceIds[0].Value);
        }

        [TestMethod]
        public void UT_Reader_CultureReader_Read()
        {
            //arrange
            var reader = new CultureReader();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"resource_de.json");

            //act
            string result = reader.Read(path);

            //assert
            Assert.AreEqual("de", result);
        }
    }
}
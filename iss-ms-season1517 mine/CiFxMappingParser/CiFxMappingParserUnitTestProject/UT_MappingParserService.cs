using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiFxMappingParser.Service;
using CiFxMappingParser;
using System.Collections.Generic;
using CiFxMappingParser.Reader;
using System.IO;

namespace CiFxMappingParserUnitTestProject
{
    [TestClass]
    public class UT_MappingParserService
    {
        [TestMethod]
        public void UT_MappingParserService_GetLocalizedMapping_Base_NoRegex_NoSourceMapping_NoLocIDMapping()
        {
            //arrange
            RegexService regexService = new RegexService();
            MappingContent sourceMappingContent = new SourceMappingReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base\sourceMapping.json"));

            ResourceContent englishResourceContent = new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base\resource_en-us.json"), "en-us");

            Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>
            {
                { "fr", new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                  @"MappingService\Base\resource_fr.json"), "fr")
                }
            };

            //act
            MappingParserService mpservice = new MappingParserService(regexService);
            MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, null, null);

            //assert
            Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr"));
            Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
            Assert.AreEqual(0, result.TotalDuplicatedEnglishValues.Count);
            Assert.AreEqual(2, result.UIMappingToLocIdContent.Mapping.Count);
            Assert.AreEqual(1, result.LocalizedMappingsList.Count);
            Assert.AreEqual("AboutPanel|AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]["text"].ResourceIds[0].ResourceId);
            Assert.AreEqual("About Skype Preview", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]["text"].ResourceIds[0].Value);
            Assert.AreEqual("A propos de Skype Preview", result.LocalizedMappingsList["fr"].Items["mk_aboutTitle"].Text);
        }

        [TestMethod]
        public void UT_MappingParserService_GetLocalizedMapping_Base_NoRegex__NoSourceMapping_WithLocIdMapping()
        {
            //arrange
            RegexService regexService = new RegexService();
            MappingContent sourceMappingContent = new SourceMappingReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base\sourceMapping.json"));

            ResourceContent englishResourceContent = new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base\resource_en-us.json"), "en-us");

            Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>
            {
                { "fr", new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                  @"MappingService\Base\resource_fr.json"), "fr")
                }
            };
            UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base\mappingToLocId.json"));

            //act
            MappingParserService mpservice = new MappingParserService(regexService);
            MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, null, uiMappingToLocIdContent);

            //assert
            Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr"));
            Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
            Assert.AreEqual(0, result.TotalDuplicatedEnglishValues.Count);
            Assert.AreEqual(2, result.UIMappingToLocIdContent.Mapping.Count);
            Assert.AreEqual(1, result.LocalizedMappingsList.Count);
            Assert.AreEqual("AboutPanel|AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]["text"].ResourceIds[0].ResourceId);
            Assert.AreEqual("A propos de Skype Preview", result.LocalizedMappingsList["fr"].Items["mk_aboutTitle"].Text);
        }

        [TestMethod]
        public void UT_MappingParserService_GetLocalizedMapping_Base_WithRegex_WithSourceMapping_NoLocIDMapping()
        {
            //arrange
            RegexService regexService = new RegexService();
            MappingContent sourceconfigMappingContent = new SourceMappingReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base_Regex\sourceConfigMapping.json"));

            MappingContent sourceMappingContent = new SourceMappingReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base_Regex\sourceMapping.json"));

            ResourceContent englishResourceContent = new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MappingService\Base_Regex\resource_en-us.json"), "en-us");

            Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>
            {
                { "fr", new ResourceReader().Read(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                  @"MappingService\Base_Regex\resource_fr.json"), "fr")
                }
            };

            //act
            MappingParserService mpservice = new MappingParserService(regexService);
            MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, sourceconfigMappingContent, null);

            //assert
            Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr"));
            Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
            Assert.AreEqual(0, result.TotalDuplicatedEnglishValues.Count);
            Assert.AreEqual(2, result.UIMappingToLocIdContent.Mapping.Count);
            Assert.AreEqual(1, result.LocalizedMappingsList.Count);
            Assert.AreEqual("AboutPanel|AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]["text"].ResourceIds[0].ResourceId);

            Assert.AreEqual("static_[\"A propos de Skype Preview|Ce texte est a propos de About panel.\"]", result.LocalizedMappingsList["fr"].Items["mk_aboutTitle"].Text);
            Assert.AreEqual("static.startsWith_[\"Ce texte est a propos de About panel.\"]", result.LocalizedMappingsList["fr"].Items["mk_aboutTitle"].Accessibility_id);
            Assert.AreEqual("i0118", result.LocalizedMappingsList["fr"].Items["mk_accessibilityLabel"].Id);
            Assert.AreEqual("static_endsWith_[\"un message audio\"]", result.LocalizedMappingsList["fr"].Items["mk_accessibilityLabel"].Text);
        }
    }
}
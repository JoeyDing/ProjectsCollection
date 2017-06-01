using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CiFxMappingParser.Service;
using System.Collections.Generic;
using CiFxMappingParser;
using System.IO;
using CiFxMappingParser.Reader;
using System.Linq;

namespace CiFxMappingParserUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// Find 1 translation by ResourceID
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about title"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel:{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "!!!!About Skype Title!!!!"
        /////     }
        ///// }
        /////
        ///// </summary>
        //[TestMethod]
        //public void BasicCase()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem = new MappingContentItem();
        //    mappingContentItem.Text = "about title";
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value = new Dictionary<string, string>();
        //    loc_value.Add("AboutTitle", "!!!!About Skype Title!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr-fr"));
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //    Assert.AreEqual(1, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(1, result.LocalizedMappingsList.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //    Assert.AreEqual("!!!!About Skype Title!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// Find 1 translation by ResourceID
        ///// multiple properties under mapping key
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about text"
        /////         "text2": "about text2"
        /////         "text3": "about text3"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "!!!!About Skype Title!!!!"
        /////         "text2": "about text2"
        /////         "text3": "about text3"
        /////     }
        ///// }
        /////
        ///// </summary>

        //[TestMethod]
        //public void BasicCase_MultiplePropertiesUnderOneMappingKey()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem = new MappingContentItem();
        //    mappingContentItem.Text = "about title";
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value = new Dictionary<string, string>();
        //    loc_value.Add("AboutTitle", "!!!!About Skype Title!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr-fr"));
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //    Assert.AreEqual(1, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(1, result.LocalizedMappingsList.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //    Assert.AreEqual("!!!!About Skype Title!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// mapping key's not found
        ///// "text" value's found in english file
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about text"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        /////
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        /////
        /////     "mk_aboutText":{
        /////         "text": "!!!!About Skype Title!!!!"
        /////     }
        /////
        /////
        ///// </summary>
        //[TestMethod]
        //public void NoMappingKeyInFile()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem = new MappingContentItem();
        //    mappingContentItem.Text = "about title";
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value = new Dictionary<string, string>();
        //    loc_value.Add("AboutTitle", "!!!!About Skype Title!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //    Assert.AreEqual(1, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(1, result.LocalizedMappingsList.Count);
        //    Assert.AreEqual("!!!!About Skype Title!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// Find two different translations by two different ResourceIDs
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about text"
        /////     }
        /////
        /////     "mk_aboutMain":{
        /////         "text": "main text"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        /////
        /////     "MainPanel":{
        /////     "AboutMain": "main text"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        /////    "AboutPanel":{
        /////    "AboutMain", "!!!!Main!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// "mk_aboutMain": @"MainPanel\AboutMain"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "!!!!About Skype Title!!!!"
        /////     }
        /////
        /////     "mk_aboutMain":{
        /////         "text": "!!!!Main!!!!"
        /////     }
        ///// }
        /////
        ///// </summary>
        //[TestMethod]
        //public void TwoDifTranslationsTwoDifResourceIDs()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem1 = new MappingContentItem { Text = "about title" };
        //    MappingContentItem mappingContentItem2 = new MappingContentItem { Text = "main text" };
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem1 }, { "mk_aboutMain", mappingContentItem2 } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value1 = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    var eng_Value2 = new Dictionary<string, string> { { "AboutMain", "main text" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value1 }, { "MainPanel", eng_Value2 } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value1 = new Dictionary<string, string>();
        //    loc_value1.Add("AboutTitle", "!!!!About Skype Title!!!!");
        //    var loc_value2 = new Dictionary<string, string>();
        //    loc_value2.Add("AboutMain", "!!!!Main!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value1 }, { "MainPanel", loc_value2 } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" }, { "mk_aboutMain", @"MainPanel\AboutMain" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr-fr"));
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(2, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(2, result.LocalizedMappingsList["fr-fr"].Items.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //    Assert.AreEqual("!!!!About Skype Title!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //    Assert.AreEqual(@"MainPanel\AboutMain", result.UIMappingToLocIdContent.Mapping["mk_aboutMain"]);
        //    Assert.AreEqual("!!!!Main!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutMain"].Text);
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// Find two translations, one no translation(resource can't be found) by three ResourceIDs
        /////
        /////  INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about text"
        /////     }
        /////
        /////     "mk_aboutMain":{
        /////         "text": "main text"
        /////     }
        /////
        /////     "mk_aboutWarning":{
        /////         "text": "warning text"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        /////
        /////     "MainPanel":{
        /////     "AboutMain": "main text"
        /////     }
        /////
        /////      "WarningPanel":{
        /////     "AboutWarning": "warning text"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        /////    "AboutPanel":{
        /////    "AboutMain", "!!!!Main!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// "mk_aboutMain": @"MainPanel\AboutMain"
        ///// "mk_aboutWarning": @"WarningPanel\AboutWarning"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "!!!!About Skype Title!!!!"
        /////     }
        /////
        /////     "mk_aboutMain":{
        /////         "text": "!!!!Main!!!!"
        /////     }
        ///// }
        /////
        ///// </summary>
        //[TestMethod]
        //public void TwoDifTranslationsOneNoTransThreeDifResourceIDs()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem1 = new MappingContentItem { Text = "about title" };
        //    MappingContentItem mappingContentItem2 = new MappingContentItem { Text = "main text" };
        //    MappingContentItem mappingContentItem3 = new MappingContentItem { Text = "warning text" };
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem1 }, { "mk_aboutMain", mappingContentItem2 }, { "mk_aboutWarning", mappingContentItem3 } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value1 = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    var eng_Value2 = new Dictionary<string, string> { { "AboutMain", "main text" } };
        //    var eng_Value3 = new Dictionary<string, string> { { "AboutWarning", "warning text" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value1 }, { "MainPanel", eng_Value2 }, { "WarningPanel", eng_Value3 } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value1 = new Dictionary<string, string>();
        //    loc_value1.Add("AboutTitle", "!!!!About Skype Text!!!!");
        //    var loc_value2 = new Dictionary<string, string>();
        //    loc_value2.Add("AboutMain", "!!!!Main!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value1 }, { "MainPanel", loc_value2 } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" } };
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutMain", @"MainPanel\AboutMain" } };
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutWarning", @"WarningPanel\AboutWarning" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr-fr"));
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(3, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(3, result.LocalizedMappingsList["fr-fr"].Items.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //    Assert.AreEqual("!!!!About Skype Text!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //    Assert.AreEqual(@"MainPanel\AboutMain", result.UIMappingToLocIdContent.Mapping["mk_aboutMain"]);
        //    Assert.AreEqual("!!!!Main!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutMain"].Text);
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// No translation's found by a ResourceID
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about title"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        /////  "mk_aboutText":{
        /////         "text": ""
        /////     }
        /////
        /////
        ///// </summary>
        //[TestMethod]
        //public void NoTranslationOneResoourceID()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem = new MappingContentItem { Text = "about title" };
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value1 = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value1 } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value = new Dictionary<string, string>();
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(1, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(1, result.LocalizedMappingsList.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// mapping key's not found
        ///// "text" not found found in english file
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutTitle":{
        /////         "Accessibility_id": "about title1"
        /////     }
        /////
        /////     "mk_aboutTitle2":{
        /////         "Accessibility_id": "about title2"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        /////    "mk_aboutTitle":{
        /////         "Accessibility_id": ""
        /////     }
        /////
        /////     "mk_aboutTitle2":{
        /////         "Accessibility_id": ""
        /////     }
        /////
        ///// </summary>
        //[TestMethod]
        //public void NotFoundText()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem1 = new MappingContentItem { Accessibility_id = "about title1" };
        //    MappingContentItem mappingContentItem2 = new MappingContentItem { Accessibility_id = "about title2" };
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem1 }, { "mk_aboutTitle2", mappingContentItem2 } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value = new Dictionary<string, string>();
        //    loc_value.Add("AboutTitle", "!!!!About Skype Title!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //    Assert.AreEqual(2, result.LocalizedMappingsList["fr-fr"].Items.Count);
        //    Assert.AreEqual(null, result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //    Assert.AreEqual(null, result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle2"].Text);
        //}

        ///// <summary>
        ///// UIMappingToLocID file exists
        ///// RecourceID can be found based on mapping key
        ///// Find more than one translations by a ResourceID
        /////
        ///// INPUT---------------------------------------------------
        /////
        ///// sourceMappingContent
        ///// {
        /////     "mk_aboutText":{
        /////         "text": "about title"
        /////     }
        ///// }
        /////
        ///// englishResourceContent
        ///// {
        /////     "AboutPanel":{
        /////     "AboutTitle": "about title"
        /////     }
        ///// }
        /////
        ///// localizedResourceContent
        ///// {
        /////    "AboutPanel":{
        /////     "AboutTitle":"!!!!About Skype Title!!!!"
        /////    }
        /////
        /////    "TestPanel":{
        /////     "AboutTest":"!!!!About Skype Test!!!!"
        /////    }
        ///// }
        /////
        ///// uiMappingToLocIdContent
        ///// {
        ///// "mk_aboutTitle" : @"AboutPanel\AboutTitle"
        ///// }
        /////
        ///// EXPECTED RESULT-----------------------------------------
        /////
        ///// "mk_aboutText":{
        /////         "text": "about title"
        /////     }
        /////
        ///// </summary>
        //[TestMethod]
        //public void MultipleTranslationsOneResourceID()
        //{
        //    //arrange
        //    MappingContent sourceMappingContent = new MappingContent();
        //    MappingContentItem mappingContentItem = new MappingContentItem { Text = "about title" };
        //    sourceMappingContent.Items = new Dictionary<string, MappingContentItem> { { "mk_aboutTitle", mappingContentItem } };

        //    ResourceContent englishResourceContent = new ResourceContent();
        //    englishResourceContent.Culture = "en-us";
        //    var eng_Value1 = new Dictionary<string, string> { { "AboutTitle", "about title" } };
        //    englishResourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", eng_Value1 } };

        //    Dictionary<string, ResourceContent> localizedResourceContent = new Dictionary<string, ResourceContent>();
        //    var resourceContent = new ResourceContent { };
        //    resourceContent.Culture = "fr-fr";
        //    var loc_value1 = new Dictionary<string, string>();
        //    loc_value1.Add("AboutTitle", "!!!!About Skype Text!!!!");
        //    var loc_value2 = new Dictionary<string, string>();
        //    loc_value2.Add("AboutTest", "!!!!About Skype Test!!!!");
        //    resourceContent.Content = new Dictionary<string, Dictionary<string, string>> { { "AboutPanel", loc_value1 }, { "TestPanel", loc_value2 } };
        //    localizedResourceContent.Add("fr-fr", resourceContent);

        //    UIMappingToLocIdContent uiMappingToLocIdContent = new UIMappingToLocIdContent();
        //    uiMappingToLocIdContent.Mapping = new Dictionary<string, string>() { { "mk_aboutTitle", @"AboutPanel\AboutTitle" } };

        //    //act
        //    MappingParserService mpservice = new MappingParserService();
        //    MappingResult result = mpservice.GetLocalizedMapping(sourceMappingContent, englishResourceContent, localizedResourceContent, uiMappingToLocIdContent);

        //    //assert
        //    Assert.IsTrue(result.LocalizedMappingsList.ContainsKey("fr-fr"));
        //    Assert.IsTrue(result.UIMappingToLocIdContent.Mapping.ContainsKey("mk_aboutTitle"));
        //    Assert.AreEqual(1, result.UIMappingToLocIdContent.Mapping.Count);
        //    Assert.AreEqual(1, result.LocalizedMappingsList.Count);
        //    Assert.AreEqual(@"AboutPanel\AboutTitle", result.UIMappingToLocIdContent.Mapping["mk_aboutTitle"]);
        //    Assert.AreEqual("!!!!About Skype Text!!!!", result.LocalizedMappingsList["fr-fr"].Items["mk_aboutTitle"].Text);
        //    Assert.AreEqual(null, result.TotalDuplicatedEnglishValues);
        //}

        [TestMethod]
        public void ReadingConfigMappingFile_Test()
        {
            //parepare
            string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ui_hash_map.json");
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ui_hash_map_Config.json");
            var sourceMappingContent = new SourceMappingReader().Read(sourcePath);
            var configMappingContent = new SourceMappingReader().Read(configPath);
            var service = new MappingParserService();

            var result = new List<MatchingPattern>();
            foreach (var item in sourceMappingContent.Items)
            {
                var configContentItem = new MappingContentItem();
                if (configMappingContent.Items.ContainsKey(item.Key))
                    configContentItem = configMappingContent.Items[item.Key];
                var sourceContentItem = item.Value;
                //act
                result = service.GetMatchingPattern(sourceContentItem, configContentItem);
            }
            //assert
            Assert.IsTrue(result.Last().Values.Count == 3 && result.Last().Values.Last().SourceValue == @".*Just now.*");
        }

        [TestMethod]
        public void MappingParser_Test()
        {
        }
    }
}
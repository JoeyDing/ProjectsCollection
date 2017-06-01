using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CiFxMappingParser.Service;

namespace CiFxMappingParserUnitTestProject
{
    [TestClass]
    public class UT_RegexService
    {
        [TestMethod]
        public void UT_RegexService_GetValues()
        {
            //arrange
            string source = @"new UiSelector().textMatches(\"".*hello.*|.*world.*\"")";
            string pattern = @"new UiSelector\(\).textMatches\(\\""([^|]+)\|([^|]+)\\""\)";
            var regexService = new RegexService();
            //act
            string[] result = regexService.GetValues(source, pattern);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(".*hello.*", result[0]);
            Assert.AreEqual(".*world.*", result[1]);
        }

        [TestMethod]
        public void UT_RegexService_ReplaceValues()
        {
            //arrange
            string source = @"new UiSelector().textMatches(\"".*hello.*|.*world.*\"")";
            string pattern = @"new UiSelector\(\).textMatches\(\\""([^|]+)\|([^|]+)\\""\)";
            var values = new string[] { "hola", "mundo" };
            var regexService = new RegexService();
            //act
            string result = regexService.ReplaceValues(source, pattern, values);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(@"new UiSelector().textMatches(\""hola|mundo\"")", result);
        }
    }
}
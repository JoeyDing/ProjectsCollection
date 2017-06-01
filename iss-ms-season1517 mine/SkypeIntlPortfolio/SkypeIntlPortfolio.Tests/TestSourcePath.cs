using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Tests
{
    [TestClass]
    public class TestSourcePath
    {
        [TestMethod]
        public void Test_sourcePathValid_LycServer()
        {
            var service = new SourcePathValidator();
            string sourcePath = @"\\skype_drop\FS_SKYPE_TLL\LCT\s";

            bool result = service.IsValid(sourcePath, false);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_sourcePathValid()
        {
            var service = new SourcePathValidator();

            string sourcePath = @"\\sbsrel\Prerelease\lcs\7.0\\";
            bool result = service.IsValid(sourcePath, true);
            Assert.IsTrue(result);
        }
    }
}
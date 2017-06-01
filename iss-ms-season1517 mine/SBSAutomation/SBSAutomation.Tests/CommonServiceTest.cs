using Microsoft.VisualStudio.TestTools.UnitTesting;
using SBSAutomation.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Tests
{
    [TestClass]
    public class CommonServiceTest
    {
        [TestMethod]
        public void CommonService_TakeFullScreenshot()
        {
            //prepare
            var fullScreenService = new FullScreenshotService();
            var saveService = new SaveService();

            //execute
            var imageArray = fullScreenService.TakeFullScreenShot();
            saveService.Save(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "test.PNG"), imageArray);
        }
    }
}
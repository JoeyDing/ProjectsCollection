using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Threading;

namespace SBSAutomation.Services
{
    public class VerifyVersionNumberService : IVerifyVersionNumber
    {
        private RemoteWebDriver driver;
        private ITakeWebScreenshot screenshotService;
        private ISave saveFileService;
        private string url;

        public ISave SaveFileService
        {
            get
            {
                return this.saveFileService;
            }
        }

        public ITakeWebScreenshot ScreenshotService
        {
            get
            {
                return this.screenshotService;
            }
        }

        public RemoteWebDriver WebDriver
        {
            get
            {
                return this.driver;
            }
        }

        public VerifyVersionNumberService(RemoteWebDriver webDriver, string url, ITakeWebScreenshot screenshotService, ISave saveFileService)
        {
            this.driver = webDriver;
            this.url = url;
            this.screenshotService = screenshotService;
            this.saveFileService = saveFileService;
        }
        public bool VerifyVersionNumber()
        {
            this.WebDriver.Navigate().GoToUrl(this.url);
            Thread.Sleep(2000);
            var screenShot = this.ScreenshotService.TakeWebScreenshot(this.WebDriver);
            this.saveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versionNumberOnRightUpperofPage.png"), screenShot.AsByteArray);
            return true;
        }
    }
}

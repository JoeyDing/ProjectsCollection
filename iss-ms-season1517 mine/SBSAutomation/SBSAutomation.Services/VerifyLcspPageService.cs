using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using SBSAutomation.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SBSAutomation.Services
{
    public class VerifyLcspPageService : ICanVerifyLcspPage
    {
        private RemoteWebDriver driver;
        private ITakeWebScreenshot screenshotService;
        private ISave saveFileService;
        private string lscpUrl;

        public RemoteWebDriver WebDriver
        {
            get
            {
                return this.driver;
            }
        }

        public ITakeWebScreenshot ScreenshotService
        {
            get
            {
                return this.screenshotService;
            }
        }

        public ISave SaveFileService
        {
            get
            {
                return this.saveFileService;
            }
        }

        public VerifyLcspPageService(RemoteWebDriver driver, ITakeWebScreenshot screenshotService, ISave saveFileService, string lscpUrl)
        {
            this.driver = driver;
            this.screenshotService = screenshotService;
            this.saveFileService = saveFileService;
            this.lscpUrl = lscpUrl;
        }

        public bool VerifyHomePage()
        {
            ////1 Navigate to URL
            //this.driver.Navigate().GoToUrl(lscpUrl);
            ////1.1 open an new tab.incase the page returns empty
            //IWebElement body = driver.FindElement(By.TagName("body"));
            //body.SendKeys(Keys.Control + 't');
            ////1.2 wait until the [ge's loading successfully
            //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //driver.Url = lscpUrl;
            //IWebElement myDynamicElement = driver.FindElement(By.Id("silverlightControlHost"));
            //var uiElement = myDynamicElement.FindElement(By.Name("HOME"));
            //if(uiElement.Text=="HOME")
            //{
            //    //2 Take screenshot
            //    var screen = this.screenshotService.TakeWebScreenshot(this.driver);
            //    this.saveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "homePage.png"), screen.AsByteArray);
            //    driver.Quit();
            //    return true;
            //}

            //else
            //{
            //    throw new Exception("sorry, the url you provided can't be loaded");
            //}

            //1 Navigate to URL
            this.driver.Navigate().GoToUrl(lscpUrl);
            //1.1 open an new tab.incase the page returns empty
            //IWebElement body = driver.FindElement(By.TagName("body"));
            //body.SendKeys(Keys.Control + 't');
            //this.driver.Navigate().GoToUrl(lscpUrl);
            Thread.Sleep(4000);
            //2 Take screenshot
            var screen = this.screenshotService.TakeWebScreenshot(this.driver);
            this.saveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "homePage.png"), screen.AsByteArray);
            return true;
        }
    }
}
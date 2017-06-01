using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using SBSAutomation.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using System.Diagnostics;

namespace SBSAutomation.Services
{
    public class VerifyTooltipsService : IVerifyToolTips
    {
        private RemoteWebDriver remoteWebDriver;
        private ISave saveFileService;
        private ITakeWebScreenshot takeWebScreenshot;
        private string exeFilePath;
        private string url;
        public RemoteWebDriver RemoteWebDriver
        {
            get
            {
                return remoteWebDriver;
            }
        }

        public ISave SaveFileService
        {
            get
            {
                return saveFileService;
            }
        }

        public ITakeWebScreenshot TakeWebScreenshotService
        {
            get
            {
                return takeWebScreenshot;
            }
        }

        //the 3rd argument type can be srevice or interface but we choose interface, since change of screenshot method might be happened. 
        //but service can decide to select interface or not(and implementation is confirmed to be done when it impelmentates the interface)
        VerifyTooltipsService(RemoteWebDriver remoteWebDriver, string url, string exeFilePath,ITakeWebScreenshot takeWebScreenshot, ISave saveFileService)
        {
            this.remoteWebDriver = remoteWebDriver;
            this.exeFilePath = exeFilePath;
            this.takeWebScreenshot = takeWebScreenshot;
            this.saveFileService = saveFileService;
            this.url = url;
        }

        public bool VerifyTooltips()
        {
            ////check if the source folder is there or not
            //bool exeFilePathExists = System.IO.Directory.Exists(this.exeFilePath);
            //if (!exeFilePathExists)
            //{
            //    throw new ArgumentException(".exe file doesn't exist.");
            //}
            ////1 launch .exe file
            //Process.Start(exeFilePath);
            //var btn_Next = this.remoteWebDriver.FindElementById("31321");
            //btn_Next.Click();
            ////2 Navigate to an webpage
            //this.remoteWebDriver.Navigate().GoToUrl(this.url);
            ////3 Click "LyncServerReports" link on the "ReportServer" home page. 
            ////"?%2fLyncServerReports&rs:Command=ListChildren"  "LyncServerReports"
            //var link_btn_lyncServerReports = this.remoteWebDriver.FindElementById("456");
            //link_btn_lyncServerReports.Click();
            ////4 Click "Reports Home Page" link on "LyncServerReports" page. 
            //var link_btn_reportsHomePage = this.remoteWebDriver.FindElementById("2131");
            ////"?%2fLyncServerReports%2fReports+Home+Page&rs:Command=Render"
            //link_btn_reportsHomePage.Click();
            ////comparision link
            ////class "A47751b1445af434792a2dfae70271e0191l"
            ////click curve that returns from the page. 
            return true;
        }
    }
}
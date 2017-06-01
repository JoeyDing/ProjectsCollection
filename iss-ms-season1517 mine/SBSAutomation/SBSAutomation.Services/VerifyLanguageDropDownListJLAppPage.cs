using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public class VerifyLanguageDropDownListJLAppPage : IVerifyLanguageDropDownListJLAppPage
    {
        public VerifyLanguageDropDownListJLAppPage(InternetExplorerDriver driver, ITakeWebScreenshot screenshotService, ISave saveFileService, ICanSignIn signInSerivce)
        {
            this.webDriver = driver;
            this.screenShotService = screenshotService;
            this.saveService = saveFileService;
            this.signInService = signInSerivce;
        }

        private InternetExplorerDriver webDriver;

        public InternetExplorerDriver WebDriver
        {
            get { return webDriver; }
        }

        private ITakeWebScreenshot screenShotService;

        public ITakeWebScreenshot ScreenShotService
        {
            get { return screenShotService; }
        }

        private ISave saveService;

        public ISave SaveService
        {
            get { return saveService; }
        }

        private ICanSignIn signInService;

        public ICanSignIn SignInService
        {
            get { return signInService; }
        }

        public bool Verify(string signInUrl, string userName, string password, string verifyURL, string outputDir)
        {
            //Console.WriteLine("256545:Sign In to " + signInUrl);
            //1. Sign In

            signInService.SignIn(signInUrl, userName, password, outputDir);

            //2. https://meet.vdomain.com:4443/archcdrtentant1/archcdruser10/1231321321321
            // check the drop down list
            this.webDriver.Navigate().GoToUrl(verifyURL);
            IWebElement dropDownList = this.webDriver.FindElementById("languageSelectCmb15");

            var options = dropDownList.FindElements(By.TagName("option"));

            //Console.WriteLine("256545:Dump dropbox value into file..");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDir, string.Format("{0:yyyy-MM-dd_hh-mm-ss}_dropdownList.txt", System.DateTime.Now))))
            {
                foreach (IWebElement option in options)
                {
                    String text = option.Text;
                    file.WriteLine(text);
                }
            }
            dropDownList.Click();
            Thread.Sleep(2000);
            //Console.WriteLine("256545:Take screenshot..");

            var screen = screenShotService.TakeWebScreenshot(this.webDriver);
            //saveService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDir, string.Format("{0:yyyy-MM-dd_hh-mm-ss}_dropdownList.jpg", System.DateTime.Now)), screen.AsByteArray);
            saveService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VerifyDropdownList.png"), screen.AsByteArray);
            //Console.WriteLine("256545:Finish..");
            return true;
        }
    }
}
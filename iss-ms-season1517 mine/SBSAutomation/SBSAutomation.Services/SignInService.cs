using OpenQA.Selenium;
using OpenQA.Selenium.IE;
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
    public class SignInService : ICanSignIn
    {
        public SignInService( InternetExplorerDriver webDriver,ISave saveService,ITakeWebScreenshot screenShotService)
        {
            this.webDriver = webDriver;
            this.saveService = saveService;
            this.screenShotService = screenShotService;
        }

       
        private ISave saveService;
        public ISave SaveService {
            get {
                return saveService;
            }
        }
        public bool IsSignedIn
        {
            get
            {
                try
                {
                    var imButton = this.WebDriver.FindElement(By.XPath("//button[@tabindex='32573']"));
                    return imButton.Displayed;
                }
                catch
                {
                    return false;
                }
            }
        }

        private InternetExplorerDriver webDriver;

        public InternetExplorerDriver WebDriver
        {
            get
            {
                return webDriver;
            }
        }

       

        private ITakeWebScreenshot screenShotService;
        public ITakeWebScreenshot ScreenShotService {
            get {
                return screenShotService;
            }
        }

    
        public bool SignIn(string signInURL,string username, string password, string outputDir)
        {
            string testResultDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDir );
            if (!Directory.Exists(testResultDirectory))
            {
                Directory.CreateDirectory(testResultDirectory);
            }

            this.WebDriver.Navigate().GoToUrl(signInURL);

            //access inner iframe
            Thread.Sleep(1000);
         
            var userNameBox = this.WebDriver.FindElementById("loginUsername");
            var pwdBox = this.WebDriver.FindElementById("loginPassword");
            userNameBox.SendKeys(username);
            pwdBox.SendKeys(password);

            //2 Find Sign in org button and click it
            var signInOrg = this.WebDriver.FindElementById("signInButton");
            signInOrg.Click();

           
            var signInScreen = this.screenShotService.TakeWebScreenshot(this.WebDriver);
            saveService.Save(Path.Combine(testResultDirectory,
                string.Format("{0:yyyy-MM-dd_hh-mm-ss}-SignInPage_withUserPwd.jpg", System.DateTime.Now)),
                signInScreen.AsByteArray);
            Thread.Sleep(1000);
            return this.IsSignedIn;
        }
    }
}
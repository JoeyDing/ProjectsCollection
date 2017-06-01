using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using SBSAutomation.Core;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.IE;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

namespace SBSAutomation.Services
{
    public class VerifyLyncWebSchedulerSignInService : IVerifyLyncWebSchedulerSignInPage
    {

        private InternetExplorerDriver driver;
        private ITakeWebScreenshot screenshotService;
        private ISave saveFileService;
        private string url;
        private ICanSignIn signInService;
        private string username;
        private string password;
        private string outputDirectory;


        public InternetExplorerDriver RemoteWebDriver
        {
            get
            {
                return driver;
            }
        }

        public ISave SaveFiles
        {
            get
            {
                return saveFileService;
            }
        }

        public ITakeWebScreenshot TakeWebScreenshot
        {
            get
            {
                return screenshotService;
            }
        }

        public ICanSignIn SignInService
        {
            get
            {
                return signInService;
            }
        }

        public VerifyLyncWebSchedulerSignInService(InternetExplorerDriver driver, string url, ITakeWebScreenshot screenshotService, ISave saveFileService, ICanSignIn signInService, string username,string password,string outputDirectory)
        {
            this.driver = driver;
            this.url = url;
            this.screenshotService = screenshotService;
            this.saveFileService = saveFileService;
            this.username = username;
            this.password = password;
            this.outputDirectory = outputDirectory;
            this.signInService = signInService;
        }

        public bool VerifyLyncWebSchedulerSignIn()
        {
            this.RemoteWebDriver.Navigate().GoToUrl(this.url);
            Thread.Sleep(2000);
            //take a screenshot for login page(verify the logo,field name...)
            var screenshotOne = this.screenshotService.TakeWebScreenshot(this.RemoteWebDriver);
            this.saveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LyncWebSchedulerSignInPage.png"), screenshotOne.AsByteArray);

            //var signInService = new SignInService(this.RemoteWebDriver, SaveFiles, screenshotService);
            SignInService.SignIn(this.url, this.username, this.password, this.outputDirectory);
            Thread.Sleep(2000);
            var signInScreen = this.screenshotService.TakeWebScreenshot(this.RemoteWebDriver);
            this.saveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LyncWebSchedulerContinueResult.png"), signInScreen.AsByteArray);
            return true;
        }
    }
}

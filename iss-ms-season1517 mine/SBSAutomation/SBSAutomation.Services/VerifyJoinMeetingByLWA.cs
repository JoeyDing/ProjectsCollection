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
    public class VerifyJoinMeetingByLWA : IVerifyJoinMeetingByLWA
    {

        public VerifyJoinMeetingByLWA(InternetExplorerDriver driver, ITakeWebScreenshot screenshotService, ISave saveFileService, ICanSignIn signInSerivce)
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

        public bool Verify(string signInUrl, string userName,string password, string verifyURL,string outputDir)
        {
            //Console.WriteLine("256545:Sign In to "+signInUrl);
            //1. Sign In
            signInService.SignIn(signInUrl, userName, password, outputDir);


            //1.Create Meeting
            IWebElement tbSubject = this.webDriver.FindElementById("subject");
            tbSubject.SendKeys("automation256540");

            IWebElement btnSaveMeeting = this.webDriver.FindElementById("btnSaveMeeting");
            btnSaveMeeting.Click();

            Thread.Sleep(2000);
            var lnkInvite = this.webDriver.FindElementById("inviteLink").FindElement(By.TagName("a"));
            var linkText = lnkInvite.GetAttribute("href");
            string domain = "https://meet.vdomain.com";
            linkText = "https://meet.vdomain.com:4443/" + linkText.Substring(domain.Length + 1);
            Console.WriteLine(linkText);

            this.webDriver.Navigate().GoToUrl(linkText);
            Thread.Sleep(4000);

            this.webDriver.SwitchTo().Alert().Dismiss();
            this.webDriver.SwitchTo().Frame(0);


            var tbUserName = this.webDriver.FindElementByXPath("//input[@tabindex='6020']");
            var tbPassword = this.webDriver.FindElementByXPath("//input[@tabindex='6688']");
            var btnJoinMeeting = this.webDriver.FindElementByXPath("//button[@tabindex='9363']");
            tbUserName.SendKeys(userName);
            tbPassword.SendKeys(password);
            btnJoinMeeting.Click();
            Thread.Sleep(4000);

            //Console.WriteLine("256540:Take screenshot..");
            var screen = screenShotService.TakeWebScreenshot(this.webDriver);
            saveService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"verifyJoinMeeting.png"), screen.AsByteArray);
            //Console.WriteLine("256540:Finish..");
            return true;
        }
    }
}

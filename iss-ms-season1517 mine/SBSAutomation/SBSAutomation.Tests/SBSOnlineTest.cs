using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using SBSAutomation.Core.Services;
using SBSAutomation.Services;
using SBSAutomation.Services.Common;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SBSAutomation.Tests
{
    [TestClass]
    public class SBSOnlineTest
    {
        [TestMethod]
        public void SBSOnline_VerifyLcspHomePage()
        {
            //prepare
            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveSevice = new SaveService();
            var lcspService = new VerifyLcspPageService(driver, screenShotService, saveSevice, "https://server.vdomain.com/cscp");
            var screenshotService = new WebScreenshotService();
            //execute
            bool result = lcspService.VerifyHomePage();
            driver.Quit();

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_CheckExisitngFilesUnderOCO()
        {
            //execute
            var checkExsitingFilesService1 = new VerifyExistingFilesService();
            //@"B88O13-10:\F:\SU Build\OCO"
            bool result = checkExsitingFilesService1.VerifyExistingFiles(@"C:\Users\v-joding\Desktop\OCO", AppDomain.CurrentDomain.BaseDirectory, "ResultFileUnderOCO.txt");
            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_CheckExisitngFilesUnderSpeech()
        {
            //execute
            var checkExsitingFilesService = new VerifyExistingFilesService();
            //@"B88O13-10:\F:\SU Build\OCO"
            bool result = checkExsitingFilesService.VerifyExistingFiles(@"C:\Users\v-joding\Desktop\OCO", AppDomain.CurrentDomain.BaseDirectory, "ResultFileUnderSpeech.txt");
            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_VerifyStartWindowForOCSUMUtil()
        {
            //prepare
            var fullScreenShotService = new FullScreenshotService();
            var saveSevice = new SaveService();
            var verifyStartWindowForOCSUMUtilService = new VerifyStarWndowsForOCSUMUtilService(fullScreenShotService, saveSevice, @"C:\Program Files\Common Files\Skype for Business Server 2015\Support\OcsUmUtil.exe");
            //execute
            bool result = verifyStartWindowForOCSUMUtilService.VerifyStartWindowsForOCSUMUtil();
            //assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 256545 W15 – Join Launcher – Verify the language drop down list in JL App page
        /// https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_workitems?id=256545&_a=edit
        /// </summary>
        [TestMethod]
        public void SBSOnline_VerifyLanguageDropDownListJLAppPage()
        {
            var userName = "vdomain\\archcdruser10";
            var pwd = "07Apples";
            var outputDir = "256545";

            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveService = new SaveService();
            var signInService = new SignInService(driver, saveService, screenShotService);

            var signURL = "https://ext.vdomain.com:4443/scheduler";
            var verifyURL = "https://meet.vdomain.com:4443/archcdrtentant1/archcdruser10/1231321321321";

            VerifyLanguageDropDownListJLAppPage testCase = new VerifyLanguageDropDownListJLAppPage(driver, screenShotService, saveService, signInService);
            bool result = testCase.Verify(signURL, userName, pwd, verifyURL, outputDir);
            driver.Quit();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_VerifyVersionNumberOnRightUpperofPage()
        {
            //prepare
            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveSevice = new SaveService();
            var service = new VerifyVersionNumberService(driver, "https://server.vdomain.com/cscp", screenShotService, saveSevice);

            //execute
            bool result = service.VerifyVersionNumber();
            driver.Quit();

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_VerifyPerformanceCounter()
        {
            //prepare
            var screenShotService = new FullScreenshotService();
            var saveService = new SaveService();
            var runCommandService = new RunCommandService();
            var verifyPerfCounter = new VerifyPerformanceCounterService(screenShotService, runCommandService, saveService,"Netlogon");

            //execute
            bool result = verifyPerfCounter.VerifyLsMediaCounter();

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_VerifyLyncWebSchedulerSignIn()
        {
            //prepare
            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveSevice = new SaveService();
            var signInService = new SignInService(driver, saveSevice, screenShotService);
            var service = new VerifyLyncWebSchedulerSignInService(driver, "https://ext.vdomain.com:4443/scheduler", screenShotService, saveSevice, signInService, "vdomain\archcdruser10", "07Apples", "256554");

            //execute
            bool result = service.VerifyLyncWebSchedulerSignIn();
            driver.Quit();

            //assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SBSOnline_VerifyServiceOnSeFe()
        {
            //prepare
            var screenShotService = new FullScreenshotService();
            var runCommandService = new RunCommandService();
            var saveService = new SaveService();
            var verifyPerfCounter = new VerifyServiceOnSeFeService(screenShotService, runCommandService, saveService, "Remote");

            //execute
            bool result = verifyPerfCounter.VerifyServiceOnSeFe();

            //assert
            Assert.IsTrue(result);
        }


        /// <summary>
        /// 256540 LWA- Verify the user can join the meeting by LWA
        /// https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_workitems?_a=edit&fullScreen=True&id=256540&triage=true
        /// </summary>
        [TestMethod]
        public void SBSOnline_VerifyJoinMeetingByLWA() {
            var userName = "vdomain\\archcdruser10";
            var pwd = "07Apples";
            var outputDir = "256540";
           
            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveService = new SaveService();
            var signInService = new SignInService(driver, saveService, screenShotService);

            var signURL = "https://ext.vdomain.com:4443/scheduler";
            var verifyURL = "https://meet.enujay3.com:4443/test1/KSAMDTRJ ";

            VerifyJoinMeetingByLWA testCase = new VerifyJoinMeetingByLWA(driver, screenShotService, saveService, signInService);
            testCase.Verify(signURL, userName, pwd, verifyURL, outputDir);
        }

        [TestMethod]
        public void SBSAutomation_VerifyActionMenu()
        {
            //prepare
            var screenShotService = new FullScreenshotService();
            var saveService = new SaveService();
            var verifyActionMenuService = new VerifyActionMenuService(@"C:\Program Files\Skype for Business Server 2015\Administrative Tools\Microsoft.Rtc.Management.TopologyBuilder.exe", screenShotService, saveService);
            //execute
            bool result = verifyActionMenuService.VerifyActionMenu();
            //assert
            Assert.IsTrue(result);
        }
    }
}
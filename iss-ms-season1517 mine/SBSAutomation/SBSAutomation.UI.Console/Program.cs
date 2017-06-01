using OpenQA.Selenium.IE;
using SBSAutomation.Core.Services;
using SBSAutomation.Services;
using SBSAutomation.Services.Common;
using SBSAutomation.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //1.Test Case 256514:LSCP - Home - Verify mainUI and function
            Console.WriteLine("Test Case 256514:LSCP - Home - Verify mainUI and function");
            var driver = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService = new WebScreenshotService();
            var saveSevice = new SaveService();
            var lcspService = new VerifyLcspPageService(driver, screenShotService, saveSevice, "https://server.vdomain.com/cscp");
            var screenshotService = new WebScreenshotService();
            bool result = lcspService.VerifyHomePage();
            driver.Quit();



            //2.Test Case 256518:LSCP - Check the version number on the right upper of main page.
            Console.WriteLine("Test Case 256518:LSCP - Check the version number on the right upper of main page");
            var driver2 = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService2 = new WebScreenshotService();
            var saveSevice2 = new SaveService();
            var service2 = new VerifyVersionNumberService(driver2, "https://server.vdomain.com/cscp", screenShotService2, saveSevice2);
            bool result2 = service2.VerifyVersionNumber();
            driver2.Quit();



            //3.Test Case 256532:Services-Verify all the services of Lync Server on SE/FE can display and start properly.
            Console.WriteLine("Test Case 256532:Services - Verify all the services of Lync Server on SE / FE can display and start properly");
            var screenShotService3 = new FullScreenshotService();
            var runCommandService3 = new RunCommandService();
            var saveService3 = new SaveService();
            var verifyPerfCounter3 = new VerifyServiceOnSeFeService(screenShotService3, runCommandService3, saveService3, "Skype for Business");
            //var verifyPerfCounter3 = new VerifyServiceOnSeFeService(screenShotService3, runCommandService3, saveService3, "Remote");
            bool result3 = verifyPerfCounter3.VerifyServiceOnSeFe();



            //4.Test Case 256535:OCSUMUtil-Verify the start windows for OCSUMUtil.exe、、
            Console.WriteLine("Test Case 256535:OCSUMUtil-Verify the start windows for OCSUMUtil.exe");
            var fullScreenShotService4 = new FullScreenshotService();
            var saveSevice4 = new SaveService();
            var verifyStartWindowForOCSUMUtilService4 = new VerifyStarWndowsForOCSUMUtilService(fullScreenShotService4, saveSevice4, @"C:\Program Files\Common Files\Skype for Business Server 2015\Support\OcsUmUtil.exe");
            //var verifyStartWindowForOCSUMUtilService4 = new VerifyStarWndowsForOCSUMUtilService(fullScreenShotService4, saveSevice4, @"C:\Program Files\TestFolder\testExeFile.exe");
            bool result4 = verifyStartWindowForOCSUMUtilService4.VerifyStartWindowsForOCSUMUtil();



            //5.Test Case 256536:Topology Builder -Lync Server 2013, Topology Builder -Verify 'Action' menu.
            Console.WriteLine("Test Case 256536:Topology Builder -Lync Server 2013, Topology Builder -Verify 'Action' menu");
            var screenShotService5 = new FullScreenshotService();
            var saveService5 = new SaveService();
            var verifyActionMenuService5 = new VerifyActionMenuService(@"C:\Program Files\Skype for Business Server 2015\Administrative Tools\Microsoft.Rtc.Management.TopologyBuilder.exe", screenShotService5, saveService5);
            //execute
            bool result5 = verifyActionMenuService5.VerifyActionMenu();



            //6.Test Case 256540:LWA- Verify the user can join the meeting by LWA.
            try
            {
                Console.WriteLine("Test Case 256540:LWA- Verify the user can join the meeting by LWA");
                var userName6 = "vdomain\\archcdruser10";
                var pwd6 = "07Apples";
                var outputDir6 = "256540";
                var driver6 = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
                var screenShotService6 = new WebScreenshotService();
                var saveService6 = new SaveService();
                var signInService6 = new SignInService(driver6, saveService6, screenShotService6);
                var signURL6 = "https://ext.vdomain.com:4443/scheduler";
                var verifyURL6 = "https://meet.enujay3.com:4443/test1/KSAMDTRJ ";
                VerifyJoinMeetingByLWA testCase = new VerifyJoinMeetingByLWA(driver6, screenShotService6, saveService6, signInService6);
                testCase.Verify(signURL6, userName6, pwd6, verifyURL6, outputDir6);
                driver6.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred when Test Case 256540 is running : '{0}'", e);
            }



            //7.Test Case 256545:W15 - Join Launcher – Verify the language drop down list in JL App page
            Console.WriteLine("Test Case 256545:W15 - Join Launcher – Verify the language drop down list in JL App page");
            var userName7 = "vdomain\\archcdruser10";
            var pwd7 = "07Apples";
            var outputDir7 = "256545";
            var driver7 = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService7 = new WebScreenshotService();
            var saveService7 = new SaveService();
            var signInService7 = new SignInService(driver7, saveService7, screenShotService7);
            var signURL_7 = "https://ext.vdomain.com:4443/scheduler";
            var verifyURL_7 = "https://meet.vdomain.com:4443/archcdrtentant1/archcdruser10/1231321321321";
            VerifyLanguageDropDownListJLAppPage testCase7 = new VerifyLanguageDropDownListJLAppPage(driver7, screenShotService7, saveService7, signInService7);
            testCase7.Verify(signURL_7, userName7, pwd7, verifyURL_7, outputDir7);
            driver7.Quit();



            //8.Test Case 256554:Web Scheduler-Sign in- Verify Lync Web Scheduler sign in page.
            Console.WriteLine("Test Case 256554:Web Scheduler-Sign in- Verify Lync Web Scheduler sign in page");
            var driver8 = new InternetExplorerDriver(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Driver"));
            var screenShotService8 = new WebScreenshotService();
            var saveSevice8 = new SaveService();
            var signInService8 = new SignInService(driver8, saveSevice8, screenShotService8);
            var service8 = new VerifyLyncWebSchedulerSignInService(driver8, "https://ext.vdomain.com:4443/scheduler", screenShotService8, saveSevice8, signInService8, "vdomain\archcdruser10", "07Apples", "256554");
            bool result8 = service8.VerifyLyncWebSchedulerSignIn();
            driver8.Quit();




            //9.Test Case 256590:Performance Counter
            Console.WriteLine("Test Case 256590:Performance Counter");
            var screenShotService9 = new FullScreenshotService();
            var saveService9 = new SaveService();
            var runCommandService9 = new RunCommandService();

            var verifyPerfCounter9 = new VerifyPerformanceCounterService(screenShotService9, runCommandService9, saveService9, "LS:MEDIA - Operations");
            bool result9 = verifyPerfCounter9.VerifyLsMediaCounter();



            //10.Test Case 256806:MSI- Verify all the Lync Server installation MSI files are existed in the correct Lync Server build folder
            Console.WriteLine("Test Case 256806-1:MSI- Verify all the Lync Server installation MSI files are existed in the correct Lync Server build folder");
            var checkExsitingFilesService = new VerifyExistingFilesService();
            bool result10_1 = checkExsitingFilesService.VerifyExistingFiles(@"C:\Users\Administrator.vdomain\Desktop\7.0.1201.0\OCO\amd64\Setup\Speech", AppDomain.CurrentDomain.BaseDirectory, "ResultFileUnderSpeech.txt");
            Console.WriteLine("Test Case 256806-2:MSI- Verify all the Lync Server installation MSI files are existed in the correct Lync Server build folder");
            var checkExsitingFilesService1 = new VerifyExistingFilesService();
            bool result10_2 = checkExsitingFilesService1.VerifyExistingFiles(@"C:\Users\Administrator.vdomain\Desktop\7.0.1201.0\OCO\amd64\Setup", AppDomain.CurrentDomain.BaseDirectory, "ResultFileUnderSetup.txt");
        }
    }
}
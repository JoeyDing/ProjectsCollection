using SBSAutomation.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SBSAutomation.Core;
using TestStack.White;

namespace SBSAutomation.Services
{
    public class VerifyStarWndowsForOCSUMUtilService : IVerifyStartWindowsForOCSUMUtil
    {
        private ITakeFullScreenShot fullScreenshotService;
        private ISave saveFileService;
        private string exeFilePath;

        public ITakeFullScreenShot ScreenshotService
        {
            get
            {
                return fullScreenshotService;
            }
        }

        public ISave SaveFileService
        {
            get
            {
                return saveFileService;
            }
        }

        public VerifyStarWndowsForOCSUMUtilService(ITakeFullScreenShot fullScreenshotService, ISave saveFileService, string exeFilePath)
        {
            this.fullScreenshotService = fullScreenshotService;
            this.saveFileService = saveFileService;
            this.exeFilePath = exeFilePath;
        }

        //public bool VerifyStartWindowsForOCSUMUtil(string exeFilePath, ITakeWebScreenshot screenshotService)
        public bool VerifyStartWindowsForOCSUMUtil()
        {
            //launch .exe file
            //check if the source folder is there or not
            bool exeFilePathExists = File.Exists(this.exeFilePath);
            if (!exeFilePathExists)
            {
                throw new ArgumentException(".exe file doesn't exist.");
            }

            //1 launch .exe file
            Process.Start(exeFilePath);
            Thread.Sleep(2000);
            //move the top window to make sure two opened windows can be captured clearly
            //3.1 Get service Window Instance
            var process = Process.GetProcessesByName("OcsUmUtil").First();
            Application app = Application.Attach(process.Id);
            var window = app.GetWindows().First();
            WindowExtensions.Move(window, 20, 220);
            //2 Take screenshot
            var byteImage = this.ScreenshotService.TakeFullScreenShot();
            //3 save screenshot
            this.SaveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshotForVerifyingStartWindowsForOCUMUtil.png"),byteImage);
            return true;
        }
    }
}
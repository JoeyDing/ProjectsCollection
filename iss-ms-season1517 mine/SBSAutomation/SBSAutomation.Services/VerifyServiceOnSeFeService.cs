using SBSAutomation.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace SBSAutomation.Services
{
    public class VerifyServiceOnSeFeService : IVerifyServiceOnSeFe
    {
        private readonly string serviceName;
        private readonly ISave saveFileService;

        public ISave SaveFileService
        {
            get { return saveFileService; }
        }

        private ITakeFullScreenShot screenshotService;

        public ITakeFullScreenShot ScreenshotService
        {
            get { return screenshotService; }
        }

        private readonly IRunCommand runCommandService;

        public IRunCommand RunCommandService
        {
            get { return runCommandService; }
        }

        public VerifyServiceOnSeFeService(ITakeFullScreenShot screenshotService, IRunCommand runCommandService, ISave saveFileService, string serviceName = "Skype for Business")
        {
            this.screenshotService = screenshotService;
            this.runCommandService = runCommandService;
            this.saveFileService = saveFileService;
            this.serviceName = serviceName;
        }

        private bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }

            return false;
        }

        public bool VerifyServiceOnSeFe()
        {
            //0 set global timout to 10 seconds, to avoid timeout exception after 5s
            CoreAppXmlConfiguration.Instance.BusyTimeout = 10000;

            //1 Close any mmc window open
            string appName = "mmc";
            if (this.IsProcessOpen(appName))
            {
                Process.GetProcessesByName(appName).ToList().ForEach(proc => proc.Kill());
            }

            //2 run service mmc window
            this.runCommandService.RunCommand("mmc services.msc");

            Thread.Sleep(4000);

            //3 run automation

            //3.1 Get service Window Instance
            var process = Process.GetProcessesByName("mmc").First();
            Application app = Application.Attach(process.Id);
            var windowMonitor = app.GetWindows().First();
            windowMonitor.DisplayState = TestStack.White.UIItems.WindowItems.DisplayState.Maximized;

            //3.2 Get grid
            var grid = windowMonitor.Get<ListView>(SearchCriteria.ByAutomationId("12786"));
            var all = grid.Rows.Where(c => c.Name.ToLower().StartsWith(serviceName.ToLower())).ToList();
            var lastRow = all.Last();
            lastRow.Select();
            var verticalScroll = grid.ScrollBars.Vertical;
            var element = windowMonitor.Get(SearchCriteria.ByText(lastRow.Name));

            //select element
            bool found = false;
            while (verticalScroll.IsNotMinimum && verticalScroll.Value!=verticalScroll.MaximumValue)
            {
            grid.Focus();
            if (element.Visible)
            {
                found = true;
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                verticalScroll.ScrollUp();
                break;
            }
            verticalScroll.ScrollDown();
            }


            var byteImage = this.ScreenshotService.TakeFullScreenShot();
            this.SaveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshotofRunningServicesMsc.png"), byteImage);
            return true;
        }
    }
}
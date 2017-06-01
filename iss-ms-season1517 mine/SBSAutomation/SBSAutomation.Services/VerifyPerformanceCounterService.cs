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
    public class VerifyPerformanceCounterService : IVerifyPerformanceCounter
    {
        private readonly string counterName;
        private readonly ITakeFullScreenShot screenshotService;
        private readonly ISave saveService;
        private readonly IRunCommand runCommandService;

        public VerifyPerformanceCounterService(
            ITakeFullScreenShot screenshotService,
            IRunCommand runCommandService,
            ISave saveService,
            string counterName = "LS:MEDIA - Operations")
        {
            this.screenshotService = screenshotService;
            this.saveService = saveService;
            this.runCommandService = runCommandService;
            this.counterName = counterName;
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

        public bool VerifyLsMediaCounter()
        {
            //0 set global timout to 10 seconds, to avoid timeout exception after 5s
            CoreAppXmlConfiguration.Instance.BusyTimeout = 5000;

            //1 Close any mmc window open
            string appName = "mmc";
            if (this.IsProcessOpen(appName))
            {
                Process.GetProcessesByName(appName).ToList().ForEach(proc => proc.Kill());
            }

            //2 run performance monitor app
            runCommandService.RunCommand("mmc perfmon.msc");
            Thread.Sleep(4000);

            //3 run automation
            var process = Process.GetProcessesByName("mmc").First();
            Application app = Application.Attach(process.Id);
            var windowMonitor = app.GetWindows().First();

            var titleBar = windowMonitor.Get(SearchCriteria.ByText("Performance Monitor"));
            titleBar.Click();

            var btnGreen = windowMonitor.Items[9];
            btnGreen.Click();
            Thread.Sleep(3000);

            var windowAddCountercounter = app.GetWindows().Last();
            //check checkbox
            var checkBox = windowAddCountercounter.Get<CheckBox>(SearchCriteria.ByText("Show description"));
            checkBox.Click();

            //get grid
            var dataGrid = windowAddCountercounter.Get<ListView>(SearchCriteria.ByText("Counter detail level:"));

            var verticalScroll = dataGrid.ScrollBars.Vertical;
            var element = windowAddCountercounter.Get(SearchCriteria.ByText(this.counterName));

            //select element
            bool found = false;
            while (verticalScroll.IsNotMinimum)
            {
                dataGrid.Focus();
                if (element.Visible)
                {
                    found = true;

                    //double click element
                    verticalScroll.ScrollUp();
                    element.Focus();
                    element.Click();
                    element.DoubleClick();
                    break;
                }

                //verticalScroll.ScrollUpLarge();
                //Thread.Sleep(300);
                verticalScroll.ScrollUp();
            }

            Thread.Sleep(2000);
            var byteImage = this.screenshotService.TakeFullScreenShot();
            this.saveService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"VerifyPerformanceCounter.png"),byteImage);
            if (!found)
            {
                throw new Exception("Cannot find item");
            }
            return true;
        }
    }
}
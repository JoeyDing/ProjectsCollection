using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using TestStack.White;
using TestStack.White.UIItems.Finders;

namespace SBSAutomation.Services
{
    public class VerifyActionMenuService : IVerifyActionMenu
    {
        private ISave saveFileService;
        private ITakeFullScreenShot takeFullScreenShotService;
        private string appPath;

        public ISave SaveFileService
        {
            get
            {
                return saveFileService;
            }
        }

        public string AppPath
        {
            get
            {
                return this.appPath;
                     
            }
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

        public ITakeFullScreenShot TakeFullScreenShotService
        {
            get
            {
                return takeFullScreenShotService;
            }
        }

        public VerifyActionMenuService(string appPath, ITakeFullScreenShot takeFullScreenShotService,ISave saveFileService)
        {
            this.appPath = appPath;
            this.takeFullScreenShotService = takeFullScreenShotService;
            this.saveFileService = saveFileService;
        }
        public bool VerifyActionMenu()
        {
            //1 Close any opened TopologyBuilder window
            string appName = "Microsoft.Rtc.Management.TopologyBuilder";
            if (this.IsProcessOpen(appName))
            {
                Process.GetProcessesByName(appName).ToList().ForEach(proc => proc.Kill());
            }

            //2 check if the source folder is there or not
            bool exeFilePathExists = File.Exists(this.AppPath);
            if (!exeFilePathExists)
            {
                throw new ArgumentException("TopologyBuilder.exe file doesn't exist.");
            }

            //3 run Topologybuilder.exe
            Process.Start(this.AppPath);
            Thread.Sleep(2000);

            //4 run automation
            var process = Process.GetProcessesByName("Microsoft.Rtc.Management.TopologyBuilder").First();
            Application app = Application.Attach(process.Id);
            var windowsTopologyBuilder = app.GetWindows();
            Thread.Sleep(2000);
            //4.1 close the top topology builder window firstly
            var subWindow = windowsTopologyBuilder.Where(w => w.Name == "Topology Builder").First();
            subWindow.Close();
            //4.2 get th main window and its action button from menu bar
            var mainWindow = windowsTopologyBuilder.Where(w => w.Name == "Skype for Business Server 2015, Topology Builder").First();
            var windowMenuBar = mainWindow.GetMenuBar(SearchCriteria.ByAutomationId("TopoBuilderMenu"));
            var btnAction = windowMenuBar.MenuItemBy(SearchCriteria.ByAutomationId("ActionMenu"));
            Thread.Sleep(2000);
            btnAction.Click();

            //5 take screenshot
            var byteImage = this.TakeFullScreenShotService.TakeFullScreenShot();
            //6 save file
            this.SaveFileService.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"verifyTopologyBuilderActionMenu.png"), byteImage);
            return true;
        }
    }
}

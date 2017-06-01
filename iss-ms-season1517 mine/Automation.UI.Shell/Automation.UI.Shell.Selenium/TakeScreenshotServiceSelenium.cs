using Automation.UI.Shell.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using OpenQA.Selenium;

namespace Automation.UI.Shell.Selenium
{
    public class TakeScreenshotServiceSelenium : ITakeScreenshotSelenium
    {
        private RemoteWebDriver webDriver;
        public void SetWebDriver(RemoteWebDriver driver)
        {
            this.webDriver = driver;
        }
        public void SaveScreenshot(string path, byte[] screenshot)
        {
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                filestream.Write(screenshot, 0, screenshot.Count());
            }
        }
        public ScreenShotResult TakeScreenShot()
        {
            var result = webDriver.GetScreenshot();
            return new ScreenShotResult(result.AsByteArray);
        }
    }
}

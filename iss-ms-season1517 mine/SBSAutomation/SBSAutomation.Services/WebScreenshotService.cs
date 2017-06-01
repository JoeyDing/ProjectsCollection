using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SBSAutomation.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Services
{
    public class WebScreenshotService : ITakeWebScreenshot
    {
        public Screenshot TakeWebScreenshot(RemoteWebDriver selenium)
        {
            System.Diagnostics.Debug.WriteLine("Saving...");
            return selenium.GetScreenshot();
        }
    }
}
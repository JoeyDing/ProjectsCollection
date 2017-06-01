using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;
using OpenQA.Selenium.Remote;

namespace Automation.UI.Shell.Selenium
{
    public interface ITakeScreenshotSelenium: ITakeScreenshot
    {
        void SetWebDriver(RemoteWebDriver driver);
    }
}

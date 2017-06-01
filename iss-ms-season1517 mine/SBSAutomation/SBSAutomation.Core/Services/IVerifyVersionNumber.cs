using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyVersionNumber
    {
        RemoteWebDriver WebDriver { get; }
        ITakeWebScreenshot ScreenshotService { get; }
        ISave SaveFileService { get; }
        bool VerifyVersionNumber();
    }
}

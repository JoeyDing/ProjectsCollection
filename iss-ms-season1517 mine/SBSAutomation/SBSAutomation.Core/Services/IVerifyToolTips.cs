using System;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyToolTips
    {
        ITakeWebScreenshot TakeWebScreenshotService { get; }
        RemoteWebDriver RemoteWebDriver { get; }
        ISave SaveFileService { get; }
        bool VerifyTooltips();
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface ITakeWebScreenshot
    {
        Screenshot TakeWebScreenshot(RemoteWebDriver selenium);
    }
}
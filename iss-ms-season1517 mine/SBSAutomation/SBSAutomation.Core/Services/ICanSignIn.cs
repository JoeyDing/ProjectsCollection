using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface ICanSignIn
    {
        InternetExplorerDriver WebDriver { get; }

        ITakeWebScreenshot ScreenShotService { get; }

        ISave SaveService { get; }

        bool IsSignedIn { get; }

        bool SignIn(string signInURL, string username, string password, string outputDir);
    }
}
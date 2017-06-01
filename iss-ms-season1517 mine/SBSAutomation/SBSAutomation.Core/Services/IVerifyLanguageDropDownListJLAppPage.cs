using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyLanguageDropDownListJLAppPage
    {
        InternetExplorerDriver WebDriver { get; }

        ITakeWebScreenshot ScreenShotService { get; }

        ISave SaveService { get; }

        ICanSignIn SignInService { get; }

        bool Verify(string signInUrl, string userName, string password, string verifyURL, string outputDir);
    }
}
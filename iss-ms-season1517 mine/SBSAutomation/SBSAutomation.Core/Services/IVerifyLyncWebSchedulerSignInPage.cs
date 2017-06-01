using System;
using System.Collections.Generic;
using SBSAutomation.Core.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.IE;

namespace SBSAutomation.Core
{
    public interface IVerifyLyncWebSchedulerSignInPage
    {
        //properties should be removed in the future, cuz doesn't obey single-responsbility principle
        InternetExplorerDriver RemoteWebDriver { get; }
        //sign in interaface
         ITakeWebScreenshot TakeWebScreenshot { get; }
        ISave SaveFiles { get; }

        ICanSignIn SignInService { get; }

        bool VerifyLyncWebSchedulerSignIn();
    }
}

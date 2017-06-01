using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyStartWindowsForOCSUMUtil
    {
        ITakeFullScreenShot ScreenshotService { get; }
        ISave SaveFileService { get; }

        bool VerifyStartWindowsForOCSUMUtil();
    }
}
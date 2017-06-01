using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyServiceOnSeFe
    {
        IRunCommand RunCommandService { get; }

        ITakeFullScreenShot ScreenshotService { get; }

        ISave SaveFileService { get; }

        bool VerifyServiceOnSeFe();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface IVerifyActionMenu
    {
        ITakeFullScreenShot TakeFullScreenShotService { get; }
        ISave SaveFileService { get; }
        bool VerifyActionMenu();
    }
}

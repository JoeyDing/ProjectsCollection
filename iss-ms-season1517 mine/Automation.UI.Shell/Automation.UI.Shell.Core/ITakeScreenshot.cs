using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Core
{
    public interface ITakeScreenshot
    {
        void SaveScreenshot(string path, byte[] screenshot);

        ScreenShotResult TakeScreenShot();
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Core
{
    public interface ITakeScreenshotFull: ITakeScreenshot
    {
        void SetProcessName(string name);
    }
}

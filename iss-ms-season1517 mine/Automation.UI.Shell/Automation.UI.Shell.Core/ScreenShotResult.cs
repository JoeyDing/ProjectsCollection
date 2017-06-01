using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Core
{
    public class ScreenShotResult
    {
        public ScreenShotResult(byte[] data)
        {
            this.Screenshot = data;
        }
        public byte[] Screenshot { get; set; }
    }
}

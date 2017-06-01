using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSAutomation.Core.Services
{
    public interface ITakeFullScreenShot
    {
        byte[] TakeFullScreenShot();
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;
using Automation.UI.Shell.Core.Common;
using TestStack.White.UIItems;

namespace Automation.UI.Shell.TestStack
{
    public class TakeScreenshotServiceTestStack : ITakeScreenshotTestStack
    {
        private UIItem item;
        public void SetUIItem(UIItem item)
        {
            this.item = item;
        }
        public ScreenShotResult TakeScreenShot()
        {
            Bitmap screenShot = item.VisibleImage;
            return new ScreenShotResult(ConvertBitMapToByteArray.BitMapToByteArray(screenShot));
        }
        public void SaveScreenshot(string path, byte[] screenshot)
        {
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                filestream.Write(screenshot, 0, screenshot.Count());
            }
        }
    }
}

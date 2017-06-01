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
using System.Windows.Forms;
using Automation.UI.Shell.Core.Common;

namespace Automation.UI.Shell.Core
{
    public class TakeScreenshotFullService : ITakeScreenshotFull
    {
        private string processName;
        public void SetProcessName(string name)
        {
            this.processName = name;
        }
        public void SaveScreenshot(string path, byte[] screenshot)
        {
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                filestream.Write(screenshot, 0, screenshot.Count());
            }
        }
        public ScreenShotResult TakeScreenShot()
        {
            Rectangle bounds;
            if (processName.Equals(string.Empty))
            {
                bounds = Screen.GetBounds(Point.Empty);
            }
            else
            {
                Process[] processes = Process.GetProcessesByName(processName);
                IntPtr windowHandle = new IntPtr();
                foreach (Process p in processes)
                {
                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        // get App process nor background process
                        windowHandle = p.MainWindowHandle;
                    }
                }
                var appScreen = System.Windows.Forms.Screen.FromHandle(windowHandle);
                bounds = appScreen.Bounds;
            }
            //Create a new bitmap.
            Bitmap bmpScreenshot = new Bitmap(bounds.Width,
                                           bounds.Height,
                                           PixelFormat.Format32bppArgb);


            //Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);


            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(bounds.X,
                                         bounds.Y,
                                         0,
                                         0,
                                         bounds.Size,
                                         CopyPixelOperation.SourceCopy);

            return new ScreenShotResult(ConvertBitMapToByteArray.BitMapToByteArray(bmpScreenshot));
        }
    }
}

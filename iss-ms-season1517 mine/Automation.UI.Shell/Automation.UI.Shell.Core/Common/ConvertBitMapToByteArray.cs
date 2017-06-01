using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Core.Common
{
    public static class ConvertBitMapToByteArray
    {
        public static byte[] BitMapToByteArray(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = stream.ToArray();
            return bytes;
        }
    }
}

using ImageViewer_UITest.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEAF.Plugins.ImageViewer
{
    public class GifImage
    {
        private Image gifImage;
        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame = -1;
         private int step = 1;

        public GifImage(Bitmap bmp)
        {
            gifImage = bmp;
            //initialize
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            //gets the GUID
            //total frames in the animation
            frameCount = gifImage.GetFrameCount(dimension);
         }

       

        public Image GetNextFrame()
        {

            currentFrame += step;
            if (currentFrame >= frameCount)
                currentFrame = frameCount-1;
            else if (currentFrame < 0)
                currentFrame = 0;
            return GetFrame(currentFrame);
        }

        public Image GetFrame(int index)
        {

            gifImage.SelectActiveFrame(dimension, index);
            //find the frame
            return (Image)gifImage.Clone();
            //return a copy of it
        }
    }
}

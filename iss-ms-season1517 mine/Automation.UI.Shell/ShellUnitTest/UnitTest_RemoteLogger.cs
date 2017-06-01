using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestForAddNewProduct
{
    [TestClass]
    public class UnitTest_RemoteLogger
    {
        [TestMethod]
        public void UnitTest_RemoteLogger_PostImage()
        {
            //arrange
            string batchID = RemotLogger.Client.Lib.StateLoggerLib.ClientPostLogStart();
            string appName = "TestApp";
            string testName = "TestCase1";
            string cultureName = "zh-cn";
            List<string> itemPath = new List<string>
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Pictures\Signin_Skype_Chrome.png")
            };
            DateTime runDate = DateTime.Now;

            List<string> imageBinary = new List<string>();
            Image imageFormat = Image.FromFile(itemPath[0]);
            string imageBaseString = ImageToBase64(imageFormat, System.Drawing.Imaging.ImageFormat.Jpeg);
            imageBinary.Add(imageBaseString);
            //act
            RemotLogger.Client.Lib.StateLoggerLib.ClientPostState(batchID, appName, testName, cultureName, true, itemPath, imageBinary, runDate);

            //assert
        }
        private static string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}

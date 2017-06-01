using LEAF.Plugins.ImageViewer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ImageViewer_Unittest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string url = @"https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1";
            string fileName = "UrlImage.png";

            //var service = new DownloadImageService();
           // service.Download(fileName, url);
        }
    }
}
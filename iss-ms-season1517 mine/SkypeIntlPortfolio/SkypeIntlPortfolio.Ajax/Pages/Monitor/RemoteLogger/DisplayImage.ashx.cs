using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor.RemoteLogger
{
    /// <summary>
    /// Summary description for DisplayImage
    /// </summary>
    public class DisplayImage : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private static SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["pid"] != null) {
                context.Response.ContentType = "image/jpeg";

                int pid = int.Parse(context.Request.Params["pid"]);
                var item = dbContext.RemoteStateLoggerImages.Where(r => r.Id == pid).FirstOrDefault();
                if (item != null)
                {
                    string dumpPath = ConfigurationSettings.AppSettings["RemoteLoggerImagePath"].ToString();
                    int idx = item.ImagePath.IndexOf("WebUpload");
                    if (idx != -1) {
                        string imagePath = item.ImagePath.Substring(idx+ "WebUpload".Count());
                        string imageRealPath = dumpPath + imagePath;
                        Bitmap image = new Bitmap(imageRealPath);
                        image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    }
                   
                }
            }
        }

       
    }
}
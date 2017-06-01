using SkypeIntlMonitoring.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor
{
    /// <summary>
    /// Summary description for FileLoader
    /// </summary>
    public class Downloader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fullPath = (string)context.Request["path"];
            var dbContext = new SkypeIntlMonitoringContext();
            dbContext.UpdateDownloadCount(fullPath);
             try
            {
                context.Response.ContentType = "application/octet-stream";
                int i1 = fullPath.LastIndexOf("\\");
                if (i1 != -1)
                {
                    string filename = fullPath.Substring(i1 + 1);
                    String Header = "Attachment; Filename=" + filename;
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", Header);
                    HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache");
                    FileStream fstream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                    context.Response.Write(fstream);
                }
               
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/html";
                context.Response.Write("<h2>File Not Found</h2>");
            }

            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
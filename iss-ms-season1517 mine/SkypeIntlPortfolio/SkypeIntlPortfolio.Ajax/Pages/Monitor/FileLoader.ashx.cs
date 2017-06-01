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
    public class FileLoader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/html";
            //string fullPath = "\\\\SKYPEiNTL\\SkypeIntlTools\\Tools\\BuildAutoCopier\\logs\\trps.log";
            string fullPath = (string)context.Request["fullpath"];
            try
            {
                FileStream fstream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                string fileContents = "<h2>" + fullPath + "</h2><br/>";
                using (StreamReader reader = new StreamReader(fstream))
                {
                    fileContents += reader.ReadToEnd();
                    fileContents = fileContents.Replace("\r\n", "<br/>");
                    context.Response.Write(fileContents);
                }
            }
            catch (Exception ex)
            {
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
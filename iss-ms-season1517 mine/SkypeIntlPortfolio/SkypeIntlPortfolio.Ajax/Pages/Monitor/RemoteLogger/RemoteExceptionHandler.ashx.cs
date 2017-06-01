using RemoteLogger.Lib;
using RemoteLogger.Lib.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RemoteLogger
{
    /// <summary>
    /// Summary description for RemoteExceptionHandler
    /// </summary>
    public class RemoteExceptionHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            ExceptionLoggerLib.ServerProcessPostedExceptions(context);
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
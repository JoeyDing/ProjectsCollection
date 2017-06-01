using RemoteLogger.Lib;
using RemoteLogger.Lib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemoteLogger
{
    /// <summary>
    /// Summary description for StateLoggerHandler
    /// </summary>
    public class StateLoggerHandler : IHttpHandler
    {
        //StateLoggerHandler
        public void ProcessRequest(HttpContext context)
        {
            StateLoggerLib.ServerPocessPosted(context);
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
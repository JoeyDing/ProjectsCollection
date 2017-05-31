using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoeyMVCWebsite.Helpers
{
    public class MyErrorHandler : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            Log(filterContext.Exception);
            //base.OnException(filterContext);
        }

        private void Log(Exception exception)
        {
            //log exception here..
            Debug.WriteLine(exception.Message);
        }
    }
}
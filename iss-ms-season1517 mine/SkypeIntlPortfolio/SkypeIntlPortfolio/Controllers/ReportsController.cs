using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Telerik.Reporting.Cache.Interfaces;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace SkypeIntlPortfolio.Controllers
{
    public class ReportsController : ReportsControllerBase
    {
        protected override IReportResolver CreateReportResolver()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            var reportsPath = Path.Combine(appPath, @"~\Reports");
            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }

        protected override ICache CreateCache()
        {
            string appPath = HttpContext.Current.Server.MapPath("~");
            return Telerik.Reporting.Services.Engine.CacheFactory.CreateFileCache(appPath);
        }
    }
}
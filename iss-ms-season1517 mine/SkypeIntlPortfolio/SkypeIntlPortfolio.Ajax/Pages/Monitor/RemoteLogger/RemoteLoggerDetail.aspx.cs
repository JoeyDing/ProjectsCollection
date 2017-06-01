using RemoteLogger.Lib;
using SkypeIntlPortfolio.Ajax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RemoteLogger
{
    public partial class RemoteLoggerDetail : System.Web.UI.Page
    {
        private static SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            int Id = int.Parse(Request["Id"].ToString());
            SkypeIntlPortfolio.Ajax.RemoteLogger log = dbContext.RemoteLoggers.Where(r => r.Id == Id).FirstOrDefault();
            this.lbException.Text = log.Exception;
            this.lbStackTrace.Text = log.ExceptionStackTrace;
            this.lbUpdateDate.Text = "Generated:" + DateTime.Now.ToString("ddd, d MMM yyyy HH:mm:ss");
        }
    }
}
using SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor
{
    public partial class RemoteStateLogger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RemoteLoggerPresenter rlp = new RemoteLoggerPresenter(this.custom_RemoteLoggerControl);
        }
    }
}
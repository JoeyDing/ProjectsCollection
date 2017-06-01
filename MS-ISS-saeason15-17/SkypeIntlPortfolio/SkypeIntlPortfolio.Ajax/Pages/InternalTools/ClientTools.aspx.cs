using SkypeIntlMonitoring.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages.Monitor
{
    public partial class ClientTools : System.Web.UI.Page
    {
        public class ListGroup<T>
        {
            public string GroupName { get; set; }
            public int Count { get; set; }
            public List<T> Items { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                using (var context = new SkypeIntlMonitoringContext())
                {
                    Dictionary<string, List<ClientToolsDownload>> tools = context.ClientToolsDownloads.GroupBy(t => t.name).ToDictionary(t => t.Key, t => t.ToList());
                    List<ListGroup<ClientToolsDownload>> ds = new List<ListGroup<ClientToolsDownload>>();
                    foreach (KeyValuePair<string, List<ClientToolsDownload>> pair in tools)
                    {
                        ListGroup<ClientToolsDownload> grp = new ListGroup<ClientToolsDownload>();
                        grp.GroupName = pair.Key;
                        grp.Count = pair.Value.Count;
                        grp.Items = pair.Value;
                        ds.Add(grp);
                    }
                    this.listview_tools.DataSource = ds;
                    this.listview_tools.DataBind();

                }

            }
        }
    }
}
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
    public partial class JobLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
             
                using (var context = new SkypeIntlMonitoringContext())
                {
                    List<Tool> tools= context.Tools.OrderBy(j => j.Name).ToList();
                    
                    this.radListBox_Tools.DataSource = context.Tools.OrderBy(j => j.Name).ToList();
                    this.radListBox_Tools.DataBind();

                    Tool tool1 = tools.FirstOrDefault();
                    this.gridview_JobsLog.DataSource = context.LogFiles.Where(j=>j.ToolID==tool1.ToolID).OrderBy(j => j.FullPath).ToList();
                    this.gridview_JobsLog.DataBind();
                }

            }
        }

     
        

        protected void radCombo_Tools_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            using (var context = new SkypeIntlMonitoringContext())
            {
                var toolID = int.Parse(e.Value);
                this.gridview_JobsLog.DataSource = context.LogFiles.Where(j => (j.ToolID ==toolID)).OrderBy(j => j.FullPath).ToList();
                this.gridview_JobsLog.DataBind();
            }

        
           
        }
      
        
        protected void gridview_JobsLog_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                HyperLink hLink = (HyperLink)item.FindControl("HyperLink1");
                if(hLink!=null)
                hLink.Attributes.Add("onclick", "RowClick('" + item.ItemIndex + "');");
            }
        }

        protected void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            List<int> tools = new List<int>();
            foreach (RadListBoxItem item in this.radListBox_Tools.Items)
            {
                CheckBox chk= (CheckBox)item.FindControl("chkSel");
                if (chk != null&&chk.Checked)
                {
                   Label toolId = (Label)item.FindControl("lbToolID");
                    tools.Add(int.Parse(toolId.Text));
                }

            }
            using (var context = new SkypeIntlMonitoringContext())
            {
                this.gridview_JobsLog.DataSource = context.LogFiles.Where(j => tools.Contains(j.ToolID)).OrderBy(j => j.FullPath).ToList();
                this.gridview_JobsLog.DataBind();
            }
        }
    }
}
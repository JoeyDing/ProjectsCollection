using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class ScheduleWindows : System.Web.UI.UserControl
    {
        public Dictionary<int, string> MilestoneCategories
        {
            get
            {
                Dictionary<int, string> result = null;
                if (ViewState["MilestoneCategories"] == null)
                {
                    using (var context = new SkypeIntlPlanningPortfolioEntities())
                    {
                        ViewState["MilestoneCategories"] = result = context.MilestoneCategories.ToDictionary(m => m.MilestoneCategoryKey, m => m.Milestone_Category_Name);
                    }
                }
                else
                    result = ViewState["MilestoneCategories"] as Dictionary<int, string>;
                return result;
            }
        }

        private const string C_Product_Key = "ProductKey";
        private const string C_Product_Name = "ProductName";

        private const string C_Release_Key = "ReleaseKey";
        private const string C_Release_Name = "ReleaseName";

        private const string C_Milestone_Key = "MilestoneKey";
        private const string C_Milestone_Name = "MilestoneName";
        private const string C_Milestone_Start_Date = "Milestone_Start_Date";
        private const string C_Milestone_End_Date = "Milestone_End_Date";

        private const string C_UiAction = "UiAction";
        private const string C_Trigger_Control_UniqueID = "TriggerControl";

        protected void Page_Load(object sender, EventArgs e)
        {
            RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
            manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate((o, ee) =>
            {
                manager_AjaxRequest(o, ee);
            });

            if (!this.radcombobox_milestone_categories.Items.Any())
            {
                foreach (var category in this.MilestoneCategories)
                {
                    var item = new RadComboBoxItem();
                    item.Value = category.Key.ToString();
                    item.Text = category.Value;
                    this.radcombobox_milestone_categories.Items.Add(item);
                    item.DataBind();
                };
            }
        }

        protected void manager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            //    var argSplit = e.Argument.Split(',');
            //    if (argSplit.Count() == 3 && argSplit[0] == "ajaxupdate")
            //    {
            //        string updateType = argSplit[1];
            //        string sourceCtrlUniqueID = argSplit[2];
            //        if (sourceCtrlUniqueID.StartsWith(this.UniqueID))
            //        {
            //            switch (updateType)
            //            {
            //                case "project":
            //                    this.ShowWindow(
            //                                     action: UiAction.Update
            //                                   , triggerCtrlUniqueID: sourceCtrlUniqueID
            //                                   , targetWindow: this.radwindow_addProject
            //                                   , dataAttributes: new Dictionary<WebControl, string[]>()
            //                               {
            //                                    { this.radpanelbar_projects_root, new string[]
            //                                    { C_Product_Key, C_Product_Name } },
            //                                    { this.GetControlFromUniqueID(sourceCtrlUniqueID,this.radpanelbar_projects_root.ID)
            //                                        , new string[]
            //                                    {C_Project_Key, C_Project_Name } },
            //                               }
            //                                   , updateWindowFunction: (window) =>
            //                                   {
            //                                       this.label_window_addProject.Text = string.Format(@"{0}\{1}: ", window.Attributes[C_Product_Name], window.Attributes[C_Project_Name]);
            //                                       this.radtextbox_projectName.Text = window.Attributes[C_Project_Name];
            //                                       this.radbutton_window_addProject.Text = "Update";
            //                                   });

            //                    break;

            //                case "release":
            //                    this.ShowWindow(
            //                                     action: UiAction.Update
            //                                   , triggerCtrlUniqueID: sourceCtrlUniqueID
            //                                   , targetWindow: this.radwindow_addRelease
            //                                   , dataAttributes: new Dictionary<WebControl, string[]>()
            //                               {
            //                                    { this.radpanelbar_projects_root, new string[]
            //                                    { C_Product_Key, C_Product_Name } },
            //                                    { this.GetControlFromUniqueID(sourceCtrlUniqueID,this.radpanelbar_projects_root.ID)
            //                                        , new string[]
            //                                    {C_Project_Key, C_Project_Name } },
            //                                     { this.GetControlFromUniqueID(sourceCtrlUniqueID,"radpanelbar_releases_root")
            //                                        , new string[]
            //                                    {C_Release_Key, C_Release_Name } },
            //                               }
            //                                   , updateWindowFunction: (window) =>
            //                                   {
            //                                       this.label_window_addRelease.Text = string.Format(@"{0}\{1}\{2}: ", window.Attributes[C_Product_Name], window.Attributes[C_Project_Name], window.Attributes[C_Release_Name]);
            //                                       this.radtextbox_ReleaseName.Text = window.Attributes[C_Release_Name];
            //                                       this.radbutton_window_addRelease.Text = "Update";
            //                                   });

            //                    break;

            //                default:
            //                    break;
            //            }
            //        }
            //    }
        }

        public void ShowProjectModal()
        {
        }
    }
}
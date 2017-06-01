using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class ScheduleControlLight : System.Web.UI.UserControl, ICustomProjectControl
    {
        private ProductInfo productInfo;

        public ProductInfo ProductInfo
        {
            get { return productInfo; }
            set
            {
                productInfo = value;
                //canRefresh = true;
            }
        }

        public ScheduleWindows ScheduleWindows
        {
            get
            {
                if (this.ViewState["ScheduleWindowsID"] is string)
                {
                    return this.Page.FindControl(this.ViewState["ScheduleWindowsID"].ToString()) as ScheduleWindows;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["ScheduleWindowsID"] = value.UniqueID;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var calendar = GetCalendar();
            foreach (var datePicker in this.GetChildrenOfType<RadDatePicker>(this.radtreeview_root))
            {
                datePicker.SharedCalendar = calendar;
            }
        }

        private RadCalendar GetCalendar()
        {
            var placeholder = this.GetChildrenOfType<PlaceHolder>(this.Page).FirstOrDefault(c => c.ID == "sharedCalendarPlaceHolder");
            RadCalendar calendar = null;
            if ((calendar = this.GetChildrenOfType<RadCalendar>(placeholder).FirstOrDefault()) == null)
            {
                calendar = new RadCalendar();
                calendar.RangeMinDate = new DateTime(2006, 1, 1);
                calendar.ID = "sharedDynamicCalendar";
                calendar.EnableMultiSelect = false;
                placeholder.Controls.Add(calendar);
            }
            return calendar;
        }

        public void Refresh()
        {
            this.radtreeview_root.Nodes.Clear();

            if (this.ProductInfo != null)
            {
                this.radtreeview_root.Attributes["ProductKey"] = this.ProductInfo.ProductKey.ToString();

                //set releases info
                var releases = this.ProductInfo.Releases;
                var radtreeview_release_root = radtreeview_root.Nodes[0].FindControl("radtreeview_release_root") as RadTreeView;
                foreach (var release in releases)
                {
                    var releaseItem = new RadTreeNode();
                    releaseItem.Attributes["ReleaseKey"] = release.ReleaseKey.ToString();
                    radtreeview_release_root.Nodes.Add(releaseItem);
                    var radtreeview_release_child = releaseItem.FindControl("radtreeview_release_child") as RadTreeView;
                    var radtreeviewItem_release = radtreeview_release_child.Nodes[0];
                    radtreeviewItem_release.Text = release.Release_Name;

                    //set milestones info
                    var milestones = release.Milestones;
                    var radtreeview_milestone_root = radtreeviewItem_release.Nodes[0].FindControl("radtreeview_milestone_root") as RadTreeView;

                    foreach (var milestone in milestones)
                    {
                        var milestoneItem = new RadTreeNode();
                        milestoneItem.Attributes["MilestoneKey"] = milestone.MilestoneKey.ToString();
                        radtreeview_milestone_root.Nodes.Add(milestoneItem);

                        var label_milestoneName = milestoneItem.FindControl("label_milestoneName") as Label;
                        label_milestoneName.Text = milestone.Milestone_Name;

                        var text_milestoneAssignTo = milestoneItem.FindControl("MileStoneAssignedTo") as RadTextBox;
                        text_milestoneAssignTo.Text = milestone.Milestone_Assigned_To;

                        var raddatepicker_milestone_from = milestoneItem.FindControl("raddatepicker_milestone_from") as RadDatePicker;
                        raddatepicker_milestone_from.SharedCalendar = this.GetCalendar();
                        //raddatepicker_milestone_from.SharedCalendarID = this.GetCalendar().UniqueID;

                        var raddatepicker_milestone_to = milestoneItem.FindControl("raddatepicker_milestone_to") as RadDatePicker;
                        raddatepicker_milestone_to.SharedCalendar = this.GetCalendar();

                        raddatepicker_milestone_from.SelectedDate = milestone.Milestone_Start_Date.Value;
                        raddatepicker_milestone_to.SelectedDate = milestone.Milestone_End_Date.Value;
                    }
                }

                this.radtreeview_root.DataBind();
            }
        }

        protected void raddatepicker_milestone_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
        }

        private IEnumerable<T> GetChildrenOfType<T>(Control parent) where T : class
        {
            var childQueue = new Queue<Control>();
            foreach (Control child in parent.Controls.Cast<object>().Where(c => c is Control))
            {
                childQueue.Enqueue(child);
            }
            while (childQueue.Any())
            {
                Control currentItem = childQueue.Dequeue();
                if (currentItem is T)
                {
                    yield return currentItem as T;
                }

                foreach (Control child in currentItem.Controls.Cast<object>().Where(c => c is Control))
                {
                    childQueue.Enqueue(child);
                }
            }
        }

        protected void linkButton_addNewProject_Click(object sender, EventArgs e)
        {
            if (this.ScheduleWindows != null)
            {
                this.ScheduleWindows.ShowProjectModal();
            }
        }
    }
}
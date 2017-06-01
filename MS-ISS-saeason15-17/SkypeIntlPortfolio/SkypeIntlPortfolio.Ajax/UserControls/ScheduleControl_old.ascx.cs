using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class ScheduleControl_old : System.Web.UI.UserControl, ICustomProjectControl
    {
        private enum UiAction
        {
            Add,
            Update,
            Delete
        }

        public Dictionary<int, string> MilestoneCategories
        {
            get
            {
                Dictionary<int, string> result = null;
                if (this.Session["MilestoneCategories"] == null)
                {
                    using (var context = new SkypeIntlPlanningPortfolioEntities())
                    {
                        this.Session["MilestoneCategories"] = result = context.MilestoneCategories.ToDictionary(m => m.MilestoneCategoryKey, m => m.Milestone_Category_Name);
                    }
                }
                else
                    result = this.Session["MilestoneCategories"] as Dictionary<int, string>;
                return result;
            }
        }

        private const string C_Product_Key = "ProductKey";
        private const string C_Product_Name = "ProductName";
        private const string C_Product_Family = "ProductFamily";

        private const string C_Release_Key = "ReleaseKey";
        private const string C_Release_Name = "ReleaseName";
        private const string C_Release_Start_Date = "Release_Start_Date";
        private const string C_Release_End_Date = "Release_End_Date";
        private const string C_Release_Assigned_To = "Release_Assigned_To";
        private const string C_VSO_Tag = "VSO_Tag";

        private const string C_Milestone_Key = "MilestoneKey";
        private const string C_Milestone_Name = "MilestoneName";
        private const string C_Milestone_Start_Date = "Milestone_Start_Date";
        private const string C_Milestone_End_Date = "Milestone_End_Date";
        private const string C_Milestone_Assigned_To = "Milestone_Assigned_To";

        private const string C_TestSchedule_Key = "TestScheduleKey";
        private const string C_TestSchedule_Name = "TestScheduleName";
        private const string C_TestSchedule_Start_Date = "TestSchedule_Start_Date";
        private const string C_TestSchedule_End_Date = "TestSchedule_End_Date";
        private const string C_TestSchedule_Assigned_Reources = "C_TestSchedule_Assigned_Reources";

        private const string C_UiAction = "UiAction";
        private const string C_Trigger_Control_UniqueID = "TriggerControl";

        private const string VSO_Project = "LOCALIZATION";

        private ProductInfo productInfo;

        public ProductInfo ProductInfo
        {
            get { return productInfo; }
            set
            {
                productInfo = value;
                canRefresh = true;
            }
        }

        private bool canRefresh;

        public bool IsRadScheduleHidden
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsRadScheduleHidden)
            {
                RegisterJavaScript();
                RadAjaxManager manager = RadAjaxManager.GetCurrent(Page);
                manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate((o, ee) =>
                {
                    manager_AjaxRequest(o, ee);
                });

                //cannot use radajax manager proxy to ajaxify control that get updated dynamically
                //must be put in the pageload

                if (this.GetChildrenOfType<RadDatePicker>(this.scheduleControl_panel_mainContentPanel).Any())
                {
                    foreach (var datePicker in this.GetChildrenOfType<RadDatePicker>(this.scheduleControl_panel_mainContentPanel))
                    {
                        datePicker.SharedCalendar = GetCalendar();
                    }
                }
            }
        }

        public void Refresh()
        {
            if (this.canRefresh)
            {
                this.canRefresh = false;

                if (this.ProductInfo != null)
                {
                    scheduleControl_panel_mainContentPanel.Attributes[C_Product_Key] = this.ProductInfo.ProductKey.ToString();
                    scheduleControl_panel_mainContentPanel.Attributes[C_Product_Name] = this.ProductInfo.Product_Name.ToString();

                    //set releases info
                    var releases = ProductInfo.Releases;
                    var radpanelbar_release_root = this.scheduleControl_panel_mainContentPanel.FindControl("radpanelbar_releases_root") as RadPanelBar;
                    foreach (var release in releases)
                    {
                        var releaseItem = new RadPanelItem();
                        releaseItem.Attributes[C_Product_Key] = this.ProductInfo.ProductKey.ToString();
                        releaseItem.Attributes[C_Release_Key] = release.ReleaseKey.ToString();
                        releaseItem.Attributes[C_Release_Name] = release.Release_Name;
                        releaseItem.Attributes[C_VSO_Tag] = release.Release_Tags;
                        releaseItem.Attributes[C_Release_Start_Date] = release.Release_Start_Date.ToString();
                        releaseItem.Attributes[C_Release_End_Date] = release.Release_End_Date.ToString();
                        releaseItem.Attributes[C_Release_Assigned_To] = release.Release_Assigned_To == null ? "" : release.Release_Assigned_To.Replace("<", "(").Replace(">", ")");

                        radpanelbar_release_root.Items.Add(releaseItem);

                        var radpanelbar_release_child = releaseItem.FindControl("radpanelbar_releases_child") as RadPanelBar;

                        //add link
                        var link_VsoItem_release = radpanelbar_release_child.Items[0].Header.FindControl("link_VsoItem_release") as HyperLink;
                        link_VsoItem_release.Text = release.ReleaseKey.ToString();
                        link_VsoItem_release.NavigateUrl = release.Release_Url;

                        var raddatepicker_Release_from = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_Release_from") as RadDatePicker;
                        raddatepicker_Release_from.SharedCalendar = this.GetCalendar();
                        var raddatepicker_Release_to = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_Release_to") as RadDatePicker;
                        raddatepicker_Release_to.SharedCalendar = this.GetCalendar();

                        var labelCustomTag = radpanelbar_release_child.Items[0].Header.FindControl("Label_CustomTagOnUI") as Label;

                        raddatepicker_Release_from.SelectedDate = release.Release_Start_Date.Value;
                        raddatepicker_Release_to.SelectedDate = release.Release_End_Date.Value;

                        var releaseTypeStoredInDB = release.Release_Tags;
                        if (releaseTypeStoredInDB != null)
                        {
                            var customTags = getTheCustomVSOTags(releaseTypeStoredInDB);
                            if (customTags.Count() != 0)
                            {
                                labelCustomTag.Text = customTags.Aggregate((a, b) => a + ";" + b);
                            }
                        }
                        //set milestones info
                        var milestones = release.Milestones;
                        var radpanelbar_milestones_root = radpanelbar_release_child.Items[0].FindControl("radpanelbar_milestones_root") as RadPanelBar;

                        foreach (var milestone in milestones)
                        {
                            var milestoneItem = new RadPanelItem();
                            milestoneItem.Attributes[C_Product_Key] = this.ProductInfo.ProductKey.ToString();
                            milestoneItem.Attributes[C_Product_Name] = this.ProductInfo.Product_Name.ToString();
                            milestoneItem.Attributes[C_Release_Key] = release.ReleaseKey.ToString();
                            milestoneItem.Attributes[C_Release_Name] = release.Release_Name.ToString();
                            milestoneItem.Attributes[C_Milestone_Key] = milestone.MilestoneKey.ToString();
                            milestoneItem.Attributes[C_Milestone_Name] = milestone.Milestone_Name.ToString();

                            radpanelbar_milestones_root.Items.Add(milestoneItem);

                            var label_milestoneName = milestoneItem.FindControl("label_milestoneName") as Label;
                            label_milestoneName.Text = milestone.Milestone_Name;

                            var link_VsoItem_Milestone = milestoneItem.FindControl("link_VsoItem_Milestone") as HyperLink;
                            string vsoTag;
                            if (Utils.TryParseCategoryNameToVsoTag(milestone.MilestoneCategoryName, out vsoTag))
                            {
                                link_VsoItem_Milestone.Text = string.Format("{0}", vsoTag);
                                link_VsoItem_Milestone.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(release.ReleaseKey, vsoTag, VSO_Project, productInfo.Family);
                            }
                            else
                            {
                                var label_VsoQuery = milestoneItem.FindControl("label_VsoQuery") as Label;
                                label_VsoQuery.Visible = false;
                                link_VsoItem_Milestone.Visible = false;
                            }

                            //fill the combobox on control with items
                            RadComboBox milestone_categoriesOnControl = milestoneItem.FindControl("radcombobox_milestone_categoriesOnControl") as RadComboBox;
                            foreach (var category in this.MilestoneCategories)
                            {
                                RadComboBoxItem radItem = new RadComboBoxItem();
                                radItem.Text = category.Value;
                                radItem.Value = category.Key.ToString();
                                ShowToopTipsinComboBox(radItem);
                                if (radItem.Text == milestone.MilestoneCategoryName)
                                {
                                    radItem.Selected = true;
                                }

                                milestone_categoriesOnControl.Items.Add(radItem);
                                radItem.DataBind();
                            }

                            var raddatepicker_milestone_from = milestoneItem.FindControl("raddatepicker_milestone_from") as RadDatePicker;
                            raddatepicker_milestone_from.SharedCalendar = this.GetCalendar();
                            var raddatepicker_milestone_to = milestoneItem.FindControl("raddatepicker_milestone_to") as RadDatePicker;
                            raddatepicker_milestone_to.SharedCalendar = this.GetCalendar();

                            raddatepicker_milestone_from.SelectedDate = milestone.Milestone_Start_Date.Value;
                            raddatepicker_milestone_to.SelectedDate = milestone.Milestone_End_Date.Value;
                        }
                        //set testShceules info
                        var testSchedules = release.TestSchedules;

                        var radpanelbar_testSchedules_root = radpanelbar_release_child.Items[0].FindControl("radpanelbar_testSchedules_root") as RadPanelBar;

                        foreach (var testSchedule in testSchedules)
                        {
                            var testScheduleItem = new RadPanelItem();
                            testScheduleItem.Attributes[C_Product_Key] = this.ProductInfo.ProductKey.ToString();
                            testScheduleItem.Attributes[C_Product_Name] = this.ProductInfo.Product_Name.ToString();
                            testScheduleItem.Attributes[C_Release_Key] = release.ReleaseKey.ToString();
                            testScheduleItem.Attributes[C_Release_Name] = release.Release_Name.ToString();
                            testScheduleItem.Attributes[C_TestSchedule_Key] = testSchedule.TestScheduleKey.ToString();
                            testScheduleItem.Attributes[C_TestSchedule_Name] = testSchedule.TestScheduleName.ToString();
                            testScheduleItem.Attributes[C_TestSchedule_Start_Date] = testSchedule.TestScheduleStartDate.ToString();
                            testScheduleItem.Attributes[C_TestSchedule_End_Date] = testSchedule.TestScheduleEndDate.ToString();
                            testScheduleItem.Attributes[C_TestSchedule_Assigned_Reources] = testSchedule.AssignedResources.ToString();

                            radpanelbar_testSchedules_root.Items.Add(testScheduleItem);

                            var label_testScheduleName = testScheduleItem.FindControl("label_testScheduleaName") as Label;
                            label_testScheduleName.Text = testSchedule.TestScheduleName;
                            var VSOTag_testPlan = testScheduleItem.FindControl("VSOTag_testPlan") as Label;
                            var link_VsoItem_testSchedule = testScheduleItem.FindControl("link_VsoItem_TestSchedule") as HyperLink;
                            var link_VsoItem_TestMilestoneCategory = testScheduleItem.FindControl("link_VsoItem_TestMilestoneCategory") as HyperLink;

                            if (string.IsNullOrEmpty(testSchedule.TestScheduleUrl))
                            {
                                link_VsoItem_testSchedule.Visible = false;
                            }
                            else
                            {
                                link_VsoItem_testSchedule.Text = testSchedule.TestScheduleKey.ToString();
                                link_VsoItem_testSchedule.NavigateUrl = testSchedule.TestScheduleUrl;
                            }

                            var curretnMilestoneCategoryKey = testSchedule.MilestoneCategoryKey;

                            //fill the combobox on control with items
                            RadComboBox test_categoriesOnControl = testScheduleItem.FindControl("radcombobox_testPlan_categoriesOnControl") as RadComboBox;
                            string selectedMilestoenCategory = "";
                            foreach (var category in this.MilestoneCategories)
                            {
                                RadComboBoxItem radItem = new RadComboBoxItem();
                                radItem.Text = category.Value;
                                radItem.Value = category.Key.ToString();
                                ShowToopTipsinComboBox(radItem);
                                if (radItem.Value == curretnMilestoneCategoryKey.ToString())
                                {
                                    radItem.Selected = true;
                                    selectedMilestoenCategory = radItem.Text;
                                }

                                test_categoriesOnControl.Items.Add(radItem);
                                radItem.DataBind();
                            }

                            string vsoTag;

                            if (!string.IsNullOrEmpty(testSchedule.TestScheduleUrl) && Utils.TryParseCategoryNameToVsoTag(selectedMilestoenCategory, out vsoTag))
                            {
                                link_VsoItem_TestMilestoneCategory.Text = string.Format("{0}", vsoTag);
                                link_VsoItem_TestMilestoneCategory.NavigateUrl = testSchedule.TestScheduleUrl;
                            }
                            else
                            {
                                VSOTag_testPlan.Visible = false;
                                link_VsoItem_TestMilestoneCategory.Visible = false;
                            }

                            var raddatepicker_testSchedule_from = testScheduleItem.FindControl("raddatepicker_testSchedule_from") as RadDatePicker;
                            raddatepicker_testSchedule_from.SharedCalendar = this.GetCalendar();
                            var raddatepicker_testSchedule_to = testScheduleItem.FindControl("raddatepicker_testSchedule_to") as RadDatePicker;
                            raddatepicker_testSchedule_to.SharedCalendar = this.GetCalendar();

                            var radTextBox_MainUi_AssignedResources = testScheduleItem.FindControl("radTextBox_MainUi_AssignedResources") as Label;
                            radTextBox_MainUi_AssignedResources.Text = testSchedule.AssignedResources.ToString();

                            raddatepicker_testSchedule_from.SelectedDate = testSchedule.TestScheduleStartDate.Value;
                            raddatepicker_testSchedule_to.SelectedDate = testSchedule.TestScheduleEndDate.Value;
                        }
                    }
                }
            }
        }

        private List<string> getTheCustomVSOTags(string releaseStr)
        {
            List<string> customTags = releaseStr.Split(';').Distinct().ToList();
            List<string> filteredTags = new List<string> { };

            foreach (string item in customTags)
            {
                string trimmedItem = item.Trim();
                if (trimmedItem != "Loc_Release" && !trimmedItem.Contains("Loc_ReleaseStartDate"))
                {
                    filteredTags.Add(trimmedItem);
                }
            }

            return filteredTags;
        }

        private void RegisterJavaScript()
        {
            string jsProjectKey = this.Page.ClientID + "_editProject";
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(this.Page.GetType(), jsProjectKey))
            {
                var test = string.Format(
                @"function editProject(manager, senderUniqueID){{
                  var ajaxManager = $find(manager);
                  var args = ""ajaxupdate,project,"" + senderUniqueID;
                  manager.ajaxRequest(args);
                  }}"
                );
                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(),
                 jsProjectKey, @"..\UserControls\ScheduleControl.js");
            }
        }

        #region Common

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

        private WebControl GetControlFromUniqueID(string uniqueID, string parentID = null)
        {
            if (parentID == null)
                return this.Page.FindControl(uniqueID) as WebControl;
            else
            {
                Regex rx = new Regex(string.Format(@"(.*?)\${0}\$[a-z][0-9]+", parentID), RegexOptions.IgnoreCase);

                // Find matches.
                string parentUniqueID = rx.Matches(uniqueID)[0].Value;
                return this.Page.FindControl(parentUniqueID) as WebControl;
            }
        }

        private void ShowWindow(UiAction action, string triggerCtrlUniqueID, WebControl targetWindow, Dictionary<WebControl, string[]> dataAttributes, Action<WebControl> updateWindowFunction)
        {
            //set window Action mode
            targetWindow.Attributes[C_UiAction] = action.ToString();
            targetWindow.Attributes[C_Trigger_Control_UniqueID] = triggerCtrlUniqueID;

            //transfer specified attribute key/values to targetWindow
            foreach (var attr in dataAttributes)
            {
                foreach (string attrValue in attr.Value)
                {
                    targetWindow.Attributes[attrValue] = attr.Key.Attributes[attrValue];
                }
            }

            //run custom updates to window
            if (updateWindowFunction != null)
            {
                updateWindowFunction(targetWindow);
            }

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowWindow", string.Format("ShowExistingWindow('{0}');", targetWindow.ClientID), true);
        }

        private bool AddUpdateData(WebControl targetWindow, Action<SkypeIntlPlanningPortfolioEntities> updateDbFunction, BaseValidator[] validatorList = null)
        {
            //window validation
            bool isValid = true;
            IEnumerable<BaseValidator> controlsToValidate = null;
            controlsToValidate = validatorList != null ? validatorList : this.GetChildrenOfType<BaseValidator>(targetWindow);
            foreach (BaseValidator validationControl in controlsToValidate)
            {
                validationControl.Validate();
                if (isValid && !validationControl.IsValid)
                    isValid = false;
            }

            //update database if window is valid
            if (isValid)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    updateDbFunction(context);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", targetWindow.ClientID), true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", targetWindow.ClientID), true);
            }

            return isValid;
        }

        private T GetFirstParentOfType<T>(WebControl child) where T : class
        {
            Control currentParent = child.Parent;
            while (currentParent != null)
            {
                if (currentParent is T)
                    return currentParent as T;
                currentParent = currentParent.Parent;
            }
            return null;
        }

        private void ClearWindows()
        {
            foreach (var treeview in this.GetChildrenOfType<RadTreeView>(this.panel_schedule_window))
            {
                treeview.Nodes.Clear();
            }
        }

        private Control GetWindowItem(RadTreeView parent, string parentName)
        {
            //cleanup other nodes
            foreach (var treeview in this.GetChildrenOfType<RadTreeView>(this.panel_schedule_window))
            {
                if (treeview.UniqueID != parent.UniqueID)
                {
                    treeview.Nodes.Clear();
                }
            }

            RadTreeNode node;
            if (parent.Nodes.Count > 0)
            {
                node = parent.Nodes[0];
            }
            else
            {
                node = new RadTreeNode();
                parent.Nodes.Add(node);
                node.DataBind();
            }

            var result = node.FindControl(parentName);
            return result;
        }

        protected void manager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            var argSplit = e.Argument.Split(',');
            if (argSplit.Count() == 3 && argSplit[0] == "ajaxupdate")
            {
                string updateType = argSplit[1];
                string sourceCtrlUniqueID = argSplit[2];
                if (sourceCtrlUniqueID.StartsWith(this.UniqueID))
                {
                    if (updateType == "release")
                    {
                        var releaseWindow = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease") as WebControl;
                        // pass the arguments to Release Update window
                        this.ShowWindow(
                                         action: UiAction.Update
                                       , triggerCtrlUniqueID: sourceCtrlUniqueID
                                       , targetWindow: releaseWindow
                                       , dataAttributes: new Dictionary<WebControl, string[]>()
                                       {
                                            { this.scheduleControl_panel_mainContentPanel, new string[]{ C_Product_Key, C_Product_Name } },
                                             { this.GetControlFromUniqueID(sourceCtrlUniqueID,"radpanelbar_releases_root")
                                                , new string[]
                                            {C_Release_Key, C_Release_Name, C_Release_Start_Date, C_Release_End_Date, C_Release_Assigned_To,C_VSO_Tag} },
                                       }
                                       , updateWindowFunction: (window) =>
                                       {
                                           var label_window_addRelease = (Label)releaseWindow.FindControl("label_window_addRelease");
                                           var radtextbox_ReleaseName = (RadTextBox)releaseWindow.FindControl("radtextbox_ReleaseName");
                                           var raddatepicker_ReleaseStartDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseStartDate");
                                           var raddatepicker_ReleaseEndDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseEndDate");
                                           var radiobuttonList_CustomTag = (RadioButtonList)releaseWindow.FindControl("radioButtonList_ReleaseType");
                                           var radbutton_window_addRelease = (RadButton)releaseWindow.FindControl("radbutton_window_addRelease");
                                           var radPanelBar_miletoneInReleaseWindow = (RadPanelBar)releaseWindow.FindControl("radPanelBar_miletoneInReleaseWindow");
                                           var radButton_UI_addMilestone = (RadButton)releaseWindow.FindControl("radButton_UI_addMilestone");

                                           label_window_addRelease.Text = string.Format(@"{0} \ {1}: ", window.Attributes[C_Product_Name], window.Attributes[C_Release_Name]);
                                           radtextbox_ReleaseName.Text = window.Attributes[C_Release_Name];
                                           if (releaseWindow.Attributes[C_Release_Start_Date] != "" || releaseWindow.Attributes[C_Release_End_Date] != "")
                                           {
                                               raddatepicker_ReleaseStartDate.SelectedDate = Convert.ToDateTime(window.Attributes[C_Release_Start_Date]);
                                               raddatepicker_ReleaseEndDate.SelectedDate = Convert.ToDateTime(window.Attributes[C_Release_End_Date]);
                                           }

                                           foreach (ListItem tag in radiobuttonList_CustomTag.Items)
                                           {
                                               if (window.Attributes[C_VSO_Tag] != null)
                                               {
                                                   if (window.Attributes[C_VSO_Tag].Contains(tag.Value))
                                                   {
                                                       tag.Selected = true;
                                                   }
                                               }
                                           }

                                           radbutton_window_addRelease.Text = "Update";
                                           radPanelBar_miletoneInReleaseWindow.Visible = false;
                                           radButton_UI_addMilestone.Visible = false;
                                       });

                        var updatepanel = this.GetFirstParentOfType<UpdatePanel>(releaseWindow);
                        updatepanel.Update();
                    }
                    else if (updateType == "milestone")
                    {
                        var milestoneWindow = this.GetWindowItem(this.treeview_updateMilestone, "radwindow_MilestoneNameUpdate") as WebControl;
                        // pass the arguments to milestone Update window
                        this.ShowWindow(
                                         action: UiAction.Update
                                       , triggerCtrlUniqueID: sourceCtrlUniqueID
                                       , targetWindow: milestoneWindow
                                       , dataAttributes: new Dictionary<WebControl, string[]>()
                                       {
                                            { this.scheduleControl_panel_mainContentPanel, new string[]{ C_Product_Key, C_Product_Name, C_Release_Key, C_Release_Name, C_Milestone_Key, C_Milestone_Name } },
                                             { this.GetControlFromUniqueID(sourceCtrlUniqueID,"radpanelbar_milestones_root")
                                                , new string[]
                                            {C_Product_Key, C_Product_Name, C_Release_Key, C_Release_Name, C_Milestone_Key, C_Milestone_Name} },
                                       }
                                       , updateWindowFunction: (window) =>
                                       {
                                           var label_WindowMilestoneNameUpdate = (Label)milestoneWindow.FindControl("label_WindowMilestoneNameUpdate");
                                           var radTextBox_MilestoneNameUpdate = (RadTextBox)milestoneWindow.FindControl("radTextBox_MilestoneNameUpdate");

                                           label_WindowMilestoneNameUpdate.Text = string.Format(@"{0} \ {1} \ {2}: ", window.Attributes[C_Product_Name], window.Attributes[C_Release_Name], window.Attributes[C_Milestone_Name]);
                                           radTextBox_MilestoneNameUpdate.Text = window.Attributes[C_Milestone_Name];
                                       });

                        var updatepanel = this.GetFirstParentOfType<UpdatePanel>(milestoneWindow);
                        updatepanel.Update();
                    }
                    else if (updateType == "testSchedule")
                    {
                        var testscheduleWindow = this.GetWindowItem(this.treeview_updateTestSchedule, "updatePanel_updateTestSchedule_panel") as WebControl;
                        this.ShowWindow(
                                         action: UiAction.Update
                                       , triggerCtrlUniqueID: sourceCtrlUniqueID
                                       , targetWindow: testscheduleWindow
                                       , dataAttributes: new Dictionary<WebControl, string[]>()
                                       {
                                           { this.scheduleControl_panel_mainContentPanel, new string[]{ C_Product_Key, C_Product_Name, C_Release_Key, C_Release_Name} },
                                            {this.GetControlFromUniqueID(sourceCtrlUniqueID,"radpanelbar_testSchedules_root"),
                                               new string[]{ C_TestSchedule_Key, C_TestSchedule_Name, C_TestSchedule_Start_Date, C_TestSchedule_End_Date, C_Release_Name,C_TestSchedule_Assigned_Reources} }
                                       }
                                       , updateWindowFunction: (window) =>
                                       {
                                           var labelTestname = (Label)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_title");
                                           // var labelStartData = (RadDatePicker)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_startDatePicker");
                                           // var labelEndData = (RadDatePicker)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_endDatePicker");
                                           var tbTestname = (RadTextBox)testscheduleWindow.FindControl("updatePanel_updateTestSchedul_testcaseName");
                                           var tbTestAssignedReources = (RadTextBox)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_assignedResourcesradTextBox");
                                           labelTestname.Text = string.Format(@"{0} \ {1} \ {2}: ", window.Attributes[C_Product_Name], window.Attributes[C_Release_Name], window.Attributes[C_TestSchedule_Name]);
                                           if (testscheduleWindow.Attributes[C_TestSchedule_Start_Date] != "" || testscheduleWindow.Attributes[C_TestSchedule_End_Date] != "")
                                           {
                                               string testschedule_start_date = window.Attributes[C_TestSchedule_Start_Date];
                                               string testschedule_end_date = window.Attributes[C_TestSchedule_End_Date];
                                               //labelStartData.SelectedDate = Convert.ToDateTime(testschedule_start_date);
                                               //labelEndData.SelectedDate = Convert.ToDateTime(testschedule_end_date);
                                           }
                                           tbTestname.Text = string.Format(@"{0} ", window.Attributes[C_TestSchedule_Name]);
                                           tbTestAssignedReources.Text = window.Attributes[C_TestSchedule_Assigned_Reources];
                                       });

                        var updatepanel = this.GetFirstParentOfType<UpdatePanel>(testscheduleWindow);
                        updatepanel.Update();
                    }
                }
            }
        }

        private void ShowToopTipsinComboBox(RadComboBoxItem radItem)
        {
            switch (radItem.Text)
            {
                case "locready":
                    radItem.ToolTip = "Phase \"from loc-startup\" to \"loc-operations are ready to begin\"";
                    break;

                case "locstart":
                    radItem.ToolTip = "LSPs begin loc(can be the week they are expected to begin,not a specific date)";
                    break;

                case "progressing":
                    radItem.ToolTip = "Regular loc-cadence for the project";
                    break;

                case "endgame":
                    radItem.ToolTip = "Product/release sign-off is coming and PM needs to drive zero-ing LSP-KPIs";
                    break;

                case "signoff":
                    radItem.ToolTip = "Localization has already been completed and Test Pass is required to verify everything is in order for Core's RTM/RTO";
                    break;

                case "retro":
                    radItem.ToolTip = "Post-release retrospective";
                    break;

                default:
                    break;
            }
        }

        #endregion Common

        #region AddUpdate-Releases

        protected void linkButton_addNewRelease_Click(object sender, EventArgs e)
        {
            var releaseWindow = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease") as WebControl;
            this.ShowWindow(
                 action: UiAction.Add
               , triggerCtrlUniqueID: (sender as WebControl).UniqueID
               , targetWindow: releaseWindow
               , dataAttributes: new Dictionary<WebControl, string[]>()
               {
                 { this.scheduleControl_panel_mainContentPanel,
                   new string[] { C_Product_Key, C_Product_Name } }
               }

               , updateWindowFunction: (window) =>
               {
                   var label_window_addRelease = (Label)releaseWindow.FindControl("label_window_addRelease");
                   var radtextbox_ReleaseName = (RadTextBox)releaseWindow.FindControl("radtextbox_ReleaseName");
                   var raddatepicker_ReleaseStartDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseStartDate");
                   var raddatepicker_ReleaseEndDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseEndDate");
                   var radiobuttonList_VsoTag = (RadioButtonList)releaseWindow.FindControl("radioButtonList_ReleaseType");
                   var radbutton_window_addRelease = (RadButton)releaseWindow.FindControl("radbutton_window_addRelease");
                   var radPanelBar_miletoneInReleaseWindow = (RadPanelBar)releaseWindow.FindControl("radPanelBar_miletoneInReleaseWindow");
                   var radButton_UI_addMilestone = (RadButton)releaseWindow.FindControl("radButton_UI_addMilestone");
                   var radAutoCompleteBox_existingReleases = (RadAutoCompleteBox)releaseWindow.FindControl("RadAutoCompleteBox_existingRelease");

                   radAutoCompleteBox_existingReleases.Entries.Clear();

                   radtextbox_ReleaseName.Text = "";
                   raddatepicker_ReleaseStartDate.Clear();
                   raddatepicker_ReleaseEndDate.Clear();
                   if (radiobuttonList_VsoTag.SelectedItem != null)
                   {
                       radiobuttonList_VsoTag.SelectedItem.Selected = false;
                   }
                   radPanelBar_miletoneInReleaseWindow.Items.Clear();
                   //clear the typed textbox

                   label_window_addRelease.Text = string.Format(@"{0} \ Add Release: ", window.Attributes[C_Product_Name]);
                   radbutton_window_addRelease.Text = "Add";
                   radButton_UI_addMilestone.Visible = true;
                   radPanelBar_miletoneInReleaseWindow.Visible = true;
               });

            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(releaseWindow);
            updatepanel.Update();
        }

        public void radbutton_window_addRelease_Click(object sender, EventArgs e)
        {
            var releaseWindow = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease") as WebControl;
            var radtextbox_ReleaseName = (RadTextBox)releaseWindow.FindControl("radtextbox_ReleaseName");
            var raddatepicker_ReleaseStartDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseStartDate");
            var raddatepicker_ReleaseEndDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseEndDate");
            var radbuttonlist_ReleaseType = (RadioButtonList)releaseWindow.FindControl("radioButtonList_ReleaseType");

            var radPanelBar_miletoneInReleaseWindow = (RadPanelBar)releaseWindow.FindControl("radPanelBar_miletoneInReleaseWindow");

            var updateType = (UiAction)Enum.Parse(typeof(UiAction), releaseWindow.Attributes[C_UiAction]);
            RadPanelItem currentItem = null;
            Release release = null;
            Milestone milestone = null;
            MilestoneCategory milestoneCate = null;
            bool success = false;

            switch (updateType)
            {
                case UiAction.Add:
                    var releasesRoot = this.scheduleControl_panel_mainContentPanel.FindControl("radpanelbar_releases_root") as RadPanelBar;

                    success = this.AddUpdateData(releaseWindow,
                       (context) =>
                       {
                           var vsoContext = Utils.GetVsoContext();

                           string title = radtextbox_ReleaseName.Text;
                           DateTime locStartDate = raddatepicker_ReleaseStartDate.SelectedDate.Value;
                           DateTime dueDate = raddatepicker_ReleaseEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                           string releaseType = radbuttonlist_ReleaseType.SelectedValue;
                           //convert to vso tag format
                           string selectedVsoTag = "";
                           if (releaseType != null)
                           {
                               selectedVsoTag = ConvertRelaseTypeToVsoTag(releaseType);
                           }
                           int productKey = int.Parse(releaseWindow.Attributes[C_Product_Key]);
                           //var product = context.Products_New.Select(c => new { c.ProductKey, c.Product_Name, c.ProductFamily.Product_Family }).FirstOrDefault(c => c.ProductKey == productKey);
                           var product = context.Products_New.Select(c => new { c.ProductKey, c.Product_Name, c.ProductFamily.Product_Family, c.Localization_VSO_Path }).FirstOrDefault(c => c.ProductKey == productKey);
                           if (product != null)
                           {
                               string productName = product.Product_Name;
                               string projectName = "LOCALIZATION";
                               string family = product.Product_Family;
                               string areaPath = product.Localization_VSO_Path;
                               string iterationPath = "LOCALIZATION";

                               //1 create VSO Release

                               var newEpic = vsoContext.CreateVsoWorkItem(
                                   type: TaskTypes.Epic,
                                   projectName: projectName,
                                   title: title,
                                   areaPath: areaPath,
                                   iterationPath: iterationPath,
                                   assignedTo: "",
                                   //tags: new string[] { "Loc_Release", string.Format("Loc_ReleaseStartDate:{0}", locStartDate.ToString("M/d/yy")), selectedVsoTag },
                                   tags: new string[] { "Loc_Release", selectedVsoTag },
                                   prepareFunction: (fields) =>
                                   {
                                       //adding start date and due date
                                       var f_startDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StartDate" }, { "value", locStartDate } };
                                       var f_dueDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.DueDate" }, { "value", dueDate } };
                                       fields.Add(f_startDate);
                                       fields.Add(f_dueDate);
                                   });

                               //add to database
                               release = new Release();
                               release.VSO_ID = (int)newEpic["id"];
                               release.VSO_Title = title;
                               release.VSO_LocStartDate = locStartDate;
                               release.VSO_DueDate = dueDate;
                               release.VSO_ProductName = productName;
                               release.VSO_AreaPath = areaPath;
                               release.VSO_IterationPath = iterationPath;
                               release.VSO_Tags = (string)newEpic["fields"]["System.Tags"];
                               release.VSO_ChangedDate = (DateTime)newEpic["fields"]["System.ChangedDate"];
                               release.VSO_Status = (string)newEpic["fields"]["System.State"];
                               release.VSO_Type = (string)newEpic["fields"]["System.WorkItemType"];
                               release.VSO_Family = family;
                               release.ProductKey = int.Parse(releaseWindow.Attributes[C_Product_Key]);

                               var url = string.Format("{0}/DefaultCollection/{1}/_workitems/edit/{2}", vsoContext.VsoUrl, projectName, release.VSO_ID);
                               release.VSO_Url = url;
                               //create a list to store the selected especs
                               List<string> selectedESpecsList = new List<string>();
                               if (radPanelBar_miletoneInReleaseWindow.Items.Count != 0)
                               {
                                   for (int i = 0; i < radPanelBar_miletoneInReleaseWindow.Items.Count; i++)
                                   {
                                       milestone = new Milestone();
                                       var milestoneName = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("radTextBox_mileStoneName_InWindow") as RadTextBox;
                                       var milestoneCategory = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("radcombobox_milestone_categoriesInWindow") as RadComboBox;
                                       var milestoneStartDate = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("raddatepicker_milestone_from_InWindow") as RadDatePicker;
                                       var milestoneEndDate = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("raddatepicker_milestone_to_InWindow") as RadDatePicker;
                                       var milestoneAssignedTo = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("radtextbox_miletoneInReleaseWindow_MilestoneAssignedTo") as RadTextBox;

                                       //get the list of eSpecs
                                       var radPanelBar_eSpecs_child = radPanelBar_miletoneInReleaseWindow.Items[i].FindControl("radPanelBar_eSpecs_child") as RadPanelBar;
                                       var radlistbox_eSpecs = radPanelBar_eSpecs_child.Items[0].FindControl("radListBoxeSpecs") as RadListBox;

                                       //get the textbox
                                       foreach (var eSpecs in radlistbox_eSpecs.CheckedItems)
                                       {
                                           var especName = (eSpecs.FindControl("radTextBoxeSpecs") as RadTextBox).Text;
                                           var eSpecEstimatePoints = (eSpecs.FindControl("radText_eSpecsEstimate") as RadTextBox).Text;
                                           //add newly created espec into epic
                                           string newEpicUrl = (string)newEpic["_links"]["self"]["href"];
                                           //get the vsoTag
                                           string vsoTag;
                                           string link_VsoItem_Milestone = milestoneCategory.SelectedItem.Text;
                                           if (Utils.TryParseCategoryNameToVsoTag(link_VsoItem_Milestone, out vsoTag))
                                           {
                                               link_VsoItem_Milestone = string.Format("{0}", vsoTag);
                                           }

                                           var neweSpecs = vsoContext.CreateVsoWorkItem
                                          (
                                              type: TaskTypes.EnablingSpecification,
                                              projectName: projectName,
                                              title: especName,
                                              areaPath: areaPath,
                                              iterationPath: iterationPath,
                                              assignedTo: "",
                                              referenceWorkItemUrl: newEpicUrl,
                                              linkType: LinkTypes.Child,
                                              tags: new string[] { link_VsoItem_Milestone },
                                              prepareFunction: (fields) =>
                                              {
                                                  //adding eSpec Estimate Points
                                                  var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StoryPoints" }, { "value", eSpecEstimatePoints } };
                                                  fields.Add(f_eSpecEstimate);
                                              });
                                       }

                                       milestone.Release = release;
                                       milestone.ProductKey = release.ProductKey;

                                       milestone.Milestone_Name = milestoneName.Text;
                                       if (milestoneCategory.SelectedItem != null)
                                       {
                                           milestone.MilestoneCategoryKey = int.Parse(milestoneCategory.SelectedItem.Value);
                                       }
                                       else
                                       {
                                           milestoneCate = new MilestoneCategory();
                                           //connect the milesotne(child) to milestonecategory(parent
                                           milestone.MilestoneCategory = milestoneCate;
                                           if (!context.MilestoneCategories.Any(c => c.Milestone_Category_Name == milestoneCategory.Text))
                                           {
                                               milestoneCate.Milestone_Category_Name = milestoneCategory.Text;
                                               context.MilestoneCategories.Add(milestoneCate);
                                           }
                                       }
                                       milestone.Milestone_Start_Date = milestoneStartDate.SelectedDate.Value;
                                       milestone.Milestone_End_Date = milestoneEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                                       milestone.Milestone_Assigned_To = milestoneAssignedTo.Text;

                                       release.Milestones.Add(milestone);
                                   }
                               }

                               context.Releases.Add(release);
                               context.SaveChanges();
                           }
                       });

                    if (success)
                    {
                        //update release node
                        var releaseItem = new RadPanelItem();
                        releaseItem.Attributes[C_Product_Key] = release.ProductKey.ToString();
                        releaseItem.Attributes[C_Release_Key] = release.VSO_ID.ToString();
                        releaseItem.Attributes[C_Release_Name] = release.VSO_Title;
                        releaseItem.Attributes[C_Release_Start_Date] = release.VSO_LocStartDate.ToString();
                        releaseItem.Attributes[C_Release_End_Date] = release.VSO_DueDate.ToString();
                        releaseItem.Attributes[C_VSO_Tag] = release.VSO_Tags;
                        releasesRoot.Items.Add(releaseItem);

                        var radpanelbar_release_child = releaseItem.FindControl("radpanelbar_releases_child") as RadPanelBar;

                        //add link
                        var link_VsoItem_release = radpanelbar_release_child.Items[0].Header.FindControl("link_VsoItem_release") as HyperLink;
                        link_VsoItem_release.Text = release.VSO_ID.ToString();
                        link_VsoItem_release.NavigateUrl = release.VSO_Url;

                        var raddatepicker_release_from = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_release_from") as RadDatePicker;
                        raddatepicker_release_from.SharedCalendar = this.GetCalendar();

                        var raddatepicker_release_to = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_release_to") as RadDatePicker;
                        raddatepicker_release_to.SharedCalendar = this.GetCalendar();

                        raddatepicker_release_from.SelectedDate = raddatepicker_ReleaseStartDate.SelectedDate;
                        raddatepicker_release_to.SelectedDate = raddatepicker_ReleaseEndDate.SelectedDate;

                        var label_VsoTag = radpanelbar_release_child.Items[0].Header.FindControl("Label_CustomTagOnUI") as Label;

                        label_VsoTag.Text = "";
                        var selectedVSOTag = ConvertRelaseTypeToVsoTag(radbuttonlist_ReleaseType.SelectedValue);
                        label_VsoTag.Text = selectedVSOTag;

                        var milestonesRoot = radpanelbar_release_child.Items[0].FindControl("radpanelbar_milestones_root") as RadPanelBar;

                        foreach (var mItem in release.Milestones)
                        {
                            var milestoneItem = new RadPanelItem();
                            milestoneItem.Attributes[C_Milestone_Key] = mItem.MilestoneKey.ToString();
                            milestoneItem.Attributes[C_Milestone_Name] = mItem.Milestone_Name.ToString();
                            milestonesRoot.Items.Add(milestoneItem);
                            var label_milestoneName = milestoneItem.FindControl("label_milestoneName") as Label;
                            label_milestoneName.Text = mItem.Milestone_Name;

                            var link_VsoItem_Milestone = milestoneItem.FindControl("link_VsoItem_Milestone") as HyperLink;
                            string vsoTag;

                            if (Utils.TryParseCategoryNameToVsoTag(this.MilestoneCategories[mItem.MilestoneCategoryKey], out vsoTag))
                            {
                                link_VsoItem_Milestone.Text = string.Format("{0}", vsoTag);
                                link_VsoItem_Milestone.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(release.VSO_ID, vsoTag, VSO_Project, scheduleControl_panel_mainContentPanel.Attributes[C_Product_Family]);
                            }
                            else
                            {
                                var label_VsoQuery = milestoneItem.FindControl("label_VsoQuery") as Label;
                                label_VsoQuery.Visible = false;
                                link_VsoItem_Milestone.Visible = false;
                            }

                            var milestoneCategoryName = milestoneItem.FindControl("radcombobox_milestone_categoriesOnControl") as RadComboBox;

                            foreach (var category in this.MilestoneCategories)
                            {
                                RadComboBoxItem radItem = new RadComboBoxItem();
                                radItem.Text = category.Value;
                                radItem.Value = category.Key.ToString();
                                ShowToopTipsinComboBox(radItem);

                                milestoneCategoryName.Items.Add(radItem);
                                if (category.Key == mItem.MilestoneCategoryKey)
                                {
                                    radItem.Selected = true;
                                }

                                radItem.DataBind();
                            };

                            if (mItem.MilestoneCategory != null)
                            {
                                if (!this.MilestoneCategories.Any(c => c.Value == mItem.MilestoneCategory.Milestone_Category_Name))
                                {
                                    RadComboBoxItem radItem = new RadComboBoxItem();
                                    radItem.Text = mItem.MilestoneCategory.Milestone_Category_Name;
                                    radItem.Value = mItem.MilestoneCategory.MilestoneCategoryKey.ToString();
                                    milestoneCategoryName.Items.Add(radItem);
                                }

                                var radcombobox_UI_addMinestoneCategoryName = milestoneItem.FindControl("radcombobox_milestone_categoriesOnControl") as RadComboBox;
                                RadComboBoxItem item = radcombobox_UI_addMinestoneCategoryName.FindItemByText(mItem.MilestoneCategory.Milestone_Category_Name);
                                item.Selected = true;
                            }

                            var raddatepicker_milestone_from = milestoneItem.FindControl("raddatepicker_milestone_from") as RadDatePicker;
                            raddatepicker_milestone_from.SharedCalendar = this.GetCalendar();

                            var raddatepicker_milestone_to = milestoneItem.FindControl("raddatepicker_milestone_to") as RadDatePicker;
                            raddatepicker_milestone_to.SharedCalendar = this.GetCalendar();

                            raddatepicker_milestone_from.SelectedDate = mItem.Milestone_Start_Date.Value;
                            raddatepicker_milestone_to.SelectedDate = mItem.Milestone_End_Date.Value;
                        }

                        releasesRoot.DataBind();
                    }

                    break;

                case UiAction.Update:
                    currentItem = this.GetControlFromUniqueID(releaseWindow.Attributes[C_Trigger_Control_UniqueID], "radpanelbar_releases_root") as RadPanelItem;
                    success = this.AddUpdateData(releaseWindow,
                       (context) =>
                       {
                           int releaseKey = int.Parse(releaseWindow.Attributes[C_Release_Key]);
                           string title = radtextbox_ReleaseName.Text;
                           DateTime locStartDate = raddatepicker_ReleaseStartDate.SelectedDate.Value;
                           DateTime dueDate = raddatepicker_ReleaseEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                           string customVsoTag = ConvertRelaseTypeToVsoTag(radbuttonlist_ReleaseType.SelectedValue);

                           //update VSO release
                           var vsoContext = Utils.GetVsoContext();

                           var updatedEpic = vsoContext.UpdateVsoWorkItem(
                               id: releaseKey,
                               fields: new Dictionary<string, string>{
                                        {"System.Title",title},
                                        {"Microsoft.VSTS.Scheduling.StartDate", locStartDate.ToString()},
                                        {"Microsoft.VSTS.Scheduling.DueDate", dueDate.ToString()},
                                        {"System.Tags",string.Concat("Loc_Release; ",customVsoTag)}
                               });

                           //update database
                           var releaseToUpdate = context.Releases.FirstOrDefault(c => c.VSO_ID == releaseKey);
                           releaseToUpdate.VSO_Title = title;
                           releaseToUpdate.VSO_LocStartDate = locStartDate;
                           releaseToUpdate.VSO_DueDate = dueDate;
                           releaseToUpdate.VSO_Tags = (string)updatedEpic["fields"]["System.Tags"];

                           context.SaveChanges();
                       });

                    if (success)
                    {
                        //update release node
                        currentItem.Attributes[C_Release_Name] = radtextbox_ReleaseName.Text;
                        currentItem.Attributes[C_Release_Start_Date] = raddatepicker_ReleaseStartDate.SelectedDate.ToString();
                        currentItem.Attributes[C_Release_End_Date] = raddatepicker_ReleaseEndDate.SelectedDate.ToString();
                        currentItem.Attributes[C_VSO_Tag] = ConvertRelaseTypeToVsoTag(radbuttonlist_ReleaseType.SelectedValue);

                        var radpanelbar_release_child = currentItem.FindControl("radpanelbar_releases_child") as RadPanelBar;

                        var raddatepicker_release_from = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_release_from") as RadDatePicker;
                        raddatepicker_release_from.SharedCalendar = this.GetCalendar();

                        var raddatepicker_release_to = radpanelbar_release_child.Items[0].Header.FindControl("raddatepicker_release_to") as RadDatePicker;
                        raddatepicker_release_to.SharedCalendar = this.GetCalendar();

                        raddatepicker_release_from.SelectedDate = raddatepicker_ReleaseStartDate.SelectedDate;
                        raddatepicker_release_to.SelectedDate = raddatepicker_ReleaseEndDate.SelectedDate;

                        var label_CustomeVsoTag = radpanelbar_release_child.Items[0].Header.FindControl("Label_CustomTagOnUI") as Label;

                        label_CustomeVsoTag.Text = "";
                        var selectedVSOTag = ConvertRelaseTypeToVsoTag(radbuttonlist_ReleaseType.SelectedValue);
                        label_CustomeVsoTag.Text = selectedVSOTag;

                        currentItem.DataBind();
                    }
                    break;

                default:
                    break;
            }
            this.updatePanel_scheduleControl.Update();
        }

        protected void radButton_UI_addMilestone_Click(object sender, EventArgs e)
        {
            CreateMilestonesRowsInAddReleaseWindow();
        }

        private string ConvertRelaseTypeToVsoTag(string releasetag)
        {
            string vsoTag = "";
            switch (releasetag)
            {
                case "SLA1":
                    vsoTag = "Loc_SLA1";
                    break;

                case "SLA2":
                    vsoTag = "Loc_SLA2";
                    break;

                case "SLA3":
                    vsoTag = "Loc_SLA3";
                    break;
            }
            return vsoTag;
        }

        protected RadPanelItem CreateMilestonesRowsInAddReleaseWindow()
        {
            var radPanelBar_miletoneInReleaseWindow = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease").FindControl("radPanelBar_miletoneInReleaseWindow") as RadPanelBar;
            var radpanelItem = new RadPanelItem();
            radPanelBar_miletoneInReleaseWindow.Items.Add(radpanelItem);

            //find the Radcombobox in the itemtemplate/radpanel
            var radcombobox_milestone_categoriesNameInWindow = radpanelItem.FindControl("radcombobox_milestone_categoriesInWindow") as RadComboBox;
            foreach (var category in this.MilestoneCategories)
            {
                RadComboBoxItem radItem = new RadComboBoxItem();
                radItem.Text = category.Value;
                radItem.Value = category.Key.ToString();
                ShowToopTipsinComboBox(radItem);
                radcombobox_milestone_categoriesNameInWindow.Items.Add(radItem);
                radItem.DataBind();
            }
            radPanelBar_miletoneInReleaseWindow.DataBind();

            radpanelItem.FindControl("radtextbox_miletoneInReleaseWindow_MilestoneAssignedTo").Visible = false;
            radpanelItem.FindControl("label_miletoneInReleaseWindow_MilestoneAssignedTo").Visible = false;

            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(radPanelBar_miletoneInReleaseWindow);
            updatepanel.Update();
            return radpanelItem;
        }

        protected void radButton__window_deleteMilestone_Click(object sender, EventArgs e)
        {
            var milestoneDeleteButton = sender as RadButton;
            var currentItem = this.GetControlFromUniqueID(milestoneDeleteButton.UniqueID, "radPanelBar_miletoneInReleaseWindow") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);
            var updatePanel = this.GetFirstParentOfType<UpdatePanel>(currentItem.PanelBar);
            updatePanel.Update();
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", "modal_addingRelease"), true);
        }

        #endregion AddUpdate-Releases

        #region AddUpdate-TestSchedule

        protected void linkButton_addNewTestSchedule_Click(object sender, EventArgs e)
        {
            var testScheduleWindow = this.GetWindowItem(this.treeview_addTestSchedule, "radwindow_addTestSchedule") as WebControl;
            var label_window_addTestSchedule = (Label)testScheduleWindow.FindControl("label_window_addTestSchedule");
            var radcombobox_MilestoneCategoryName_InTestPlanWindowTop = (RadComboBox)testScheduleWindow.FindControl("radcombobox_MilestoneCategoryName_InTestPlanWindowTop");
            var radTextBox_WindowTop_TestScheduleName = (RadTextBox)testScheduleWindow.FindControl("radtextbox_windowTop_TestPlanID");
            var raddatepicker_WindowTop_TestScheduleStartDate = (RadDatePicker)testScheduleWindow.FindControl("raddatepicker_windowTop_TestScheduleStartDate");
            var raddatepicker_WindowTop_TestScheduleEndDate = (RadDatePicker)testScheduleWindow.FindControl("raddatepicker_windowTop_TestScheduleEndDate");
            var radTextBox_windowTop_AssignedResources = (RadTextBox)testScheduleWindow.FindControl("radTextBox_windowTop_AssignedResources");
            var radTextBox_windowTop_Iteration = (RadTextBox)testScheduleWindow.FindControl("radTextBox_windowTop_Iteration");
            var label_testPlanIterationWarning = (Label)testScheduleWindow.FindControl("label_testPlanIterationWarning");
            var radPanelBar_testScheduleInTestScheduleWindow = (RadPanelBar)testScheduleWindow.FindControl("radPanelBar_testScheduleInTestScheduleWindow");
            var radbutton_window_addTestSchedule = (RadButton)testScheduleWindow.FindControl("radbutton_window_addTestSchedule");

            if (!radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Items.Any())
            {
                foreach (var category in this.MilestoneCategories)
                {
                    RadComboBoxItem radItem = new RadComboBoxItem();
                    radItem.Text = category.Value;
                    radItem.Value = category.Key.ToString();
                    ShowToopTipsinComboBox(radItem);
                    radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Items.Add(radItem);
                    radItem.DataBind();
                }
            }

            this.ShowWindow(
                action: UiAction.Add
                , triggerCtrlUniqueID: (sender as WebControl).UniqueID
                , targetWindow: testScheduleWindow
                , dataAttributes: new Dictionary<WebControl, string[]>()
                    {
                        { this.scheduleControl_panel_mainContentPanel,
                        new string[] { C_Product_Key, C_Product_Name }
                        },

                        { this.GetControlFromUniqueID((sender as WebControl).UniqueID,"radpanelbar_releases_root"),
                        new string[] { C_Product_Key, C_Release_Key, C_Release_Name }
                        }
                    }

                , updateWindowFunction: (window) =>
                {
                    radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Text = " ";
                    radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Text = "Enter Custom Category";
                    radcombobox_MilestoneCategoryName_InTestPlanWindowTop.DataBind();
                    radTextBox_WindowTop_TestScheduleName.Text = "";
                    raddatepicker_WindowTop_TestScheduleStartDate.Clear();
                    raddatepicker_WindowTop_TestScheduleEndDate.Clear();
                    radTextBox_windowTop_AssignedResources.Text = "0";
                    radTextBox_windowTop_Iteration.Text = "LOCALIZATION";
                    label_testPlanIterationWarning.Visible = false;
                    //delete the extra testSchedules
                    radPanelBar_testScheduleInTestScheduleWindow.Items.Clear();

                    label_window_addTestSchedule.Text = string.Format(@"{0} \ {1} \ Add Test Plan: ", window.Attributes[C_Product_Name], window.Attributes[C_Release_Name]);
                    radbutton_window_addTestSchedule.Text = "Add";
                });
            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(testScheduleWindow);
            updatepanel.Update();
        }

        protected void radbutton_window_addTestSchedule_Click(object sender, EventArgs e)
        {
            var testScheduleWindow = this.GetWindowItem(this.treeview_addTestSchedule, "radwindow_addTestSchedule") as WebControl;
            //get the controls from the top row
            var radTextBox_windowTop_testPlanID = (RadTextBox)testScheduleWindow.FindControl("radtextbox_windowTop_TestPlanID");
            var radcombobox_MilestoneCategoryName_InTestPlanWindowTop = (RadComboBox)testScheduleWindow.FindControl("radcombobox_MilestoneCategoryName_InTestPlanWindowTop");
            var raddatepicker_windowTop_TestStartDate = (RadDatePicker)testScheduleWindow.FindControl("raddatepicker_windowTop_TestScheduleStartDate");
            var raddatepicker_windowTop_TestEndDate = (RadDatePicker)testScheduleWindow.FindControl("raddatepicker_windowTop_TestScheduleEndDate");
            var radTextBox_windowTop_AssignedResources = (RadTextBox)testScheduleWindow.FindControl("radTextBox_windowTop_AssignedResources");
            var radTextBox_windowTop_Iteration = (RadTextBox)testScheduleWindow.FindControl("radTextBox_windowTop_Iteration");
            var label_testPlanIterationWarning = (Label)testScheduleWindow.FindControl("label_testPlanIterationWarning");
            var updatePanel_addTestSchedule_iTerationPath = (UpdatePanel)testScheduleWindow.FindControl("updatePanel_addTestSchedule_iTerationPath");
            var radPanelBar_testScheduleInTestScheduleWindow = (RadPanelBar)testScheduleWindow.FindControl("radPanelBar_testScheduleInTestScheduleWindow");
            var updateType = (UiAction)Enum.Parse(typeof(UiAction), testScheduleWindow.Attributes[C_UiAction]);
            TestSchedule testSchedule = null;
            List<TestSchedule> testScheduleList = null;
            MilestoneCategory firstMilestoneCate = null;
            MilestoneCategory milestoneCate = null;
            bool success = false;
            //get all the controls of extra test schedules rows
            RadPanelItem currentReleaseItem = null;
            currentReleaseItem = this.GetControlFromUniqueID(testScheduleWindow.Attributes[C_Trigger_Control_UniqueID], "radpanelbar_releases_child") as RadPanelItem;
            var testSchedulesRoot = currentReleaseItem.FindControl("radpanelbar_testSchedules_root") as RadPanelBar;
            switch (updateType)
            {
                case UiAction.Add:
                    try
                    {
                        success = this.AddUpdateData(testScheduleWindow,
                                             (context) =>
                                             {
                                                 //add to DB
                                                 testSchedule = new TestSchedule();
                                                 testSchedule.TestSchedule_Name = radTextBox_windowTop_testPlanID.Text;
                                                 testSchedule.TestSchedule_Start_Date = raddatepicker_windowTop_TestStartDate.SelectedDate.Value;
                                                 testSchedule.TestSchedule_End_Date = raddatepicker_windowTop_TestEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                                                 int assignedResources;
                                                 if (Int32.TryParse(radTextBox_windowTop_AssignedResources.Text, out assignedResources))
                                                 {
                                                     testSchedule.AssignedResources = assignedResources;
                                                 }
                                                 else
                                                 {
                                                     testSchedule.AssignedResources = 0;
                                                 }
                                                 testSchedule.ReleaseKey = int.Parse(testScheduleWindow.Attributes["ReleaseKey"]);
                                                 testSchedule.ProductKey = int.Parse(testScheduleWindow.Attributes["ProductKey"]);

                                                 // customize item should be considered
                                                 if (radcombobox_MilestoneCategoryName_InTestPlanWindowTop.SelectedItem != null)
                                                 {
                                                     testSchedule.MilestoneCategoryKey = int.Parse(radcombobox_MilestoneCategoryName_InTestPlanWindowTop.SelectedItem.Value);
                                                 }
                                                 else
                                                 {
                                                     firstMilestoneCate = new MilestoneCategory();
                                                     testSchedule.MilestoneCategory = firstMilestoneCate;
                                                     if (!context.MilestoneCategories.Any(c => c.Milestone_Category_Name == radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Text))
                                                     {
                                                         firstMilestoneCate.Milestone_Category_Name = radcombobox_MilestoneCategoryName_InTestPlanWindowTop.Text;
                                                         context.MilestoneCategories.Add(firstMilestoneCate);
                                                     }
                                                 }

                                                 //create VSO test item
                                                 var vsoContext = Utils.GetVsoContext();
                                                 var productKey = int.Parse(testScheduleWindow.Attributes["ProductKey"]);
                                                 //var product = context.Products.Select(c => new { c.ProductKey, c.Product_Name, c.Family }).First(c => c.ProductKey == productKey);
                                                 //code below'll use Products_New
                                                 var product = context.Products_New.Select(c => new { c.ProductKey, c.Product_Name, c.ProductFamily.Product_Family, c.Localization_VSO_Path }).FirstOrDefault(c => c.ProductKey == productKey);
                                                 if (product != null)
                                                 {
                                                     string productName = product.Product_Name;
                                                     string family = product.Product_Family;
                                                     //string areaPath = string.Format("{0}\\{1}", vsoContext.GetProjectTeamDefaultArea(VSO_Project, family), productName);
                                                     string areaPath = product.Localization_VSO_Path;

                                                     string vsoTag;
                                                     string link_VsoItem_Test = radcombobox_MilestoneCategoryName_InTestPlanWindowTop.SelectedItem.Text;
                                                     if (Utils.TryParseCategoryNameToVsoTag(link_VsoItem_Test, out vsoTag))
                                                     {
                                                         link_VsoItem_Test = string.Format("{0}", vsoTag);
                                                     }

                                                     var ids = new int[] { testSchedule.ReleaseKey };
                                                     string currentEpicUrl = String.Format("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/{0}", ids[0]);

                                                     var testName = testSchedule.TestSchedule_Name;
                                                     var startDate = testSchedule.TestSchedule_Start_Date;
                                                     var finishDate = testSchedule.TestSchedule_End_Date;
                                                     var testPlanIterationTop = radTextBox_windowTop_Iteration.Text.Trim();

                                                     var newTestPlan = vsoContext.CreateTestPlan(VSO_Project, testName, areaPath, testPlanIterationTop, startDate.Value, finishDate.Value);
                                                     testSchedule.TestScheduleKey = (int)newTestPlan["id"];

                                                     var updatedTestPlan = vsoContext.UpdateVsoWorkItem(testSchedule.TestScheduleKey,
                                                         new Dictionary<string, string>{
                                                    {"System.Tags",string.Concat("Loc_TestPlan; ", link_VsoItem_Test)}},
                                                         (c) =>
                                                         {
                                                             var f_relation = new Dictionary<string, object>()
                                                        {
                                                            { "op", "add" },
                                                            { "path", string.Format("/relations/-") },
                                                            { "value", new Dictionary<string,object>()
                                                                {
                                                                    { "rel", "System.LinkTypes.Hierarchy-Reverse"},
                                                                    { "url", currentEpicUrl},
                                                                }
                                                            }
                                                        };
                                                             c.Add(f_relation);
                                                         });

                                                     var vsoTestScheduleUrl = string.Format("{0}/DefaultCollection/{1}/_testManagement?planId={2}", vsoContext.VsoUrl, VSO_Project, testSchedule.TestScheduleKey);
                                                     testSchedule.Vso_Web_Url = vsoTestScheduleUrl;
                                                     testScheduleList = new List<TestSchedule>();
                                                     testScheduleList.Add(testSchedule);

                                                     if (radPanelBar_testScheduleInTestScheduleWindow.Items.Count != 0)
                                                     {
                                                         for (int i = 0; i < radPanelBar_testScheduleInTestScheduleWindow.Items.Count; i++)
                                                         {
                                                             TestSchedule extraTestSchedule = new TestSchedule();
                                                             extraTestSchedule.ReleaseKey = int.Parse(testScheduleWindow.Attributes["ReleaseKey"]);
                                                             extraTestSchedule.ProductKey = int.Parse(testScheduleWindow.Attributes["ProductKey"]);

                                                             var milestoneCategory = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("radcombobox_MilestoneCategoryName_InTestPlanWindowMiddle") as RadComboBox;
                                                             var extraTestSchedule_Name = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("radTextBox_testScheduleName_InTestScheduleWindow") as RadTextBox;
                                                             var extraTestSchedule_Start_Date = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("raddatepicker_TestStartDate") as RadDatePicker;
                                                             var extraTestSchedule_End_Date = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("raddatepicker_TestEndDate") as RadDatePicker;
                                                             var extraTestSchedule_AssignedResources = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("radTextBox_ExtraRow_AssignedResources") as RadTextBox;
                                                             var radTextBox_ExtraRow_Iteration = radPanelBar_testScheduleInTestScheduleWindow.Items[i].FindControl("radTextBox_ExtraRow_Iteration") as RadTextBox;
                                                             string extra_VsoTag;
                                                             string link_VsoItem_Milestone = milestoneCategory.SelectedItem.Text;
                                                             if (Utils.TryParseCategoryNameToVsoTag(link_VsoItem_Milestone, out extra_VsoTag))
                                                             {
                                                                 link_VsoItem_Milestone = string.Format("{0}", extra_VsoTag);
                                                             }

                                                             //create vso test item
                                                             var newExtraTestPlan = vsoContext.CreateTestPlan(VSO_Project, extraTestSchedule_Name.Text, areaPath, radTextBox_ExtraRow_Iteration.Text.Trim(), extraTestSchedule_Start_Date.SelectedDate.Value, extraTestSchedule_End_Date.SelectedDate.Value);
                                                             extraTestSchedule.TestScheduleKey = (int)newExtraTestPlan["id"];

                                                             var updatedExtraTestPlan = vsoContext.UpdateVsoWorkItem(extraTestSchedule.TestScheduleKey,
                                                                 new Dictionary<string, string>{
                                                             {"System.Tags",string.Concat("Loc_TestPlan; ", link_VsoItem_Milestone)}},
                                                                 (c) =>
                                                                 {
                                                                     var f_relation = new Dictionary<string, object>()
                                                                                {
                                                                                    { "op", "add" },
                                                                                    { "path", string.Format("/relations/-") },
                                                                                    { "value", new Dictionary<string,object>()
                                                                                        {
                                                                                            { "rel", "System.LinkTypes.Hierarchy-Reverse"},
                                                                                            { "url", currentEpicUrl},
                                                                                        }
                                                                                    }
                                                                                };
                                                                     c.Add(f_relation);
                                                                 });

                                                             var extraVsoTestScheduleUrl = string.Format("{0}/DefaultCollection/{1}/_testManagement?planId={2}", vsoContext.VsoUrl, VSO_Project, extraTestSchedule.TestScheduleKey);
                                                             extraTestSchedule.Vso_Web_Url = extraVsoTestScheduleUrl;

                                                             extraTestSchedule.TestSchedule_Name = extraTestSchedule_Name.Text;
                                                             extraTestSchedule.TestSchedule_Start_Date = extraTestSchedule_Start_Date.SelectedDate.Value;
                                                             extraTestSchedule.TestSchedule_End_Date = extraTestSchedule_End_Date.SelectedDate.Value;

                                                             if (milestoneCategory.SelectedItem != null)
                                                             {
                                                                 extraTestSchedule.MilestoneCategoryKey = int.Parse(milestoneCategory.SelectedItem.Value);
                                                             }
                                                             else
                                                             {
                                                                 milestoneCate = new MilestoneCategory();
                                                                 //connect the milesotne(child) to milestonecategory(parent)
                                                                 extraTestSchedule.MilestoneCategory = milestoneCate;
                                                                 if (!context.MilestoneCategories.Any(c => c.Milestone_Category_Name == milestoneCategory.Text))
                                                                 {
                                                                     milestoneCate.Milestone_Category_Name = milestoneCategory.Text;
                                                                     context.MilestoneCategories.Add(milestoneCate);
                                                                 }
                                                             }

                                                             int extraAssignedResources;
                                                             if (Int32.TryParse(extraTestSchedule_AssignedResources.Text, out extraAssignedResources))
                                                             {
                                                                 extraTestSchedule.AssignedResources = extraAssignedResources;
                                                             }
                                                             else
                                                             {
                                                                 extraTestSchedule.AssignedResources = 0;
                                                             }

                                                             if (testScheduleList == null)
                                                             {
                                                                 testScheduleList = new List<TestSchedule>();
                                                             }
                                                             testScheduleList.Add(extraTestSchedule);
                                                         }
                                                     }

                                                     foreach (var testScheduleItem in testScheduleList)
                                                     {
                                                         context.TestSchedules.Add(testScheduleItem);
                                                     }
                                                     context.SaveChanges();
                                                 }
                                             });
                    }
                    catch (Exception)
                    {
                        label_testPlanIterationWarning.Visible = true;
                        updatePanel_addTestSchedule_iTerationPath.Update();
                    }

                    if (success)
                    {
                        //update testSchedule nodes
                        if (testScheduleList != null)
                        {
                            foreach (var tItem in testScheduleList)
                            {
                                RadPanelItem testItem = new RadPanelItem();
                                testItem.Attributes[C_TestSchedule_Key] = tItem.TestScheduleKey.ToString();
                                testItem.Attributes[C_TestSchedule_Name] = tItem.TestSchedule_Name;
                                testSchedulesRoot.Items.Add(testItem);

                                var label_testScheduleName = testItem.FindControl("label_testScheduleaName") as Label;
                                label_testScheduleName.Text = tItem.TestSchedule_Name;
                                var link_VsoItem_TestSchedule = testItem.FindControl("link_VsoItem_TestSchedule") as HyperLink;
                                if (string.IsNullOrEmpty(tItem.Vso_Web_Url))
                                {
                                    link_VsoItem_TestSchedule.Visible = false;
                                }
                                else
                                {
                                    link_VsoItem_TestSchedule.Text = tItem.TestScheduleKey.ToString();
                                    link_VsoItem_TestSchedule.NavigateUrl = tItem.Vso_Web_Url;
                                }

                                var link_VsoItem_TestMilestoneCategory = testItem.FindControl("link_VsoItem_TestMilestoneCategory") as HyperLink;
                                string vsoTag;
                                if (Utils.TryParseCategoryNameToVsoTag(this.MilestoneCategories[tItem.MilestoneCategoryKey.Value], out vsoTag))
                                {
                                    link_VsoItem_TestMilestoneCategory.Text = string.Format("{0}", vsoTag);
                                    link_VsoItem_TestMilestoneCategory.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(testSchedule.ReleaseKey, vsoTag, VSO_Project, scheduleControl_panel_mainContentPanel.Attributes[C_Product_Family]);
                                }
                                else
                                {
                                    var vSOTag_testPlan = testItem.FindControl("VSOTag_testPlan") as Label;
                                    vSOTag_testPlan.Visible = false;
                                    link_VsoItem_TestMilestoneCategory.Visible = false;
                                }

                                // choose the selected MilestoneCategory Name from the comboBox for original or customized value
                                // firstly polulate the combobox with existing data from DB
                                var test_milestoneCategoryName = testItem.FindControl("radcombobox_testPlan_categoriesOnControl") as RadComboBox;
                                foreach (var category in this.MilestoneCategories)
                                {
                                    RadComboBoxItem radItem = new RadComboBoxItem();
                                    radItem.Text = category.Value;
                                    radItem.Value = category.Key.ToString();
                                    ShowToopTipsinComboBox(radItem);

                                    test_milestoneCategoryName.Items.Add(radItem);
                                    if (category.Key == tItem.MilestoneCategoryKey)
                                    {
                                        radItem.Selected = true;
                                    }
                                    radItem.DataBind();
                                };
                                if (tItem.MilestoneCategory != null)
                                {
                                    if (!this.MilestoneCategories.Any(c => c.Value == tItem.MilestoneCategory.Milestone_Category_Name))
                                    {
                                        RadComboBoxItem radItem = new RadComboBoxItem();
                                        radItem.Text = tItem.MilestoneCategory.Milestone_Category_Name;
                                        radItem.Value = tItem.MilestoneCategory.MilestoneCategoryKey.ToString();
                                        test_milestoneCategoryName.Items.Add(radItem);
                                    }

                                    //secondly populate the combobox with the customized data
                                    var radcombobox_testPlan_categoriesOnControl = testItem.FindControl("radcombobox_testPlan_categoriesOnControl") as RadComboBox;

                                    RadComboBoxItem item = radcombobox_testPlan_categoriesOnControl.FindItemByText(tItem.MilestoneCategory.Milestone_Category_Name);
                                    item.Selected = true;
                                }

                                var raddatepicker_testSchedule_from = testItem.FindControl("raddatepicker_testSchedule_from") as RadDatePicker;
                                raddatepicker_testSchedule_from.SelectedDate = tItem.TestSchedule_Start_Date;
                                var raddatepicker_testSchedule_to = testItem.FindControl("raddatepicker_testSchedule_to") as RadDatePicker;
                                raddatepicker_testSchedule_to.SelectedDate = tItem.TestSchedule_End_Date;
                                var radTextBox_MainUi_AssignedResources = testItem.FindControl("radTextBox_MainUi_AssignedResources") as Label;
                                radTextBox_MainUi_AssignedResources.Text = tItem.AssignedResources.Value.ToString();

                                testSchedulesRoot.DataBind();
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
            this.updatePanel_scheduleControl.Update();
        }

        protected void updatePanel_updateTestSchedule_radButton_update_Click(object sender, EventArgs e)
        {
            TestSchedule testschedule = null;
            bool success = false;

            var testscheduleWindow = this.GetWindowItem(this.treeview_updateTestSchedule, "updatePanel_updateTestSchedule_panel") as WebControl;
            success = this.AddUpdateData(testscheduleWindow,
                        (context) =>
                        {
                            var testKey = int.Parse(testscheduleWindow.Attributes[C_TestSchedule_Key].ToString());
                            var labelTestname = (Label)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_title");
                            var tbTestname = (RadTextBox)testscheduleWindow.FindControl("updatePanel_updateTestSchedul_testcaseName");

                            var textBoxAssignedResources = (RadTextBox)testscheduleWindow.FindControl("updatePanel_updateTestSchedule_assignedResourcesradTextBox");

                            //update VSO test
                            var vsoContext = Utils.GetVsoContext();

                            var updatedEpic = vsoContext.UpdateVsoWorkItem(
                                id: testKey,
                                fields: new Dictionary<string, string>{
                                        {"System.Title",tbTestname.Text}
                               });

                            //update DB
                            testschedule = context.TestSchedules.Where(c => c.TestScheduleKey == testKey).FirstOrDefault();
                            testschedule.TestSchedule_Name = tbTestname.Text;
                            int updatedAssignedResources;
                            if (int.TryParse(textBoxAssignedResources.Text, out updatedAssignedResources))
                            {
                                testschedule.AssignedResources = updatedAssignedResources;
                            }
                            context.SaveChanges();
                        });

            if (success)
            {
                var currentTestschedule = this.GetControlFromUniqueID(testscheduleWindow.Attributes[C_Trigger_Control_UniqueID], "radpanelbar_releases_child") as RadPanelItem;
                var testscheduleRoot = currentTestschedule.FindControl("radpanelbar_testSchedules_root") as RadPanelBar;

                //update milestone node
                var testPanelItem = this.GetChildrenOfType<RadPanelItem>(testscheduleRoot).Where(c => c.UniqueID == testscheduleWindow.Attributes[C_Trigger_Control_UniqueID]).First();
                var labelTestName = testPanelItem.FindControl("label_testScheduleaName") as Label;
                labelTestName.Text = testschedule.TestSchedule_Name;
                var radTextBox_MainUi_AssignedResources = testPanelItem.FindControl("radTextBox_MainUi_AssignedResources") as Label;
                radTextBox_MainUi_AssignedResources.Text = testschedule.AssignedResources.Value.ToString();
                testPanelItem.Attributes[C_TestSchedule_Name] = testschedule.TestSchedule_Name;
                testPanelItem.Attributes[C_TestSchedule_Assigned_Reources] = testschedule.AssignedResources.Value.ToString();

                testscheduleRoot.DataBind();
                this.updatePanel_scheduleControl.Update();
            }
        }

        protected void radButton_addTestInTestScheduleWindow_Click(object sender, EventArgs e)
        {
            var testScheduleWindow = this.GetWindowItem(this.treeview_addTestSchedule, "radwindow_addTestSchedule") as WebControl;
            var radPanelBar_testScheduleInTestScheduleWindow = (RadPanelBar)testScheduleWindow.FindControl("radPanelBar_testScheduleInTestScheduleWindow");
            var radpanelItem = new RadPanelItem();
            radPanelBar_testScheduleInTestScheduleWindow.Items.Add(radpanelItem);

            //find the Radcombobox in the itemtemplate/radpanel
            var radcombobox_MilestoneCategoryName_InTestPlanWindowMiddle = radpanelItem.FindControl("radcombobox_MilestoneCategoryName_InTestPlanWindowMiddle") as RadComboBox;
            foreach (var category in this.MilestoneCategories)
            {
                RadComboBoxItem radItem = new RadComboBoxItem();
                radItem.Text = category.Value;
                radItem.Value = category.Key.ToString();
                ShowToopTipsinComboBox(radItem);
                radcombobox_MilestoneCategoryName_InTestPlanWindowMiddle.Items.Add(radItem);
                radItem.DataBind();
            }

            radPanelBar_testScheduleInTestScheduleWindow.DataBind();
            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(radPanelBar_testScheduleInTestScheduleWindow);
            updatepanel.Update();
        }

        #endregion AddUpdate-TestSchedule

        #region AddUpdate-Milestone

        protected void linkButton_addNewMilestone_Click(object sender, EventArgs e)
        {
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl;
            var radTextBox_mileStoneName_OnMilestoneWindowTop = (RadTextBox)milestoneWindow.FindControl("radTextBox_mileStoneName_OnMilestoneWindowTop");
            var radcombobox_MilestoneCategoryName_InMilestoneWindowTop = (RadComboBox)milestoneWindow.FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindowTop");
            var raddatepicker_windowTop_MilestoneStartDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneStartDate");
            var raddatepicker_windowTop_MilestoneEndDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneEndDate");
            var radPanelBar_miletoneInMilestoneWindow = (RadPanelBar)milestoneWindow.FindControl("radPanelBar_miletoneInMilestoneWindow");
            var label_window_addMilestone = (Label)milestoneWindow.FindControl("label_window_addMilestone");

            var radPanelBar_newMilestoneWindowTop_eSpecs = (RadPanelBar)milestoneWindow.FindControl("radPanelBar_newMilestoneWindowTop_eSpecs");
            var radListBoxeOnTopeSpecs = (RadListBox)radPanelBar_newMilestoneWindowTop_eSpecs.Items[0].FindControl("radListBoxeOnAddNewMilestoneSpecs");

            var radbutton_window_addMilestone = (RadButton)milestoneWindow.FindControl("radbutton_window_addMilestone");

            if (!radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Items.Any())
            {
                foreach (var category in this.MilestoneCategories)
                {
                    RadComboBoxItem radItem = new RadComboBoxItem();
                    radItem.Text = category.Value;
                    radItem.Value = category.Key.ToString();
                    ShowToopTipsinComboBox(radItem);
                    radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Items.Add(radItem);
                    radItem.DataBind();
                }
            }

            this.ShowWindow(
                 action: UiAction.Add
               , triggerCtrlUniqueID: (sender as WebControl).UniqueID
               , targetWindow: milestoneWindow
               , dataAttributes: new Dictionary<WebControl, string[]>()
               {
                 { this.scheduleControl_panel_mainContentPanel,
                   new string[] { C_Product_Key, C_Product_Name }
                 },

                   { this.GetControlFromUniqueID((sender as WebControl).UniqueID,"radpanelbar_releases_root"),
                   new string[] { C_Product_Key, C_Release_Key, C_Release_Name }
                 },
               }
               , updateWindowFunction: (window) =>
               {
                   radTextBox_mileStoneName_OnMilestoneWindowTop.Text = "";
                   radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Text = " ";
                   radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Text = "Enter Custom Category";
                   radcombobox_MilestoneCategoryName_InMilestoneWindowTop.DataBind();
                   raddatepicker_windowTop_MilestoneStartDate.Clear();
                   raddatepicker_windowTop_MilestoneEndDate.Clear();
                   radListBoxeOnTopeSpecs.Items.Clear();

                   //delete the extra milestones
                   radPanelBar_miletoneInMilestoneWindow.Items.Clear();

                   label_window_addMilestone.Text = string.Format(@"{0} \ {1} \ Add Milestone: ", window.Attributes[C_Product_Name], window.Attributes[C_Release_Name]);
                   radbutton_window_addMilestone.Text = "Add";
               });

            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(milestoneWindow);
            updatepanel.Update();
        }

        public void radbutton_window_addMilestone_Click(object sender, EventArgs e)
        {
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl;
            var radTextBox_mileStoneName_OnMilestoneWindowTop = (RadTextBox)milestoneWindow.FindControl("radTextBox_mileStoneName_OnMilestoneWindowTop");
            var radcombobox_MilestoneCategoryName_InMilestoneWindowTop = (RadComboBox)milestoneWindow.FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindowTop");
            var raddatepicker_windowTop_MilestoneStartDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneStartDate");
            var raddatepicker_windowTop_MilestoneEndDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneEndDate");
            var radPanelBar_miletoneInMilestoneWindow = (RadPanelBar)milestoneWindow.FindControl("radPanelBar_miletoneInMilestoneWindow");
            var updateType = (UiAction)Enum.Parse(typeof(UiAction), milestoneWindow.Attributes[C_UiAction]);
            RadPanelItem currentReleaseItem = null;
            Milestone milestone = null;
            List<Milestone> milestoneList = new List<Milestone>();
            MilestoneCategory firstMilestoneCate = null;
            MilestoneCategory milestoneCate = null;
            bool success = false;

            currentReleaseItem = this.GetControlFromUniqueID(milestoneWindow.Attributes[C_Trigger_Control_UniqueID], "radpanelbar_releases_child") as RadPanelItem;
            var milestonesRoot = currentReleaseItem.FindControl("radpanelbar_milestones_root") as RadPanelBar;

            switch (updateType)
            {
                case UiAction.Add:
                    success = this.AddUpdateData(milestoneWindow,
                    (context) =>
                    {
                        milestone = new Milestone();
                        milestone.Milestone_Name = radTextBox_mileStoneName_OnMilestoneWindowTop.Text;
                        milestone.ReleaseKey = int.Parse(milestoneWindow.Attributes["ReleaseKey"]);
                        milestone.ProductKey = int.Parse(milestoneWindow.Attributes["ProductKey"]);
                        // customize item should be considered
                        if (radcombobox_MilestoneCategoryName_InMilestoneWindowTop.SelectedItem != null)
                        {
                            milestone.MilestoneCategoryKey = int.Parse(radcombobox_MilestoneCategoryName_InMilestoneWindowTop.SelectedItem.Value);
                        }
                        else
                        {
                            firstMilestoneCate = new MilestoneCategory();
                            //firstMilestoneCate the milesotne(child) to milestonecategory(parent
                            milestone.MilestoneCategory = firstMilestoneCate;
                            if (!context.MilestoneCategories.Any(c => c.Milestone_Category_Name == radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Text))
                            {
                                firstMilestoneCate.Milestone_Category_Name = radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Text;
                                context.MilestoneCategories.Add(firstMilestoneCate);
                            }
                        }
                        //get the current VSO epic and then insert especs under it
                        var vsoContext = Utils.GetVsoContext();
                        var ids = new int[] { milestone.ReleaseKey };
                        string currentEpicUrl = String.Format("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/{0}", ids[0]);
                        //get the list of eSpecs
                        var panel = radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Parent as Panel;
                        var panelbar = panel.FindControl("radPanelBar_newMilestoneWindowTop_eSpecs") as RadPanelBar;
                        var radlistbox_eSpecs = panelbar.Items[0].FindControl("radListBoxeOnAddNewMilestoneSpecs") as RadListBox;

                        //var product = context.Products.Select(c => new { c.ProductKey, c.Product_Name, c.Family }).First(c => c.ProductKey == milestone.ProductKey);
                        //code below'll use Products_New table
                        //var product = context.Products.Select(c => new { c.ProductKey, c.Product_Name, c.Family }).FirstOrDefault(c => c.ProductKey == milestone.ProductKey);
                        var product = context.Products_New.Select(c => new { c.ProductKey, c.Product_Name, c.ProductFamily.Product_Family, c.Localization_VSO_Path }).FirstOrDefault(c => c.ProductKey == milestone.ProductKey);
                        if (product != null)
                        {
                            string productName = product.Product_Name;
                            string projectName = "LOCALIZATION";
                            string family = product.Product_Family;
                            //string areaPath = string.Format("{0}\\{1}", vsoContext.GetProjectTeamDefaultArea(projectName, family), productName);
                            string areaPath = product.Localization_VSO_Path;
                            string iterationPath = "LOCALIZATION";

                            foreach (var VSOeSPecs in radlistbox_eSpecs.CheckedItems)
                            {
                                var especName = (VSOeSPecs.FindControl("radTextBox_AddNewMilestoneWindowTop_eSpecs") as RadTextBox).Text;
                                var eSpecEstimatePoints = (VSOeSPecs.FindControl("radText_AddNewMilestoneWindowTop_eSpecsEstimate") as RadTextBox).Text;

                                string vsoTag;
                                string link_VsoItem_Milestone = radcombobox_MilestoneCategoryName_InMilestoneWindowTop.SelectedItem.Text;
                                if (Utils.TryParseCategoryNameToVsoTag(link_VsoItem_Milestone, out vsoTag))
                                {
                                    link_VsoItem_Milestone = string.Format("{0}", vsoTag);
                                }

                                vsoContext.CreateVsoWorkItem
                                    (
                                        type: TaskTypes.EnablingSpecification,
                                        projectName: projectName,
                                        title: especName,
                                        areaPath: areaPath,
                                        iterationPath: iterationPath,
                                        assignedTo: "",
                                        referenceWorkItemUrl: currentEpicUrl,
                                        linkType: LinkTypes.Child,
                                        tags: new string[] { link_VsoItem_Milestone },
                                        prepareFunction: (fields) =>
                                        {
                                            //adding eSpec Estimate Points

                                            var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StoryPoints" }, { "value", eSpecEstimatePoints } };
                                            fields.Add(f_eSpecEstimate);
                                        }
                                     );
                            }

                            milestone.Milestone_Start_Date = raddatepicker_windowTop_MilestoneStartDate.SelectedDate;
                            milestone.Milestone_End_Date = raddatepicker_windowTop_MilestoneEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                            milestoneList.Add(milestone);
                            //check if multiple milestones were created,if yes, add each of them to the DB
                            if (radPanelBar_miletoneInMilestoneWindow.Items.Count != 0)
                            {
                                for (int i = 0; i < radPanelBar_miletoneInMilestoneWindow.Items.Count; i++)
                                {
                                    Milestone extraMilestone = new Milestone();
                                    var milestoneName = radPanelBar_miletoneInMilestoneWindow.Items[i].FindControl("radTextBox_mileStoneName_InMilestoneWindow") as RadTextBox;
                                    var milestoneCategory = radPanelBar_miletoneInMilestoneWindow.Items[i].FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindow") as RadComboBox;
                                    var milestoneStartDate = radPanelBar_miletoneInMilestoneWindow.Items[i].FindControl("raddatepicker_MilestoneStartDate") as RadDatePicker;
                                    var milestoneEndDate = radPanelBar_miletoneInMilestoneWindow.Items[i].FindControl("raddatepicker_MilestoneEndDate") as RadDatePicker;
                                    //get the list of eSpecs
                                    var radPanelBar_addMilestoneWindowLowerPart_eSpecs = radPanelBar_miletoneInMilestoneWindow.Items[i].FindControl("radPanelBar_newMilestoneWindowLowerPart_eSpecs") as RadPanelBar;
                                    var radListBox_lower_eSpecs = radPanelBar_addMilestoneWindowLowerPart_eSpecs.Items[0].FindControl("radListBoxeOnAddNewMilestoneLowerPartSpecs") as RadListBox;

                                    foreach (var VSOeSPecs in radListBox_lower_eSpecs.CheckedItems)
                                    {
                                        var extraEspecName = (VSOeSPecs.FindControl("radTextBox_AddNewMilestoneWindowLowerPart_eSpecs") as RadTextBox).Text;
                                        var eSpecEstimatePoints = (VSOeSPecs.FindControl("radText_AddNewMilestoneWindowLowerPart_eSpecsEstimate") as RadTextBox).Text;

                                        string vsoTag;
                                        string link_VsoItem_Milestone = milestoneCategory.SelectedItem.Text;
                                        if (Utils.TryParseCategoryNameToVsoTag(link_VsoItem_Milestone, out vsoTag))
                                        {
                                            link_VsoItem_Milestone = string.Format("{0}", vsoTag);
                                        }

                                        vsoContext.CreateVsoWorkItem
                                            (
                                                type: TaskTypes.EnablingSpecification,
                                                projectName: projectName,
                                                //title: VSOeSPecs.Value,
                                                title: extraEspecName,
                                                areaPath: areaPath,
                                                iterationPath: iterationPath,
                                                assignedTo: "",
                                                referenceWorkItemUrl: currentEpicUrl,
                                                linkType: LinkTypes.Child,
                                                tags: new string[] { link_VsoItem_Milestone },
                                                prepareFunction: (fields) =>
                                                {
                                                    //adding eSpec Estimate Points
                                                    var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StoryPoints" }, { "value", eSpecEstimatePoints } };
                                                    fields.Add(f_eSpecEstimate);
                                                }
                                            );
                                    }

                                    extraMilestone.ReleaseKey = int.Parse(milestoneWindow.Attributes["ReleaseKey"]);
                                    extraMilestone.ProductKey = int.Parse(milestoneWindow.Attributes["ProductKey"]);

                                    extraMilestone.Milestone_Name = milestoneName.Text;

                                    if (milestoneCategory.SelectedItem != null)
                                    {
                                        extraMilestone.MilestoneCategoryKey = int.Parse(milestoneCategory.SelectedItem.Value);
                                    }
                                    else
                                    {
                                        milestoneCate = new MilestoneCategory();
                                        //connect the milesotne(child) to milestonecategory(parent)
                                        extraMilestone.MilestoneCategory = milestoneCate;
                                        if (!context.MilestoneCategories.Any(c => c.Milestone_Category_Name == milestoneCategory.Text))
                                        {
                                            milestoneCate.Milestone_Category_Name = milestoneCategory.Text;
                                            context.MilestoneCategories.Add(milestoneCate);
                                        }
                                    }

                                    extraMilestone.Milestone_Start_Date = milestoneStartDate.SelectedDate.Value;
                                    extraMilestone.Milestone_End_Date = milestoneEndDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                                    milestoneList.Add(extraMilestone);
                                }
                            }
                            foreach (var milestoneItem in milestoneList)
                            {
                                context.Milestones.Add(milestoneItem);
                            }
                            context.SaveChanges();
                        }
                    });

                    if (success)
                    {
                        //update milestone nodes
                        foreach (var mItem in milestoneList)
                        {
                            var milestoneItem = new RadPanelItem();
                            milestoneItem.Attributes[C_Milestone_Key] = mItem.MilestoneKey.ToString();
                            milestoneItem.Attributes[C_Milestone_Name] = mItem.Milestone_Name.ToString();
                            milestonesRoot.Items.Add(milestoneItem);
                            var label_milestoneName = milestoneItem.FindControl("label_milestoneName") as Label;
                            label_milestoneName.Text = mItem.Milestone_Name;

                            var link_VsoItem_Milestone = milestoneItem.FindControl("link_VsoItem_Milestone") as HyperLink;
                            string vsoTag;
                            if (Utils.TryParseCategoryNameToVsoTag(this.MilestoneCategories[mItem.MilestoneCategoryKey], out vsoTag))
                            {
                                link_VsoItem_Milestone.Text = string.Format("{0}", vsoTag);
                                link_VsoItem_Milestone.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(milestone.ReleaseKey, vsoTag, VSO_Project, scheduleControl_panel_mainContentPanel.Attributes[C_Product_Family]);
                            }
                            else
                            {
                                var label_VsoQuery = milestoneItem.FindControl("label_VsoQuery") as Label;
                                label_VsoQuery.Visible = false;
                                link_VsoItem_Milestone.Visible = false;
                            }

                            // choose the selected MilestoneCategory Name from the comboBox for original or customized value
                            // firstly polulate the combobox with existing data from DB
                            var milestoneCategoryName = milestoneItem.FindControl("radcombobox_milestone_categoriesOnControl") as RadComboBox;
                            foreach (var category in this.MilestoneCategories)
                            {
                                RadComboBoxItem radItem = new RadComboBoxItem();
                                radItem.Text = category.Value;
                                radItem.Value = category.Key.ToString();
                                ShowToopTipsinComboBox(radItem);

                                milestoneCategoryName.Items.Add(radItem);
                                if (category.Key == mItem.MilestoneCategoryKey)
                                {
                                    radItem.Selected = true;
                                }
                                radItem.DataBind();
                            };
                            if (mItem.MilestoneCategory != null)
                            {
                                if (!this.MilestoneCategories.Any(c => c.Value == mItem.MilestoneCategory.Milestone_Category_Name))
                                {
                                    RadComboBoxItem radItem = new RadComboBoxItem();
                                    radItem.Text = mItem.MilestoneCategory.Milestone_Category_Name;
                                    radItem.Value = mItem.MilestoneCategory.MilestoneCategoryKey.ToString();
                                    milestoneCategoryName.Items.Add(radItem);
                                }

                                //secondly polulate the combobox with the customized data
                                var radcombobox_UI_addMinestoneCategoryName = milestoneItem.FindControl("radcombobox_milestone_categoriesOnControl") as RadComboBox;

                                RadComboBoxItem item = radcombobox_UI_addMinestoneCategoryName.FindItemByText(mItem.MilestoneCategory.Milestone_Category_Name);
                                item.Selected = true;
                            }

                            var raddatepicker_milestone_from = milestoneItem.FindControl("raddatepicker_milestone_from") as RadDatePicker;
                            raddatepicker_milestone_from.SharedCalendar = this.GetCalendar();

                            var raddatepicker_milestone_to = milestoneItem.FindControl("raddatepicker_milestone_to") as RadDatePicker;
                            raddatepicker_milestone_to.SharedCalendar = this.GetCalendar();

                            raddatepicker_milestone_from.SelectedDate = mItem.Milestone_Start_Date.Value;
                            raddatepicker_milestone_to.SelectedDate = mItem.Milestone_End_Date.Value;
                            milestonesRoot.DataBind();
                        }
                    }
                    break;

                default:
                    break;
            }
            this.updatePanel_scheduleControl.Update();
        }

        protected void radButton_MlestoneWindow_deleteMilestone_Click(object sender, EventArgs e)
        {
            var milestoneDeleteButton = sender as RadButton;
            var currentItem = this.GetControlFromUniqueID(milestoneDeleteButton.UniqueID, "radPanelBar_miletoneInMilestoneWindow") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);
            var updatePanel = this.GetFirstParentOfType<UpdatePanel>(currentItem.PanelBar);
            updatePanel.Update();
        }

        protected void radButton_addMilestoneInMilestoneWindow_Click(object sender, EventArgs e)
        {
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl;
            var radPanelBar_miletoneInMilestoneWindow = (RadPanelBar)milestoneWindow.FindControl("radPanelBar_miletoneInMilestoneWindow");

            var radpanelItem = new RadPanelItem();
            radPanelBar_miletoneInMilestoneWindow.Items.Add(radpanelItem);

            //find the Radcombobox in the itemtemplate/radpanel
            var radcombobox_milestone_categoriesNameInMilestoneWindow = radpanelItem.FindControl("radcombobox_MilestoneCategoryName_InMilestoneWindow") as RadComboBox;
            foreach (var category in this.MilestoneCategories)
            {
                RadComboBoxItem radItem = new RadComboBoxItem();
                radItem.Text = category.Value;
                radItem.Value = category.Key.ToString();
                ShowToopTipsinComboBox(radItem);
                radcombobox_milestone_categoriesNameInMilestoneWindow.Items.Add(radItem);
                radItem.DataBind();
            }
            radPanelBar_miletoneInMilestoneWindow.DataBind();

            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(radPanelBar_miletoneInMilestoneWindow);
            updatepanel.Update();
        }

        #endregion AddUpdate-Milestone

        #region Link to VSO Item

        protected void link_VsoItem_Click(object sender, EventArgs e)
        {
        }

        #endregion Link to VSO Item

        #region Delete-'X' - Release/Milestone/TestSchedule

        protected void radButton_MilestoneDelete_Click(object sender, EventArgs e)
        {
            var deleteMilestoneWarningWindow = this.GetWindowItem(this.treeview_deleteMilestoneWarning, "radwindow_deleteMilestone_Warning") as WebControl;
            var milestoneDeleteButton = sender as RadButton;
            var deleteMilestoneRadPanelItem = this.GetControlFromUniqueID(milestoneDeleteButton.UniqueID, "radpanelbar_milestones_root") as RadPanelItem;

            this.ShowWindow(
                                action: UiAction.Delete
                              , triggerCtrlUniqueID: deleteMilestoneRadPanelItem.UniqueID
                              , targetWindow: deleteMilestoneWarningWindow
                              , dataAttributes: new Dictionary<WebControl, string[]>()
                                        {
                                             { this.GetControlFromUniqueID(deleteMilestoneRadPanelItem.UniqueID,"radpanelbar_milestones_root")
                                                 , new string[] {C_Milestone_Key}
                                             }
                                        }
                              , updateWindowFunction: (window) =>
                              {
                                  var label_deleteMilestoneWarning = (Label)deleteMilestoneWarningWindow.FindControl("label_deleteMilestoneWarning");
                                  label_deleteMilestoneWarning.Text = "Are you sure you want to delete it?";
                              });

            //var updatepanel = this.GetFirstParentOfType<UpdatePanel>(releaseWindow);
            //updatepanel.Update();
            this.updatePanel_deleteMilestoneWarning.Update();
        }

        protected void radButton_OK_deleteMilestoneWarning_Click(object sender, EventArgs e)
        {
            var deleteMilestoneWarningWindow = this.GetWindowItem(this.treeview_deleteMilestoneWarning, "radwindow_deleteMilestone_Warning") as WebControl;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(deleteMilestoneWarningWindow.Attributes[C_Trigger_Control_UniqueID]) as RadPanelItem;
                int milestoneid = int.Parse(deleteMilestoneWarningWindow.Attributes[C_Milestone_Key]);
                Milestone milestone = context.Milestones.First(m => m.MilestoneKey == milestoneid);

                context.Milestones.Remove(milestone);
                context.SaveChanges();

                // update(refresh) milestone node
                item.PanelBar.Items.Remove(item);
            }
            this.updatePanel_scheduleControl.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", deleteMilestoneWarningWindow.ClientID), true);
        }

        protected void radButton_TestScheduleDelete_Click(object sender, EventArgs e)
        {
            var deleteTestWarningWindow = this.GetWindowItem(this.treeview_deleteTestWarning, "radwindow_deleteTest_Warning") as WebControl;
            var testDeleteButton = sender as RadButton;

            var deleteTestRadPanelItem = this.GetControlFromUniqueID(testDeleteButton.UniqueID, "radpanelbar_testSchedules_root") as RadPanelItem;

            this.ShowWindow(
                                action: UiAction.Delete
                              , triggerCtrlUniqueID: deleteTestRadPanelItem.UniqueID
                              , targetWindow: deleteTestWarningWindow
                              , dataAttributes: new Dictionary<WebControl, string[]>()
                                        {
                                             { this.GetControlFromUniqueID(deleteTestRadPanelItem.UniqueID,"radpanelbar_testSchedules_root")
                                                 , new string[] {C_TestSchedule_Key}
                                             }
                                        }
                              , updateWindowFunction: (window) =>
                              {
                                  var label_deleteTestWarning = (Label)deleteTestWarningWindow.FindControl("label_deleteTestWarning");
                                  label_deleteTestWarning.Text = "Are you sure you want to delete it?";
                              });

            this.updaterPanel_deleteTestWarning.Update();
        }

        protected void radButton_OK_deleteTestWarning_Click(object sender, EventArgs e)
        {
            var deleteTestWarningWindow = this.GetWindowItem(this.treeview_deleteTestWarning, "radwindow_deleteTest_Warning") as WebControl;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(deleteTestWarningWindow.Attributes[C_Trigger_Control_UniqueID]) as RadPanelItem;
                int testid = int.Parse(deleteTestWarningWindow.Attributes[C_TestSchedule_Key]);

                //update vso
                var vsoContext = Utils.GetVsoContext();

                var fields = new Dictionary<string, string> { { "System.State", "Inactive" }, { "System.IterationPath", @"LOCALIZATION\_Trash_" }, { "System.AreaPath", @"LOCALIZATION\_Trash_" } };
                //var fields = new Dictionary<string, string> { {  "System.IterationPath", @"LOCALIZATION\_Trash_" }, { "System.AreaPath", @"LOCALIZATION\_Trash_" } };
                var closedItem = vsoContext.UpdateVsoWorkItem(testid, fields);

                //update db
                TestSchedule testSchedule = context.TestSchedules.First(t => t.TestScheduleKey == testid);
                testSchedule.Deleted = true;
                //context.TestSchedules.Remove(testSchedule);
                context.SaveChanges();

                item.PanelBar.Items.Remove(item);
            }
            this.updatePanel_scheduleControl.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", deleteTestWarningWindow.ClientID), true);
        }

        protected void radButton_ReleaseDelete_Click(object sender, EventArgs e)
        {
            var deleteReleaseWarningWindow = this.GetWindowItem(this.treeview_deleteReleaseWarning, "radwindow_deleteRelease_Warning") as WebControl;
            var releaseDeleteButton = sender as RadButton;
            var deleteReleaseRadPanelItem = this.GetControlFromUniqueID(releaseDeleteButton.UniqueID, "radpanelbar_releases_root") as RadPanelItem;

            this.ShowWindow(
                                action: UiAction.Delete
                              , triggerCtrlUniqueID: deleteReleaseRadPanelItem.UniqueID
                              , targetWindow: deleteReleaseWarningWindow
                              , dataAttributes: new Dictionary<WebControl, string[]>()
                                        {
                                             { this.GetControlFromUniqueID(deleteReleaseRadPanelItem.UniqueID,"radpanelbar_releases_root")
                                                 , new string[] {C_Release_Key}
                                             }
                                        }
                              , updateWindowFunction: (window) =>
                              {
                                  var label_deleteReleaseWarning = (Label)deleteReleaseWarningWindow.FindControl("label_deleteReleaseWarning");
                                  label_deleteReleaseWarning.Text = "Are you sure you want to delete it?";
                              });

            this.updatePanel_deleteReleaseWarning.Update();
        }

        protected void radButton_TestScheduleWindow_deleteTestSchedule_Click(object sender, EventArgs e)
        {
            var testDeleteButton = sender as RadButton;

            var currentItem = this.GetControlFromUniqueID(testDeleteButton.UniqueID, "radPanelBar_testScheduleInTestScheduleWindow") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);

            var updatePanel = this.GetFirstParentOfType<UpdatePanel>(currentItem.PanelBar);
            updatePanel.Update();
        }

        protected void radButton_OK_deleteReleaseWarning_Click(object sender, EventArgs e)
        {
            var deleteReleaseWarningWindow = this.GetWindowItem(this.treeview_deleteReleaseWarning, "radwindow_deleteRelease_Warning") as WebControl;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(deleteReleaseWarningWindow.Attributes[C_Trigger_Control_UniqueID]) as RadPanelItem;
                int releaseid = int.Parse(deleteReleaseWarningWindow.Attributes[C_Release_Key]);

                //update vso
                var vsoContext = Utils.GetVsoContext();
                List<int> intList = new List<int> { releaseid };
                List<string> stateList = new List<string> { "System.State" };
                var vsoEpic = vsoContext.GetListOfWorkItemsByIDs(intList, stateList);
                //check  epic's state
                JObject fields = (JObject)vsoEpic["value"][0]["fields"];
                string systemState = Utils.GetValue<string>(fields, "System.State");

                if (systemState != "Closed")
                {
                    var fieldsOne = new Dictionary<string, string> { { "System.State", "Closed" }, { "System.IterationPath", @"LOCALIZATION\_Trash_" }, { "System.AreaPath", @"LOCALIZATION\_Trash_" } };
                    var closedItem = vsoContext.UpdateVsoWorkItem(releaseid, fieldsOne);
                    var fieldsTwo = new Dictionary<string, string> { { "Microsoft.VSTS.Common.ResolvedReason", "Withdrawn" } };
                    var updatedItem = vsoContext.UpdateVsoWorkItem(releaseid, fieldsTwo);
                }

                //update db
                Release release = context.Releases.First(r => r.VSO_ID == releaseid);
                release.Deleted = true;
                context.SaveChanges();

                // update(refresh) release node
                item.PanelBar.Items.Remove(item);
            }

            this.updatePanel_scheduleControl.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", deleteReleaseWarningWindow.ClientID), true);
        }

        #endregion Delete-'X' - Release/Milestone/TestSchedule

        #region UI-element's Value Changed methods

        protected void radcombobox_milestone_categoriesOnControl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var milestoneCatagory = sender as RadComboBox;
            if (!string.IsNullOrWhiteSpace(e.Value))
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var item = this.GetControlFromUniqueID(milestoneCatagory.UniqueID, "radpanelbar_milestones_root");
                    int milestoneid = int.Parse(item.Attributes[C_Milestone_Key]);
                    var milestone = context.Milestones.First(m => m.MilestoneKey == milestoneid);
                    milestone.MilestoneCategoryKey = int.Parse(e.Value);

                    var link_VsoItem_Milestone = milestoneCatagory.Parent.FindControl("link_VsoItem_Milestone") as HyperLink;
                    //convert it to vso-tag
                    string vsoTag;
                    if (Utils.TryParseCategoryNameToVsoTag(milestoneCatagory.Text, out vsoTag))
                    {
                        link_VsoItem_Milestone.Text = vsoTag;
                        //int epicID = int.Parse(item.Attributes[C_Release_Key]);
                        int epicID = milestone.ReleaseKey;
                        link_VsoItem_Milestone.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(epicID, vsoTag, VSO_Project, scheduleControl_panel_mainContentPanel.Attributes[C_Product_Family]);
                        this.updatePanel_scheduleControl.Update();
                    }
                    else
                    {
                        var vsoItemHyperlink = item.FindControl("link_VsoItem_Milestone") as HyperLink;
                        var label_VsoQuery = item.FindControl("label_VsoQuery") as Label;
                        label_VsoQuery.Visible = false;
                        link_VsoItem_Milestone.Visible = false;
                    }

                    context.SaveChanges();
                }
            }
        }

        protected void raddatepicker_milestone_from_InWindow_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var rad_DatePicker = sender as RadDatePicker;
            var currentItem = this.GetControlFromUniqueID(rad_DatePicker.UniqueID, "radPanelBar_miletoneInReleaseWindow") as RadPanelItem;
            var rad_startDatePicker = currentItem.FindControl("raddatepicker_milestone_from_InWindow") as RadDatePicker;
            var rad_toDatePicker = currentItem.FindControl("raddatepicker_milestone_to_InWindow") as RadDatePicker;
            //fill out the end date of milestone
            rad_toDatePicker.SelectedDate = rad_startDatePicker.SelectedDate.Value;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", "modal_addingRelease"), true);
        }

        protected void raddatepicker_MilestoneStartDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var rad_DatePicker = sender as RadDatePicker;
            var currentItem = this.GetControlFromUniqueID(rad_DatePicker.UniqueID, "radPanelBar_miletoneInMilestoneWindow") as RadPanelItem;
            var rad_startDatePicker = currentItem.FindControl("raddatepicker_MilestoneStartDate") as RadDatePicker;
            var rad_toDatePicker = currentItem.FindControl("raddatepicker_MilestoneEndDate") as RadDatePicker;
            //fill out the end date of milestone
            rad_toDatePicker.SelectedDate = rad_startDatePicker.SelectedDate.Value;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", "modal_addingMilestone"), true);
        }

        protected void raddatepicker_MilestoneEndDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", "modal_addingMilestone"), true);
        }

        protected void raddatepicker_windowTop_MilestoneStartDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl; ;
            var raddatepicker_windowTop_MilestoneStartDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneStartDate");
            var raddatepicker_windowTop_MilestoneEndDate = (RadDatePicker)milestoneWindow.FindControl("raddatepicker_windowTop_MilestoneEndDate");

            raddatepicker_windowTop_MilestoneEndDate.SelectedDate = raddatepicker_windowTop_MilestoneStartDate.SelectedDate.Value;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RefreshWindow", string.Format("RefreshWindow('{0}');", "modal_addingMilestone"), true);
        }

        protected void raddatepicker_milestone_from_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_milestones_root");
                int milestoneid = int.Parse(item.Attributes[C_Milestone_Key]);
                var milestone = context.Milestones.First(m => m.MilestoneKey == milestoneid);

                if (e.NewDate != null && milestone.Milestone_End_Date >= e.NewDate)
                {
                    milestone.Milestone_Start_Date = e.NewDate;
                    context.SaveChanges();
                }
                else
                {
                    datepicker.SelectedDate = milestone.Milestone_Start_Date;
                }
            }
        }

        protected void raddatepicker_milestone_to_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_milestones_root");
                int milestoneid = int.Parse(item.Attributes[C_Milestone_Key]);
                var milestone = context.Milestones.First(m => m.MilestoneKey == milestoneid);

                if (e.NewDate != null && milestone.Milestone_Start_Date <= e.NewDate)
                {
                    milestone.Milestone_End_Date = e.NewDate.Value.AddDays(1).AddSeconds(-1);
                    context.SaveChanges();
                }
                else
                {
                    datepicker.SelectedDate = milestone.Milestone_End_Date;
                }
            }
        }

        protected void raddatepicker_Release_from_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_releases_root");
                int releaseID = int.Parse(item.Attributes[C_Release_Key]);
                var release = context.Releases.First(r => r.VSO_ID == releaseID);

                if (e.NewDate != null && release.VSO_DueDate >= e.NewDate)
                {
                    release.VSO_LocStartDate = e.NewDate.Value;
                    context.SaveChanges();
                }
                else
                {
                    datepicker.SelectedDate = release.VSO_LocStartDate;
                }
            }
        }

        protected void raddatepicker_Release_to_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_releases_root");
                int releaseID = int.Parse(item.Attributes[C_Release_Key]);
                var release = context.Releases.First(r => r.VSO_ID == releaseID);

                if (e.NewDate != null && release.VSO_LocStartDate <= e.NewDate)
                {
                    release.VSO_DueDate = e.NewDate.Value;
                    context.SaveChanges();
                }
                else
                {
                    datepicker.SelectedDate = release.VSO_DueDate;
                }
            }
        }

        #endregion UI-element's Value Changed methods

        protected void radbutton_UpdateMilestoneName_Update_Click(object sender, EventArgs e)
        {
            RadPanelItem currentReleaseItem = null;
            Milestone milestone = null;
            bool success = false;

            var milestoneWindow = this.GetWindowItem(this.treeview_updateMilestone, "radwindow_MilestoneNameUpdate") as WebControl;

            currentReleaseItem = this.GetControlFromUniqueID(milestoneWindow.Attributes[C_Trigger_Control_UniqueID], "radpanelbar_releases_child") as RadPanelItem;
            var milestonesRoot = currentReleaseItem.FindControl("radpanelbar_milestones_root") as RadPanelBar;

            success = this.AddUpdateData(milestoneWindow,
                        (context) =>
                        {
                            var radTextBox_MilestoneNameUpdate = (RadTextBox)milestoneWindow.FindControl("radTextBox_MilestoneNameUpdate");
                            milestone = new Milestone();
                            milestone.Milestone_Name = radTextBox_MilestoneNameUpdate.Text;
                            milestone.MilestoneKey = int.Parse(milestoneWindow.Attributes["MilestoneKey"]);

                            //update VSO mielstone
                            //var vsoContext = Utils.GetVsoContext();

                            //var updatedMilestone = vsoContext.UpdateVsoWorkItem(
                            //                id: milestone.MilestoneKey,
                            //                fields: new Dictionary<string, string>{
                            //                        {"System.Title",milestone.Milestone_Name}
                            //});

                            //update database
                            var milestoneToUpdate = context.Milestones.FirstOrDefault(c => c.MilestoneKey == milestone.MilestoneKey);
                            milestoneToUpdate.Milestone_Name = milestone.Milestone_Name;

                            context.SaveChanges();
                        });

            if (success)
            {
                //update milestone node
                var milestonePanelItem = this.GetChildrenOfType<RadPanelItem>(milestonesRoot).Where(c => c.UniqueID == milestoneWindow.Attributes[C_Trigger_Control_UniqueID]).First();
                var label_milestoneName = milestonePanelItem.FindControl("label_milestoneName") as Label;
                label_milestoneName.Text = milestone.Milestone_Name;

                milestonesRoot.DataBind();
                this.updatePanel_scheduleControl.Update();
            }
        }

        protected void radcombobox_milestone_categoriesInWindow_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get the current item
            var radCombobox_categoryName = sender as RadComboBox;
            var currentItem = this.GetControlFromUniqueID(radCombobox_categoryName.UniqueID, "radPanelBar_miletoneInReleaseWindow") as RadPanelItem;
            string selectedMilestoneCategory = e.Text;
            FillEnablingSpecificationList(currentItem, selectedMilestoneCategory);
        }

        protected void FillEnablingSpecificationList(RadPanelItem currentItem, string selectedMilestoneCategory)
        {
            var radPanelBar_eSpecs_child = currentItem.FindControl("radPanelBar_eSpecs_child") as RadPanelBar;
            var radlistbox_eSpecs = radPanelBar_eSpecs_child.Items[0].FindControl("radListBoxeSpecs") as RadListBox;
            radlistbox_eSpecs.Items.Clear();

            //get the product name
            var radwindow_addRelease = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease") as Panel;
            var productName = radwindow_addRelease.Attributes["ProductName"];

            string radTextBoxName_eSpecs = "radTextBoxeSpecs";
            //populate radlsitbox with data
            populateRadListBox_EspecsWithData(productName, selectedMilestoneCategory, radlistbox_eSpecs, radTextBoxName_eSpecs);
            //update the window
            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(currentItem.PanelBar);
            updatepanel.Update();
        }

        protected void radcombobox_MilestoneCategoryName_InMilestoneWindowTop_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get the current radlistbox
            var radcombobox_MilestoneCategoryName_InMilestoneWindowTop = sender as RadComboBox;
            var panel = radcombobox_MilestoneCategoryName_InMilestoneWindowTop.Parent as Panel;
            var panelbar = panel.FindControl("radPanelBar_newMilestoneWindowTop_eSpecs") as RadPanelBar;
            var radlistbox_eSpecs = panelbar.Items[0].FindControl("radListBoxeOnAddNewMilestoneSpecs") as RadListBox;
            radlistbox_eSpecs.Items.Clear();
            //get the product name
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl;
            var productName = milestoneWindow.Attributes["ProductName"];
            string selectedMilestoneCategory = e.Text;
            //populate radlsitbox with data
            string radTextBoxName_eSpecs = "radTextBox_AddNewMilestoneWindowTop_eSpecs";
            populateRadListBox_EspecsWithData(productName, selectedMilestoneCategory, radlistbox_eSpecs, radTextBoxName_eSpecs);
            //update the window
            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(panelbar);
            updatepanel.Update();
        }

        protected void radcombobox_MilestoneCategoryName_InMilestoneWindowLowerPart_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get current control
            var currentCombobox_eSpecs = sender as RadComboBox;
            var currentItem = this.GetControlFromUniqueID(currentCombobox_eSpecs.UniqueID, "radPanelBar_miletoneInMilestoneWindow") as RadPanelItem;
            var radPanelBar_addMilestoneWindowLowerPart_eSpecs = currentItem.FindControl("radPanelBar_newMilestoneWindowLowerPart_eSpecs") as RadPanelBar;
            var radListBox_eSpecs = radPanelBar_addMilestoneWindowLowerPart_eSpecs.Items[0].FindControl("radListBoxeOnAddNewMilestoneLowerPartSpecs") as RadListBox;
            radListBox_eSpecs.Items.Clear();
            //get the product name
            var milestoneWindow = this.GetWindowItem(this.treeview_addMilestone, "radwindow_addMilestone") as WebControl;
            var productName = milestoneWindow.Attributes["ProductName"];
            string selectedMilestoneCategory = e.Text;
            string radTextBoxName_eSpecs = "radTextBox_AddNewMilestoneWindowLowerPart_eSpecs";
            populateRadListBox_EspecsWithData(productName, selectedMilestoneCategory, radListBox_eSpecs, radTextBoxName_eSpecs);

            var updatepanel = this.GetFirstParentOfType<UpdatePanel>(currentItem.PanelBar);
            updatepanel.Update();
        }

        protected void populateRadListBox_EspecsWithData(string productName, string selectedMilestoneCategory, RadListBox radListBox_Especs, string radtextboxName_eSpecs)
        {
            //fill different data based on the selected item
            List<string> eSpecsList = new List<string>();
            switch (selectedMilestoneCategory)
            {
                case "locready":
                    //fill radListBox with specific data
                    eSpecsList.Add(String.Format("{0}: Prepare loc-infra(unfreeze,onboard,...)", productName));
                    eSpecsList.Add(String.Format("{0}: Prepare English screenshots", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;

                case "locstart":
                    eSpecsList.Add(String.Format("{0}: Loc Kick-Off request to LSPs", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;

                case "progressing":
                    eSpecsList.Add(String.Format("{0}: Initial TestPass", productName));
                    eSpecsList.Add(String.Format("{0}: Policheck Periodic Scan", productName));
                    eSpecsList.Add(String.Format("{0}: Weekly Fabric Engineering Report", productName));
                    eSpecsList.Add(String.Format("{0}: Weekly Production Dashboard Checks", productName));
                    eSpecsList.Add(String.Format("{0}: Linguistic Review on Localized Screenshots", productName));
                    eSpecsList.Add(String.Format("{0}: GB-Certification", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;

                case "endgame":
                    eSpecsList.Add(String.Format("{0}: LSPs Fabric Endgame", productName));
                    eSpecsList.Add(String.Format("{0}: Endgame Engineering CheckPoints", productName));
                    eSpecsList.Add(String.Format("{0}: Daily Fabric checks", productName));
                    eSpecsList.Add(String.Format("{0}: Full Test-Pass", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;

                case "signoff":
                    eSpecsList.Add(String.Format("{0}: Loc Signoff Test-Pass", productName));
                    eSpecsList.Add(String.Format("{0}: Engineering Checks (LocVer, LocComplete, no ship-blocker bugs active)", productName));
                    eSpecsList.Add(String.Format(@"{0}: Policheck\Geopol signoff", productName));
                    eSpecsList.Add(String.Format("{0}: GB Certification signoff", productName));
                    eSpecsList.Add(String.Format("{0}: UA Content ready ", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;

                case "retro":
                    eSpecsList.Add(String.Format("{0}: Loc Retro", productName));
                    eSpecsList.Add(String.Format(@"{0}: Freeze\block project", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    eSpecsList.Add(String.Format("{0}: enter title here", productName));
                    break;
            }

            for (int i = 0; i < eSpecsList.Count; i++)
            {
                RadListBoxItem eSpecsItem = new RadListBoxItem();
                radListBox_Especs.Items.Add(eSpecsItem);

                //get the textbox
                var radTextBox_eSpecs = radListBox_Especs.Items[i].FindControl(radtextboxName_eSpecs) as RadTextBox;
                radTextBox_eSpecs.Text = eSpecsList[i];
                eSpecsItem.Value = radTextBox_eSpecs.Text;
            }
        }

        protected void RadAutoCompleteBox_existingRelease_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            //clean old data
            var releaseWindow = this.GetWindowItem(this.treeview_addRelease, "radwindow_addRelease") as WebControl;
            var releaseNameRadTextBox = (RadTextBox)releaseWindow.FindControl("radtextbox_ReleaseName");
            var raddatepicker_ReleaseStartDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseStartDate");
            var raddatepicker_ReleaseEndDate = (RadDatePicker)releaseWindow.FindControl("raddatepicker_ReleaseEndDate");
            releaseNameRadTextBox.Text = "";
            raddatepicker_ReleaseStartDate.Clear();
            raddatepicker_ReleaseEndDate.Clear();

            var radPanelBar_miletoneInReleaseWindow = releaseWindow.FindControl("radPanelBar_miletoneInReleaseWindow") as RadPanelBar;
            radPanelBar_miletoneInReleaseWindow.Items.Clear();

            //assign new value to controls
            var selected_RelesaeInfo = sender as RadAutoCompleteBox;
            var selected_ReleaseName = selected_RelesaeInfo.Entries[0].Text;
            var selected_ReleaseKey = Convert.ToInt32(selected_RelesaeInfo.Entries[0].Value);
            selected_RelesaeInfo.Entries.Clear();

            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                //var selectedReleaseKey = Convert.ToInt32(e.Value);
                var releaseInfo = context.Releases.First(r => r.VSO_ID == selected_ReleaseKey);
                releaseNameRadTextBox.Text = e.Entry.Text;
                raddatepicker_ReleaseStartDate.SelectedDate = releaseInfo.VSO_LocStartDate;
                raddatepicker_ReleaseEndDate.SelectedDate = releaseInfo.VSO_DueDate;
                var milestonesList = context.Milestones.Where(r => r.ReleaseKey == selected_ReleaseKey).ToList();

                //below is the code to fill the MILESTONE ROWS
                foreach (var milestone in milestonesList)
                {
                    //create milestone row
                    var currentMilestoneItem = CreateMilestonesRowsInAddReleaseWindow();
                    var milestoneCategory = milestone.MilestoneCategory;
                    //select the specified category
                    var milestoneCategoryCombobox = currentMilestoneItem.FindControl("radcombobox_milestone_categoriesInWindow") as RadComboBox;
                    foreach (RadComboBoxItem item in milestoneCategoryCombobox.Items)
                    {
                        if (item.Text == milestoneCategory.Milestone_Category_Name && item.Selected == false)
                        {
                            item.Selected = true;
                        }
                    }

                    //create eSpecs list
                    FillEnablingSpecificationList(currentMilestoneItem, milestoneCategory.Milestone_Category_Name);

                    var milestoneNameTextbox = currentMilestoneItem.FindControl("radTextBox_mileStoneName_InWindow") as RadTextBox;
                    var milestoneFromDatePicker = currentMilestoneItem.FindControl("raddatepicker_milestone_from_InWindow") as RadDatePicker;
                    var milestoneToDatePicker = currentMilestoneItem.FindControl("raddatepicker_milestone_to_InWindow") as RadDatePicker;

                    milestoneNameTextbox.Text = milestone.Milestone_Name;
                    milestoneFromDatePicker.SelectedDate = milestone.Milestone_Start_Date;
                    milestoneToDatePicker.SelectedDate = milestone.Milestone_End_Date;
                }

                var updatepanelReleaseWindow = this.GetFirstParentOfType<UpdatePanel>(radPanelBar_miletoneInReleaseWindow);
                var updatepanelAutoCompletebox = this.GetFirstParentOfType<UpdatePanel>(releaseNameRadTextBox);
                updatepanelReleaseWindow.Update();
                updatepanelAutoCompletebox.Update();
            }
        }

        protected void radcombobox_testPlan_categoriesOnControl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var milestoneCatagory = sender as RadComboBox;
            if (!string.IsNullOrWhiteSpace(e.Value))
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var item = this.GetControlFromUniqueID(milestoneCatagory.UniqueID, "radpanelbar_testSchedules_root");
                    int testPlanid = int.Parse(item.Attributes[C_TestSchedule_Key]);
                    var testPlan = context.TestSchedules.First(m => m.TestScheduleKey == testPlanid);
                    testPlan.MilestoneCategoryKey = int.Parse(e.Value);

                    var link_VsoItem_TestMilestoneCategory = milestoneCatagory.Parent.FindControl("link_VsoItem_TestMilestoneCategory") as HyperLink;
                    //convert it to vso-tag
                    string vsoTag;
                    if (Utils.TryParseCategoryNameToVsoTag(milestoneCatagory.Text, out vsoTag))
                    {
                        link_VsoItem_TestMilestoneCategory.Text = vsoTag;
                        int epicID = testPlan.ReleaseKey;
                        link_VsoItem_TestMilestoneCategory.NavigateUrl = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(epicID, vsoTag, VSO_Project, scheduleControl_panel_mainContentPanel.Attributes[C_Product_Family]);
                        this.updatePanel_scheduleControl.Update();
                    }
                    else
                    {
                        var vsoItemHyperlink = item.FindControl("link_VsoItem_TestMilestoneCategory") as HyperLink;
                        var label_VsoQuery = item.FindControl("VSOTag_testPlan") as Label;
                        label_VsoQuery.Visible = false;
                        link_VsoItem_TestMilestoneCategory.Visible = false;
                    }

                    context.SaveChanges();
                    //update VSO test
                    var vsoContext = Utils.GetVsoContext();
                    JObject testPlantagsList = vsoContext.GetListOfWorkItemsByIDs(new int[] { testPlanid }, new string[] { "System.Tags" });

                    //Dictionary<string, int> vsoTagDictionary = new Dictionary<string, int>();

                    var values = testPlantagsList["value"];
                    foreach (var element in values)
                    {
                        var fields = element["fields"] as JObject;
                        string tags = Utils.GetValue<string>(fields, "System.Tags");
                        if (!string.IsNullOrEmpty(tags))
                        {
                            tags = Utils.FilterTags(tags);
                            string[] uniqueTags = tags.Split(';');
                            //check if the list contains the catgories inside of db
                            List<string> extraVsoTags = new List<string>();
                            string vsoTagString = "";
                            foreach (string uniqueTag in uniqueTags)
                            {
                                if (Utils.CheckExtraVSOTags(uniqueTag))
                                {
                                    extraVsoTags.Add(uniqueTag);
                                    vsoTagString += uniqueTag + ";";
                                }
                            }
                            if (vsoTagString.Length != 0)
                            {
                                vsoTagString.Remove(vsoTagString.Length - 1, 1);
                            }
                            if (!extraVsoTags.Contains("Loc_TestPlan"))
                            {
                                var updatedTestPlanVsoTag = vsoContext.UpdateVsoWorkItem(
                                           id: testPlanid,
                                           fields: new Dictionary<string, string>{
                                                    {"System.Tags",string.Concat("Loc_TestPlan; ", vsoTag, vsoTagString)}
                                           });
                            }
                            else
                            {
                                var updatedTestPlanVsoTag = vsoContext.UpdateVsoWorkItem(
                                              id: testPlanid,
                                              fields: new Dictionary<string, string>{
                                                    {"System.Tags",string.Concat(vsoTag+";",vsoTagString)}
                                           });
                            }
                        }
                    }
                }
            }
        }

        protected void raddatepicker_testSchedule_from_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_testSchedules_root");
                int testPlanId = int.Parse(item.Attributes[C_TestSchedule_Key]);
                var testPlan = context.TestSchedules.First(t => t.TestScheduleKey == testPlanId);
                string startDate = "";

                if (e.NewDate != null && testPlan.TestSchedule_End_Date >= e.NewDate)
                {
                    testPlan.TestSchedule_Start_Date = e.NewDate.Value;
                    context.SaveChanges();
                    startDate = e.NewDate.Value.ToString();
                }
                else
                {
                    datepicker.SelectedDate = testPlan.TestSchedule_Start_Date.Value;
                    startDate = testPlan.TestSchedule_Start_Date.Value.ToString();
                }
                //update VSO testplan info
                var vsoContext = Utils.GetVsoContext();

                var updatedTestPlan = vsoContext.UpdateVsoWorkItem(
                    id: testPlanId,
                    fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.StartDate", startDate}
                               });
            }
        }

        protected void raddatepicker_testSchedule_to_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            var datepicker = sender as RadDatePicker;
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var item = this.GetControlFromUniqueID(datepicker.UniqueID, "radpanelbar_testSchedules_root");
                int testPlanId = int.Parse(item.Attributes[C_TestSchedule_Key]);
                var testPlan = context.TestSchedules.First(t => t.TestScheduleKey == testPlanId);
                string endDate = "";

                if (e.NewDate != null && testPlan.TestSchedule_Start_Date <= e.NewDate)
                {
                    testPlan.TestSchedule_End_Date = e.NewDate.Value;
                    context.SaveChanges();
                    endDate = e.NewDate.Value.ToString();
                }
                else
                {
                    datepicker.SelectedDate = testPlan.TestSchedule_End_Date.Value;
                    endDate = testPlan.TestSchedule_End_Date.Value.ToString();
                }

                //update VSO testplan info
                var vsoContext = Utils.GetVsoContext();
                var updatedTestPlan = vsoContext.UpdateVsoWorkItem(
                    id: testPlanId,
                    fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.FinishDate", endDate}
                               });
            }
        }
    }
}
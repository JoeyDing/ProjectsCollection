using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Schedule
{
    public partial class ScheduleControl : System.Web.UI.UserControl, IScheduleView
    {
        public ProductInfo ProductInfo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //in order to use the js functions from specified file
            RegisterJavaScript();
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

        public void Refresh()
        {
        }

        public Dictionary<int, List<GetReleaseOfProduct_Result>> ReleaseOfProduct_Result
        {
            get
            {
                return ViewState["GetReleaseOfProduct_Result"] as Dictionary<int, List<GetReleaseOfProduct_Result>>;
            }
            set
            {
                ViewState["GetReleaseOfProduct_Result"] = value;
            }
        }

        public Dictionary<int, List<GetMilestoneOfRelease_Result>> MilestoneOfRelease_Result
        {
            get
            {
                return ViewState["GetMilestoneOfRelease_Result"] as Dictionary<int, List<GetMilestoneOfRelease_Result>>;
            }
            set
            {
                ViewState["GetMilestoneOfRelease_Result"] = value;
            }
        }

        public Dictionary<int, List<GetTestPlanOfRelease_Result>> TestPlanOfRelease_Result
        {
            get
            {
                return ViewState["GetTestPlanOfRelease_Result"] as Dictionary<int, List<GetTestPlanOfRelease_Result>>;
            }
            set
            {
                ViewState["GetTestPlanOfRelease_Result"] = value;
            }
        }

        public List<Products_New> ProductsList
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<int, string> SelectedPruductFromManagments { get; set; }

        public List<MilestoneCategory> MilestoneCategoryList
        {
            get
            {
                return this.ViewState["MilestoneCategory"] as List<MilestoneCategory>;
            }
            set
            {
                this.ViewState["MilestoneCategory"] = value;
            }
        }

        public int TotalReleases
        {
            get
            {
                return this.ViewState["TotalReleases"] != null ? (int)ViewState["TotalReleases"] : 0;
            }
            set
            {
                this.ViewState["TotalReleases"] = value;
            }
        }

        public int TotalTestSchedules
        {
            get
            {
                return this.ViewState["TotalTestSchedules"] != null ? (int)ViewState["TotalTestSchedules"] : 0;
            }
            set
            {
                this.ViewState["TotalTestSchedules"] = value;
            }
        }

        public int TotalMileStones
        {
            get
            {
                return this.ViewState["TotalMileStones"] != null ? (int)ViewState["TotalMileStones"] : 0;
            }
            set
            {
                this.ViewState["TotalMileStones"] = value;
            }
        }

        public bool isMultipleCancelled
        {
            get
            {
                if (Session.Contents["isMultipleCancelled"] == null)
                    Session.Contents["isMultipleCancelled"] = false;
                return (bool)Session.Contents["isMultipleCancelled"];
            }
            set
            {
                Session.Contents["isMultipleCancelled"] = value;
            }
        }

        public event Func<int, int, int, List<GetReleaseOfProduct_Result>> GetReleases;

        public event Func<int, int, int, List<GetMilestoneOfRelease_Result>> GetMileStones;

        public event Func<int, int, int, List<GetTestPlanOfRelease_Result>> GetTestPlans;

        //public event Action<Release, int, string> InsertRelease;

        public event Func<Release, int, string, int> InsertRelease;

        //public event Action<Milestone, string, List<EspecInfo>, int> InsertMilestone;
        public event Action<List<List<MilestoneInfoFromModal>>, int> InsertMilestone;

        public event Action<List<List<TestScheduleInfoFromModal>>, int> InsertTestPlan;

        public event Func<List<MilestoneCategory>> GetMilestoneCategory;

        public event Action<Release> UpdateRelease;

        public event Action<MilestoneInfoFromModal> UpdateMilestone;

        public event Action<TestSchedule> UpdateTestSchedule;

        public event Action<int> DeleteRelease;

        public event Action<int> DeleteMilestone;

        public event Action<int> DeleteTestSchedule;

        public event Func<int, int> GetTotalReleases;

        public event Func<string, string, List<string>> GetEspecList;

        public event Func<int, int> GetTotalTestSchedules;

        public event Func<int, int> GetTotalMileStones;

        public event Func<int, Release> GetReleasesByReleaseKey;

        public event Func<int, Milestone> GetMilestoneByKey;

        public event Func<string, List<string>> GetCustomTagsByReleaseTag;

        public event Func<List<int>, bool> IsProductCancelled;

        public event Func<int, List<Milestone>> GetMilestonesByReleaseKey;

        protected void RadGridSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (ReleaseOfProduct_Result == null)
                this.ReleaseOfProduct_Result = new Dictionary<int, List<GetReleaseOfProduct_Result>>();

            if (MilestoneOfRelease_Result == null)
                this.MilestoneOfRelease_Result = new Dictionary<int, List<GetMilestoneOfRelease_Result>>();

            if (TestPlanOfRelease_Result == null)
                this.TestPlanOfRelease_Result = new Dictionary<int, List<GetTestPlanOfRelease_Result>>();
            if (this.ESpecList == null)
                this.ESpecList = new List<EspecInfo>();
            //call presenter
            if (IsPostBack)
                isMultipleCancelled = false;

            if (this.SelectedPruductFromManagments != null && this.SelectedPruductFromManagments.Count > 0)
            {
                //if (this.GetTotalReleases != null)
                //    this.TotalReleases = this.GetTotalReleases(1);
                //else
                //    this.RadGridSchedule.VirtualItemCount = this.TotalReleases;
                isMultipleCancelled = this.IsProductCancelled(this.SelectedPruductFromManagments.Keys.ToList());
                var selectedProductInfo = this.SelectedPruductFromManagments.Select(x => new { ProductID = x.Key, ProductName = x.Value.ToString() }).ToList();
                this.RadGridSchedule.DataSource = selectedProductInfo;
            }
            if (this.SelectedPruductFromManagments == null || this.SelectedPruductFromManagments.Count == 0)
            {
                this.label_warning_cancelledProduct.Visible = true;
                if (isMultipleCancelled)
                    this.label_warning_cancelledProduct.Text = "Sorry, the product you select is cancelled";
                else
                    this.label_warning_cancelledProduct.Text = "Sorry, the product you select doesn't exist,please change it to another one";
            }
        }

        /// this method'll be called different times depends on how many gridtableview exist in the parent table(1 in product, 2 in relase)
        /// for example: expand product, 1st case'll be executed, if relase's expanded, 2nd and 3rd case'll be gone through seperately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGridSchedule_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "ReleaseDetails":
                    {
                        int productKey = int.Parse(dataItem.GetDataKeyValue("ProductID").ToString());

                        //paging size
                        if (this.GetTotalReleases != null)
                            e.DetailTableView.VirtualItemCount = this.GetTotalReleases(productKey);

                        int currentPageIndex = e.DetailTableView.CurrentPageIndex;
                        int pageSize = e.DetailTableView.PageSize;

                        if (this.GetReleases != null)
                            e.DetailTableView.DataSource = this.GetReleases(productKey, currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                        break;
                    }

                case "MilestoneDetails":
                    {
                        int releaseKey = int.Parse(dataItem.GetDataKeyValue("VSO_ID").ToString());

                        //paging size
                        if (this.GetTotalMileStones != null)
                            e.DetailTableView.VirtualItemCount = this.GetTotalMileStones(releaseKey);

                        int currentPageIndex = e.DetailTableView.CurrentPageIndex;
                        int pageSize = e.DetailTableView.PageSize;
                        if (this.GetMileStones != null)
                            e.DetailTableView.DataSource = this.GetMileStones(releaseKey, currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);

                        break;
                    }

                case "TestPlanDetails":
                    {
                        int releaseKey = int.Parse(dataItem.GetDataKeyValue("VSO_ID").ToString());

                        e.DetailTableView.VirtualItemCount = this.GetTotalTestSchedules(releaseKey);

                        int currentPageIndex = e.DetailTableView.CurrentPageIndex;
                        int pageSize = e.DetailTableView.PageSize;
                        e.DetailTableView.DataSource = this.GetTestPlans(releaseKey, currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                    }
                    break;
            }
        }

        protected void RadGridSchedule_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid grid = (sender as RadGrid);

            if (e.Item.OwnerTableView.Name == "TestPlanDetails")
            {
                AddCaptionForPopupModal("TestPlanDetails", "TestSchedule_Name", "Test Plan", e);
                if (e.Item != null && e.CommandName.Equals("Delete"))
                {
                    var editableItem = ((GridDataItem)e.Item);

                    int testPlanKey = int.Parse(editableItem.GetDataKeyValue("TestScheduleKey").ToString());
                    this.DeleteTestSchedule(testPlanKey);
                }
                else if (e.Item != null && e.CommandName.Equals("PerformInsert"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    //1st row from modal:

                    RadComboBox radComboBoxMilestoneCate = (RadComboBox)editableItem.FindControl("RadComboBox_MilestoneCategory_TestPlan");
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radtextbox_TestPlanID");
                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_TestScheduleStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_TestScheduleEndDate");
                    RadTextBox radtxtbxAssignedResources = (RadTextBox)editableItem.FindControl("radTextBox_AssignedResources");
                    RadTextBox radtxtbxIteration = (RadTextBox)editableItem.FindControl("radTextBox_Iteration");
                    int releaseKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("VSO_ID").ToString());
                    var releaseTable = Utils.GetFirstParentOfType<GridTableView>(e.Item.OwnerTableView);
                    int productKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());

                    List<TestScheduleInfoFromModal> firstRow_testScheduleInfoFromModal = this.GatheringAllTestPlanInfoFromModal(releaseKey, productKey, radComboBoxMilestoneCate, tbName
                                , datepickerStart, datepickerEnd, radtxtbxAssignedResources, radtxtbxIteration);

                    //extra rows from modal
                    List<List<TestScheduleInfoFromModal>> extraRow_TestScheduleInfoFromModal = new List<List<TestScheduleInfoFromModal>>();

                    RadPanelBar radPanelBarExtraTestPlans = (RadPanelBar)editableItem.FindControl("radPanelBar_extra_testPlans");
                    if (radPanelBarExtraTestPlans != null)
                    {
                        foreach (RadPanelItem panelItem in radPanelBarExtraTestPlans.Items)
                        {
                            RadComboBox radComboBox_extra_milestoneCategory = (RadComboBox)panelItem.FindControl("RadComboBox_extra_milestoneCategory");
                            RadTextBox radtextbox_extra_testPlanName = (RadTextBox)panelItem.FindControl("radtextbox_extra_testPlanName");
                            RadDatePicker raddatepicker_extra_testPlanFrom = (RadDatePicker)panelItem.FindControl("raddatepicker_extra_testPlanFrom");
                            RadDatePicker raddatepicker_extra_testPlanTo = (RadDatePicker)panelItem.FindControl("raddatepicker_extra_testPlanTo");
                            RadTextBox radTextBox_extra_assignedResource = (RadTextBox)panelItem.FindControl("radTextBox_extra_assignedResource");
                            RadTextBox radTextBox_extra_iteration = (RadTextBox)panelItem.FindControl("radTextBox_extra_iteration");

                            extraRow_TestScheduleInfoFromModal.Add(
                                this.GatheringAllTestPlanInfoFromModal(releaseKey, productKey, radComboBox_extra_milestoneCategory, radtextbox_extra_testPlanName
                                , raddatepicker_extra_testPlanFrom, raddatepicker_extra_testPlanTo, radTextBox_extra_assignedResource, radTextBox_extra_iteration));
                        }
                    }

                    //merge first test plan into extra testplans
                    extraRow_TestScheduleInfoFromModal.Add(firstRow_testScheduleInfoFromModal);
                    try
                    {
                        this.InsertTestPlan(extraRow_TestScheduleInfoFromModal, productKey);
                        //this.InsertTestPlan(testPlanItem, newMilestoneCateName, productKey, radtxtbxIteration.Text);
                        editableItem.OwnerTableView.Rebind();
                    }
                    catch (Exception)
                    {
                        // cancel insert command event and  remain values in edit mode
                        e.Canceled = true;
                        var testplanWarningMessage = (Label)editableItem.FindControl("label_testPlanIterationWarning");
                        testplanWarningMessage.Visible = true;
                    }
                }
                else if (e.Item != null && e.CommandName.Equals("Update"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    var radComboBoxMilestoneCate = (RadComboBox)editableItem.FindControl("RadComboBox_MilestoneCategory_TestPlan");
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radtextbox_TestPlanID");

                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_TestScheduleStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_TestScheduleEndDate");
                    RadTextBox txtbxAssignedResources = (RadTextBox)editableItem.FindControl("radTextBox_AssignedResources");
                    int mileStoneCateKey = (radComboBoxMilestoneCate.SelectedItem != null) ? int.Parse(radComboBoxMilestoneCate.SelectedValue) : 0;
                    var testPlanItem = new TestSchedule();
                    testPlanItem.TestScheduleKey = int.Parse(editableItem.GetDataKeyValue("TestScheduleKey").ToString());
                    testPlanItem.TestSchedule_Name = tbName.Text;
                    testPlanItem.TestSchedule_Start_Date = datepickerStart.SelectedDate.Value;
                    testPlanItem.TestSchedule_End_Date = datepickerEnd.SelectedDate.Value;
                    int assignedResources = 0;
                    Int32.TryParse(txtbxAssignedResources.Text, out assignedResources);
                    testPlanItem.AssignedResources = assignedResources;
                    testPlanItem.MilestoneCategoryKey = mileStoneCateKey;
                    this.UpdateTestSchedule(testPlanItem);
                    editableItem.OwnerTableView.Rebind();
                }
            }
            else if (e.Item.OwnerTableView.Name == "MilestoneDetails")
            {
                AddCaptionForPopupModal("MilestoneDetails", "Milestone_Name", "Milestone", e);
                if (e.Item != null && e.CommandName.Equals("Delete"))
                {
                    var editableItem = ((GridDataItem)e.Item);

                    int mileStoneKey = int.Parse(editableItem.GetDataKeyValue("MilestoneKey").ToString());
                    this.DeleteMilestone(mileStoneKey);
                }
                else if (e.Item != null && e.CommandName.Equals("PerformInsert"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    //1st row from modal:

                    RadComboBox radComboBoxMilestoneCate = (RadComboBox)editableItem.FindControl("RadComboBox_MilestoneCategory_Milestone");
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radTextBox_mileStoneName");
                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_MilestoneStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_MilestoneEndDate");
                    RadPanelBar radPanelBar_milestone_eSpecs = (RadPanelBar)editableItem.FindControl("radPanelBar_milestone_eSpecs_child");
                    RadListBox radListBoxEs = (RadListBox)radPanelBar_milestone_eSpecs.Items[0].FindControl("radListBoxeSpecs");

                    int releaseKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("VSO_ID").ToString());
                    var releaseTable = Utils.GetFirstParentOfType<GridTableView>(e.Item.OwnerTableView);
                    int productKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());

                    List<MilestoneInfoFromModal> firstRow_milestoneInfoFromModal = this.GatheringAllMilestoneInfoFromModal(releaseKey, productKey, radComboBoxMilestoneCate, tbName
                                , datepickerStart, datepickerEnd, radListBoxEs);

                    //extra rows from modal
                    List<List<MilestoneInfoFromModal>> extraRow_milestoneInfoFromModal = new List<List<MilestoneInfoFromModal>>();

                    RadPanelBar radPanelBarExtraMilestones = (RadPanelBar)editableItem.FindControl("radPanelBar_extra_miletones");
                    if (radPanelBarExtraMilestones != null)
                    {
                        foreach (RadPanelItem panelItem in radPanelBarExtraMilestones.Items)
                        {
                            RadComboBox radComboBox_extra_milestoneCate = (RadComboBox)panelItem.FindControl("radcombobox_extra_MilestoneCategoryName");
                            RadTextBox tb_extra_milestoneName = (RadTextBox)panelItem.FindControl("radTextBox_extra_mileStoneName");
                            RadDatePicker datepicker_extra_milestoneStart = (RadDatePicker)panelItem.FindControl("raddatepicker_extra_milestoneStartDate");
                            RadDatePicker datepicker_extra_milestoneEnd = (RadDatePicker)panelItem.FindControl("raddatepicker_extra_milestoneEndDate");
                            //make sure this lower es list can be found
                            RadPanelBar radPanelBar_extra_eSpecs = (RadPanelBar)panelItem.FindControl("radPanelBar_newMilestoneWindowLowerPart_eSpecs");
                            RadListBox radListBox_extra_Es = (RadListBox)radPanelBar_extra_eSpecs.Items[0].FindControl("radListBoxeOnAddNewMilestoneLowerPartSpecs");

                            extraRow_milestoneInfoFromModal.Add(
                                this.GatheringAllMilestoneInfoFromModal(releaseKey, productKey, radComboBox_extra_milestoneCate, tb_extra_milestoneName
                                , datepicker_extra_milestoneStart, datepicker_extra_milestoneEnd, radListBox_extra_Es));
                        }
                    }

                    //merge first test plan into extra milestones
                    extraRow_milestoneInfoFromModal.Add(firstRow_milestoneInfoFromModal);
                    this.InsertMilestone(extraRow_milestoneInfoFromModal, productKey);
                    editableItem.OwnerTableView.Rebind();
                }
                else if (e.Item != null && e.CommandName.Equals("Update"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    var radComboBoxMilestoneCate = (RadComboBox)editableItem.FindControl("RadComboBox_MilestoneCategory_Milestone");
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radTextBox_mileStoneName");

                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_MilestoneStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_MilestoneEndDate");

                    //get the first item of enabling specification
                    RadPanelBar radPanelBar_milestone_eSpecs = (RadPanelBar)editableItem.FindControl("radPanelBar_milestone_eSpecs_child");
                    RadListBox radListBoxEs = (RadListBox)radPanelBar_milestone_eSpecs.Items[0].FindControl("radListBoxeSpecs");

                    int releaseKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("VSO_ID").ToString());
                    var releaseTable = Utils.GetFirstParentOfType<GridTableView>(e.Item.OwnerTableView);
                    int productKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());

                    List<MilestoneInfoFromModal> firstRow_milestoneInfoFromModal = this.GatheringAllMilestoneInfoFromModal(releaseKey, productKey, radComboBoxMilestoneCate, tbName
                                , datepickerStart, datepickerEnd, radListBoxEs);


                    int mileStoneCateKey = (radComboBoxMilestoneCate.SelectedItem != null) ? int.Parse(radComboBoxMilestoneCate.SelectedValue) : 0;

                    MilestoneInfoFromModal milestoneInfoFromModal = null;
                    if (firstRow_milestoneInfoFromModal!=null)
                    {
                        //since we know at most there is only one es under one miletone(for this case)
                        milestoneInfoFromModal = firstRow_milestoneInfoFromModal.First();
                        milestoneInfoFromModal.Milestone.MilestoneKey = int.Parse(editableItem.GetDataKeyValue("MilestoneKey").ToString());
                        milestoneInfoFromModal.Milestone.Milestone_Name = tbName.Text;
                        milestoneInfoFromModal.Milestone.Milestone_Start_Date = datepickerStart.SelectedDate.Value;
                        milestoneInfoFromModal.Milestone.Milestone_End_Date = datepickerEnd.SelectedDate.Value.AddDays(1).AddSeconds(-1);
                        milestoneInfoFromModal.Milestone.MilestoneCategoryKey = mileStoneCateKey;

                        this.UpdateMilestone(milestoneInfoFromModal);
                        editableItem.OwnerTableView.Rebind();
                    }
                }
            }
            else
            {
                AddCaptionForPopupModal("ReleaseDetails", "VSO_Title", "Release", e);
                if (e.Item != null && e.CommandName.Equals("Delete"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    string sReleaseKey = editableItem.GetDataKeyValue("VSO_ID").ToString();

                    int releaseKey = int.Parse(sReleaseKey);

                    this.DeleteRelease(releaseKey);
                    editableItem.OwnerTableView.Rebind();
                }
                else if (e.Item != null && e.CommandName.Equals("PerformInsert"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radtextbox_ReleaseName");
                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseEndDate");
                    RadioButtonList vsotagRadioButtonList = (RadioButtonList)editableItem.FindControl("radioButtonList_ReleaseType");
                    HiddenField hdReleaseKey = (HiddenField)editableItem.FindControl("hdReleaseKey");
                    HiddenField hdProductKey = (HiddenField)editableItem.FindControl("hdProductKey");
                    RadPanelBar radPanelBarMiletonesInRelease = (RadPanelBar)editableItem.FindControl("radPanelBar_release_miletones");

                    string copy_releaseName = tbName.Text;
                    DateTime copy_startDate = datepickerStart.SelectedDate.Value;
                    DateTime copy_endDate = datepickerEnd.SelectedDate.Value;
                    string customTag = this.ConvertRelaseTypeToVsoTag(vsotagRadioButtonList.SelectedValue);

                    Release release = new Release();
                    release.VSO_LocStartDate = copy_startDate;
                    release.VSO_DueDate = copy_endDate;
                    release.VSO_Title = copy_releaseName;
                    string sProductId = ((GridTableView)e.Item.Parent.NamingContainer).ParentItem.GetDataKeyValue("ProductID").ToString();
                    int productKey = int.Parse(sProductId);
                    int addedReleaseKey = this.InsertRelease(release, productKey, customTag);
                    //int releaseKey = int.Parse(hdReleaseKey.Value);
                    //check if it contains any created milestones,if yes, insert milestone as well.
                    if (radPanelBarMiletonesInRelease != null && radPanelBarMiletonesInRelease.Items.Count != 0)
                    {
                        //rows from modal
                        List<List<MilestoneInfoFromModal>> milestoneInfoFromModal = new List<List<MilestoneInfoFromModal>>();

                        foreach (RadPanelItem panelItem in radPanelBarMiletonesInRelease.Items)
                        {
                            RadComboBox radComboBox_extra_milestoneCate = (RadComboBox)panelItem.FindControl("radcombobox_release_milestoenCategories");
                            RadTextBox tb_extra_milestoneName = (RadTextBox)panelItem.FindControl("radTextBox_release_mileStoneName");
                            RadDatePicker datepicker_extra_milestoneStart = (RadDatePicker)panelItem.FindControl("raddatepicker_release_milestoneFrom");
                            RadDatePicker datepicker_extra_milestoneEnd = (RadDatePicker)panelItem.FindControl("raddatepicker_release_milestoneTo");
                            //make sure this lower es list can be found
                            RadPanelBar radPanelBar_extra_eSpecs = (RadPanelBar)panelItem.FindControl("radPanelBar_release_eSpecs_child");
                            RadListBox radListBox_extra_Es = (RadListBox)radPanelBar_extra_eSpecs.Items[0].FindControl("radListBoxeSpecs_release_milestone");

                            milestoneInfoFromModal.Add(
                                this.GatheringAllMilestoneInfoFromModal(addedReleaseKey, productKey, radComboBox_extra_milestoneCate, tb_extra_milestoneName
                                , datepicker_extra_milestoneStart, datepicker_extra_milestoneEnd, radListBox_extra_Es));
                        }

                        this.InsertMilestone(milestoneInfoFromModal, productKey);
                    }

                    editableItem.OwnerTableView.Rebind();
                }
                else if (e.Item != null && e.CommandName.Equals("Update"))
                {
                    GridEditableItem editableItem = (GridEditableItem)e.Item;
                    RadTextBox tbName = (RadTextBox)editableItem.FindControl("radtextbox_ReleaseName");
                    RadDatePicker datepickerStart = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)editableItem.FindControl("raddatepicker_ReleaseEndDate");
                    RadioButtonList releaseVsoTagList = (RadioButtonList)editableItem.FindControl("radioButtonList_ReleaseType");
                    HiddenField hdReleaseKey = (HiddenField)editableItem.FindControl("hdReleaseKey");

                    string copy_releaseName = tbName.Text;
                    DateTime copy_startDate = datepickerStart.SelectedDate.Value;
                    DateTime copy_endDate = datepickerEnd.SelectedDate.Value;
                    int releaseKey = int.Parse(hdReleaseKey.Value);
                    string customTag = this.ConvertRelaseTypeToVsoTag(releaseVsoTagList.SelectedValue);
                    Release release = this.GetReleasesByReleaseKey(releaseKey);
                    if (release != null)
                    {
                        release.VSO_LocStartDate = copy_startDate;
                        release.VSO_DueDate = copy_endDate;
                        release.VSO_Title = copy_releaseName;
                        //release.VSO_Tags = string.Concat("Loc_Release; ", string.Format("Loc_ReleaseStartDate:{0}; {1}", copy_startDate.ToString("M/d/yy"), customTag));
                        release.VSO_Tags = string.Concat("Loc_Release; ", customTag);
                        this.UpdateRelease(release);
                    }
                    editableItem.OwnerTableView.Rebind();
                }
            }
        }

        private List<MilestoneInfoFromModal> GatheringAllMilestoneInfoFromModal(int releaseKey, int productKey, RadComboBox radComboBox_milestoneCategory, RadTextBox radtextbox_testPlanName, RadDatePicker raddatepicker_milestoneFrom, RadDatePicker raddatepicker_milestoneTo, RadListBox radListBox_extra_Es)
        {
            List<MilestoneInfoFromModal> milestoneInfoList = new List<MilestoneInfoFromModal>();
            Milestone milestoneItem = new Milestone();
            milestoneItem.Milestone_Name = radtextbox_testPlanName.Text;
            milestoneItem.Milestone_Start_Date = raddatepicker_milestoneFrom.SelectedDate.Value;
            milestoneItem.Milestone_End_Date = raddatepicker_milestoneTo.SelectedDate.Value;

            int mileStoneCateKey = (radComboBox_milestoneCategory.SelectedItem != null) ? int.Parse(radComboBox_milestoneCategory.SelectedValue) : 0;
            milestoneItem.MilestoneCategoryKey = mileStoneCateKey;
            milestoneItem.ReleaseKey = releaseKey;
            milestoneItem.ProductKey = productKey;
            string newMilestoneCateName = (radComboBox_milestoneCategory.SelectedItem == null) ? radComboBox_milestoneCategory.Text : null;
            this.ESpecList = new List<EspecInfo>();
            foreach (RadListBoxItem item in radListBox_extra_Es.CheckedItems)
            {
                var especName = (RadTextBox)item.FindControl("radTextBox_eSpecName");

                var especEstimate = (RadTextBox)item.FindControl("radTextBox_eSpecEstimate");
                this.ESpecList.Add(new EspecInfo { EspecName = especName.Text, EstimatedPoints = especEstimate.Text });
            }
            MilestoneInfoFromModal milestoneInfoFromModal = new MilestoneInfoFromModal();
            milestoneInfoFromModal.Milestone = milestoneItem;
            milestoneInfoFromModal.NewCatagory = newMilestoneCateName;
            milestoneInfoFromModal.eSpecList = ESpecList;
            milestoneInfoList.Add(milestoneInfoFromModal);
            return milestoneInfoList;
        }

        private List<TestScheduleInfoFromModal> GatheringAllTestPlanInfoFromModal(int releaseKey, int productKey, RadComboBox radComboBox_milestoneCategory, RadTextBox radtextbox_testPlanName, RadDatePicker raddatepicker_testPlanFrom, RadDatePicker raddatepicker_testPlanTo, RadTextBox radTextBox_assignedResource, RadTextBox radTextBox_iteration)
        {
            List<TestScheduleInfoFromModal> testPlansInfoList = new List<TestScheduleInfoFromModal>();
            TestSchedule testPlanItem = new TestSchedule();
            testPlanItem.TestSchedule_Name = radtextbox_testPlanName.Text;
            testPlanItem.TestSchedule_Start_Date = raddatepicker_testPlanFrom.SelectedDate.Value;
            testPlanItem.TestSchedule_End_Date = raddatepicker_testPlanTo.SelectedDate.Value;
            int assignedResources = 0;
            Int32.TryParse(radTextBox_assignedResource.Text, out assignedResources);
            testPlanItem.AssignedResources = assignedResources;

            int mileStoneCateKey = (radComboBox_milestoneCategory.SelectedItem != null) ? int.Parse(radComboBox_milestoneCategory.SelectedValue) : 0;
            testPlanItem.MilestoneCategoryKey = mileStoneCateKey;
            testPlanItem.ReleaseKey = releaseKey;
            testPlanItem.ProductKey = productKey;
            string newMilestoneCateName = (radComboBox_milestoneCategory.SelectedItem == null) ? radComboBox_milestoneCategory.Text : null;
            TestScheduleInfoFromModal testScheduleInfoFromModal = new TestScheduleInfoFromModal();
            testScheduleInfoFromModal.TestSchedule = testPlanItem;
            testScheduleInfoFromModal.NewCatagory = newMilestoneCateName;
            testScheduleInfoFromModal.ItearationInfo = radTextBox_iteration.Text;
            testPlansInfoList.Add(testScheduleInfoFromModal);
            return testPlansInfoList;
        }

        private void AddCaptionForPopupModal(string modalName, string boundColumnName, string itemType, GridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                var editableItem = ((GridDataItem)e.Item);
                if (modalName == "MilestoneDetails" || modalName == "TestPlanDetails")
                {
                    string itemName = editableItem[boundColumnName].Text;
                    string releaseName_milestoneOrTest = e.Item.OwnerTableView.ParentItem["VSO_Title"].Text;
                    string productName_milestoneOrTest = editableItem.OwnerTableView.ParentItem.OwnerTableView.ParentItem["ProductName"].Text;
                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} \ {1} \ {2}:", productName_milestoneOrTest, releaseName_milestoneOrTest, itemName);
                }
                else
                {
                    //release:
                    string productName_release = editableItem.OwnerTableView.ParentItem["ProductName"].Text;
                    string releaseName_release = editableItem["VSO_Title"].Text;
                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} \ {1}:", productName_release, releaseName_release);
                }
            }

            if (e.CommandName == "InitInsert")
            {
                //milestone:  //test plan:
                if (modalName == "MilestoneDetails" || modalName == "TestPlanDetails")
                {
                    string releaseName_milestoneOrTest = e.Item.OwnerTableView.ParentItem["VSO_Title"].Text;
                    string productName_milestoneOrTest = e.Item.OwnerTableView.ParentItem.OwnerTableView.ParentItem["ProductName"].Text;
                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} \ {1} \ Add New {2}:", productName_milestoneOrTest, releaseName_milestoneOrTest, itemType);
                }
                else
                {
                    //release:
                    string productName_release = e.Item.OwnerTableView.ParentItem["ProductName"].Text;
                    e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = string.Format(@"{0} \ Add New {1}:", productName_release, "Release");
                }
            }
        }

        protected void RadGridSchedule_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                //the two lines of code are used to center the rad grid popup window
                GridEditFormItem editForm = (GridEditFormItem)e.Item;
                editForm.EditFormCell.CssClass = "centerPopUpModal";

                string name = e.Item.OwnerTableView.Name;
                var data = e.Item.OwnerTableView.ParentItem as GridEditableItem;
                var item = (GridEditableItem)e.Item;

                if (name == "MilestoneDetails")
                {
                    var mailestoneList = this.MilestoneCategoryList;
                    RadComboBox radcomboBoxMilestoneCategory = null;
                    if (this.GetMilestoneCategory != null)
                    {
                        //todo: this should be removed to is postpostback
                        this.MilestoneCategoryList = this.GetMilestoneCategory();
                        radcomboBoxMilestoneCategory = (RadComboBox)item.FindControl("RadComboBox_MilestoneCategory_Milestone");
                        var milestoneCategoryList = this.MilestoneCategoryList;
                        radcomboBoxMilestoneCategory.DataSource = milestoneCategoryList;
                        radcomboBoxMilestoneCategory.DataTextField = "Milestone_Category_Name";
                        radcomboBoxMilestoneCategory.DataValueField = "MilestoneCategoryKey";
                        radcomboBoxMilestoneCategory.DataBind();

                        string milestoneCategory = DataBinder.Eval(e.Item.DataItem, "Milestone_Category_Name").ToString();
                        if (!String.IsNullOrEmpty(milestoneCategory))
                            radcomboBoxMilestoneCategory.SelectedValue = milestoneCategoryList.First(c => c.Milestone_Category_Name.Equals(milestoneCategory)).MilestoneCategoryKey.ToString();
                    }

                    RadTextBox tbName = (RadTextBox)item.FindControl("radTextBox_mileStoneName");
                    RadDatePicker datepickerStart = (RadDatePicker)item.FindControl("raddatepicker_MilestoneStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)item.FindControl("raddatepicker_MilestoneEndDate");
                    RadTextBox radTextBox_releaseName = (RadTextBox)item.FindControl("radTextBox_releaseName");
                    RadButton rbtAddNewMilestone = (RadButton)item.FindControl("radButton_addMultipleMilestones");
                    RadPanelBar radPanelBarMilestoneESpecs = (RadPanelBar)item.FindControl("radPanelBar_milestone_eSpecs_child");

                    if (item.RowIndex != -1)
                    {
                        // edit
                        rbtAddNewMilestone.Visible = false;
                        radPanelBarMilestoneESpecs.Visible = true;
                        //populate the es list based on the miletones category
                        if(radcomboBoxMilestoneCategory!=null)
                            this.GetEsInfoByMilestone(radcomboBoxMilestoneCategory);

                        //e.Item.OwnerTableView.EditFormSettings.PopUpSettings.ScrollBars = ScrollBars.None;
                        string sMilestoneKey = item.GetDataKeyValue("MilestoneKey").ToString();
                        Milestone milestone = this.GetMilestoneByKey(int.Parse(sMilestoneKey));
                        tbName.Text = milestone.Milestone_Name;
                        datepickerStart.SelectedDate = milestone.Milestone_Start_Date.Value;
                        datepickerEnd.SelectedDate = milestone.Milestone_End_Date.Value;
                    }
                    else
                    {
                        //insert
                        rbtAddNewMilestone.Visible = true;
                        if (e.Item.OwnerTableView.ParentItem != null)
                        {
                            //in order to fill front label with value, then relkease name can be passed to milestoen name when adding new milestone
                            radTextBox_releaseName.Text = e.Item.OwnerTableView.ParentItem["VSO_Title"].Text;
                        }
                    }
                }
                else if (name == "ReleaseDetails")
                {
                    RadTextBox tbName = (RadTextBox)item.FindControl("radtextbox_ReleaseName");
                    Label copyFromPreReleaseLabel = (Label)item.FindControl("CopyFromPreReleaseLabel");
                    RadAutoCompleteBox radAutoCompleteBox = (RadAutoCompleteBox)item.FindControl("RadAutoCompleteBox_existingRelease");
                    RadDatePicker datepickerStart = (RadDatePicker)item.FindControl("raddatepicker_ReleaseStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)item.FindControl("raddatepicker_ReleaseEndDate");
                    RadioButtonList releaseVsoTagList = (RadioButtonList)item.FindControl("radioButtonList_ReleaseType");
                    RadButton rbtAddNewMilestone = (RadButton)item.FindControl("radButton_release_addMultipleMilestones");
                    if (item.RowIndex != -1)
                    {
                        //edit
                        rbtAddNewMilestone.Visible = false;
                        copyFromPreReleaseLabel.Visible = false;
                        radAutoCompleteBox.Visible = false;
                        //e.Item.OwnerTableView.EditFormSettings.PopUpSettings.ScrollBars = ScrollBars.None;
                        string sReleaseKey = item.GetDataKeyValue("VSO_ID").ToString();

                        Release release = this.GetReleasesByReleaseKey(int.Parse(sReleaseKey));
                        tbName.Text = release.VSO_Title;
                        datepickerStart.SelectedDate = release.VSO_LocStartDate;
                        datepickerEnd.SelectedDate = release.VSO_DueDate;
                        if (release.VSO_Tags != null)
                        {
                            var customVsoTags = this.GetCustomTagsByReleaseTag(release.VSO_Tags);
                            foreach (ListItem tagItem in releaseVsoTagList.Items)
                            {
                                if (customVsoTags.Contains(this.ConvertRelaseTypeToVsoTag(tagItem.Value)))
                                {
                                    tagItem.Selected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //insert
                        rbtAddNewMilestone.Visible = true;
                    }
                }
                else if (name == "TestPlanDetails")
                {
                    RadTextBox tbName = (RadTextBox)item.FindControl("radtextbox_TestPlanID");
                    RadDatePicker datepickerStart = (RadDatePicker)item.FindControl("raddatepicker_TestScheduleStartDate");
                    RadDatePicker datepickerEnd = (RadDatePicker)item.FindControl("raddatepicker_TestScheduleEndDate");
                    RadTextBox txtbxAssignedResources = (RadTextBox)item.FindControl("radTextBox_AssignedResources");
                    if (item.RowIndex != -1)
                    {
                        tbName.Text = item["TestSchedule_Name"].Text;
                        datepickerStart.SelectedDate = DateTime.Parse(item["TestSchedule_Start_Date"].Text);
                        datepickerEnd.SelectedDate = DateTime.Parse(item["TestSchedule_End_Date"].Text);
                        txtbxAssignedResources.Text = item["AssignedResources"].Text.Contains("&nbsp;") ? "" : item["AssignedResources"].Text;
                    }
                    Label labelIteration = (Label)item.FindControl("label_Iteraion");
                    RadTextBox txtbxIteration = (RadTextBox)item.FindControl("radTextBox_Iteration");
                    RadButton rbtAddNewTestPlan = (RadButton)item.FindControl("radButton_addMultipleTestPlans");
                    if (e.Item.ItemIndex == -1)
                    {
                        // insert
                        labelIteration.Visible = true;
                        txtbxIteration.Visible = true;
                        rbtAddNewTestPlan.Visible = true;
                    }
                    else
                    {
                        // edit
                        labelIteration.Visible = false;
                        txtbxIteration.Visible = false;
                        rbtAddNewTestPlan.Visible = false;
                    }

                    if (this.GetMilestoneCategory != null)
                    {
                        this.MilestoneCategoryList = this.GetMilestoneCategory();
                        var mailestoneList = this.MilestoneCategoryList;

                        var radcomboBoxMilestoneCategory = (RadComboBox)item.FindControl("RadComboBox_MilestoneCategory_TestPlan");
                        var milestoneCategoryList = this.MilestoneCategoryList;
                        radcomboBoxMilestoneCategory.DataSource = milestoneCategoryList;
                        radcomboBoxMilestoneCategory.DataTextField = "Milestone_Category_Name";
                        radcomboBoxMilestoneCategory.DataValueField = "MilestoneCategoryKey";
                        radcomboBoxMilestoneCategory.DataBind();

                        string milestoneCategory = DataBinder.Eval(e.Item.DataItem, "Milestone_Category_Name").ToString();
                        if (!String.IsNullOrEmpty(milestoneCategory))
                            radcomboBoxMilestoneCategory.SelectedValue = milestoneCategoryList.First(c => c.Milestone_Category_Name.Equals(milestoneCategory)).MilestoneCategoryKey.ToString();
                    }
                }
            }
        }

        #region manage extra(added/deleted) items

        protected void radcombobox_release_milestoenCategories_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get current control
            var currentCombobox_eSpecs = sender as RadComboBox;
            var currentItem = this.GetControlFromUniqueID(currentCombobox_eSpecs.UniqueID, "radPanelBar_release_miletones") as RadPanelItem;
            var radPanelBar_addMilestoneWindowLowerPart_eSpecs = currentItem.FindControl("radPanelBar_release_eSpecs_child") as RadPanelBar;
            var radListBox_eSpecs = radPanelBar_addMilestoneWindowLowerPart_eSpecs.Items[0].FindControl("radListBoxeSpecs_release_milestone") as RadListBox;

            if (this.GetEspecList != null)
            {
                Control parent = ((RadComboBox)sender).Parent;
                //GetFirstParentOfType : this method is used to get the nearest parent control by given control
                var releaseTable = Utils.GetFirstParentOfType<GridTableView>((RadComboBox)sender);
                var releaseItem = Utils.GetFirstParentOfType<GridItem>((RadComboBox)sender);
                if (releaseItem is GridEditFormInsertItem)
                {
                    //ParentItem is a property
                    int selctedProductKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());
                    string selctedProductName = "";
                    if (this.SelectedPruductFromManagments.ContainsKey(selctedProductKey))
                    {
                        selctedProductName = this.SelectedPruductFromManagments[selctedProductKey];
                    }
                    List<string> eSpecsList = new List<string>();
                    string selectedCategory = e.Text;

                    eSpecsList = this.GetEspecList(selctedProductName, selectedCategory);
                    //databinding for radListBoxESpecs
                    radListBox_eSpecs.Visible = true;
                    PopulateDataForEspecsList(radListBox_eSpecs, eSpecsList);
                }
            }
        }

        protected void radButton_release_deleteMilestone_Click(object sender, EventArgs e)
        {
            var milestoneDeleteButton = sender as RadButton;
            var currentItem = this.GetControlFromUniqueID(milestoneDeleteButton.UniqueID, "radPanelBar_release_miletones") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);
        }

        protected void radButton_release_addMultipleMilestones_Click(object sender, EventArgs e)
        {
            var radButtonAddExtraMilestone = sender as RadButton;
            RadPanelBar radPanelBarExtraTestMilestones = radButtonAddExtraMilestone.Parent.FindControl("radPanelBar_release_miletones") as RadPanelBar;
            RadPanelItem radPanelItem = new RadPanelItem();
            //after radpoanelitem's added to the radpanelbar, then itemtemplate can be accessed/displayed.
            radPanelBarExtraTestMilestones.Items.Add(radPanelItem);
            var radComboBox_extra_milestoneCategory = radPanelItem.FindControl("radcombobox_release_milestoenCategories") as RadComboBox;
            var radTextBox_release_releaseName = radPanelItem.FindControl("radTextBox_release_releaseName") as RadTextBox;
            RadTextBox radtextbox_ReleaseName = radButtonAddExtraMilestone.Parent.FindControl("radtextbox_ReleaseName") as RadTextBox;
            if (radtextbox_ReleaseName != null)
                radTextBox_release_releaseName.Text = radtextbox_ReleaseName.Text;

            if (this.GetMilestoneCategory != null)
            {
                this.MilestoneCategoryList = this.GetMilestoneCategory();
                var milestoneCategoryList = this.MilestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataSource = milestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataTextField = "Milestone_Category_Name";
                radComboBox_extra_milestoneCategory.DataValueField = "MilestoneCategoryKey";
                radComboBox_extra_milestoneCategory.DataBind();
            }
        }

        protected void radButton_addMultipleMilestones_Click(object sender, EventArgs e)
        {
            var radButtonAddExtraMilestone = sender as RadButton;
            RadPanelBar radPanelBarExtraTestMilestones = radButtonAddExtraMilestone.Parent.FindControl("radPanelBar_extra_miletones") as RadPanelBar;
            RadPanelItem radPanelItem = new RadPanelItem();
            //after radpoanelitem's added to the radpanelbar, then itemtemplate can be accessed/displayed.
            radPanelBarExtraTestMilestones.Items.Add(radPanelItem);
            var radComboBox_extra_milestoneCategory = radPanelItem.FindControl("radcombobox_extra_MilestoneCategoryName") as RadComboBox;
            //assign release name(display=false) to the UI
            var radTextBox_extra_releaseName = radPanelItem.FindControl("radTextBox_extra_releaseName") as RadTextBox;
            RadTextBox radTextBox_releaseName = radButtonAddExtraMilestone.Parent.FindControl("radTextBox_releaseName") as RadTextBox;
            radTextBox_extra_releaseName.Text = radTextBox_releaseName.Text;
            if (this.GetMilestoneCategory != null)
            {
                this.MilestoneCategoryList = this.GetMilestoneCategory();
                var milestoneCategoryList = this.MilestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataSource = milestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataTextField = "Milestone_Category_Name";
                radComboBox_extra_milestoneCategory.DataValueField = "MilestoneCategoryKey";
                radComboBox_extra_milestoneCategory.DataBind();
            }
        }

        protected void radButton_addMultipleTestPlans_Click(object sender, EventArgs e)
        {
            var radButtonAddExtraTestPlan = sender as RadButton;
            RadPanelBar radPanelBarExtraTestPlans = radButtonAddExtraTestPlan.Parent.FindControl("radPanelBar_extra_testPlans") as RadPanelBar;
            RadPanelItem radPanelItem = new RadPanelItem();
            //after radpoanelitem's added to the radpanelbar, then itemtemplate can be accessed/displayed.
            radPanelBarExtraTestPlans.Items.Add(radPanelItem);
            var radComboBox_extra_milestoneCategory = radPanelItem.FindControl("RadComboBox_extra_milestoneCategory") as RadComboBox;
            if (this.GetMilestoneCategory != null)
            {
                this.MilestoneCategoryList = this.GetMilestoneCategory();
                var milestoneCategoryList = this.MilestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataSource = milestoneCategoryList;
                radComboBox_extra_milestoneCategory.DataTextField = "Milestone_Category_Name";
                radComboBox_extra_milestoneCategory.DataValueField = "MilestoneCategoryKey";
                radComboBox_extra_milestoneCategory.DataBind();
            }
        }

        protected void radcombobox_extra_MilestoneCategoryName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //get current control
            var currentCombobox_eSpecs = sender as RadComboBox;
            var currentItem = this.GetControlFromUniqueID(currentCombobox_eSpecs.UniqueID, "radPanelBar_extra_miletones") as RadPanelItem;
            var radPanelBar_addMilestoneWindowLowerPart_eSpecs = currentItem.FindControl("radPanelBar_newMilestoneWindowLowerPart_eSpecs") as RadPanelBar;
            var radListBox_eSpecs = radPanelBar_addMilestoneWindowLowerPart_eSpecs.Items[0].FindControl("radListBoxeOnAddNewMilestoneLowerPartSpecs") as RadListBox;

            if (this.GetEspecList != null)
            {
                Control parent = ((RadComboBox)sender).Parent;
                //GetFirstParentOfType : this method is used to get the nearest parent control by given control
                var milestoneTable = Utils.GetFirstParentOfType<GridTableView>((RadComboBox)sender);
                var milestoneItem = Utils.GetFirstParentOfType<GridItem>((RadComboBox)sender);
                if (milestoneItem is GridEditFormInsertItem)
                {
                    var releaseTable = Utils.GetFirstParentOfType<GridTableView>(milestoneTable);
                    //ParentItem is a property
                    int selctedProductKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());
                    string selctedProductName = "";
                    if (this.SelectedPruductFromManagments.ContainsKey(selctedProductKey))
                    {
                        selctedProductName = this.SelectedPruductFromManagments[selctedProductKey];
                    }
                    //RadListBox radListBoxESpecs = (RadListBox)parent.FindControl("radListBoxeSpecs");
                    List<string> eSpecsList = new List<string>();
                    string selectedCategory = e.Text;

                    eSpecsList = this.GetEspecList(selctedProductName, selectedCategory);
                    //databinding for radListBoxESpecs
                    radListBox_eSpecs.Visible = true;
                    PopulateDataForEspecsList(radListBox_eSpecs, eSpecsList);
                }
            }
        }

        protected void radButton_extra_deleteMilestone_Click(object sender, EventArgs e)
        {
            var milestoneDeleteButton = sender as RadButton;
            var currentItem = this.GetControlFromUniqueID(milestoneDeleteButton.UniqueID, "radPanelBar_extra_miletones") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);
        }

        protected void RadAutoCompleteBox_existingRelease_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            Control parent = ((RadAutoCompleteBox)sender).Parent;
            RadTextBox tbName = (RadTextBox)parent.FindControl("radtextbox_ReleaseName");
            RadDatePicker datepickerStart = (RadDatePicker)parent.FindControl("raddatepicker_ReleaseStartDate");
            RadDatePicker datepickerEnd = (RadDatePicker)parent.FindControl("raddatepicker_ReleaseEndDate");
            RadioButtonList releaseVsoTagList = (RadioButtonList)parent.FindControl("radioButtonList_ReleaseType");

            var copy_ReleaseName = e.Entry.Text;
            var copy_ReleaseKey = int.Parse(e.Entry.Value);
            Release releaseInfo = this.GetReleasesByReleaseKey(copy_ReleaseKey);
            datepickerStart.Clear();
            datepickerEnd.Clear();

            tbName.Text = e.Entry.Text;
            datepickerStart.SelectedDate = releaseInfo.VSO_LocStartDate;
            datepickerEnd.SelectedDate = releaseInfo.VSO_DueDate;

            if (releaseInfo.VSO_Tags != null)
            {
                var customVsoTags = this.GetCustomTagsByReleaseTag(releaseInfo.VSO_Tags);
                foreach (ListItem tagItem in releaseVsoTagList.Items)
                {
                    if (customVsoTags.Contains(this.ConvertRelaseTypeToVsoTag(tagItem.Value)))
                    {
                        tagItem.Selected = true;
                    }
                }
            }

            List<Milestone> milestoensList = this.GetMilestonesByReleaseKey(copy_ReleaseKey);
            AddMultMilestonesUnderRelease(milestoensList, ((RadAutoCompleteBox)sender).Parent);
            ((RadAutoCompleteBox)sender).Entries.Clear();
        }

        private void AddMultMilestonesUnderRelease(List<Milestone> milestonesList, Control control)
        {
            //var radButtonAddExtraMilestone = sender as RadButton;
            RadPanelBar radPanelBarExtraTestMilestones = control.Parent.FindControl("radPanelBar_release_miletones") as RadPanelBar;
            var milestonecateList = this.GetMilestoneCategory();

            //clean up the existing milestones 
            if (radPanelBarExtraTestMilestones.Items != null)
                radPanelBarExtraTestMilestones.Items.Clear();

            foreach (var milestone in milestonesList)
            {
                RadPanelItem radPanelItem = new RadPanelItem();
                //after radpoanelitem's added to the radpanelbar, then itemtemplate can be accessed/displayed.
                radPanelBarExtraTestMilestones.Items.Add(radPanelItem);
                var radComboBox_extra_milestoneCategory = radPanelItem.FindControl("radcombobox_release_milestoenCategories") as RadComboBox;
                //var radTextBox_release_releaseName = radPanelItem.FindControl("radTextBox_release_releaseName") as RadTextBox;
                //RadTextBox radtextbox_ReleaseName = control.Parent.FindControl("radtextbox_ReleaseName") as RadTextBox;
                //if (radtextbox_ReleaseName != null)
                //    radTextBox_release_releaseName.Text = radtextbox_ReleaseName.Text;
                if (this.GetMilestoneCategory != null)
                {
                    this.MilestoneCategoryList = this.GetMilestoneCategory();
                    var milestoneCategoryList = this.MilestoneCategoryList;
                    radComboBox_extra_milestoneCategory.DataSource = milestoneCategoryList;
                    radComboBox_extra_milestoneCategory.DataTextField = "Milestone_Category_Name";
                    radComboBox_extra_milestoneCategory.DataValueField = "MilestoneCategoryKey";
                    radComboBox_extra_milestoneCategory.DataBind();
                }

                var milestoneName = milestonecateList.Where(x => x.MilestoneCategoryKey == milestone.MilestoneCategoryKey).Select(x => x.Milestone_Category_Name).FirstOrDefault();
                foreach (RadComboBoxItem item in radComboBox_extra_milestoneCategory.Items)
                {
                    if (item.Text == milestoneName && item.Selected == false)
                    {
                        item.Selected = true;
                    }
                }

                var milestoneNameTextbox = radPanelItem.FindControl("radTextBox_release_mileStoneName") as RadTextBox;
                var milestoneFromDatePicker = radPanelItem.FindControl("raddatepicker_release_milestoneFrom") as RadDatePicker;
                var milestoneToDatePicker = radPanelItem.FindControl("raddatepicker_release_milestoneTo") as RadDatePicker;

                milestoneNameTextbox.Text = milestone.Milestone_Name;
                milestoneFromDatePicker.SelectedDate = milestone.Milestone_Start_Date;
                milestoneToDatePicker.SelectedDate = milestone.Milestone_End_Date;
            }
        }

        protected void RadComboBox_MilestoneCategory_Milestone_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox combo = (RadComboBox)sender;
            this.GetEsInfoByMilestone(combo);
        }

        private void GetEsInfoByMilestone(RadComboBox milestoneCateComboBox)
        {
            //RadComboBox combo = (RadComboBox)sender;
            GridEditableItem editedItem = milestoneCateComboBox.NamingContainer as GridEditableItem;
            //make es-related labels visble
            RadPanelBar radPanelBar_milestone_eSpecs = (RadPanelBar)editedItem.FindControl("radPanelBar_milestone_eSpecs_child");
            RadListBox radListBoxESpecs = (RadListBox)radPanelBar_milestone_eSpecs.Items[0].FindControl("radListBoxeSpecs");
            if (this.GetEspecList != null)
            {
                Control parent = milestoneCateComboBox.Parent;
                //GetFirstParentOfType : this method is used to get the nearest paretn control by given control

                var milestoneTable = Utils.GetFirstParentOfType<GridTableView>(milestoneCateComboBox);
                var milestoneItem = Utils.GetFirstParentOfType<GridItem>(milestoneCateComboBox);
                
                var releaseTable = Utils.GetFirstParentOfType<GridTableView>(milestoneTable);
                //ParentItem is a property
                int selctedProductKey = int.Parse(releaseTable.ParentItem.GetDataKeyValue("ProductID").ToString());
                string selctedProductName = "";
                if (this.SelectedPruductFromManagments.ContainsKey(selctedProductKey))
                {
                    selctedProductName = this.SelectedPruductFromManagments[selctedProductKey];
                }
                List<string> eSpecsList = new List<string>();
                string selectedCategory = milestoneCateComboBox.SelectedItem.Text;

                eSpecsList = this.GetEspecList(selctedProductName, selectedCategory);
                //databinding for radListBoxESpecs
                radListBoxESpecs.Visible = true;
                PopulateDataForEspecsList(radListBoxESpecs, eSpecsList);
            }
        }

        private void PopulateDataForEspecsList(RadListBox radListBoxESpecs, List<string> eSpecsList)
        {
            radListBoxESpecs.Items.Clear();
            foreach (string eSpec in eSpecsList)
            {
                RadListBoxItem radlistboxitem = new RadListBoxItem();
                //radlistboxitem.Text = eSpec;
                radListBoxESpecs.Items.Add(radlistboxitem);
                var targeteSpecTextBox = radlistboxitem.FindControl("radTextBox_eSpecName") as RadTextBox;
                targeteSpecTextBox.Text = eSpec;
            }
        }

        protected void radButton_extra_deleteTestSchedule_Click(object sender, EventArgs e)
        {
            var testPlanDeleteButton = sender as RadButton;
            var currentItem = this.GetControlFromUniqueID(testPlanDeleteButton.UniqueID, "radPanelBar_extra_testPlans") as RadPanelItem;
            currentItem.PanelBar.Items.Remove(currentItem);
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

        #endregion manage extra(added/deleted) items

        private void RefreshCache(GridTableView currentTable, int pagesize, int pageindex)
        {
            string name = currentTable.Name;
            switch (name)
            {
                case "MilestoneDetails":
                    int releaseKey = int.Parse(currentTable.ParentItem.GetDataKeyValue("VSO_ID").ToString());
                    this.MilestoneOfRelease_Result[releaseKey] = this.GetMileStones(releaseKey, pageindex * pagesize, (pageindex + 1) * pagesize);
                    break;

                case "TestPlanDetails":
                    break;

                case "ReleaseDetails":
                    break;
            }
        }

        public List<EspecInfo> ESpecList
        {
            get
            {
                return ViewState["ESpecList"] as List<EspecInfo>;
            }
            set
            {
                ViewState["ESpecList"] = value;
            }
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

        public void RadGridSchedule_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            //first  call method get relase("1,endpage inxed shoudl be the last item number")

            //store the result "R1"

            //second time: srart page num "new N1" and end page num"new N2".

            //R1(new start page(new N1, new N2))
        }

        public int GetTotalItems()
        {
            return RadGridSchedule.Items.Count;
        }

        
    }

    public class MilestoneInfoFromModal
    {
        public Milestone Milestone { get; set; }
        public string NewCatagory { get; set; }
        public List<EspecInfo> eSpecList { get; set; }
    }

    public class TestScheduleInfoFromModal
    {
        public TestSchedule TestSchedule { get; set; }
        public string NewCatagory { get; set; }
        public string ItearationInfo { get; set; }
    }
}
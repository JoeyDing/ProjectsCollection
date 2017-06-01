using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using SkypeIntlPortfolio.Ajax.UserControls.LOS;
using SkypeIntlPortfolio.Ajax.UserControls.Service;
using SkypeIntlPortfolio.Ajax.UserControls.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    [Serializable]
    public class TimelineInfo
    {
        public int ProductKey { get; set; }

        public int ReleaseKey { get; set; }

        public string ProductGroupKey { get; set; }

        public string ProductGroupName { get; set; }

        //public double Key { get; set; }
        public string Key { get; set; }

        public int? MilestoneKey { get; set; }

        public int? MilestoneCategoryKey { get; set; }

        public string Name { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? End_Date { get; set; }

        public string MilestoneCategoryName { get; set; }

        public string MilestoneAssignedTo { get; set; }

        public string ReleaseAssignedTo { get; set; }

        public string ItemType { get; set; }

        public string Family { get; set; }

        public string VsoItemQuery { get; set; }

        public int? TestScheduleKey { get; set; }

        public string TestScheduleName { get; set; }

        public int AssignedResources { get; set; }

        public object ResourceKey { get; set; }

        public string ProductsAffected { get; set; }
        public string PeopleAffected { get; set; }

        //not used
        public string RecurrenceRule { get; set; }

        public string RecurrenceParentId { get; set; }
    }

    public partial class LineOfSight : UserControl, ILineOfSightView
    {
        public Dictionary<int, string> SelectedProducts
        {
            get
            {
                if (Session.Contents["SelectedProductsReporting"] == null)
                    Session.Contents["SelectedProductsReporting"] = new Dictionary<int, string>();
                return Session.Contents["SelectedProductsReporting"] as Dictionary<int, string>;
            }
            set { Session.Contents["SelectedProductsReporting"] = value; }
        }

        public Dictionary<string, string> EntriesDict
        {
            get
            {
                return this.ViewState["EntriesDict"] as Dictionary<string, string>;
            }
            set
            {
                this.ViewState["EntriesDict"] = value;
            }
        }

        private IEnumerable<string> checkedItemsText;

        public event EventHandler OnRedirecToUrlWithQueryStringClicked;

        public event Func<int, VacationInfo> GetVacationInfoesByID;

        public event Func<int, Dictionary<string, string>> GetAffectedProductsByVacID;

        public event Func<int, List<int>> GetAffectedLocationsIDsByVacationID;

        public event Func<IEnumerable<string>, Dictionary<int, string>> GetProductsWithCheckedLocations;

        public event Func<List<VacationRelatedInfo>> GetVacationRelatedInfoList;

        public event Action<Milestone, string> UpdateMileStone;

        public event Action<Release> UpdateRelease;

        public event Action<TestSchedule, string> UpdateTestPlan;

        public event Action<VacationInfo, IEnumerable<int>, List<int>> UpdateVacation;

        public event Func<List<int>> GetAllProductKeys;

        public event Action<Milestone> UpdateMileStoneForDragAndDrop;

        public event Action<Release> UpdateReleaseForDragAndDrop;

        public event Action<TestSchedule> UpdateTestPlanForDragAndDrop;

        public event Action<VacationInfo> UpdateVacationForDragAndDrop;

        public Dictionary<string, bool> CheckedReleaseMilestone
        {
            get
            {
                if (Session["CheckedReleaseMilestoneLineOfSight"] == null)
                    Session["CheckedReleaseMilestoneLineOfSight"] = new Dictionary<string, bool>();
                return Session["CheckedReleaseMilestoneLineOfSight"] as Dictionary<string, bool>;
            }
            set
            {
                Session["CheckedReleaseMilestoneLinOfSight"] = value;
            }
        }

        public List<TimelineInfo> TimelineInfos
        {
            get
            {
                var result = this.Session["TimelineInfos"] as List<TimelineInfo>;
                return result;
            }
            set
            {
                this.Session["TimelineInfos"] = value;
            }
        }

        public List<VacationRelatedInfo> VacationInfoList
        {
            get
            {
                return this.Session["VacationInfos"] as List<VacationRelatedInfo>;
            }
            set
            {
                this.Session["VacationInfos"] = value;
            }
        }

        public Dictionary<int, double> VsoTaskHoursOfEpic
        {
            get
            {
                Dictionary<int, double> result = null;
                result = this.Session["VsoTaskHoursOfEpic"] as Dictionary<int, double>;
                return result;
            }
            set
            {
                this.Session["VsoTaskHoursOfEpic"] = value;
            }
        }

        public Dictionary<VsoTaskInfo, double> VsoTaskHoursOfMilestone
        {
            get
            {
                Dictionary<VsoTaskInfo, double> result = null;
                result = this.Session["VsoTaskHoursOfMilestone"] as Dictionary<VsoTaskInfo, double>;
                return result;
            }
            set
            {
                this.Session["VsoTaskHoursOfMilestone"] = value;
            }
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

        public List<ProductInfo> ProductInfos
        {
            get
            {
                var result = this.Session["LineOfSightProductInfos"] as List<ProductInfo>;
                return result;
            }
            set
            {
                this.Session["LineOfSightProductInfos"] = value;
            }
        }

        public RadListBox RadlistboxReleaseAndMilestone
        {
            get
            {
                if (this.RadListBox_Release_Milestone != null)
                {
                    return this.RadListBox_Release_Milestone;
                }
                return null;
            }

            set
            {
                if (this.RadListBox_Release_Milestone != null)
                {
                    this.RadListBox_Release_Milestone = value;
                }
            }
        }

        public List<int> AllProductsIds
        {
            get { return ViewState["AllProductsIds"] as List<int>; }
            set { ViewState["AllProductsIds"] = value; }
        }

        public int NumberOfSelectedItems
        {
            get
            {
                int numberOfSelectedItems = Convert.ToInt32(this.Session["NumberOfSelectedItems"]);
                return numberOfSelectedItems;
            }

            set
            {
                this.Session["NumberOfSelectedItems"] = value;
            }
        }

        public int DisplayID
        {
            get;
            set;
        }

        public bool IsRadScheduleHidden
        {
            get;
            set;
        }

        private int selectedValueFromRadioList = -1;
        //{
        //    get { return ViewState["SelectedIndexFromRadioList"] != null ? (int)ViewState["SelectedIndexFromRadioList"] : -1; }
        //    set { ViewState["SelectedIndexFromRadioList"] = value; }
        //}

        private DateTime getDateOfLastSunday(DateTime from)
        {
            //get the date of last sunday
            int IndexDayOfToday = (int)from.DayOfWeek;
            DateTime specificDateToShow = System.DateTime.Today.AddDays(-IndexDayOfToday);
            return specificDateToShow;
        }

        private List<ProductInfo> FilterProducts()
        {
            var result = new List<ProductInfo>();

            var checkedItems = this.RadListBox_Release_Milestone.CheckedItems;
            this.checkedItemsText = checkedItems.Select(c => c.Text);
            var checkedItemsWithoutVacation = checkedItems.Where(c => c.Text != "Vacation");
            int totalCheckedNumber = checkedItemsWithoutVacation.Count();

            var checkedItemsTextWithoutVacation = checkedItemsWithoutVacation.Select(c => c.Text);
            //remove the unrelated parts if radlistbox contains release
            if (checkedItemsTextWithoutVacation.Contains("Release"))
            {
                if (totalCheckedNumber == 3)
                {
                    result = this.ProductInfos;
                }
                else if (totalCheckedNumber == 2)
                {
                    if (checkedItemsTextWithoutVacation.Contains("Milestone"))
                    {
                        foreach (var product in this.ProductInfos)
                        {
                            //this way will avoid breaking the original data stored in the session(ProductInfos)
                            var newProduct = Utils.Clone(product);
                            foreach (var release in newProduct.Releases)
                            {
                                //remove testschedule part
                                release.TestSchedules = new TestScheduleInfo[0];
                            }
                            result.Add(newProduct);
                        }
                    }
                    //if contains Testschedule
                    else if (checkedItemsTextWithoutVacation.Contains("Test Plan"))
                    {
                        foreach (var product in this.ProductInfos)
                        {
                            //this way will avoid breaking the original data stored in the session(ProductInfos)

                            var newProduct = Utils.Clone(product);
                            foreach (var release in newProduct.Releases)
                            {
                                //remove mielstone part
                                release.Milestones = new MilestoneInfo[0];
                            }
                            result.Add(newProduct);
                        }
                    }
                }
                //radlistbox only contains release
                else
                {
                    foreach (var product in this.ProductInfos)
                    {
                        //this way will avoid breaking the original data stored in the session(ProductInfos)
                        var newProduct = Utils.Clone(product);
                        foreach (var release in newProduct.Releases)
                        {
                            //remove mielstone part
                            release.Milestones = new MilestoneInfo[0];
                            //remove testschedule part
                            release.TestSchedules = new TestScheduleInfo[0];
                        }
                        result.Add(newProduct);
                    }
                }
            }
            //hide the release if radlistbox doesn't contain release
            else
            {
                if (totalCheckedNumber == 0)
                {
                    result = new List<ProductInfo>();
                }
                else
                {
                    bool showMilestone = checkedItemsTextWithoutVacation.Contains("Milestone");
                    bool showTestSchedule = checkedItemsTextWithoutVacation.Contains("Test Plan");
                    foreach (var product in this.ProductInfos)
                    {
                        var newProduct = Utils.Clone(product);
                        foreach (var release in newProduct.Releases)
                        {
                            //in order to get the release name
                            release.IsHidden = true;
                            if (!showMilestone)
                                release.Milestones = new MilestoneInfo[0];

                            if (!showTestSchedule)
                                release.TestSchedules = new TestScheduleInfo[0];
                        }
                        result.Add(newProduct);
                    }
                }
            }

            return result;
        }

        public void Refresh()
        {
            //ProductInfos should also be updated here(need to do this in the future)

            if (this.GetVacationRelatedInfoList != null)
                this.VacationInfoList = GetVacationRelatedInfoList();
            this.ProductInfos = Utils.GetProductInfo(SelectedProducts.Keys.ToArray()).Where(c => c.Releases != null).ToList();
            if (ProductInfos != null)
            {
                //1 filter the products releases/milestones based on checked items
                var productsToDisplay = this.FilterProducts();

                //2 create appointements and resources
                List<TimelineInfo> timelineInfos = productsToDisplay
                                                    .SelectMany(d => d.Releases.SelectMany(e => e.Milestones.Select(f => new { d.Product_Name, d.Family, Milestone = f })))
                                                    .Select(c => new TimelineInfo()
                                                    {
                                                        ProductKey = c.Milestone.ProductKey,
                                                        ReleaseKey = c.Milestone.ReleaseKey,
                                                        Key = string.Concat(c.Milestone.ReleaseKey, "_", c.Milestone.MilestoneKey),
                                                        MilestoneCategoryKey = c.Milestone.MilestoneCategoryKey,
                                                        MilestoneKey = c.Milestone.MilestoneKey,
                                                        Name = c.Milestone.Milestone_Name,
                                                        Start_Date = c.Milestone.Milestone_Start_Date,
                                                        End_Date = c.Milestone.Milestone_End_Date,
                                                        MilestoneCategoryName = c.Milestone.MilestoneCategoryName,
                                                        ItemType = "milestone",
                                                        Family = c.Family,
                                                        RecurrenceRule = c.Milestone.RecurrenceRule,
                                                        RecurrenceParentId = c.Milestone.RecurrenceParentId,
                                                        ProductGroupKey = string.Concat(c.Milestone.ProductKey, "_milestone"),
                                                        ProductGroupName = string.Concat(c.Product_Name, " (Milestones)"),
                                                        ResourceKey = c.Milestone.MilestoneCategoryKey
                                                    }).ToList();

                timelineInfos.AddRange(productsToDisplay
                                                      .SelectMany(d => d.Releases.SelectMany(t => t.TestSchedules.Select(g => new { d.Product_Name, d.Family, TestSchedule = g })))
                                                      .Select(c => new TimelineInfo()
                                                      {
                                                          ProductKey = c.TestSchedule.ProductKey,
                                                          ReleaseKey = c.TestSchedule.ReleaseKey,
                                                          //Key = string.Concat(c.TestSchedule.ReleaseKey, "_", c.TestSchedule.TestScheduleKey),
                                                          Key = string.Concat(c.TestSchedule.ReleaseKey, "TestPlan", c.TestSchedule.TestScheduleKey),
                                                          MilestoneCategoryKey = c.TestSchedule.MilestoneCategoryKey,
                                                          TestScheduleKey = c.TestSchedule.TestScheduleKey,
                                                          Name = c.TestSchedule.TestScheduleName,
                                                          Start_Date = c.TestSchedule.TestScheduleStartDate,
                                                          End_Date = c.TestSchedule.TestScheduleEndDate,
                                                          VsoItemQuery = c.TestSchedule.TestScheduleUrl,
                                                          ItemType = "testPlan",
                                                          Family = c.Family,
                                                          RecurrenceRule = c.TestSchedule.RecurrenceRule,
                                                          RecurrenceParentId = c.TestSchedule.RecurrenceParentId,
                                                          ProductGroupKey = string.Concat(c.TestSchedule.ProductKey, "_testPlan"),
                                                          ProductGroupName = string.Concat(c.Product_Name, " (Test Plans)"),
                                                          ResourceKey = "testPlan",
                                                          AssignedResources = c.TestSchedule.AssignedResources
                                                      }
                                                      ).ToList());

                timelineInfos.AddRange(productsToDisplay
                                                    .SelectMany(d => d.Releases.Where(r => r.IsHidden == false).Select(e => new { d.ProductKey, d.Product_Name, Release = e }))
                                                    .Select(c => new TimelineInfo()
                                                    {
                                                        ProductKey = c.ProductKey,
                                                        ReleaseKey = c.Release.ReleaseKey,
                                                        Key = c.Release.ReleaseKey.ToString(),
                                                        VsoItemQuery = c.Release.Release_Url,
                                                        MilestoneCategoryKey = null,
                                                        MilestoneKey = null,
                                                        Name = c.Release.Release_Name,
                                                        Start_Date = c.Release.Release_Start_Date,
                                                        End_Date = c.Release.Release_End_Date,
                                                        MilestoneCategoryName = null,
                                                        ItemType = "release",
                                                        RecurrenceRule = null,
                                                        RecurrenceParentId = null,
                                                        ProductGroupKey = string.Concat(c.ProductKey, "_release"),
                                                        ProductGroupName = string.Concat(c.Product_Name, " (Releases)"),
                                                    }).ToList());
                //3 for vacation appointment
                if (this.checkedItemsText.Contains("Vacation"))
                {
                    timelineInfos.AddRange(this.VacationInfoList.Select(c => new TimelineInfo
                    {
                        ItemType = "vacationType",
                        ProductKey = 100000,
                        Key = c.VacationID.ToString(),
                        ProductGroupKey = "VacationKey",//random name,could be anything
                        ProductGroupName = "Vacation",
                        Start_Date = c.VacationStartDate,
                        End_Date = c.VacationEndDate,
                        Name = c.VacationName,
                        PeopleAffected = string.Join(";", c.PeopleAffected),
                        ProductsAffected = string.Join(";", c.ProductsAffected)
                    }));
                }

                this.TimelineInfos = timelineInfos.OrderBy(c => c.ProductKey).ThenByDescending(c => c.Key.ToString()).ToList();

                this.InitializeResources(productsToDisplay);
                if (ProductInfos.Count != 0)
                {
                    GetVsoTaskHours();
                    this.RefreshData();
                }
            }
        }

        private void GetVsoTaskHours()
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                //group elements by epicID
                //var VsoTaskHoursOfRelease = context.vm_vsoTaskInfo.ToList().GroupBy(c => c.Epic_ID).ToList();
                var VsoTaskHoursOfRelease = context.vm_vsoTaskInfo.GroupBy(c => c.Epic_ID).ToList();

                VsoTaskHoursOfMilestone = new Dictionary<VsoTaskInfo, double>(new VsoTaskInfoComparer());
                foreach (var group in VsoTaskHoursOfRelease)
                {
                    double totalHours = 0.0;
                    foreach (var value in group)
                    {
                        Dictionary<string, double> innerDictionary = new Dictionary<string, double>();
                        //sum the hours with same group key
                        if (value.VsoItemHours_Sum.HasValue)
                        {
                            totalHours += value.VsoItemHours_Sum.Value;
                        }

                        if (value.ES_Tag != null && value.VsoItemHours_Sum.HasValue)
                        {
                            if (VsoTaskHoursOfMilestone.ContainsKey(new VsoTaskInfo { EpicID = value.Epic_ID, Vsotag = GettargetVsoTag(value.ES_Tag) }))
                            {
                                VsoTaskHoursOfMilestone[new VsoTaskInfo { EpicID = value.Epic_ID, Vsotag = GettargetVsoTag(value.ES_Tag) }] += value.VsoItemHours_Sum.Value;
                            }
                            else
                            {
                                VsoTaskHoursOfMilestone.Add(new VsoTaskInfo()
                                {
                                    EpicID = value.Epic_ID,
                                    Vsotag = GettargetVsoTag(value.ES_Tag)
                                }, value.VsoItemHours_Sum.Value);
                            }
                        }
                    }
                    if (VsoTaskHoursOfEpic == null)
                    {
                        VsoTaskHoursOfEpic = new Dictionary<int, double>();
                    }
                    //VsoTaskHoursOfEpic.Add(group.Key, totalHours);
                    VsoTaskHoursOfEpic[group.Key] = totalHours;
                }
            }
        }

        private string GettargetVsoTag(string vsoTags)
        {
            string targetVsoTag = "";
            if (vsoTags.Contains("Loc_EndGame"))
            {
                targetVsoTag = "Loc_EndGame";
            }
            else if (vsoTags.Contains("Loc_Signoff"))
            {
                targetVsoTag = "Loc_Signoff";
            }
            else if (vsoTags.Contains("Loc_Ready"))
            {
                targetVsoTag = "Loc_Ready";
            }
            else if (vsoTags.Contains("Loc_Retro"))
            {
                targetVsoTag = "Loc_Retro";
            }
            else if (vsoTags.Contains("Loc_Ready"))
            {
                targetVsoTag = "Loc_Ready";
            }
            else if (vsoTags.Contains("Loc_Start"))
            {
                targetVsoTag = "Loc_Start";
            }
            else
            {
                targetVsoTag = "Loc_Progressing";
            }
            return targetVsoTag;
        }

        private void RegisterJavaScript()
        {
            string jsProjectKey = this.Page.ClientID + "_lineOfSight";
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(this.Page.GetType(), jsProjectKey))
            {
                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(),
                 jsProjectKey, @"..\UserControls\LineOfSightControl.js");
            }
        }

        private void RefreshData()
        {
            var categories = this.GetCheckedMilestoneCategories();
            if (this.TimelineInfos != null)
            {
                var data = this.TimelineInfos.Where(m => !m.MilestoneCategoryKey.HasValue || categories.Contains(m.MilestoneCategoryKey.Value)).ToList();
                this.RadScheduler1.DataSource = data;
                this.RadScheduler1.GroupBy = GroupBy.SelectedValue;
                this.RadScheduler1.DataBind();
            }
        }

        private List<int> GetCheckedMilestoneCategories()
        {
            var result = new List<int>();

            if (this.radbuttoncheck_locready.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "locready").Key);
            if (this.radbuttoncheck_locstart.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "locstart").Key);
            if (this.radbuttoncheck_progressing.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "progressing").Key);
            if (this.radbuttoncheck_retro.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "retro").Key);
            if (this.radbuttoncheck_signoff.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "signoff").Key);
            if (this.radbuttoncheck_endgame.Checked) result.Add(this.MilestoneCategories.First(c => c.Value.ToLower() == "endgame").Key);

            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsRadScheduleHidden)
            {
                if (this.Visible)
                {
                    //EntriesDict = new Dictionary<string, string>();

                    //RegisterJavaScript();
                    this.RadScheduler1.TimelineView.SlotDuration = TimeSpan.Parse(SlotDuration.SelectedValue);
                    this.RadScheduler1.AgendaView.ResourceMarkerType = ResourceMarkerType.Block;

                    //get the date of last sunday and set it to the linofsight
                    this.RadScheduler1.SelectedDate = getDateOfLastSunday(System.DateTime.Today);

                    if (!string.IsNullOrWhiteSpace(GroupBy.SelectedValue))
                    {
                        this.RadScheduler1.GroupingDirection = (Telerik.Web.UI.GroupingDirection)Enum.Parse(typeof(Telerik.Web.UI.GroupingDirection), "Vertical");
                    }

                    if (this.RadScheduler1.ResourceTypes != null && this.RadScheduler1.ResourceTypes.Any())
                    {
                        this.RadScheduler1.GroupBy = GroupBy.SelectedValue;
                    }
                    if (!IsPostBack)
                    {
                        if (ProductInfos != null && ProductInfos.Count != 0 && this.GetAllProductKeys != null)
                        {
                            this.AllProductsIds = GetAllProductKeys();
                            Refresh();
                        }
                    }
                }
            }
            else
            {
                this.panel_radSchedule.Visible = false;
                this.label_warning_cancelledProduct.Visible = true;
                this.label_warning_cancelledProduct.Text = "Sorry, the product you select is cancelled";
            }
            //by default, the items  Release, milestone and test schedule shoudl be selected
            if (CheckedReleaseMilestone.Count == 0)
            {
                foreach (var checkdItem in this.RadListBox_Release_Milestone.CheckedItems)
                {
                    CheckedReleaseMilestone.Add(checkdItem.Value, checkdItem.Checked);
                }
            }
        }

        private void InitializeResources(List<ProductInfo> productInfos)
        {
            RadScheduler1.Resources.Clear();
            RadScheduler1.ResourceTypes.Clear();
            RadScheduler1.ResourceTypes.Add(new ResourceType("Product") { ForeignKeyField = "ProductGroupKey" });
            RadScheduler1.ResourceTypes.Add(new ResourceType("Category") { ForeignKeyField = "ResourceKey" });
            //RadScheduler1.ResourceTypes.Add(new ResourceType("TimeDataType") { ForeignKeyField = "MilestoneCategoryKey" });

            if (productInfos != null)
            {
                foreach (var group in TimelineInfos.GroupBy(c => c.ProductGroupKey))
                {
                    var item = group.First();
                    RadScheduler1.Resources.Add(new Resource("Product", item.ProductGroupKey, item.ProductGroupName));
                }

                foreach (var item in productInfos.SelectMany(d => d.Releases.SelectMany(e => e.Milestones)).OrderBy(f => f.MilestoneCategoryKey).GroupBy(c => c.MilestoneCategoryKey))
                {
                    RadScheduler1.Resources.Add(new Resource("Category", item.Key, item.First().MilestoneCategoryName));
                }

                RadScheduler1.Resources.Add(new Resource("Category", "testPlan", "testPlan"));
            }
        }

        protected void radbuttoncheck_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            //access the appointment just loaded
            var appt = e.Appointment;
            //get the vsoitem url
            TimelineInfo dataItem = (TimelineInfo)appt.DataItem;
            string vsoitemUrlText = "";
            string vsoitemURL = "";
            string vsoitemHours = "0.0";
            int vacationItems = 0;
            string peopleAff = "";
            string productAff = "";

            string testMilestoneCategoryKey = "";
            int assignedResources = 0;
            if (dataItem.ItemType == "release")
            {
                vsoitemURL = dataItem.VsoItemQuery;
                vsoitemUrlText = dataItem.ReleaseKey.ToString();
                if (VsoTaskHoursOfEpic != null)
                {
                    if (VsoTaskHoursOfEpic.ContainsKey(dataItem.ReleaseKey))
                    {
                        vsoitemHours = VsoTaskHoursOfEpic[dataItem.ReleaseKey].ToString();
                    }
                }
                assignedResources = -1;
            }
            else if (dataItem.ItemType == "vacationType")
            {
                peopleAff = dataItem.PeopleAffected;
                productAff = dataItem.ProductsAffected;
                vacationItems = -1;

                assignedResources = -1;
            }
            else if (dataItem.ItemType == "milestone")
            {
                var epicID = dataItem.ReleaseKey;
                var milestoneCategory = dataItem.MilestoneCategoryName;
                var VSP_Project = "LOCALIZATION";
                var family = dataItem.Family;
                string vsoTag;
                if (Utils.TryParseCategoryNameToVsoTag(milestoneCategory, out vsoTag))
                {
                    var HyperLinkTextOfMilestone = string.Format("{0}", vsoTag);
                    var HyperLinkOfMilestone = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(epicID, vsoTag, VSP_Project, family);
                    vsoitemUrlText = dataItem.ReleaseKey.ToString();
                    VsoTaskInfo vti = new VsoTaskInfo();
                    vti.EpicID = epicID;
                    vti.Vsotag = HyperLinkTextOfMilestone;
                    //concatenate the uniquekey based on selected task info
                    string concatenatedKey = epicID + HyperLinkTextOfMilestone;
                    if (VsoTaskHoursOfMilestone != null)
                    {
                        if (VsoTaskHoursOfMilestone.ContainsKey(vti))
                        {
                            vsoitemHours = VsoTaskHoursOfMilestone[vti].ToString();
                        }
                    }
                    assignedResources = -1;
                    vsoitemUrlText = vsoTag;
                    vsoitemURL = HyperLinkOfMilestone;
                }
            }
            else
            {
                vsoitemURL = dataItem.VsoItemQuery;
                vsoitemUrlText = dataItem.TestScheduleKey.ToString();
                testMilestoneCategoryKey = dataItem.MilestoneCategoryKey.Value.ToString();
                vsoitemHours = "-1";
                if (!string.IsNullOrEmpty(dataItem.AssignedResources.ToString()))
                {
                    assignedResources = dataItem.AssignedResources;
                }
            }

            appt.Attributes.Add("VsoItemLinkText", vsoitemUrlText);
            appt.Attributes.Add("VsoItemLink", vsoitemURL);
            appt.Attributes.Add("VsoItemHours", vsoitemHours);

            appt.Attributes.Add("TestMilestoneCategoryKey", testMilestoneCategoryKey.ToString());
            appt.Attributes.Add("AssignedResources", assignedResources.ToString());
            appt.Attributes.Add("VacationItems", vacationItems.ToString());
            appt.Attributes.Add("PeopleAff", peopleAff);
            appt.Attributes.Add("ProductAff", productAff);
        }

        protected void RadListBox_Release_Milestone_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            CheckedReleaseMilestone.Clear();
            foreach (var checkdItem in this.RadListBox_Release_Milestone.CheckedItems)
            {
                CheckedReleaseMilestone.Add(checkdItem.Value, checkdItem.Checked);
            }

            //invoke the method RedirectToUrlWithQueryString from Reporting system page
            if (this.OnRedirecToUrlWithQueryStringClicked != null)
            {
                this.OnRedirecToUrlWithQueryStringClicked(this, e);
            }
        }

        protected void RadScheduler1_FormCreated(object sender, SchedulerFormCreatedEventArgs e)
        {
            if ((e.Container.Mode == SchedulerFormMode.AdvancedEdit))
            {
                var title = e.Container.FindControl("label_advanceEdit_title") as Label;
                //if the appointment is a milestone, display the combobox, if the appointment is release make combobox invisible
                RadComboBox radComboboxCategoryName = (RadComboBox)e.Container.FindControl("radcomboBox_onLineofSight_displayName");
                Label displayLabel = (Label)e.Container.FindControl("Label_displayName");
                Label assignedResourcesLabel = (Label)e.Container.FindControl("label_assignedResources");
                RadTextBox radTextBoxAssignedResources = (RadTextBox)e.Container.FindControl("radTextBox_onLineofSight_assignedResources");

                var apptKey = e.Container.Appointment.Resources[0].Key.ToString();
                DateTime apptStartTime = e.Appointment.Start;
                DateTime apptEndTIme = e.Appointment.End;
                string apptSubject = e.Appointment.Subject;

                if (apptKey.Contains("VacationKey"))
                {
                    Panel panelVcations = e.Container.FindControl("panel_Vacation") as Panel;
                    panelVcations.Visible = true;

                    title.Text = "Update Vacation";

                    RadTextBox radText_vacationDescription = e.Container.FindControl("radtextbox_VacationDescription") as RadTextBox;

                    int vacationID = int.Parse(e.Appointment.ID.ToString());

                    VacationInfo vacInfo = null;
                    int uiCateId = 0;
                    if (this.GetVacationInfoesByID != null)
                    {
                        vacInfo = GetVacationInfoesByID(vacationID);

                        radText_vacationDescription.Text = vacInfo.VacationDescription;
                        uiCateId = this.selectedValueFromRadioList == -1 ? vacInfo.UICategoryID : this.selectedValueFromRadioList;
                    }

                    RadTextBox ratext_vacationName = e.Container.FindControl("radtextbox_VacationName") as RadTextBox;
                    ratext_vacationName.Text = apptSubject;

                    RadDatePicker vacStartDateTimePicker = (RadDatePicker)e.Container.FindControl("raddatepicker_VacationStartDate");
                    vacStartDateTimePicker.SelectedDate = apptStartTime;

                    RadDatePicker vacEndDateTimePicker = (RadDatePicker)e.Container.FindControl("raddatepicker_VacationEndDate");
                    vacEndDateTimePicker.SelectedDate = apptEndTIme;

                    RadioButtonList radioList_UICategory = (RadioButtonList)e.Container.FindControl("radioButtonList_UIVacationCategory");
                    Panel oboPanel = (Panel)e.Container.FindControl("panel_checkProductsOneByOne");
                    Panel locPanel = (Panel)e.Container.FindControl("panel_checkProductsByLoc");
                    switch (uiCateId)
                    {
                        //one by one
                        case 1:

                            oboPanel.Visible = true;
                            locPanel.Visible = false;
                            radioList_UICategory.SelectedIndex = 0;

                            if (this.selectedValueFromRadioList == -1)
                            {
                                RadAutoCompleteBox racb_existingProducts = (RadAutoCompleteBox)e.Container.FindControl("RadAutoCompleteBox_existingProducts");
                                //load Autocompletebox
                                if (this.GetAffectedProductsByVacID != null)
                                {
                                    var retrievedProaducts = GetAffectedProductsByVacID(vacationID);

                                    foreach (var productInfo in retrievedProaducts)
                                    {
                                        racb_existingProducts.Entries.Add(new AutoCompleteBoxEntry(productInfo.Value, productInfo.Key.ToString()));
                                    }
                                }
                            }

                            break;
                        //by loc
                        case 2:

                            locPanel.Visible = true;
                            oboPanel.Visible = false;
                            radioList_UICategory.SelectedIndex = 1;

                            if (this.selectedValueFromRadioList == -1)
                            {
                                RadListBox radListBoxLocation = (RadListBox)e.Container.FindControl("radListBox_Location");

                                if (this.GetAffectedLocationsIDsByVacationID != null)
                                {
                                    var locationIds = GetAffectedLocationsIDsByVacationID(vacationID);
                                    foreach (RadListBoxItem radListItem in radListBoxLocation.Items)
                                    {
                                        if (locationIds.Contains(int.Parse(radListItem.Value)))
                                            radListItem.Checked = true;
                                    }
                                }
                            }

                            break;
                        //all products
                        case 3:
                            radioList_UICategory.SelectedIndex = 2;

                            oboPanel.Visible = false;
                            locPanel.Visible = false;

                            break;
                    }
                }
                else
                {
                    Panel panelRelease = e.Container.FindControl("panel_MileStoneTestPlanRelease") as Panel;
                    panelRelease.Visible = true;
                    if (apptKey.Contains("milestone"))
                    {
                        title.Text = "Update Milestone:";
                        //fill the combobox on control with categories
                        foreach (var category in this.MilestoneCategories)
                        {
                            RadComboBoxItem radItem = new RadComboBoxItem();
                            radItem.Text = category.Value;
                            if (radItem.Text == e.Appointment.Resources[1].Text.ToString())
                            {
                                radItem.Selected = true;
                            }
                            radComboboxCategoryName.Items.Add(radItem);
                            radItem.DataBind();
                        };
                        displayLabel.Text = "Milestone Name:";
                        assignedResourcesLabel.Visible = false;
                        radTextBoxAssignedResources.Visible = false;
                    }
                    else if (apptKey.Contains("release"))
                    {
                        title.Text = "Update Release:";
                        displayLabel.Text = "Release Name:";
                        radComboboxCategoryName.Visible = false;
                        assignedResourcesLabel.Visible = false;
                        radTextBoxAssignedResources.Visible = false;
                    }
                    else if (apptKey.Contains("testPlan"))
                    {
                        title.Text = "Update Test Plan:";
                        displayLabel.Text = "Test Plan Name:";
                        assignedResourcesLabel.Text = "Assigned Reources:";
                        //fill the combobox on control with categories
                        foreach (var category in this.MilestoneCategories)
                        {
                            RadComboBoxItem radItem = new RadComboBoxItem();
                            radItem.Text = category.Value;
                            radItem.Value = category.Key.ToString();
                            if (radItem.Value == e.Appointment.Attributes["TestMilestoneCategoryKey"])
                            {
                                radItem.Selected = true;
                            }
                            radComboboxCategoryName.Items.Add(radItem);
                            radItem.DataBind();
                        };
                        //fill the assigned resources textbox
                        radTextBoxAssignedResources.Text = e.Appointment.Attributes["AssignedResources"];
                    }
                    RadTextBox displayContext = (RadTextBox)e.Container.FindControl("radTextBox_onLineofSight_displayName");
                    displayContext.Text = apptSubject;

                    RadDatePicker startDateTimePicker = (RadDatePicker)e.Container.FindControl("raddatepicker_onLineofSight_milestoneStartDate");
                    startDateTimePicker.SelectedDate = apptStartTime;

                    RadDatePicker endDateTimePicker = (RadDatePicker)e.Container.FindControl("raddatepicker_onLineofSight_milestoneEndDate");
                    endDateTimePicker.SelectedDate = apptEndTIme;
                }
            }
        }

        protected void RadScheduler1_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
        {
            //1 check if drag and drop/extend or edit
            RadTextBox modifiedName = Utils.GetChildrenOfType<RadTextBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radTextBox_onLineofSight_displayName");
            var apptKey = e.Appointment.Resources[0].Key.ToString();
            //if drag/drop
            if (modifiedName == null)
            {
                //vacation
                if (apptKey.Contains("VacationKey"))
                {
                    int vacationID = int.Parse(e.Appointment.ID.ToString());

                    var vac = new VacationInfo
                    {
                        VacationID = vacationID,
                        VacationStartDate = e.ModifiedAppointment.Start,
                        VacationEndDate = e.ModifiedAppointment.End
                    };
                    if (this.UpdateVacationForDragAndDrop != null)
                        UpdateVacationForDragAndDrop(vac);
                }
                else
                {
                    //if milestone
                    if (e.Appointment.ID.ToString().Contains("_"))
                    {
                        int milestoneKey = int.Parse(e.Appointment.ID.ToString().Split(new char[] { '_' })[1]);
                        var milestoneObject = new Milestone
                        {
                            MilestoneKey = milestoneKey,
                            Milestone_Start_Date = e.ModifiedAppointment.Start,
                            Milestone_End_Date = e.ModifiedAppointment.End.AddDays(1).AddSeconds(-1)
                        };
                        if (this.UpdateMileStoneForDragAndDrop != null)
                            UpdateMileStoneForDragAndDrop(milestoneObject);
                    }
                    //if testplan
                    else if (e.Appointment.ID.ToString().Contains("TestPlan"))
                    {
                        //update VSO release
                        var vsoContext = Utils.GetVsoContext();
                        int testKey = int.Parse(e.Appointment.ID.ToString().Split(new[] { "TestPlan" }, StringSplitOptions.None)[1]);
                        var updatedTesPlan = vsoContext.UpdateVsoWorkItem(
                            id: testKey,
                            fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.StartDate", e.ModifiedAppointment.Start.ToString()},
                                        {"Microsoft.VSTS.Scheduling.FinishDate", e.ModifiedAppointment.End.ToString()}
                                   });

                        var testObject = new TestSchedule
                        {
                            TestScheduleKey = testKey,
                            TestSchedule_Start_Date = e.ModifiedAppointment.Start,
                            TestSchedule_End_Date = e.ModifiedAppointment.End,
                        };
                        if (this.UpdateTestPlanForDragAndDrop != null)
                            UpdateTestPlanForDragAndDrop(testObject);
                    }
                    //if release
                    else
                    {
                        int releaseKey = Convert.ToInt32(e.Appointment.ID);
                        //update VSO release
                        var vsoContext = Utils.GetVsoContext();

                        var updatedEpic = vsoContext.UpdateVsoWorkItem(
                            id: releaseKey,
                            fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.StartDate", e.ModifiedAppointment.Start.ToString()},
                                        {"Microsoft.VSTS.Scheduling.DueDate", e.ModifiedAppointment.End.ToString()},
                                //{"System.Tags",string.Concat("Loc_Release; ", string.Format("Loc_ReleaseStartDate:{0}", e.ModifiedAppointment.End.ToString("M/d/yy")))}
                            });

                        var releaseObject = new Release
                        {
                            VSO_ID = releaseKey,
                            VSO_LocStartDate = e.ModifiedAppointment.Start,
                            VSO_DueDate = e.ModifiedAppointment.End,
                        };

                        if (this.UpdateReleaseForDragAndDrop != null)
                            UpdateReleaseForDragAndDrop(releaseObject);
                    }
                }
            }
            //if edit panel
            else
            {
                var modifiedDisplayName = modifiedName.Text;
                RadDatePicker modifiedStartDateTimePicker = Utils.GetChildrenOfType<RadDatePicker>(this.RadScheduler1).FirstOrDefault(c => c.ID == "raddatepicker_onLineofSight_milestoneStartDate");
                var modifiedStartDate = modifiedStartDateTimePicker.SelectedDate;
                RadDatePicker modifiedEndDateTimePicker = Utils.GetChildrenOfType<RadDatePicker>(this.RadScheduler1).FirstOrDefault(c => c.ID == "raddatepicker_onLineofSight_milestoneEndDate");
                var modifiedEndDate = modifiedEndDateTimePicker.SelectedDate;
                RadTextBox modifiedAssignedResourcesTextBox = Utils.GetChildrenOfType<RadTextBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radTextBox_onLineofSight_assignedResources");
                var modifiedAssignedResources = modifiedAssignedResourcesTextBox.Text;
                Panel panelVacation = Utils.GetChildrenOfType<Panel>(this.RadScheduler1).FirstOrDefault(c => c.ID == "panel_Vacation");
                if (!panelVacation.Visible)
                {
                    //check type of displayed object
                    Label displayedLabel = Utils.GetChildrenOfType<Label>(this.RadScheduler1).FirstOrDefault(c => c.ID == "Label_displayName");
                    if (displayedLabel.Text.Contains("Milestone Name"))
                    {
                        RadComboBox selectedCategoryCombobox = Utils.GetChildrenOfType<RadComboBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radcomboBox_onLineofSight_displayName");
                        string categoriesName = selectedCategoryCombobox.Text;
                        int milestoneKey = int.Parse(e.Appointment.ID.ToString().Split(new char[] { '_' })[1]);

                        var milestoneObject = new Milestone
                        {
                            MilestoneKey = milestoneKey,
                            Milestone_Name = modifiedDisplayName,
                            Milestone_Start_Date = modifiedStartDate.Value,
                            Milestone_End_Date = modifiedEndDate.Value.AddDays(1).AddSeconds(-1)
                        };

                        if (this.UpdateMileStone != null)
                            UpdateMileStone(milestoneObject, categoriesName);
                    }
                    else if (displayedLabel.Text.Contains("Release Name"))
                    {
                        //get the VSOID(release id)
                        int releaseKey = Convert.ToInt32(e.Appointment.ID);
                        //update VSO release
                        var vsoContext = Utils.GetVsoContext();

                        var updatedEpic = vsoContext.UpdateVsoWorkItem(
                            id: releaseKey,
                            fields: new Dictionary<string, string>{
                                        {"System.Title",modifiedDisplayName},
                                        {"Microsoft.VSTS.Scheduling.StartDate", Convert.ToDateTime(modifiedStartDate).ToString()},
                                        {"Microsoft.VSTS.Scheduling.DueDate", Convert.ToDateTime(modifiedEndDate).ToString()},
                                //{"System.Tags",string.Concat("Loc_Release; ", string.Format("Loc_ReleaseStartDate:{0}", Convert.ToDateTime(modifiedStartDate).ToString("M/d/yy")))}
                            });

                        var releaseObject = new Release
                        {
                            VSO_ID = releaseKey,
                            VSO_Title = modifiedDisplayName,
                            VSO_LocStartDate = Convert.ToDateTime(modifiedStartDate),
                            VSO_DueDate = Convert.ToDateTime(modifiedEndDate)
                        };

                        if (this.UpdateRelease != null)
                            UpdateRelease(releaseObject);
                    }
                    else if (displayedLabel.Text.Contains("Test Plan Name"))
                    {
                        int testPlanKey = int.Parse(e.Appointment.ID.ToString().Split(new[] { "TestPlan" }, StringSplitOptions.None)[1]);
                        var vsoContext = Utils.GetVsoContext();
                        //convert it to vso-tag
                        RadComboBox selectedCategoryCombobox = Utils.GetChildrenOfType<RadComboBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radcomboBox_onLineofSight_displayName");
                        string vsoTag;
                        if (Utils.TryParseCategoryNameToVsoTag(selectedCategoryCombobox.Text, out vsoTag))
                        {
                            JObject testPlantagsList = vsoContext.GetListOfWorkItemsByIDs(new int[] { testPlanKey }, new string[] { "System.Tags" });
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
                                                   id: testPlanKey,
                                                   fields: new Dictionary<string, string>{
                                                          {"System.Title",modifiedDisplayName},
                                                          {"Microsoft.VSTS.Scheduling.StartDate", Convert.ToDateTime(modifiedStartDate).ToString()},
                                                          {"Microsoft.VSTS.Scheduling.FinishDate", Convert.ToDateTime(modifiedEndDate).ToString()},
                                                          {"System.Tags",string.Concat("Loc_TestPlan; ", vsoTag, vsoTagString)}
                                       });
                                    }
                                    else
                                    {
                                        var updatedTestPlanVsoTag = vsoContext.UpdateVsoWorkItem(
                                                      id: testPlanKey,
                                                      fields: new Dictionary<string, string>{
                                                          {"System.Title",modifiedDisplayName},
                                                          {"Microsoft.VSTS.Scheduling.StartDate", Convert.ToDateTime(modifiedStartDate).ToString()},
                                                          {"Microsoft.VSTS.Scheduling.FinishDate", Convert.ToDateTime(modifiedEndDate).ToString()},
                                                          {"System.Tags",string.Concat(vsoTag+";",vsoTagString)}
                                       });
                                    }
                                }
                            }
                        }
                        var categoriesName = selectedCategoryCombobox.Text;

                        var testObject = new TestSchedule
                        {
                            TestScheduleKey = testPlanKey,
                            TestSchedule_Name = modifiedDisplayName,
                            TestSchedule_Start_Date = Convert.ToDateTime(modifiedStartDate),
                            TestSchedule_End_Date = Convert.ToDateTime(modifiedEndDate),
                            AssignedResources = Int32.Parse(modifiedAssignedResources)
                        };
                        if (this.UpdateTestPlan != null)
                            UpdateTestPlan(testObject, categoriesName);
                    }
                }
                else
                {
                    RadTextBox vacationName = Utils.GetChildrenOfType<RadTextBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radtextbox_VacationName");
                    RadTextBox vacationDescription = Utils.GetChildrenOfType<RadTextBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "radtextbox_VacationDescription");
                    RadDatePicker vacationStartDate = Utils.GetChildrenOfType<RadDatePicker>(this.RadScheduler1).FirstOrDefault(c => c.ID == "raddatepicker_VacationStartDate");
                    RadDatePicker vacationEndDate = Utils.GetChildrenOfType<RadDatePicker>(this.RadScheduler1).FirstOrDefault(c => c.ID == "raddatepicker_VacationEndDate");

                    int vacationID = int.Parse(e.Appointment.ID.ToString());
                    var radioList = Utils.GetChildrenOfType<RadioButtonList>(this.RadScheduler1).FirstOrDefault(c => c.ID == ("radioButtonList_UIVacationCategory"));
                    //not selected index(use the value in the item which is hard coded to have same uicatID as in the db)
                    int uiCatId = int.Parse(radioList.SelectedValue);
                    var vacation = new VacationInfo
                    {
                        VacationID = vacationID,
                        VacationName = vacationName.Text,
                        VacationDescription = vacationDescription.Text,
                        VacationStartDate = vacationStartDate.SelectedDate.Value,
                        VacationEndDate = vacationEndDate.SelectedDate.Value,
                        UICategoryID = uiCatId
                    };
                    var vacationAffectedProductIds_hashSet = new HashSet<int>();
                    List<int> vacationAffectedLocationIds = new List<int>();

                    if (uiCatId == 3)
                        AllProductsIds.ForEach(c => vacationAffectedProductIds_hashSet.Add(c));
                    else if (uiCatId == 2)
                    {
                        var locationNames = new List<string>();
                        RadListBox radListBoxLocation = Utils.GetChildrenOfType<RadListBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == ("radListBox_Location"));
                        foreach (RadListBoxItem item in radListBoxLocation.CheckedItems)
                        {
                            vacationAffectedLocationIds.Add(int.Parse(item.Value));
                            locationNames.Add(item.Text);
                        }
                        if (this.GetProductsWithCheckedLocations != null)
                            GetProductsWithCheckedLocations(locationNames).Keys.ToList().ForEach(c => vacationAffectedProductIds_hashSet.Add(c));
                    }
                    else if (uiCatId == 1)
                    {
                        RadAutoCompleteBox racb_existingProducts = Utils.GetChildrenOfType<RadAutoCompleteBox>(this.RadScheduler1).FirstOrDefault(c => c.ID == "RadAutoCompleteBox_existingProducts");
                        //prevent duplicated productids

                        foreach (AutoCompleteBoxEntry racb in racb_existingProducts.Entries)
                        {
                            vacationAffectedProductIds_hashSet.Add(Convert.ToInt32(racb.Value));
                        }
                    }
                    if (this.UpdateVacation != null)
                        UpdateVacation(vacation, vacationAffectedProductIds_hashSet, vacationAffectedLocationIds);
                }

                //context.SaveChanges();
                Refresh();
            }
        }

        protected void radioButtonList_UIVacationCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList radioButtonList_UIVacationCategory = sender as RadioButtonList;
            //not selected index(use the value in the item which is hard coded to have same uicatID as in the db)
            //no need to create a viewstate, since this event is triggered after the postback,selectedValueFromRadioList's value will not be lost
            selectedValueFromRadioList = int.Parse(radioButtonList_UIVacationCategory.SelectedValue);
        }

        //protected void RadAutoCompleteBox_existingProducts_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        //{
        //    RadAutoCompleteBox currentRadAutoCompleteBox = sender as RadAutoCompleteBox;
        //    List<int> filteredList = new List<int>();
        //    for (int i = 0; i < currentRadAutoCompleteBox.Entries.Count; i++)
        //    {
        //        int poductKey = Convert.ToInt32(currentRadAutoCompleteBox.Entries[i].Value);
        //        int newAddedOne = Convert.ToInt32(e.Entry.Value);

        //        if (filteredList.Contains(newAddedOne) && currentRadAutoCompleteBox.Entries.Count != 1)
        //            filteredList.Add(0);
        //        else
        //            filteredList.Add(poductKey);
        //        //0 means the added item existed
        //        if (filteredList.Contains(0))
        //        {
        //            currentRadAutoCompleteBox.Entries.Remove(e.Entry);
        //            break;
        //        }
        //    }
        //}
    }
}
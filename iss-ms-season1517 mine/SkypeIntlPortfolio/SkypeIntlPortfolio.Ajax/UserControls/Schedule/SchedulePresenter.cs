using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.UserControls.Schedule
{
    public class SchedulePresenter
    {
        private IScheduleView _view;
        private const string projectName = "LOCALIZATION";
        private VsoContext vsoContext;

        public SchedulePresenter(IScheduleView view, Dictionary<int, string> selectedProducts)
        {
            this.vsoContext = Utils.GetVsoContext();
            this._view = view;
            this._view.SelectedPruductFromManagments = selectedProducts;
            //here is for subscription
            this._view.GetReleases += view_GetReleases;
            this._view.GetMileStones += _view_GetMileStones;
            this._view.GetTestPlans += _view_GetTestPlans;
            this._view.InsertRelease += _view_InsertRelease;
            this._view.InsertMilestone += _view_InsertMilestone;
            this._view.InsertTestPlan += _view_InsertTestPlan;
            this._view.UpdateRelease += _view_UpdateRelease;
            this._view.UpdateMilestone += _view_UpdateMilestone;
            this._view.UpdateTestSchedule += _view_UpdateTestSchedule;
            this._view.DeleteRelease += _view_DeleteRelease;
            this._view.DeleteMilestone += _view_DeleteMilestone;
            this._view.DeleteTestSchedule += _view_DeleteTestSchedule;
            this._view.GetMilestoneCategory += _view_GetMilestoneCategory;
            this._view.GetEspecList += _view_GetEspecList;
            this._view.GetTotalReleases += _view_GetTotalReleases;
            this._view.GetTotalTestSchedules += _view_GetTotalTestSchedules;
            this._view.GetTotalMileStones += _view_GetTotalMileStones;
            this._view.GetReleasesByReleaseKey += _view_GetReleasesByReleaseKey;
            this._view.GetMilestonesByReleaseKey += _view_GetMilestonesByReleaseKey;
            this._view.GetMilestoneByKey += _view_GetMilestoneByKey;
            this._view.GetCustomTagsByReleaseTag += _view_GetCustomTagsByReleaseTag;
            this._view.IsProductCancelled += _view_IsProductCancelled;
        }

        private List<Milestone> _view_GetMilestonesByReleaseKey(int releaseKey)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                return context.Milestones.Where(r => r.ReleaseKey == releaseKey).ToList();
            }
        }

        private Milestone _view_GetMilestoneByKey(int milestoneKey)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                return context.Milestones.Where(m => m.MilestoneKey == milestoneKey).FirstOrDefault();
            }
        }

        private string _view_GetProductFamily(int productKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.Products_New.First(c => c.ProductKey == productKey).ProductFamily.Product_Family;
                return result;
            }
        }

        private Release _view_GetReleasesByReleaseKey(int releaseKey)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                return context.Releases.Where(r => r.VSO_ID == releaseKey).FirstOrDefault();
            }
        }

        #region TotalRecords

        private int _view_GetTotalReleases(int productKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var prod = db.Products_New.First(c => c.ProductKey == productKey);
                var result = prod.Releases.Where(c => c.Deleted == false).Count();
                return result;
            }
        }

        private int _view_GetTotalMileStones(int releaseKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var release = db.Releases.First(c => c.VSO_ID == releaseKey);
                var result = release.Milestones.Count;
                return result;
            }
        }

        private int _view_GetTotalTestSchedules(int releaseKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var release = db.Releases.First(c => c.VSO_ID == releaseKey);
                var result = release.TestSchedules.Where(c => c.Deleted == false).Count();
                return result;
            }
        }

        #endregion TotalRecords

        #region Delete

        private void _view_DeleteTestSchedule(int testPlanKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var e = db.TestSchedules.First(c => c.TestScheduleKey == testPlanKey);
                e.Deleted = true;
                db.SaveChanges();

                var fields = new Dictionary<string, string> { { "System.State", "Inactive" }, { "System.IterationPath", @"LOCALIZATION\_Trash_" }, { "System.AreaPath", @"LOCALIZATION\_Trash_" } };
                vsoContext.UpdateVsoWorkItem(e.TestScheduleKey, fields);
            }
        }

        private void _view_DeleteMilestone(int milestoneKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var e = db.Milestones.First(c => c.MilestoneKey == milestoneKey);
                db.Milestones.Remove(e);
                db.SaveChanges();
            }
        }

        private void _view_DeleteRelease(int releaseKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                List<int> intList = new List<int> { releaseKey };
                List<string> stateList = new List<string> { "System.State" };
                var vsoEpic = vsoContext.GetListOfWorkItemsByIDs(intList, stateList);
                //check  epic's state
                JObject fields = (JObject)vsoEpic["value"][0]["fields"];
                string systemState = Utils.GetValue<string>(fields, "System.State");

                if (systemState != "Closed")
                {
                    var fieldsOne = new Dictionary<string, string> { { "System.State", "Closed" }, { "System.IterationPath", @"LOCALIZATION\_Trash_" }, { "System.AreaPath", @"LOCALIZATION\_Trash_" } };
                    var closedItem = vsoContext.UpdateVsoWorkItem(releaseKey, fieldsOne);
                    var fieldsTwo = new Dictionary<string, string> { { "Microsoft.VSTS.Common.ResolvedReason", "Withdrawn" } };
                    var updatedItem = vsoContext.UpdateVsoWorkItem(releaseKey, fieldsTwo);
                }

                var e = db.Releases.First(c => c.VSO_ID == releaseKey);
                e.Deleted = true;
                db.SaveChanges();
            }
        }

        #endregion Delete

        #region Update

        private void _view_UpdateTestSchedule(TestSchedule e)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                db.TestSchedules.Attach(e);
                db.Entry(e).Property(x => x.TestSchedule_Name).IsModified = true;
                db.Entry(e).Property(x => x.TestSchedule_Start_Date).IsModified = true;
                db.Entry(e).Property(x => x.TestSchedule_End_Date).IsModified = true;
                db.Entry(e).Property(x => x.AssignedResources).IsModified = true;
                db.Entry(e).Property(x => x.MilestoneCategoryKey).IsModified = true;
                db.SaveChanges();
                //update VSO test
                string vsoTag;
                string tagResult = "";
                string cateName = db.MilestoneCategories.First(c => c.MilestoneCategoryKey == e.MilestoneCategoryKey).Milestone_Category_Name;
                Utils.TryParseCategoryNameToVsoTag(cateName, out vsoTag);

                int testPlanid = e.TestScheduleKey;
                var json = vsoContext.GetListOfWorkItemsByIDs(new int[] { testPlanid }, new string[] { "System.Tags" });

                //Dictionary<string, int> vsoTagDictionary = new Dictionary<string, int>();

                var values = json["value"];

                var fields = values[0]["fields"] as JObject;
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
                        tagResult = string.Concat("Loc_TestPlan; ", vsoTag, vsoTagString);
                    }
                    else
                    {
                        tagResult = string.Concat(vsoTag + ";", vsoTagString);
                    }
                }

                vsoContext.UpdateVsoWorkItem(
                                      id: testPlanid,
                                      fields: new Dictionary<string, string>{
                                                    {"System.Tags",tagResult},
                                                    {"Microsoft.VSTS.Scheduling.StartDate", e.TestSchedule_Start_Date.ToString()},
                                                    {"Microsoft.VSTS.Scheduling.FinishDate", e.TestSchedule_End_Date.ToString()},
                                                    {"System.Title",e.TestSchedule_Name}
                                           });
            }
        }

        private void _view_UpdateMilestone(MilestoneInfoFromModal milestoneRelatedInfo)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                Milestone milestone = milestoneRelatedInfo.Milestone;
                db.Milestones.Attach(milestone);
                db.Entry(milestone).Property(x => x.MilestoneCategoryKey).IsModified = true;
                db.Entry(milestone).Property(x => x.Milestone_Name).IsModified = true;
                db.Entry(milestone).Property(x => x.Milestone_Start_Date).IsModified = true;
                db.Entry(milestone).Property(x => x.Milestone_End_Date).IsModified = true;
                db.SaveChanges();

                var product = db.Products_New.First(c => c.ProductKey == milestone.ProductKey);
                string areaPath = product.Localization_VSO_Path;
                string iterationPath = "LOCALIZATION";
                string currentEpicUrl = String.Format("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/{0}", milestone.ReleaseKey);
                string vsoTag;
                string cateName = "";
                InsertNewMileStoneCateForMileStone(milestone, milestoneRelatedInfo.NewCatagory, db);
                if (milestoneRelatedInfo.NewCatagory == null)
                {
                    int milestoneKey = milestone.MilestoneCategoryKey;
                    cateName = db.MilestoneCategories.First(c => c.MilestoneCategoryKey == milestoneKey).Milestone_Category_Name;
                    if (Utils.TryParseCategoryNameToVsoTag(cateName, out vsoTag))
                    {
                        cateName = vsoTag;
                    }
                }
                else
                    cateName = milestoneRelatedInfo.NewCatagory;

                //insert es into VSO
                foreach (EspecInfo eSPec in milestoneRelatedInfo.eSpecList)
                {
                    string especName = eSPec.EspecName;
                    string eSpecEstimatePoints = eSPec.EstimatedPoints;
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
                                        tags: new string[] {  cateName },
                                        prepareFunction: (fields) =>
                                        {
                                                var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StoryPoints" }, { "value", eSpecEstimatePoints } };
                                            fields.Add(f_eSpecEstimate);
                                        }
                                     );
                }




            }
        }

        private void _view_UpdateRelease(Release e)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                db.Releases.Attach(e);
                db.Entry(e).Property(x => x.VSO_Title).IsModified = true;
                db.Entry(e).Property(x => x.VSO_LocStartDate).IsModified = true;
                db.Entry(e).Property(x => x.VSO_DueDate).IsModified = true;
                db.Entry(e).Property(x => x.VSO_Tags).IsModified = true;
                db.SaveChanges();

                var updatedEpic = vsoContext.UpdateVsoWorkItem(
                           id: e.VSO_ID,
                           fields: new Dictionary<string, string>{
                                    {"System.Title",e.VSO_Title},
                                    {"Microsoft.VSTS.Scheduling.StartDate",e.VSO_LocStartDate.ToString()},
                                    {"Microsoft.VSTS.Scheduling.DueDate", e.VSO_DueDate.ToString()},
                                    {"System.Tags",e.VSO_Tags}
                            });
            }
        }

        #endregion Update

        #region Insert

        private void _view_InsertTestPlan(List<List<TestScheduleInfoFromModal>> listOfTestPlansList, int productKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                foreach (List<TestScheduleInfoFromModal> item in listOfTestPlansList)
                {
                    var product = db.Products_New.First(c => c.ProductKey == productKey);
                    string family = product.ProductFamily.Product_Family;
                    string areaPath = product.Localization_VSO_Path;
                    var newTestPlan = vsoContext.CreateTestPlan(projectName, item[0].TestSchedule.TestSchedule_Name, areaPath, item[0].ItearationInfo, item[0].TestSchedule.TestSchedule_Start_Date.Value, item[0].TestSchedule.TestSchedule_End_Date.Value);
                    int testScheduleKey = (int)newTestPlan["id"];
                    string currentEpicUrl = String.Format("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/{0}", item[0].TestSchedule.ReleaseKey);

                    string vsoTag;
                    string cateName = "";
                    InsertNewMileStoneCateForTestSchedule(item[0].TestSchedule, item[0].NewCatagory, db);
                    if (item[0].NewCatagory == null)
                    {
                        int milestoneCategoryKey = item[0].TestSchedule.MilestoneCategoryKey.Value;
                        cateName = db.MilestoneCategories.First(c => c.MilestoneCategoryKey == milestoneCategoryKey).Milestone_Category_Name;
                        if (Utils.TryParseCategoryNameToVsoTag(cateName, out vsoTag))
                        {
                            cateName = vsoTag;
                        }
                    }
                    else
                        cateName = item[0].NewCatagory;

                    vsoContext.UpdateVsoWorkItem(testScheduleKey,
                                                    new Dictionary<string, string>{
                                                   {"System.Tags",string.Concat("Loc_TestPlan; ", cateName)}},
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

                    item[0].TestSchedule.TestScheduleKey = testScheduleKey;
                    item[0].TestSchedule.AssignedResources = item[0].TestSchedule.AssignedResources.Value;
                    item[0].TestSchedule.Vso_Web_Url = string.Format("{0}/DefaultCollection/{1}/_testManagement?planId={2}", vsoContext.VsoUrl, projectName, item[0].TestSchedule.TestScheduleKey);
                    db.TestSchedules.Add(item[0].TestSchedule);
                }
                db.SaveChanges();
            }
        }

        private void _view_InsertMilestone(List<List<MilestoneInfoFromModal>> listOfMilestonesList, int productKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                foreach (List<MilestoneInfoFromModal> item in listOfMilestonesList)
                {
                    var product = db.Products_New.First(c => c.ProductKey == productKey);
                    string family = product.ProductFamily.Product_Family;
                    //string areaPath = string.Format("{0}\\{1}", vsoContext.GetProjectTeamDefaultArea(projectName, family), product.Product_Name);
                    string areaPath = product.Localization_VSO_Path;
                    string iterationPath = "LOCALIZATION";
                    string vsoTag;
                    string cateName = "";
                    InsertNewMileStoneCateForMileStone(item[0].Milestone, item[0].NewCatagory, db);
                    if (item[0].NewCatagory == null)
                    {
                        int milestoneKey = item[0].Milestone.MilestoneCategoryKey;
                        cateName = db.MilestoneCategories.First(c => c.MilestoneCategoryKey == milestoneKey).Milestone_Category_Name;
                        if (Utils.TryParseCategoryNameToVsoTag(cateName, out vsoTag))
                        {
                            cateName = vsoTag;
                        }
                    }
                    else
                        cateName = item[0].NewCatagory;
                    string currentEpicUrl = String.Format("https://skype.visualstudio.com/DefaultCollection/_apis/wit/workItems/{0}", item[0].Milestone.ReleaseKey);
                    foreach (EspecInfo eSPec in item[0].eSpecList)
                    {
                        string especName = eSPec.EspecName;
                        string eSpecEstimatePoints = eSPec.EstimatedPoints;
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
                                            tags: new string[] { cateName },
                                            prepareFunction: (fields) =>
                                            {
                                                //adding eSpec Estimate Points
                                                //    var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.EstimatePoints" }, { "value", eSpecEstimatePoints } };
                                                //fields.Add(f_eSpecEstimate);
                                                var f_eSpecEstimate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StoryPoints" }, { "value", eSpecEstimatePoints } };
                                                fields.Add(f_eSpecEstimate);
                                            }
                                         );
                    }

                    db.Milestones.Add(item[0].Milestone);
                }
                db.SaveChanges();
            }
        }

        private int _view_InsertRelease(Release e, int productKey, string customTag)
        {
            int addedReleaseKey = 0;
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                //Create New Release in Vso
                var product = db.Products_New.First(c => c.ProductKey == productKey);
                string family = product.ProductFamily.Product_Family;
                //string areaPath = string.Format("{0}\\{1}", vsoContext.GetProjectTeamDefaultArea(projectName, family), product.Product_Name);
                string areaPath = product.Localization_VSO_Path;
                string iterationPath = "LOCALIZATION";
                var newEpic = vsoContext.CreateVsoWorkItem(
                              type: TaskTypes.Epic,
                              projectName: projectName,
                              title: e.VSO_Title,
                              areaPath: areaPath,
                              iterationPath: iterationPath,
                              assignedTo: null,
                              //tags: new string[] { "Loc_Release", string.Format("Loc_ReleaseStartDate:{0}", e.VSO_LocStartDate.ToString("M/d/yy")), customTag },
                              tags: new string[] { "Loc_Release", customTag },
                              prepareFunction: (fields) =>
                              {
                                  var f_startDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.StartDate" }, { "value", e.VSO_LocStartDate } };
                                  var f_dueDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.DueDate" }, { "value", e.VSO_DueDate } };
                                  fields.Add(f_startDate);
                                  fields.Add(f_dueDate);
                              });
                //Insert release into db

                e.VSO_ID = (int)newEpic["id"];
                e.VSO_AreaPath = areaPath;
                e.VSO_IterationPath = iterationPath;
                e.VSO_ChangedDate = (DateTime)newEpic["fields"]["System.ChangedDate"];
                e.VSO_Status = (string)newEpic["fields"]["System.State"];
                e.VSO_Type = (string)newEpic["fields"]["System.WorkItemType"];
                e.VSO_Family = family;
                e.VSO_Url = string.Format("{0}/DefaultCollection/{1}/_workitems/edit/{2}", vsoContext.VsoUrl, projectName, e.VSO_ID);
                e.VSO_ProductName = product.Product_Name;
                e.VSO_Tags = (string)newEpic["fields"]["System.Tags"];
                e.ProductKey = productKey;

                db.Releases.Add(e);
                db.SaveChanges();
                addedReleaseKey = e.VSO_ID;
            }
            return addedReleaseKey;
        }

        #endregion Insert

        #region Get

        private List<GetTestPlanOfRelease_Result> _view_GetTestPlans(int releaseKey, int startRow, int endRow)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.GetTestPlanOfRelease(releaseKey, startRow, endRow).OrderByDescending(c => c.TestSchedule_End_Date).ThenByDescending(c => c.TestSchedule_Start_Date).ToList();
                return result;
            }
        }

        private List<GetMilestoneOfRelease_Result> _view_GetMileStones(int releaseKey, int startRow, int endRow)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.GetMilestoneOfRelease(releaseKey, startRow, endRow).OrderByDescending(c => c.Milestone_End_Date).ThenByDescending(c => c.Milestone_Start_Date).ToList();
                foreach (var item in result)
                {
                    string vsoTag;
                    string mileStoneCateName = db.MilestoneCategories.First(c => c.MilestoneCategoryKey == item.MilestoneCategoryKey).Milestone_Category_Name;
                    if (Utils.TryParseCategoryNameToVsoTag(mileStoneCateName, out vsoTag))
                    {
                        //use VSO API URL to store HyperLink Text
                        item.Vso_Api_Url = vsoTag;
                        var productKey = db.Releases.First(c => c.VSO_ID == releaseKey).ProductKey;
                        string family = db.Products_New.First(c => c.ProductKey == productKey).ProductFamily.Product_Family;
                        //use Vso_Web_Url to store HyperLink navURl
                        item.Vso_Web_Url = Utils.GenerateVsoUrl_FromEpic_ChildIItemsWithTag(releaseKey, vsoTag, "LOCALIZATION", family);
                    }
                }
                return result;
            }
        }

        private List<GetReleaseOfProduct_Result> view_GetReleases(int productKey, int startRow = 0, int totalItems = 1000)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                //var result = db.GetReleaseOfProduct(productKey, startRow, endRow).ToList();
                var result = db.GetReleaseOfProduct(productKey, 0, 10000).OrderByDescending(c => c.VSO_DueDate).ThenByDescending(c => c.VSO_LocStartDate).ToList();
                foreach (var item in result)
                {
                    string releaseTags = item.VSO_Tags;
                    if (releaseTags != null)
                    {
                        List<string> customTagsList = this._view_GetCustomTagsByReleaseTag(releaseTags);
                        if (customTagsList.Count() != 0)
                        {
                            releaseTags = customTagsList.Aggregate((a, b) => a + "; " + b);
                        }
                        else
                        {
                            releaseTags = null;
                        }
                        item.VSO_Tags = releaseTags;
                    }
                }
                return result;
            }
        }

        private List<MilestoneCategory> _view_GetMilestoneCategory()
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                return db.MilestoneCategories.ToList();
            }
        }

        private List<string> _view_GetEspecList(string productName, string selectedCategory)
        {
            return this.PopulateESPecListWithData(productName, selectedCategory);
        }

        #endregion Get

        private List<string> PopulateESPecListWithData(string productName, string selectedMilestoneCategory)
        {
            //fill different data based on the selected item
            List<string> eSpecsList = new List<string>();
            switch (selectedMilestoneCategory.ToLower())
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
            return eSpecsList;
        }

        private static void InsertNewMileStoneCateForTestSchedule(TestSchedule e, string newMileStoneCateName, SkypeIntlPlanningPortfolioEntities db)
        {
            if (newMileStoneCateName != null)
            {
                var firstMilestoneCate = new MilestoneCategory();
                //firstMilestoneCate the milesotne(child) to milestonecategory(parent
                e.MilestoneCategory = firstMilestoneCate;
                if (!db.MilestoneCategories.Any(c => c.Milestone_Category_Name == newMileStoneCateName))
                {
                    firstMilestoneCate.Milestone_Category_Name = newMileStoneCateName;
                    db.MilestoneCategories.Add(firstMilestoneCate);
                }
                else
                {
                    e.MilestoneCategoryKey = db.MilestoneCategories.First(c => c.Milestone_Category_Name == newMileStoneCateName).MilestoneCategoryKey;
                }
            }
        }

        private static void InsertNewMileStoneCateForMileStone(Milestone e, string newMileStoneCateName, SkypeIntlPlanningPortfolioEntities db)
        {
            if (newMileStoneCateName != null)
            {
                var firstMilestoneCate = new MilestoneCategory();
                //firstMilestoneCate the milesotne(child) to milestonecategory(parent
                e.MilestoneCategory = firstMilestoneCate;
                if (!db.MilestoneCategories.Any(c => c.Milestone_Category_Name == newMileStoneCateName))
                {
                    firstMilestoneCate.Milestone_Category_Name = newMileStoneCateName;
                    db.MilestoneCategories.Add(firstMilestoneCate);
                }
                else
                {
                    e.MilestoneCategoryKey = db.MilestoneCategories.First(c => c.Milestone_Category_Name == newMileStoneCateName).MilestoneCategoryKey;
                }
            }
        }

        private List<string> _view_GetCustomTagsByReleaseTag(string releaseTags)
        {
            List<string> customTags = releaseTags.Split(';').Distinct().ToList();
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

        private bool _view_IsProductCancelled(List<int> productKeys)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                bool result;
                int statusCacelledKey = db.Status.FirstOrDefault(x => x.Status1 == "Cancelled").StatusKey;
                result = db.Products_New.ToList().Where(c => productKeys.Any(x => x == c.ProductKey)).
                    All(c => c.StatusKey == statusCacelledKey);
                return result;
            }
        }
    }
}
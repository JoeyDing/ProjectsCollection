using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.SyncTool.Core;
using SkypeIntlPortfolio.SyncTool.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.VsoWorkItem { }

public class SyncTestPlanAction : ISyncActionProvider
{
    private readonly VsoContext vsoContext;
    private readonly SkypeIntlPortfolioContext skypeIntlPortfolioContext = new SkypeIntlPortfolioContext();

    //Ids & Ids mapping
    private readonly Dictionary<int, int> dict_testplan_releaseId = new Dictionary<int, int>();

    private readonly List<int> lst_testplanId = new List<int>();
    private readonly List<int> lst_releaseId = new List<int>();

    //JObjcts
    private readonly List<JToken> lst_testplanJson = new List<JToken>();

    //Db Items
    private readonly List<TestSchedule> lst_testSchedule = new List<TestSchedule>();

    private readonly Logger logger;

    public string ActionName
    {
        get
        {
            return "Sync TestPlan";
        }
    }

    public SyncTestPlanAction(Logger logger)
    {
        this.logger = logger;
        string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
        string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];

        this.vsoContext = new VsoContext(vsoRootAccount, authenticationKey);
    }

    public void Sync()
    {
        PrepareIds();
        PrepareJObjectsForTestPlan();
        PrepareDBItems();
    }

    private T GetValue<T>(JObject container, string key)
    {
        return container[key] != null ? (T) container[key].ToObject<T>() : default(T);
    }

    private void PrepareIds()
    {
        logger.LogMessage("Querying Test plans....");

        var wiql = @"SELECT [System.Id],
                            [System.WorkItemType],
                            [System.Title],
                            [System.AssignedTo],
                            [System.State],
                            [System.Tags]
                            FROM WorkItemLinks WHERE
                            (Source.[System.WorkItemType] = 'Test Plan' and
                            Source.[System.IterationPath] under 'LOCALIZATION' and
                            Source.[System.Tags] contains 'Loc_TestPlan' and
                            (
                             Source.[System.Tags] contains 'Loc_Ready' or
                             Source.[System.Tags] contains 'Loc_Start' or
                             Source.[System.Tags] contains 'Loc_Progressing' or
                             Source.[System.Tags] contains 'Loc_EndGame' or
                             Source.[System.Tags] contains 'Loc_Signoff' or
                             Source.[System.Tags] contains 'Loc_Retro'
                            )) and
                            [System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Reverse' and
                            Target.[System.WorkItemType] = 'Epic'
                            ORDER BY [System.Id] mode(MustContain)";
        var result = this.vsoContext.RunComplexQuery(null, wiql);

        if (result["workItemRelations"] != null)
        {
            foreach (var t in result["workItemRelations"])
            {
                if (t["target"] != null && t["source"] != null)
                {
                    int testplanId = (int) t["source"]["id"];
                    int releaseId = (int) t["target"]["id"];
                    dict_testplan_releaseId[testplanId] = releaseId;
                }
            }
            foreach (KeyValuePair<int, int> pair in dict_testplan_releaseId)
            {
                lst_testplanId.Add(pair.Key);
                lst_releaseId.Add(pair.Value);
            }
        }
    }

    private void PrepareJObjectsForTestPlan()
    {
        int itemPerPage = 200;
        int count = lst_testplanId.Count;
        int totalPage = count / itemPerPage;
        int remainingPage = (count % itemPerPage == 0) ? 0 : 1;
        totalPage = totalPage + remainingPage;
        for (int i = 0; i < totalPage; i++)
        {
            List<int> list_testplanIdpaged = lst_testplanId.Skip(i * itemPerPage).Take(itemPerPage).ToList();
            var json_testplanDetail = vsoContext.GetListOfWorkItemsByIDs(list_testplanIdpaged);
            if (json_testplanDetail["value"] != null)
            {
                foreach (var tp in json_testplanDetail["value"])
                {
                    lst_testplanJson.Add(tp);
                }
            }
        }
    }

    private void PrepareDBItems()
    {
        var releases = skypeIntlPortfolioContext.Releases.Select(c => new
        {
            c.VSO_ID,
            c.VSO_AreaPath
        }).ToList();
        var products = skypeIntlPortfolioContext.Products_New.Select(c => new
        {
            c.ProductKey,
            c.Localization_VSO_Path
        }).ToList();
        var milestones = skypeIntlPortfolioContext.MilestoneCategories.AsNoTracking().ToList();
        var testSchedules = skypeIntlPortfolioContext.TestSchedules.AsNoTracking().ToList();
        var missingItems = new List<JToken>();

        int totalUpdated = 0;
        int totalAdded = 0;
        foreach (var tt in lst_testplanJson)
        {
            var fields = tt["fields"] as JObject;
            string tag = GetValue<string>(fields, "System.Tags");
            string[] tags = tag.Split(';');

            string vsoTag = "";
            tags.Any(t => TryParseCategoryNameToVsoTag(t, out vsoTag));
            if (vsoTag != "")
            {
                MilestoneCategory milestone = milestones.Where(m => m.Milestone_Category_Name.Equals(vsoTag)).FirstOrDefault();
                int MilestoneKey = milestone.MilestoneCategoryKey;

                int testscheduleKey = GetValue<int>(fields, "System.Id");
                int releaseKey = dict_testplan_releaseId[testscheduleKey];
                var release = releases.Where(r => r.VSO_ID == releaseKey).FirstOrDefault();

                if (release != null)
                {
                    //string areaPath = release.VSO_AreaPath;
                    string epicAreaPath = release.VSO_AreaPath;
                    var product = products.Where(p => p.Localization_VSO_Path != null && p.Localization_VSO_Path.Equals(epicAreaPath)).FirstOrDefault();
                    if (product != null)
                    {
                        int productKey = product.ProductKey;

                        string testscheduleName = GetValue<string>(fields, "System.Title");
                        DateTime? testscheduleStartDate = GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.StartDate");
                        DateTime? testscheduleEndDate = GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.FinishDate");
                        string testPlanAreaPath = GetValue<string>(fields, "System.AreaPath");
                        //string areaPath = GetValue<string>(fields, "System.AreaPath");
                        string vsoWebUrl = String.Format(@"https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_testManagement?planId={0}", testscheduleKey);
                        string sDeleted = GetValue<string>(fields, "System.State");
                        bool deleted = testPlanAreaPath == @"LOCALIZATION\_Trash_" ? true : false;

                        TestSchedule ex = testSchedules.Where(s => s.TestScheduleKey.Equals(testscheduleKey)).FirstOrDefault();
                        DateTime changedDate = GetValue<DateTime>(fields, "System.ChangedDate");
                        TestSchedule testplan = new TestSchedule()
                        {
                            ProductKey = productKey,
                            ReleaseKey = releaseKey,
                            TestScheduleKey = testscheduleKey,
                            TestSchedule_Name = testscheduleName,
                            TestSchedule_Start_Date = testscheduleStartDate.Value,
                            TestSchedule_End_Date = testscheduleEndDate.Value,
                            Vso_Web_Url = vsoWebUrl,
                            Deleted = deleted,
                            MilestoneCategoryKey = MilestoneKey,
                            VSO_ChangedDate = changedDate
                        };

                        if (ex == null)
                        {
                            skypeIntlPortfolioContext.Entry(testplan).State = EntityState.Added;
                            skypeIntlPortfolioContext.TestSchedules.Add(testplan);

                            totalAdded++;
                        }
                        else if (ex.VSO_ChangedDate == null || ex.VSO_ChangedDate < changedDate)
                        {
                            //testplan.TestScheduleKey = testscheduleKey;
                            //skypeIntlPortfolioContext.TestSchedules.Attach(testplan);
                            //skypeIntlPortfolioContext.Entry(testplan).State = System.Data.Entity.EntityState.Modified;

                            //mark assignedreosurce column as false
                            skypeIntlPortfolioContext.TestSchedules.Attach(testplan);
                            //skypeIntlPortfolioContext.Entry(testplan).State = System.Data.Entity.EntityState.Modified;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.ProductKey).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.ReleaseKey).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.TestScheduleKey).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.TestSchedule_Name).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.TestSchedule_Start_Date).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.TestSchedule_End_Date).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.Vso_Web_Url).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.Deleted).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.MilestoneCategoryKey).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.VSO_ChangedDate).IsModified = true;
                            skypeIntlPortfolioContext.Entry(testplan).Property(a => a.AssignedResources).IsModified = false;

                            totalUpdated++;
                        }
                    }
                    else
                    {
                        missingItems.Add(tt);
                    }
                }
                else
                {
                    missingItems.Add(tt);
                }
            }
        }
        skypeIntlPortfolioContext.SaveChanges();
        logger.LogMessage(string.Format("Updating db.....Item updated: {0}, item created: {1}.", totalUpdated, totalAdded));
    }

    public static bool TryParseCategoryNameToVsoTag(string milestoneCategoryName, out string vsoTag)
    {
        if (milestoneCategoryName != null)
        {
            var category = milestoneCategoryName.ToLower();
            var allowedValues = new Dictionary<string, string>
                {
                    { "loc_ready"      , "locready"},
                    { "loc_start"      , "locstart"},
                    { "loc_progressing", "progressing"},
                    { "loc_endgame"    , "endgame"},
                    { "loc_signoff"    , "signoff"},
                    { "loc_retro"      , "retro"},
                };
            if (allowedValues.ContainsKey(category))
            {
                vsoTag = allowedValues[category];
                return true;
            }
            else
            {
                vsoTag = "";
                return false;
            }
        }
        else
            vsoTag = "";
        return false;
    }
}
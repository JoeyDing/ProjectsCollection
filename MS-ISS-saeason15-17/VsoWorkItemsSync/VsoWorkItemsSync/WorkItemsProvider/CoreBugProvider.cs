using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsoApi.Rest;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    internal class CoreBugProvider : WorkItemProviderBase<CoreBugInfo>
    {
        public CoreBugProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public HashSet<CoreBugInfo> hashset_allItem { get; set; }

        public override JObject GetWorkItems()
        {
            var wiql = "SELECT [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] FROM WorkItemLinks WHERE (Source.[System.WorkItemType] = 'Bug' and Source.[System.State] <> '' and not Source.[System.IterationPath] under 'LOCALIZATION') and ([System.Links.LinkType] = 'System.LinkTypes.Dependency-Forward') and (Target.[System.WorkItemType] = 'Bug' and Target.[System.IterationPath] under 'LOCALIZATION') ORDER BY [System.Title], [System.Id] mode(MustContain)";
            JObject result = this.VsoContext.RunComplexQuery(null, wiql);

            var allCoreBugItems = new HashSet<CoreBugInfo>(new CoreBugComparer());
            HashSet<int> hashset_BugIds = new HashSet<int>();
            if (result["workItemRelations"] != null)
            {
                foreach (var coreBug in result["workItemRelations"])
                {
                    if (coreBug["source"] != null)
                    {
                        int locBugId = (int)coreBug["target"]["id"];
                        int coreBugId = (int)coreBug["source"]["id"];
                        hashset_BugIds.Add(coreBugId);
                        hashset_BugIds.Add(locBugId);
                        //
                        allCoreBugItems.Add(new CoreBugInfo
                        {
                            Core_Bug_ID = coreBugId,
                            Loc_Bug_ID = locBugId,
                        });
                    }
                }
            }

            //put Core Bug Items into property for the second method to use
            hashset_allItem = allCoreBugItems;
            //prepare bigger json to return
            JObject biggerJson = new JObject();
            JArray jArray = new JArray();
            biggerJson.Add("value", jArray);

            //paging
            int itemPerPage = 200;
            int count = hashset_BugIds.Count;
            int totalPage = count / itemPerPage;
            int remainingPage = (count % itemPerPage == 0) ? 0 : 1;
            totalPage = totalPage + remainingPage;
            List<int> list_bugIds = new List<int>();
            for (int i = 0; i < totalPage; i++)
            {
                list_bugIds = hashset_BugIds.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                var json_bugDetail = this.VsoContext.GetListOfWorkItemsByIDs(list_bugIds);
                if (json_bugDetail["value"] != null)
                {
                    jArray.Add(json_bugDetail["value"]);
                }
            }

            return biggerJson;
        }

        public override HashSet<CoreBugInfo> PrepareDbItems(JObject workItems)
        {
            var allCoreBugItems = hashset_allItem;
            //prepare dict to store bugid(key) bugInfos(value)
            Dictionary<int, string> dict_LocUpdate = new Dictionary<int, string>();
            Dictionary<int, BugInfo> dict = new Dictionary<int, BugInfo>();

            foreach (var bugs in workItems["value"])
                foreach (var bug in bugs)
                {
                    var fields = bug["fields"] as JObject;
                    int bugId = (int)bug["id"];
                    string bugState = base.GetValue<string>(fields, "System.State");
                    string bugAreaPath = base.GetValue<string>(fields, "System.AreaPath");
                    string bugProjectName = base.GetValue<string>(fields, "System.TeamProject");
                    string bugIterationPath = base.GetValue<string>(fields, "System.IterationPath");
                    string bugTitle = base.GetValue<string>(fields, "System.Title");
                    string assignedTo = base.GetValue<string>(fields, "System.AssignedTo");
                    string resolvedReason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason");
                    string createdBy = base.GetValue<string>(fields, "System.CreatedBy");
                    string priority = base.GetValue<string>(fields, "Skype.Priority");
                    string severity = base.GetValue<string>(fields, "Microsoft.VSTS.Common.Severity");
                    string issueType = base.GetValue<string>(fields, "Skype.IssueType");
                    string worldReadinessImpact = base.GetValue<string>(fields, "Skype.WorldReadinessImpact");
                    string localizationImpact = base.GetValue<string>(fields, "Skype.LocalizationImpact");
                    DateTime? createdDate = base.GetValue<DateTime?>(fields, "System.CreatedDate");
                    DateTime? closedDate = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ClosedDate");
                    DateTime? resolvedDate = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ResolvedDate");

                    dict.Add(bugId, new BugInfo
                    {
                        Bug_Status = bugState,
                        Project_Name = bugProjectName,
                        Project_Area_Path = bugAreaPath,
                        Project_Iteration_Path = bugIterationPath,
                        Bug_Title = bugTitle,
                        Bug_Assigned_To = assignedTo,
                        Bug_Reason = resolvedReason,
                        Bug_Created_By = createdBy,
                        Bug_Priority = priority,
                        Bug_Severity = severity,
                        Bug_Issue_Type = issueType,
                        Bug_World_Readiness_Impact = worldReadinessImpact,
                        Bug_Localization_Impact = localizationImpact,
                        Bug_Created_Date = createdDate,
                        Bug_Closed_Date = closedDate,
                        Bug_Resolved_Date = resolvedDate
                    });
                }
            //loop within the hashset<CoreBugs> and use either LocBugId or CoreBugId as a key to find the value(bugInfos) in the dict prepared above

            foreach (var item in allCoreBugItems)
            {
                BugInfo coreBugItem = dict[item.Core_Bug_ID];
                BugInfo locBugItem = dict[item.Loc_Bug_ID];

                item.Loc_Bug_Status = locBugItem.Bug_Status;
                item.Core_Bug_Status = coreBugItem.Bug_Status;
                item.Core_Project_Name = coreBugItem.Project_Name;
                item.Core_Project_Area_Path = coreBugItem.Project_Area_Path;
                item.Core_Project_Iteration_Path = coreBugItem.Project_Iteration_Path;
                item.Core_Bug_Title = coreBugItem.Bug_Title;
                item.Core_Bug_Assigned_To = coreBugItem.Bug_Assigned_To;
                item.Core_Bug_Reason = coreBugItem.Bug_Reason;
                item.Core_Bug_Created_By = coreBugItem.Bug_Created_By;
                item.Core_Bug_Priority = coreBugItem.Bug_Priority;
                item.Core_Bug_Severity = coreBugItem.Bug_Severity;
                item.Core_Bug_Issue_Type = coreBugItem.Bug_Issue_Type;
                item.Core_Bug_World_Readiness_Impact = coreBugItem.Bug_World_Readiness_Impact;
                item.Core_Bug_Localization_Impact = coreBugItem.Bug_Localization_Impact;
                item.Core_Bug_Created_Date = coreBugItem.Bug_Created_Date;
                item.Core_Bug_Closed_Date = coreBugItem.Bug_Closed_Date;
                item.Core_Bug_Resolved_Date = coreBugItem.Bug_Resolved_Date;

                if (locBugItem.Bug_Status != "Closed" && locBugItem.Bug_Status != "Resolved" && (coreBugItem.Bug_Status == "Resolved" || coreBugItem.Bug_Status == "Closed"))
                {
                    dict_LocUpdate.Add(item.Loc_Bug_ID, coreBugItem.Bug_Reason);
                    item.Loc_Bug_Status = "Resolved";
                }
                //if locbugStatus not euqal to corebugStatus then update loc bug status
                //if (dict[locBugId][0] != list_Core_Fileds[0])
                //{
                //    this.VsoContext.UpdateVsoWorkItem((int)locBugId, new Dictionary<string, string>() { { "System.State", list_Core_Fileds[0] } });
                //}
            }
            this.GetWorkItemsInParralel(dict_LocUpdate);

            return allCoreBugItems;
        }

        private void GetWorkItemsInParralel(Dictionary<int, string> dictLocUpdate)
        {
            var producer = new BlockingCollection<KeyValuePair<int, string>>();

            var failedCounter = new BlockingCollection<int>();
            var counter = new BlockingCollection<int>();

            foreach (var item in dictLocUpdate)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();

            var action = new Action(() =>
            {
                KeyValuePair<int, string> itemToProcess = new KeyValuePair<int, string>();
                while (producer.TryTake(out itemToProcess))
                {
                    BlockingCollection<int> list_testSuitIds = new BlockingCollection<int>();

                    int locBugId = itemToProcess.Key;
                    string reason = itemToProcess.Value;
                    try
                    {
                        this.VsoContext.UpdateVsoWorkItem(locBugId, new Dictionary<string, string>() { { "System.State", "Resolved" }, { "Skype.FixProvidedBy", "Core" }, { "Skype.RootCauseCategory", "Intl Design" }, { "Microsoft.VSTS.Common.ResolvedReason", reason } });

                        counter.TryAdd(0);
                        Console.WriteLine(DateTime.Now.ToString() + " " + counter.Count + "( failed:" + failedCounter.Count + " )");
                    }
                    catch (Exception)
                    {
                        failedCounter.TryAdd(0);
                    }
                }
            });

            //3 Start our process in parallel
            int totalThread = 10;
            //var p = new Action[totalThread];
            var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
            Parallel.Invoke(p);
        }

        public override HashSet<CoreBugInfo> UpdateDatabase(HashSet<CoreBugInfo> workItems)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //delete data from table
                        var deleteQuery = "truncate table CoreBugInfos";
                        int iRow = base.DeleteData(dbContext, dbContextTransaction.UnderlyingTransaction, deleteQuery);
                        //insert new data
                        base.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, workItems, "CoreBugInfos");
                        HashSet<CoreBugInfo> result = new HashSet<CoreBugInfo>();

                        dbContextTransaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
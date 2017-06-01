using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    public class CoreBugFromFeedbackProvider : WorkItemProviderBase<CoreBugInfosFromFeedback>
    {
        public CoreBugFromFeedbackProvider(DateTime? fromDate) : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            var wiql = "SELECT [System.Id], [System.State], [Microsoft.VSTS.Common.ResolvedReason], [Skype.Triaged], [System.CreatedDate], [System.Title], [System.AssignedTo], [Skype.IssueType], [Skype.Language], [Skype.HowFound], [Microsoft.VSTS.Common.Priority], [Microsoft.VSTS.Common.Severity], [System.AreaPath], [System.IterationPath], [Skype.Source], [System.Tags] FROM WorkItems WHERE [System.WorkItemType] = 'Bug' and [System.CreatedBy] = 'Defect Prevention <skdevlog@microsoft.com>' and ([Skype.WorldReadinessImpact] = 'Yes' or [Skype.LocalizationImpact] = 'Yes') ORDER BY [System.Id] DESC";
            JObject result = this.VsoContext.RunComplexQuery(null, wiql);
            return result;
        }

        public override HashSet<CoreBugInfosFromFeedback> PrepareDbItems(JObject workItems)
        {
            var allCoreBugItems = new HashSet<CoreBugInfosFromFeedback>(new CoreBugFromFeedbackComparer());
            List<int> coreBugIds = new List<int>();
            foreach (var item in workItems["workItems"])
            {
                int coreBugId = int.Parse(item["id"].ToString());
                coreBugIds.Add(coreBugId);
            }

            //paging
            int itemPerPage = 200;
            int count = coreBugIds.Count;
            int totalPage = count / itemPerPage;
            int remainingPage = (count % itemPerPage == 0) ? 0 : 1;
            totalPage = totalPage + remainingPage;
            List<int> list_bugIds = new List<int>();
            for (int i = 0; i < totalPage; i++)
            {
                list_bugIds = coreBugIds.Skip(i * itemPerPage).Take(itemPerPage).ToList();
                var json_bugDetail = this.VsoContext.GetListOfWorkItemsByIDs(list_bugIds);
                if (json_bugDetail["value"] != null)
                {
                    foreach (var bug in json_bugDetail["value"])
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

                        allCoreBugItems.Add(new CoreBugInfosFromFeedback
                        {
                            ID = bugId,
                            Status = bugState,
                            Project_Name = bugProjectName,
                            Project_Area_Path = bugAreaPath,
                            Project_Iteration_Path = bugIterationPath,
                            Title = bugTitle,
                            Assigned_To = assignedTo,
                            Reason = resolvedReason,
                            Created_By = createdBy,
                            Priority = priority,
                            Severity = severity,
                            Issue_Type = issueType,
                            World_Readiness_Impact = worldReadinessImpact,
                            Localization_Impact = localizationImpact,
                            Created_Date = createdDate,
                            Closed_Date = closedDate,
                            Resolved_Date = resolvedDate
                        });
                    }
                }
            }
            return allCoreBugItems;
        }

        public override HashSet<CoreBugInfosFromFeedback> UpdateDatabase(HashSet<CoreBugInfosFromFeedback> workItems)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //delete data from table
                        var deleteQuery = "truncate table CoreBugInfosFromFeedback";
                        int iRow = base.DeleteData(dbContext, dbContextTransaction.UnderlyingTransaction, deleteQuery);
                        //insert new data
                        base.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, workItems, "CoreBugInfosFromFeedback");
                        dbContextTransaction.Commit();
                        return null;
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
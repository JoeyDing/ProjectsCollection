using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    internal class TestPlanProvider : WorkItemProviderBase<TestPlanRevision>
    {
        public TestPlanProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            string[] fieldsToQuery =
            {
                "System.ID",
                "System.WorkItemType",
                "System.Title",
                "System.AssignedTo",
                "System.State",
                "System.Tags",
                "System.AreaPath",
                "System.CreatedBy",
                "System.CreatedDate",
                "System.ChangedBy",
                "System.ChangedDate",
                "System.IterationPath",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "System.RevisedDate",
                "Microsoft.VSTS.Scheduling.StartDate",
                "Microsoft.VSTS.Scheduling.FinishDate",
                "Microsoft.VSTS.Common.ResolvedReason"
            };

            var workItemsResult = base.VsoContext_Reporting_GetAllWorkItemRevisionsFromDate
                (
                  taskType: TaskTypes.TestPlan,
                  fieldsToQuery: fieldsToQuery,
                  pauseTimeBetweenBatchInSec: 1
                );

            return workItemsResult;
        }

        public override HashSet<TestPlanRevision> PrepareDbItems(JObject workItems)
        {
            //3. Convert json data to get a full TestPlan object
            var allItems = new HashSet<TestPlanRevision>(new RevisionItemComparer<TestPlanRevision>());
            var values = workItems["values"];
            foreach (var item in values)
            {
                var fields = item["fields"] as JObject;
                int id = base.GetValue<int>(fields, "System.Id");
                string tags = base.GetValue<string>(fields, "System.Tags");
                if (!string.IsNullOrEmpty(tags))
                {
                    tags = Tag.FilterTag(tags);
                }
                allItems.Add(new TestPlanRevision
                {
                    Rev = base.GetValue<int>((JObject)item, "rev"),
                    ID = id,
                    Work_Item_Type = base.GetValue<string>(fields, "System.WorkItemType"),
                    Title = base.GetValue<string>(fields, "System.Title"),
                    Assigned_To = base.GetValue<string>(fields, "System.AssignedTo"),
                    Iteration_Path = base.GetValue<string>(fields, "System.IterationPath"),
                    State = base.GetValue<string>(fields, "System.State"),
                    Area_Path = base.GetValue<string>(fields, "System.AreaPath"),
                    Changed_By = base.GetValue<string>(fields, "System.ChangedBy"),
                    Changed_Date = base.GetValue<DateTime?>(fields, "System.ChangedDate"),
                    Created_By = base.GetValue<string>(fields, "System.CreatedBy"),
                    Created_Date = base.GetValue<DateTime?>(fields, "System.CreatedDate"),
                    Team_Project = base.GetValue<string>(fields, "System.TeamProject"),
                    Tags = tags,
                    DevSignoff = base.GetValue<string>(fields, "Skype.DevSignoff"),
                    Description = base.GetValue<string>(fields, "System.Description"),
                    Reason = base.GetValue<string>(fields, "System.Reason"),
                    Revised_Date = base.GetValue<DateTime?>(fields, "System.RevisedDate"),
                    StartDate = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.StartDate"),
                    FinishDate = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.FinishDate"),
                    Resolved_Reason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason")
                });
            }
            return allItems;
        }

        public override HashSet<TestPlanRevision> UpdateDatabase(HashSet<TestPlanRevision> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                       query: "Select Id, Rev from TestPlanRevisions",
                                       convertFunction: (reader) =>
                                       {
                                           var testPlan = new TestPlanRevision();
                                           testPlan.ID = reader.GetInt64(0);
                                           testPlan.Rev = reader.GetInt32(1);
                                           return testPlan;
                                       },
                                       destinationTableName: "TestPlanRevisions",
                                       cleanAll: this.FromDate == null ? true : false);

            return result;
        }
    }
}
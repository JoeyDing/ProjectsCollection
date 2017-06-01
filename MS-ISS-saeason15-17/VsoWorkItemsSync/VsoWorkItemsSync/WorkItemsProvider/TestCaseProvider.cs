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
    internal class TestCaseProvider : WorkItemProviderBase<TestCaseRevision>
    {
        public TestCaseProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            string[] fieldsToQuery =
            {
                "System.ID",
                "System.WorkItemType",
                "Microsoft.VSTS.Common.Priority",
                "System.Title",
                "System.AssignedTo",
                "System.State",
                "System.Tags",
                "System.AreaPath",
                "System.ChangedBy",
                "System.ChangedDate",
                "System.CreatedBy",
                "System.CreatedDate",
                "Microsoft.VSTS.Common.ClosedBy",
                "Microsoft.VSTS.Common.ClosedDate",
                "System.IterationPath",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "System.RevisedDate",
                "Microsoft.VSTS.TCM.AutomationStatus",
                "Skype.TestCaseEstimate",
                "Skype.TestType",
                "Microsoft.VSTS.TCM.Steps",
                "Microsoft.VSTS.Common.ActivatedBy",
                "Microsoft.VSTS.Common.ActivatedDate",
                "Microsoft.VSTS.Common.StateChangeDate",
                "Skype.PSID",
                "Microsoft.VSTS.Common.ResolvedReason"
            };
            var workItemResult = base.VsoContext_Reporting_GetAllWorkItemRevisionsFromDate
                (
                  taskType: TaskTypes.TestCase,
                  fieldsToQuery: fieldsToQuery,
                  pauseTimeBetweenBatchInSec: 1
                );
            return workItemResult;
        }

        public override HashSet<TestCaseRevision> PrepareDbItems(JObject workItems)
        {
            //3. Convert json data to get a full TestCase object
            var allItems = new HashSet<TestCaseRevision>(new RevisionItemComparer<TestCaseRevision>());
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
                allItems.Add(new TestCaseRevision
                {
                    Rev = base.GetValue<int>((JObject)item, "rev"),
                    ID = id,
                    Work_Item_Type = base.GetValue<string>(fields, "System.WorkItemType"),
                    Skype_Priority = base.GetValue<string>(fields, "Microsoft.VSTS.Common.Priority"),
                    Title = base.GetValue<string>(fields, "System.Title"),
                    Assigned_To = base.GetValue<string>(fields, "System.AssignedTo"),
                    Iteration_Path = base.GetValue<string>(fields, "System.IterationPath"),
                    State = base.GetValue<string>(fields, "System.State"),
                    Area_Path = base.GetValue<string>(fields, "System.AreaPath"),
                    Changed_By = base.GetValue<string>(fields, "System.ChangedBy"),
                    Changed_Date = base.GetValue<DateTime?>(fields, "System.ChangedDate"),
                    Created_By = base.GetValue<string>(fields, "System.CreatedBy"),
                    Created_Date = base.GetValue<DateTime?>(fields, "System.CreatedDate"),
                    Closed_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ClosedBy"),
                    Closed_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ClosedDate"),
                    Activated_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ActivatedBy"),
                    Activated_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ActivatedDate"),
                    State_Change_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.StateChangeDate"),
                    Team_Project = base.GetValue<string>(fields, "System.TeamProject"),
                    Tags = tags,
                    DevSignoff = base.GetValue<string>(fields, "Skype.DevSignoff"),
                    Description = base.GetValue<string>(fields, "System.Description"),
                    Reason = base.GetValue<string>(fields, "System.Reason"),
                    Revised_Date = base.GetValue<DateTime?>(fields, "System.RevisedDate"),
                    Automation_Status = base.GetValue<string>(fields, "Microsoft.VSTS.TCM.AutomationStatus"),
                    TestCase_Estimate = base.GetValue<int>(fields, "Skype.TestCaseEstimate"),
                    Test_Type = base.GetValue<string>(fields, "Skype.TestType"),
                    Steps = base.GetValue<string>(fields, "Microsoft.VSTS.TCM.Steps"),
                    PS_ID = (base.GetValue<int>(fields, "Skype.PSID")) == 0 ? (int?)null : (base.GetValue<int>(fields, "Skype.PSID")),
                    Resolved_Reason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason")
                });
            }
            return allItems;
        }

        public override HashSet<TestCaseRevision> UpdateDatabase(HashSet<TestCaseRevision> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                       query: "Select Id, Rev from TestCaseRevisions",
                                       convertFunction: (reader) =>
                                       {
                                           var testCase = new TestCaseRevision();
                                           testCase.ID = reader.GetInt64(0);
                                           testCase.Rev = reader.GetInt32(1);
                                           return testCase;
                                       },
                                       destinationTableName: "TestCaseRevisions",
                                       cleanAll: this.FromDate == null ? true : false);

            return result;
        }
    }
}
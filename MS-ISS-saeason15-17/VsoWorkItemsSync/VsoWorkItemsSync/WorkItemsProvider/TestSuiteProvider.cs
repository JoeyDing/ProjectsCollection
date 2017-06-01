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
    internal class TestSuiteProvider : WorkItemProviderBase<TestSuiteRevision>
    {
        public TestSuiteProvider(DateTime? fromDate)
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
                "Microsoft.VSTS.TCM.TestSuiteType",
                "Microsoft.VSTS.TCM.TestSuiteAudit",
                "Microsoft.VSTS.TCM.QueryText",
                "Microsoft.VSTS.Common.ResolvedReason"
            };
            var workItemResult = base.VsoContext_Reporting_GetAllWorkItemRevisionsFromDate
                (
                  taskType: TaskTypes.TestSuite,
                  fieldsToQuery: fieldsToQuery,
                  pauseTimeBetweenBatchInSec: 1
                );
            return workItemResult;
        }

        public override HashSet<TestSuiteRevision> PrepareDbItems(JObject workItems)
        {
            //3. Convert json data to get a full TestSuite object
            var allItems = new HashSet<TestSuiteRevision>(new RevisionItemComparer<TestSuiteRevision>());
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
                allItems.Add(new TestSuiteRevision
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
                    Test_Suite_Type = base.GetValue<string>(fields, "Microsoft.VSTS.TCM.TestSuiteType"),
                    Test_Suite_Audit = base.GetValue<string>(fields, "Microsoft.VSTS.TCM.TestSuiteAudit"),
                    Query_Text = base.GetValue<string>(fields, "Microsoft.VSTS.TCM.QueryText"),
                    Resolved_Reason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason")
                });
            }
            return allItems;
        }

        public override HashSet<TestSuiteRevision> UpdateDatabase(HashSet<TestSuiteRevision> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                       query: "Select Id, Rev from TestSuiteRevisions",
                                       convertFunction: (reader) =>
                                       {
                                           var testSuite = new TestSuiteRevision();
                                           testSuite.ID = reader.GetInt64(0);
                                           testSuite.Rev = reader.GetInt32(1);
                                           return testSuite;
                                       },
                                       destinationTableName: "TestSuiteRevisions",
                                       cleanAll: true);

            return result;
        }
    }
}
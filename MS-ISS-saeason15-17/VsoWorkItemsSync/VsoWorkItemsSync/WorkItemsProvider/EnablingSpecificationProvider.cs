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
    public class EnablingSpecificationProvider : WorkItemProviderBase<EnablingSpecificationRevision>
    {
        public EnablingSpecificationProvider(DateTime? fromDate)
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
                "Microsoft.VSTS.Common.ResolvedBy",
                "Microsoft.VSTS.Common.ResolvedDate",
                "System.ChangedBy",
                "System.ChangedDate",
                "Microsoft.VSTS.Common.ClosedBy",
                "System.CreatedBy",
                "System.CreatedDate",
                "Microsoft.VSTS.Common.ClosedDate",
                "Microsoft.VSTS.Scheduling.StartDate",
                "Microsoft.VSTS.Scheduling.DueDate",
                "System.IterationPath",
                "Skype.Priority",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "Skype.LocalizationImpact",
                "Skype.WorldReadinessImpact",
                "Skype.Initiator",
                "Skype.ReviewRequired",
                "Skype.Keywords",
                "Skype.Blocking",
                "Microsoft.VSTS.Scheduling.StoryPoints",
                "Skype.EstimateTShirt",
                "Skype.Release",
                "Skype.EngineeringEffort",
                "Microsoft.VSTS.Common.ResolvedReason"
            };

            var workItemsResult = base.VsoContext_Reporting_GetAllWorkItemRevisionsFromDate
                (
                  taskType: TaskTypes.EnablingSpecification,
                  fieldsToQuery: fieldsToQuery,
                  pauseTimeBetweenBatchInSec: 1
                );

            return workItemsResult;
        }

        public override HashSet<EnablingSpecificationRevision> PrepareDbItems(JObject workItems)
        {
            //3. Convert json data to get a full EnablingSpecification object
            var allItems = new HashSet<EnablingSpecificationRevision>(new RevisionItemComparer<EnablingSpecificationRevision>());
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
                allItems.Add(new EnablingSpecificationRevision
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
                    Skype_Priority = base.GetValue<string>(fields, "Skype.Priority"),
                    Team_Project = base.GetValue<string>(fields, "System.TeamProject"),
                    Tags = tags,
                    Closed_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ClosedBy"),
                    Closed_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ClosedDate"),
                    Start_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.StartDate"),
                    Due_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.DueDate"),
                    DevSignoff = base.GetValue<string>(fields, "Skype.DevSignoff"),
                    LocalizationImpact = base.GetValue<string>(fields, "Skype.LocalizationImpact"),
                    WorldReadinessImpact = base.GetValue<string>(fields, "Skype.WorldReadinessImpact"),
                    Description = base.GetValue<string>(fields, "System.Description"),
                    Resolved_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedBy"),
                    Resolved_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ResolvedDate"),
                    Reason = base.GetValue<string>(fields, "System.Reason"),
                    Initiator = base.GetValue<string>(fields, "Skype.Initiator"),
                    Review_Required = base.GetValue<string>(fields, "Skype.ReviewRequired"),
                    Keywords = base.GetValue<string>(fields, "Skype.Keywords"),
                    Blocking = base.GetValue<string>(fields, "Skype.Blocking"),
                    Estimate_Points = base.GetValue<string>(fields, "Microsoft.VSTS.Scheduling.StoryPoints"),
                    Estimate_TShirt = base.GetValue<string>(fields, "Skype.EstimateTShirt"),
                    Release = base.GetValue<string>(fields, "Skype.Release"),
                    Engineering_Effort = base.GetValue<string>(fields, "Skype.EngineeringEffort"),
                    Resolved_Reason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason")
                });
            }
            return allItems;
        }

        public override HashSet<EnablingSpecificationRevision> UpdateDatabase(HashSet<EnablingSpecificationRevision> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                       query: "Select Id, Rev from EnablingSpecificationRevisions",
                                       convertFunction: (reader) =>
                                       {
                                           var enablingSpec = new EnablingSpecificationRevision();
                                           enablingSpec.ID = reader.GetInt64(0);
                                           enablingSpec.Rev = reader.GetInt32(1);
                                           return enablingSpec;
                                       },
                                       destinationTableName: "EnablingSpecificationRevisions",
                                       cleanAll: this.FromDate == null ? true : false);

            return result;
        }
    }
}
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
    public class BugProvider : WorkItemProviderBase<BugRevision>
    {
        public BugProvider(DateTime? fromDate)
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
                "Skype.Language",
                "Skype.BranchFound",
                "Skype.BuildFound",
                "System.ChangedBy",
                "System.ChangedDate",
                "Microsoft.VSTS.Common.ClosedBy",
                "System.CreatedBy",
                "System.CreatedDate",
                "Microsoft.VSTS.Common.ClosedDate",
                "Microsoft.VSTS.Scheduling.StartDate",
                "Microsoft.VSTS.Scheduling.DueDate",
                "Skype.FixProvidedBy",
                "Skype.FixedBuild",
                "Skype.HowFound",
                "Skype.IssueType",
                "System.IterationPath",
                "Skype.Keywords",
                "Skype.PSDatabase",
                "Skype.PSID",
                "Microsoft.VSTS.Common.Severity",
                "Skype.Priority",
                "Skype.Source",
                "System.TeamProject",
                "Skype.RootCauseCategory",
                "System.Reason",
                "Microsoft.VSTS.Common.ResolvedReason",
                //new fields 07/2016
                "Skype.IssueSubtype",
                "Skype.Triaged",
                "Skype.EstimatedTestImpact",
                "Skype.FabricProject",
                "Skype.FileName",
                "Skype.ResourceID",
                "Skype.SourceString",
                "Skype.OriginalTranslation",
                "Skype.SuggestedTranslation",
                "Skype.FixType",
            };

            var allBugsResult = base.VsoContext_Reporting_GetAllWorkItemRevisionsFromDate
                (
                  taskType: TaskTypes.Bug,
                  fieldsToQuery: fieldsToQuery,
                  pauseTimeBetweenBatchInSec: 1
                );

            return allBugsResult;
        }

        public override HashSet<BugRevision> PrepareDbItems(JObject workItems)
        {
            //Convert json data to get a full bug object
            var allBugs = new HashSet<BugRevision>(new RevisionItemComparer<BugRevision>());
            var values = workItems["values"];
            foreach (var item in values)
            {
                var fields = item["fields"] as JObject;
                int id = base.GetValue<int>(fields, "System.Id");
                //filter out the same tagName
                string tags = base.GetValue<string>(fields, "System.Tags");

                if (!string.IsNullOrEmpty(tags))
                {
                    tags = Tag.FilterTag(tags);
                }

                allBugs.Add(new BugRevision
                {
                    //Mandatory items
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
                    Severity = base.GetValue<string>(fields, "Microsoft.VSTS.Common.Severity"),
                    Skype_Priority = base.GetValue<string>(fields, "Skype.Priority"),
                    Team_Project = base.GetValue<string>(fields, "System.TeamProject"),
                    //Non-mandatory
                    Branch_Found = base.GetValue<string>(fields, "Skype.BranchFound"),
                    Build_Found = base.GetValue<string>(fields, "Skype.BuildFound"),
                    Tags = tags,
                    Closed_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ClosedBy"),
                    Closed_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ClosedDate"),
                    Start_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.StartDate"),
                    Due_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Scheduling.DueDate"),
                    Fix_Provided_By = base.GetValue<string>(fields, "Skype.FixProvidedBy"),
                    Fixed_Build = base.GetValue<string>(fields, "Skype.FixedBuild"),
                    How_Was_Found = base.GetValue<string>(fields, "Skype.HowFound"),
                    Issue_Type = base.GetValue<string>(fields, "Skype.IssueType"),
                    Keywords = base.GetValue<string>(fields, "Skype.Keywords"),
                    Language = base.GetValue<string>(fields, "Skype.Language"),
                    PS_Database = base.GetValue<string>(fields, "Skype.PSDatabase"),
                    PS_ID = base.GetValue<string>(fields, "Skype.PSID"),
                    Resolved_By = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedBy"),
                    Resolved_Date = base.GetValue<DateTime?>(fields, "Microsoft.VSTS.Common.ResolvedDate"),
                    Source = base.GetValue<string>(fields, "Skype.Source"),
                    Root_Cause_Category = base.GetValue<string>(fields, "Skype.RootCauseCategory"),
                    Reason = base.GetValue<string>(fields, "System.Reason"),
                    Resolved_Reason = base.GetValue<string>(fields, "Microsoft.VSTS.Common.ResolvedReason"),

                    Issue_Subtype = base.GetValue<string>(fields, "Skype.IssueSubtype"),
                    Triaged = base.GetValue<string>(fields, "Skype.Triaged"),
                    Estimated_Test_Impact = base.GetValue<int>(fields, "Skype.EstimatedTestImpact"),
                    Fabric_Project = base.GetValue<string>(fields, "Skype.FabricProject"),
                    File_Name = base.GetValue<string>(fields, "Skype.FileName"),
                    Resource_ID = base.GetValue<string>(fields, "Skype.ResourceID"),
                    Source_String = base.GetValue<string>(fields, "Skype.SourceString"),
                    Original_Translation = base.GetValue<string>(fields, "Skype.OriginalTranslation"),
                    Suggested_Translation = base.GetValue<string>(fields, "Skype.SuggestedTranslation"),
                    Fix_Type = base.GetValue<string>(fields, "Skype.FixType"),
                });
            }
            return allBugs;
        }

        public override HashSet<BugRevision> UpdateDatabase(HashSet<BugRevision> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                        query: "Select Id, Rev from BugRevisions",
                                        convertFunction: (reader) =>
                                        {
                                            var bug = new BugRevision();
                                            bug.ID = reader.GetInt64(0);
                                            bug.Rev = reader.GetInt32(1);
                                            return bug;
                                        },
                                        destinationTableName: "BugRevisions",
                                        cleanAll: this.FromDate == null ? true : false);

            return result;
        }
    }
}
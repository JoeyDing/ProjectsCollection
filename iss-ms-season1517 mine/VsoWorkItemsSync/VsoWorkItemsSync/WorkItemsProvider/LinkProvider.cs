using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    public class LinkProvider : WorkItemProviderBase<WorkItemLink>
    {
        public LinkProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            var allLinksResult = base.VsoContext_Reporting_GetAllRelationsFromDate(2);

            return allLinksResult;
        }

        public override HashSet<WorkItemLink> PrepareDbItems(JObject workItems)
        {
            var allLinks = new HashSet<WorkItemLink>(new LinkComparer());
            var values = workItems["values"];
            foreach (var item in values)
            {
                var i = new WorkItemLink
                {
                    Source_ID = (int)item["sourceId"],
                    Target_ID = (int)item["targetId"],
                    Changed_Date = (DateTime)item["changedDate"],
                    Is_Active = (bool)item["isActive"],
                    Link_Type = (string)item["rel"],
                };
                allLinks.Add(i);
            }
            return allLinks;
        }

        public override HashSet<WorkItemLink> UpdateDatabase(HashSet<WorkItemLink> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                         query: "Select [Source Id], [Target Id], [Changed Date], [Link Type],[Is Active] from WorkItemLinks",
                                         convertFunction: (reader) =>
                                         {
                                             var item = new WorkItemLink();
                                             item.Source_ID = reader.GetInt64(0);
                                             item.Target_ID = reader.GetInt64(1);
                                             item.Changed_Date = reader.GetDateTime(2);
                                             item.Link_Type = reader.GetString(3);
                                             item.Is_Active = reader.GetBoolean(4);
                                             return item;
                                         },
                                         destinationTableName: "WorkItemLinks");

            return result;
        }
    }
}
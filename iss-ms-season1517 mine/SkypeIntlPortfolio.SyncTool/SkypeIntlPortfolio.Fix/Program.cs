using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Fix
{
    internal class Program
    {
        private class WorkItem
        {
            public int ID;
            public string Title;
            public string AreaPath;
            public string IterationPath;
            public string WorkItemType;
            public List<string> Tags;
            public DateTime DueDate;
            public DateTime LocStartDate_from_tag;
            public DateTime LocStartDate_from_startdate_field;
            public string Family;
            public string ProductName;
            public DateTime ChangedDate;
            public string AssignedTo;
            public string State;
            public string Url;
        }

        private static void Main(string[] args)
        {
            //-----1 copy all the start date from epic title that contains 'Loc_Release" and "Loc_Release StartDate" to a list


            string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];

            string projectName = "LOCALIZATION";
            var vsoContext = new VsoContext(vsoRootAccount, authenticationKey);
            // 1.2 Run query to get interesting releases
            var wiql = "SELECT [System.Id]"
                        + " FROM WorkItems"
                        + " WHERE [System.WorkItemType] = 'Epic'"
                        + " and [System.AreaPath] under '" + projectName + "'"
                        + " and [System.Tags] contains 'Loc_Release'"
                        + " and [Microsoft.VSTS.Scheduling.DueDate] <> ''";

            //Logger.LogMessage("Querying Epics....");
            var vsoIds = vsoContext.RunQuery(projectName, wiql);

            var allWorkItems = new Dictionary<int, WorkItem>();

            if (vsoIds.Any())
            {
                //query the work items (with column) by page as there is a limit of 200 items per query with vso apis
                int pageSize = 200;
                int totalPage = vsoIds.Count / pageSize + (vsoIds.Count % pageSize > 0 ? 1 : 0);
                for (int i = 1; i <= totalPage; i++)
                {
                    var idsToQuery = vsoIds.Keys.Skip(pageSize * (i - 1)).Take(pageSize);
                    var jsonResult = vsoContext.GetListOfWorkItemsByIDs(idsToQuery, new string[] { "System.Id", "System.Tags", "Microsoft.VSTS.Scheduling.StartDate", "Microsoft.VSTS.Scheduling.DueDate", "System.CreatedDate" });

                    foreach (var c in jsonResult["value"])
                    {
                        int id = int.Parse((string)c["fields"]["System.Id"]);
                        //check that a locStartDate is specified and it is valid
                        var tags = ((string)c["fields"]["System.Tags"]).Split(new char[] { ';' }).ToList();
                        var locStart_from_tag = tags.FirstOrDefault(x => x.Trim().StartsWith("Loc_ReleaseStartDate:"));
                        //var locStartDateFrom_startDateField = DateTime.Parse((string)c["fields"]["Microsoft.VSTS.Scheduling.StartDate"]);

                        DateTime finalLocstartDate;
                        DateTime locstartDateOut;
                        //if no StartDate tag is indicated, use the Created data of Epic
                        if (locStart_from_tag != null && DateTime.TryParse(locStart_from_tag.Split(new string[] { "Loc_ReleaseStartDate:" }, StringSplitOptions.None).Last(), out locstartDateOut))
                        {
                            finalLocstartDate = locstartDateOut;
                        }
                        else
                        {
                            var createdDate = DateTime.Parse((string)c["fields"]["System.CreatedDate"]);
                            //use ".Date" so that LocStart Date starts at "00:00" on the same date
                            finalLocstartDate = createdDate.Date;
                        }

                        //check that due date is bigger than locStartDate
                        var dueDate = DateTime.Parse((string)c["fields"]["Microsoft.VSTS.Scheduling.DueDate"]).AddDays(1).AddSeconds(-1);
                        if (dueDate > finalLocstartDate)
                        {
                            //int id = int.Parse((string)c["fields"]["System.Id"]);
                            var url = string.Format("{0}/DefaultCollection/{1}/_workitems/edit/{2}", vsoContext.VsoUrl, projectName, id);

                            //convert json result to anonymous type
                            allWorkItems.Add(id, new WorkItem
                            {
                                ID = id,
                                LocStartDate_from_tag = finalLocstartDate,
                            });
                        }
                    }
                }
            }

            //-----2 to fill up the start date fields with the date extracted from the tags

            var vsoSet = new HashSet<int>(allWorkItems.Keys);
            foreach(int epicID in vsoSet)
            {
                var updatedEpic = vsoContext.UpdateVsoWorkItem(
                               id: epicID,
                               fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.StartDate", allWorkItems[epicID].LocStartDate_from_tag.ToString()}
                                });
            }
        }

        private static void Patch1()
        {
            //load vso context
            string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];
            var vsoContext = new VsoContext(vsoRootAccount, authenticationKey);

            //query wrong vso data
            var wiql = "SELECT [System.Id]"
                               + " FROM WorkItems"
                               + " WHERE [System.WorkItemType] = 'Epic'"
                               + " and [System.AreaPath] under 'LOCALIZATION'"
                               + " and [System.Tags] contains 'Loc_Release'"
                               + " and [Microsoft.VSTS.Scheduling.DueDate] <> ''";

            var vsoIds = vsoContext.RunQuery("LOCALIZATION", wiql);
            var allWrongItems = new List<WorkItem>();
            if (vsoIds.Any())
            {
                //query the work items (with column) by page as there is a limit of 200 items per query with vso apis
                int pageSize = 200;
                int totalPage = vsoIds.Count / pageSize + (vsoIds.Count % pageSize > 0 ? 1 : 0);
                for (int i = 1; i <= totalPage; i++)
                {
                    var idsToQuery = vsoIds.Keys.Skip(pageSize * (i - 1)).Take(pageSize);
                    var jsonResult = vsoContext.GetListOfWorkItemsByIDs(idsToQuery, new string[] { "System.Id", "System.Title", "System.Tags", "System.AreaPath", "Microsoft.VSTS.Scheduling.DueDate" });

                    foreach (var c in jsonResult["value"])
                    {
                        int id = int.Parse((string)c["fields"]["System.Id"]);

                        //check that a locStartDate is specified and it is valid
                        var tags = ((string)c["fields"]["System.Tags"]).Split(new char[] { ';' }).ToList();
                        var locStart = tags.FirstOrDefault(x => x.Trim().StartsWith("Loc_ReleaseStartDate:"));
                        DateTime locstartDate;
                        if (locStart != null && DateTime.TryParse(locStart.Split(new string[] { "Loc_ReleaseStartDate:" }, StringSplitOptions.None).Last(), out locstartDate))
                        {
                            //check that due date is bigger than locStartDate
                            var dueDate = DateTime.Parse((string)c["fields"]["Microsoft.VSTS.Scheduling.DueDate"]).AddDays(1).AddSeconds(-1);
                            if (dueDate < locstartDate)
                            {
                                //convert json result to anonymous type
                                allWrongItems.Add(new WorkItem
                                {
                                    ID = id,
                                    Tags = tags,
                                    DueDate = dueDate,
                                    LocStartDate_from_tag = locstartDate,
                                    Title = (string)c["fields"]["System.Title"],
                                    AreaPath = (string)c["fields"]["System.AreaPath"],
                                });
                            }
                        }
                    }
                }

                var allAggregate = allWrongItems.Select(c => c.ToString()).Aggregate((a, b) => a + Environment.NewLine + b);

                //get data from db
                var allWrongKeys = allWrongItems.Select(c => c.ID).ToList();
                var dbContext = new SkypeIntlPortfolioContext();
                var dbItems = dbContext.Releases.Where(c => allWrongKeys.Contains(c.VSO_ID)).ToDictionary(k => k.VSO_ID, v => v);

                foreach (var item in allWrongItems)
                {
                    if (dbItems.ContainsKey(item.ID))
                    {
                        var dbItem = dbItems[item.ID];
                        var goodStart = dbItem.VSO_LocStartDate.Date;
                        var goodEnd = dbItem.VSO_DueDate.Date.AddDays(1).AddSeconds(-1);
                        var tags = item.Tags;
                        tags.Remove(item.Tags.FirstOrDefault(c => c.Contains("Loc_ReleaseStartDate")));
                        string locStartTag = string.Format("Loc_ReleaseStartDate:{0}", goodStart.ToString("M/d/yy"));
                        tags.Add(locStartTag);
                        string TagString = tags.Aggregate((a, b) => a + "; " + b);
                        var updatedEpic = vsoContext.UpdateVsoWorkItem(
                             id: item.ID,
                             fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.DueDate", goodEnd.ToString()},
                                        {"System.Tags", TagString}
                             });
                    }
                    else
                    {
                        var goodStart = item.DueDate.Date;
                        var goodEnd = item.LocStartDate_from_tag.Date.AddDays(1).AddSeconds(-1);
                        var tags = item.Tags;
                        tags.Remove(item.Tags.FirstOrDefault(c => c.Contains("Loc_ReleaseStartDate")));
                        string locStartTag = string.Format("Loc_ReleaseStartDate:{0}", goodStart.ToString("M/d/yy"));
                        tags.Add(locStartTag);
                        string TagString = tags.Aggregate((a, b) => a + "; " + b);
                        var updatedEpic = vsoContext.UpdateVsoWorkItem(
                             id: item.ID,
                             fields: new Dictionary<string, string>{
                                        {"Microsoft.VSTS.Scheduling.DueDate", goodEnd.ToString()},
                                        {"System.Tags", TagString}
                             });
                    }
                }
            }
        }
    }
}
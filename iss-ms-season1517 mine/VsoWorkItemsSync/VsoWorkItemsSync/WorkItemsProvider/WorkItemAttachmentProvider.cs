using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;
using VsoApi.Rest;
using System.Collections.Concurrent;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    public class WorkItemAttachmentProvider : WorkItemProviderBase<WorkItemAttachment>
    {
        public WorkItemAttachmentProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public IEnumerable<int> workItemIDs;

        public override JObject GetWorkItems()
        {
            workItemIDs = base.GetIDs_WorkItemContainingAttachments_ByQuery();

            return null;
        }

        public override HashSet<WorkItemAttachment> PrepareDbItems(JObject workItems)
        {
            VsoContext vsoContext = base.VsoContext;
            int itemsPerPage = 200;
            int totalItems = workItemIDs.Count();
            int totalPages = totalItems / itemsPerPage;
            int remaining = totalItems % itemsPerPage;
            int remaningPages = (remaining == 0) ? 0 : 1;
            totalPages = totalPages + remaningPages;

            var allWorkItemAttachments = new HashSet<WorkItemAttachment>(new WorkItemAttachmentComparer());
            var producer = new BlockingCollection<BlockingCollection<int>>();
            for (int i = 0; i < totalPages; i++)
            {
                var setOfWorkItemIDs = workItemIDs.Skip(i * itemsPerPage).Take(itemsPerPage);
                var temp = new BlockingCollection<int>();
                foreach (var id in setOfWorkItemIDs)
                {
                    temp.TryAdd(id);
                }
                producer.Add(temp);
            }
            producer.CompleteAdding();

            Action a = () =>
            {
                BlockingCollection<int> itemsToProcess = new BlockingCollection<int>();
                while (producer.TryTake(out itemsToProcess))
                {
                    var json = vsoContext.GetListOfWorkItemsByIDs(itemsToProcess);
                    foreach (var item in json["value"])
                    {
                        int workItemID = int.Parse(item["id"].ToString());
                        string workItemType = item["fields"]["System.WorkItemType"].ToString();
                        if (item["relations"] != null)
                        {
                            foreach (var relation in item["relations"])
                            {
                                if (relation["rel"].ToString() == "AttachedFile")
                                {
                                    string attachmentName = relation["attributes"]["name"].ToString();
                                    string attachmentComment = relation["attributes"]["comment"] != null ? relation["attributes"]["comment"].ToString() : null;
                                    string attachmentUrl = relation["url"].ToString();
                                    allWorkItemAttachments.Add(new WorkItemAttachment
                                    {
                                        WorkItemID = workItemID,
                                        WorkItemType = workItemType,
                                        AttachmentName = attachmentName,
                                        AttachmentUrl = attachmentUrl,
                                        AttachmentComment = attachmentComment
                                    });
                                }
                            }
                        }
                    }
                }
            };

            int totalThread = 40;
            Action[] array = Enumerable.Range(0, totalThread).Select(c => a).ToArray();
            Parallel.Invoke(array);

            return allWorkItemAttachments;
        }

        public override HashSet<WorkItemAttachment> UpdateDatabase(HashSet<WorkItemAttachment> workItems)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //delete data from table
                        var deleteQuery = "truncate table WorkItemAttachment";
                        int iRow = base.DeleteData(dbContext, dbContextTransaction.UnderlyingTransaction, deleteQuery);
                        //insert new data
                        base.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, workItems, "WorkItemAttachment");
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
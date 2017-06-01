using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using VsoApi.Rest;
using VsoWorkItemsSync.Helper;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public abstract class WorkItemProviderBase<T> //where T : IVsoItem
    {
        protected VsoContext VsoContext { get; private set; }

        protected readonly string ProjectName = "LOCALIZATION";

        public DateTime? FromDate { get; set; }

        public WorkItemProviderBase(DateTime? fromDate)
        {
            this.FromDate = fromDate;
            // 1 Get authentication key from AppConfig and use it to instatiate the VSO rest API Wrapper
            string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];
            VsoContext = new VsoContext(vsoRootAccount, authenticationKey);
        }

        protected HashSet<T> FetchData(VsoWorkItemsContext dbContext, DbTransaction transaction, string query, Func<System.Data.IDataReader, T> convertFunction)
        {
            return Utils.FetchData<T>(dbContext, transaction, query, convertFunction);
        }

        protected int DeleteData(VsoWorkItemsContext dbContext, DbTransaction transaction, string query)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 3600;
            cmd.Transaction = (SqlTransaction)transaction;
            cmd.Connection = (SqlConnection)dbContext.Database.Connection;
            cmd.CommandText = query;
            int iRow = cmd.ExecuteNonQuery();
            return iRow;
        }

        protected void InsertData(VsoWorkItemsContext dbContext, DbTransaction transaction, HashSet<T> newItems, string destinationTableName)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                (SqlConnection)dbContext.Database.Connection,
                SqlBulkCopyOptions.TableLock,
                (SqlTransaction)transaction))
            {
                bulkCopy.BulkCopyTimeout = 3600;
                bulkCopy.BatchSize = 1000;
                bulkCopy.DestinationTableName = destinationTableName;
                bulkCopy.WriteToServer(newItems.CopyToDataTable());
            }
        }

        protected List<int> GetIDs_WorkItemContainingAttachments_ByQuery()
        {
            string[] workItemTypes = { "Bug", "Enabling Specification", "Epic", "Impediment", "Task", "Test Case", "Test Plan", "Test Suite" };
            List<int> ids = GetIdsToUrlsOfWorkItems(workItemTypes);
            return ids;
        }

        private List<int> GetIdsToUrlsOfWorkItems(IEnumerable<string> workItemTypes)
        {
            var result = new List<int>();
            string query = "SELECT [System.Id], [System.WorkItemType], [System.Title], [System.State], [System.AreaPath], [System.IterationPath], [System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project and [System.AttachedFileCount] > 0 and [System.WorkItemType] = '{0}' ORDER BY [System.ChangedDate] DESC";
            foreach (var workItemType in workItemTypes)
            {
                string wiql = string.Format(query, workItemType);
                var dict = VsoContext.RunQuery("LOCALIZATION", wiql);
                result.AddRange(dict.Keys);
            }

            return result;
        }

        protected HashSet<T> InsertNewVsoItemsInDB(HashSet<T> vsoWorkItems, string query, Func<System.Data.IDataReader, T> convertFunction, string destinationTableName, bool cleanAll = false)
        {
            if (string.IsNullOrWhiteSpace(destinationTableName))
                throw new ArgumentException("destinationTableName cannot be null.");

            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        if (this.FromDate == null && cleanAll)
                        {
                            // 1 - Truncate data
                            string truncateQuery = string.Format("truncate table {0}", destinationTableName);
                            this.DeleteData(dbContext, dbContextTransaction.UnderlyingTransaction, truncateQuery);

                            // 2 - Insert new items
                            this.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, vsoWorkItems, destinationTableName);
                        }
                        else
                        {
                            // 1 - Fetch data
                            var dbItems = this.FetchData(dbContext, dbContextTransaction.UnderlyingTransaction, query, convertFunction);
                            // 2 - Get only work items that doesn't exist in db
                            vsoWorkItems.ExceptWith(dbItems);

                            // 3 - Insert new items
                            this.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, vsoWorkItems, destinationTableName);
                        }

                        dbContextTransaction.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            return vsoWorkItems;
        }

        protected T GetValue<T>(JObject container, string key)
        {
            return container[key] != null ? (T)container[key].ToObject<T>() : default(T);
        }

        protected JObject VsoContext_Reporting_GetAllWorkItemRevisionsFromDate(TaskType taskType, string[] fieldsToQuery, int pauseTimeBetweenBatchInSec)
        {
            JObject result = null;
            if (FromDate != null)
            {
                result = VsoContext.Reporting_GetAllWorkItemRevisionsFromDate(ProjectName, taskType, FromDate, fieldsToQuery, pauseTimeBetweenBatchInSec);
            }
            else
            {
                result = VsoContext.Reporting_GetAllWorkItemRevisions(ProjectName, taskType, fieldsToQuery, pauseTimeBetweenBatchInSec);
            }
            return result;
        }

        protected Dictionary<int, Dictionary<int, JObject>> VsoContext_Reporting_GetDictAllWorkItemRevisionsFromDate(TaskType taskType, string[] fieldsToQuery, int pauseTimeBetweenBatchInSec)
        {
            var result = new Dictionary<int, Dictionary<int, JObject>>();
            if (FromDate != null)
            {
                result = VsoContext.Reporting_GetDictAllWorkItemRevisions_FromDate(ProjectName, taskType, FromDate, fieldsToQuery, pauseTimeBetweenBatchInSec);
            }
            //else
            //{
            //    result = VsoContext.Reporting_GetAllWorkItemRevisions(ProjectName, taskType, fieldsToQuery, pauseTimeBetweenBatchInSec);
            //}
            return result;
        }

        protected JObject VsoContext_Reporting_GetAllRelationsFromDate(int pauseTimeBetweenBatchInSec)
        {
            JObject result = null;
            if (FromDate != null)
            {
                result = VsoContext.Reporting_GetAllRelationsFromDate(ProjectName, FromDate, pauseTimeBetweenBatchInSec);
            }
            else
            {
                result = VsoContext.Reporting_GetAllRelations(ProjectName, pauseTimeBetweenBatchInSec);
            }
            return result;
        }

        public abstract JObject GetWorkItems();

        public abstract HashSet<T> PrepareDbItems(JObject workItems);

        public abstract HashSet<T> UpdateDatabase(HashSet<T> workItems);
    }
}
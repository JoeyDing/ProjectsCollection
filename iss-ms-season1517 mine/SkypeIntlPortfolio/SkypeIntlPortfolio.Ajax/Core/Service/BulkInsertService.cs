using SkypeIntlPortfolio.Ajax.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Core.Service
{
    public class BulkInsertService : IBulkInsert
    {
        public void InsertData_Bulk<T>(SkypeIntlPlanningPortfolioEntities dbContext, IEnumerable<T> newItems, string destinationTableName)
        {
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)dbContext.Database.Connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.CheckConstraints, null))
                {
                    dbContext.Database.Connection.Open();

                    bulkCopy.EnableStreaming = false;
                    bulkCopy.BulkCopyTimeout = 3600;
                    bulkCopy.BatchSize = 50000;
                    bulkCopy.DestinationTableName = destinationTableName;
                    bulkCopy.NotifyAfter = 5000;
                    //bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler((source, arg) =>
                    //{
                    //    callback(string.Format("...copied {0} rows", arg.RowsCopied.ToString()));
                    //});
                    var data = newItems.CopyToDataTable();

                    foreach (DataColumn column in data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    try
                    {
                        bulkCopy.WriteToServer(data);
                        dbContext.Database.Connection.Close();
                    }
                    catch (Exception e)
                    {
                        //Logger.LogExceptionMsg(e);
                        //so the program can catch the failed ones
                        throw;
                    }
                }
            }
        }
    }
}
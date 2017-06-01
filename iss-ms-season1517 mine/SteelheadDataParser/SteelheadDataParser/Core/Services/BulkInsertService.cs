using SteelheadDataParser.Core;
using SteelheadDataParser.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SteelheadDataParser.Services
{
    public class BulkInsertService : IBulkInsert
    {
        private static object locker = new object();

        public void InsertData_Bulk<T>(Staging_SkypeLocalizationDataWEntities dbContext, IEnumerable<T> newItems, string destinationTableName, SqlTransaction transaction)
        {
            lock (locker)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)dbContext.Database.Connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.CheckConstraints, transaction))
                {
                    //dbContext.Database.Connection.Open();

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

                    foreach (System.Data.DataColumn column in data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    try
                    {
                        bulkCopy.WriteToServer(data);
                        //dbContext.Database.Connection.Close();
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                        //so the program can catch the failed ones
                        throw;
                    }
                }
            }
        }
    }
}
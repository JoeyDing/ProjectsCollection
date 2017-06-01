using ExtensibleDataExtraction.Lib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Sql
{
    public class SqlWriteService
    {
        public void UpdateData(string sqlTableName, string[] fields, string[][] values, string[] identities, DbContext dbContext, Logger logger)
        {
            using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    //1. compare data to get only non existing data inside db
                    string fieldsString = fields.Select(c => string.Format("[{0}]", c)).Aggregate((a, b) => a + "," + b);

                    SqlReadService sqlReadService = new SqlReadService();
                    IEnumerable<string[]> dbData = sqlReadService.ReadData(
                        dbContextTransaction.UnderlyingTransaction
                        , string.Format("SELECT {0} FROM {1}", fieldsString, sqlTableName)
                        , fields
                        , dbContext);

                    var dataOnlyInFile = new HashSet<string[]>(values, new SqlDataComparer());

                    logger.LogStart();

                    //2. according to user's choice to decide delete the data only in db or not
                    //if (deleteDBdata == true)
                    //{
                    if (identities.Count() == 0)
                        throw new Exception("Identity should be set");

                    var dataOnlyInDB = new HashSet<string[]>(dbData, new SqlDataComparer());

                    dataOnlyInDB.ExceptWith(dataOnlyInFile);

                    if (dataOnlyInDB.Any())
                    {
                        logger.LogMessage("Data are ready to be removed...");

                        RemoveDataOnlyInDB(fields, dataOnlyInDB, sqlTableName, identities, dbContext, dbContextTransaction.UnderlyingTransaction);

                        logger.LogMessage("Data have been removed...");
                    }
                    //}
                    //3. insert data only from file but not in db
                    dataOnlyInFile.ExceptWith(dbData);
                    if (dataOnlyInFile.Any())
                    {
                        logger.LogMessage("Data are ready to be added...");

                        InsertDataOnlyInFile(dbContextTransaction.UnderlyingTransaction, fields, dataOnlyInFile, sqlTableName, dbContext);

                        logger.LogMessage("Data are ready to be added...");
                    }
                    dbContextTransaction.Commit();

                    logger.LogEnd();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void RemoveDataOnlyInDB(string[] fields, HashSet<string[]> dataOnlyInDB, string sqlTableName, string[] identities, DbContext dbContext, DbTransaction transaction = null)
        {
            string tempTableName = string.Format("#tempTable_{0}", DateTime.Now.Ticks);
            ConvertFieldsAndValuesToDataTableService convertfvToDataTableService = new ConvertFieldsAndValuesToDataTableService();
            DataTable dataTable = convertfvToDataTableService.ConvertFieldsAndValuesToDataTable(fields, dataOnlyInDB);
            try
            {
                using (SqlCommand command = new SqlCommand("", dbContext.Database.Connection as SqlConnection, (SqlTransaction)transaction))
                {
                    command.CommandTimeout = 3600;

                    command.CommandText = "IF EXISTS(SELECT [name] FROM tempdb.sys.tables WHERE [name] like '" + tempTableName + "') BEGIN DROP TABLE " + tempTableName + " END;";

                    if (dbContext.Database.Connection.State == ConnectionState.Closed)
                        dbContext.Database.Connection.Open();
                    command.ExecuteNonQuery();

                    //1. Creating temp table on database
                    var builder = new StringBuilder();
                    builder.AppendLine("SELECT TOP 1 ");
                    var cols = string.Join(string.Format(",{0}", Environment.NewLine), dataTable.Columns.Cast<DataColumn>()
                                 .Select(x => string.Format("COALESCE([{0}], NULL) AS [{0}]", x.ColumnName))
                                 .ToArray());
                    builder.AppendLine(cols);
                    builder.AppendLine("INTO " + tempTableName + " FROM " + sqlTableName + " WITH (NOLOCK)");
                    command.CommandText = builder.ToString();
                    //command.CommandText = "SELECT TOP 1 * INTO #TmpTable FROM " + TableName;
                    command.ExecuteNonQuery();
                    builder.Clear();

                    command.CommandText = "TRUNCATE TABLE " + tempTableName;
                    command.ExecuteNonQuery();

                    //2. Bulk insert into temp table
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbContext.Database.Connection as SqlConnection, SqlBulkCopyOptions.TableLock, (SqlTransaction)transaction))
                    {
                        bulkCopy.BulkCopyTimeout = 3600;
                        bulkCopy.DestinationTableName = tempTableName;
                        bulkCopy.WriteToServer(dataTable);
                        bulkCopy.Close();
                    }

                    //3.  Delete destination table through "join to temptable", and dropping temp table
                    builder.AppendLine("DELETE T");

                    var joinsOn = string.Join(string.Format(" AND {0}", Environment.NewLine), identities.Select(c => string.Format("T.{0} = Temp.{0}", c)));
                    builder.AppendLine(string.Format(" FROM {0} T INNER JOIN {1} TEMP ON {2} ;", sqlTableName, tempTableName, joinsOn));

                    builder.AppendLine("DROP TABLE " + tempTableName);

                    command.CommandText = builder.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertDataOnlyInFile(DbTransaction transaction, string[] fields, HashSet<string[]> dataOnlyInFile, string sqlTableName, DbContext dbContext)
        {
            ConvertFieldsAndValuesToDataTableService convertfvToDataTableService = new ConvertFieldsAndValuesToDataTableService();

            DataTable dataTable = convertfvToDataTableService.ConvertFieldsAndValuesToDataTable(fields, dataOnlyInFile);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)dbContext.Database.Connection, SqlBulkCopyOptions.TableLock, (SqlTransaction)transaction))
            {
                bulkCopy.BatchSize = 1000;

                bulkCopy.DestinationTableName = sqlTableName;

                foreach (DataColumn column in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                }

                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
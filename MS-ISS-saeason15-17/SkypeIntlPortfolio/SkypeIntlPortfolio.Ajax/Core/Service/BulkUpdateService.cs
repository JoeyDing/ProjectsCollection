using SkypeIntlPortfolio.Ajax.Core.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Core.Service
{
    public class BulkUpdateService : IBulkUpdate
    {
        public void UpdateData_Bulk<T>(SkypeIntlPlanningPortfolioEntities dbContext, IEnumerable<T> list, string TableName, string primaryKey, string[] fieldsToUpdate)
        {
            DataTable dt = list.CopyToDataTable();
            //ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

            using (SqlCommand command = new SqlCommand("", dbContext.Database.Connection as SqlConnection))
            {
                string tempTableName = string.Format("#tempTable_{0}", DateTime.Now.Ticks);
                try
                {
                    dbContext.Database.Connection.Open();
                    command.CommandTimeout = 3600;

                    command.CommandText = "IF EXISTS(SELECT [name] FROM tempdb.sys.tables WHERE [name] like '" + tempTableName + "') BEGIN DROP TABLE " + tempTableName + " END;";
                    // command.ExecuteNonQuery();

                    //Creating temp table on database
                    var builder = new StringBuilder();
                    builder.AppendLine("SELECT TOP 1 ");
                    var cols = string.Join(string.Format(",{0}", Environment.NewLine), dt.Columns.Cast<DataColumn>()
                                 .Select(x => string.Format("COALESCE({0}, NULL) AS {0}", "[" + x.ColumnName + "]"))
                                 .ToArray());
                    builder.AppendLine(cols);
                    builder.AppendLine("INTO " + tempTableName + " FROM " + TableName + " WITH (NOLOCK)");
                    command.CommandText = builder.ToString();
                    //command.CommandText = "SELECT TOP 1 * INTO #TmpTable FROM " + TableName;
                    command.ExecuteNonQuery();
                    builder.Clear();

                    command.CommandText = "TRUNCATE TABLE " + tempTableName;
                    command.ExecuteNonQuery();

                    //Bulk insert into temp table
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(dbContext.Database.Connection as SqlConnection))
                    {
                        bulkcopy.BulkCopyTimeout = 3600;
                        bulkcopy.DestinationTableName = tempTableName;
                        bulkcopy.WriteToServer(dt);
                        bulkcopy.Close();
                    }

                    // Updating destination table, and dropping temp table

                    builder.AppendLine("UPDATE T SET");

                    string updateField = string.Join(
                        string.Format(", {0}", Environment.NewLine),
                        fieldsToUpdate.Select(c => string.Format("T.{0} = Temp.{0}", "[" + c + "]")));

                    builder.AppendLine(updateField);

                    builder.AppendLine(string.Format("FROM {0} T INNER JOIN {1} TEMP ON T.{2} = Temp.{2};", TableName, tempTableName, primaryKey));

                    builder.AppendLine("DROP TABLE " + tempTableName);

                    command.CommandText = builder.ToString();
                    command.ExecuteNonQuery();
                    dbContext.Database.Connection.Close();
                }
                catch (Exception ex)
                {
                    command.CommandText = "IF EXISTS(SELECT [name] FROM tempdb.sys.tables WHERE [name] like '" + tempTableName + "') BEGIN DROP TABLE " + tempTableName + " END;";
                    command.ExecuteNonQuery();

                    //callback(ex.ToString());
                    throw ex;
                    // Handle exception properly
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Sql
{
    public class SqlReadService
    {
        public bool TableExists(string tableName, DbContext dbContext)
        {
            bool tableExist = dbContext.Database.SqlQuery<int>(
                string.Format(@"SELECT count(*) as [Exists]
                                FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{0}'", tableName))
                             .First() == 1;

            return tableExist;
        }

        public IEnumerable<string[]> ReadData(DbTransaction transaction, string query, string[] fields, DbContext dbContext)
        {
            var result = new List<string[]>();
            SqlCommand command = new SqlCommand(
                     query,
                     dbContext.Database.Connection as SqlConnection, (SqlTransaction)transaction);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var values = new string[fields.Length];
                    for (int i = 0; i < fields.Length; i++)
                    {
                        values[i] = !reader.IsDBNull(i) ? reader.GetString(i) : null;
                    }
                    result.Add(values);
                }
            }

            reader.Close();

            return result;
        }
    }
}

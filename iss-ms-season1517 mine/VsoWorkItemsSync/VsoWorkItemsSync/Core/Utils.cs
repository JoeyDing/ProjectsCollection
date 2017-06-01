using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public static class Utils
    {
        public static HashSet<T> FetchData<T>(VsoWorkItemsContext dbContext, DbTransaction transaction, string query, Func<System.Data.IDataReader, T> convertFunction)
        {
            HashSet<T> result = new HashSet<T>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 3600;

            if (transaction != null)
                cmd.Transaction = (SqlTransaction) transaction;

            cmd.Connection = (SqlConnection) dbContext.Database.Connection;
            if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                cmd.Connection.Open();

            cmd.CommandText = query;

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // manually 'materialize' the product
                T item = convertFunction(reader);
                result.Add(item);
            }

            return result;
        }

        public static List<int> GetListOfTestCaseIds(string testSuiteAudit, out string operation)
        {
            operation = "Removed";
            if (testSuiteAudit.StartsWith("Added"))
            {
                operation = "Added";
            }

            string[] ar = testSuiteAudit.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
            string[] array_testCaseIds = ar[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> testcaseIDs = new List<int>();
            foreach (var id in array_testCaseIds)
            {
                int testcaseId = Convert.ToInt32(id);
                testcaseIDs.Add(testcaseId);
            }
            return testcaseIDs;
        }
    }
}
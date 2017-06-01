using SteelheadDataParser.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser
{
    public interface IBulkInsert
    {
        void InsertData_Bulk<T>(Staging_SkypeLocalizationDataWEntities dbContext, IEnumerable<T> newItems, string destinationTableName, SqlTransaction transaction);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VsoApi.Rest;
using Newtonsoft.Json.Linq;
using ExtensibleDataExtraction.Lib.Interfaces;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Services;
using ExtensibleDataExtraction.Lib.Data.Sql;
using System.Data;

namespace ExtensibleVSOAPI.Services
{
    /// <summary>
    /// Fetch VSO work items service
    /// </summary>
    public class FetchService : IFetch
    {
        private const string C_ProjectName = "LOCALIZATION";

        /// <summary>
        ///Fetch VSO work items that match Comfig.xml configuration
        /// </summary>
        /// <param name="extensibleItem">Object that is holding other three objects such as connectionString,JsonEndPoint and Mapping</param>
        /// <param name="dbEntityContext">Database entity context that is invoked from ExtensibleDataExtraction</param>
        /// <returns></returns>
        public string FetchData(ExtensibleItem extensibleItem, ExtensibleDbEntity dbEntityContext)
        {
            VSOContextService vsoContextService = new VSOContextService();

            ParseService parseService = new ParseService(extensibleItem);

            FetchParamsInfo fetchParamsInfo = (FetchParamsInfo)parseService.Parse();

            string sqlTableName = extensibleItem.Mapping.SqlTableName;
            bool tableExist = dbEntityContext.Database.SqlQuery<int>(
                string.Format(@"SELECT count(*) as [Exists]
                                FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{0}'", sqlTableName))
                         .First() == 1;
            //if table is not empty, fetch incremental data
            bool tableEmpty = !tableExist ? true : dbEntityContext.Database.SqlQuery<int>(
            string.Format(@"SELECT count(*) FROM {0}", sqlTableName)).ToList()[0] == 0;

            if (tableExist && !tableEmpty)
                return vsoContextService.GetAllWorkItemRevisionsFromDate(C_ProjectName, fetchParamsInfo.TaskType, fetchParamsInfo.DateTime.Value).ToString();
            else
                //fetch full data(the step to create table will be done from the core...)
                return vsoContextService.GetAllWorkItemRevisionsFromDate(C_ProjectName, fetchParamsInfo.TaskType, null).ToString();
        }
    }
}
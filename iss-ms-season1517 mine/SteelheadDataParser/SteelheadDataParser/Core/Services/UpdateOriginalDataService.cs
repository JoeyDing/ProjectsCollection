using SteelheadDataParser.Model;
using SteelheadDataParser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.Core.Services
{
    public class UpdateOriginalDataService
    {
        /// <summary>
        /// Get xml data from SteelheadData column, deserialize it into object and update original data.
        /// </summary>
        /// <param name="originalData">Original data table</param>
        /// <returns></returns>
        public List<Staging_FabricBackup_SteelheadDataParsed> ParseOriginalData(List<Staging_FabricBackup_SteelheadDataParsed> originalData)
        {
            SteelheadColumnData resultObject;
            List<Staging_FabricBackup_SteelheadDataParsed> steelHeadDataResultList = new List<Staging_FabricBackup_SteelheadDataParsed>();
            foreach (var od in originalData)
            {
                resultObject = DeserializeService.Deserialize<SteelheadColumnData>(od.SteelheadData);
                List<Rv> rvs = resultObject.Rvs.Rv;
                List<Bug> bugIds = resultObject.Bugs.Bug;

                Staging_FabricBackup_SteelheadDataParsed newStagingDatParsed;
                //group rvs by result
                var xmlDataList = rvs.GroupBy(x => x.result).ToList();
                foreach (var resultGroup in xmlDataList)
                {
                    if (resultGroup.Key == "Passed")
                    {
                        //others' four records should be filled as well
                        foreach (var value in resultGroup)
                        {
                            newStagingDatParsed = new Staging_FabricBackup_SteelheadDataParsed();
                            newStagingDatParsed.SymbolicName = od.SymbolicName;
                            newStagingDatParsed.ParserIdentifier = od.ParserIdentifier;
                            newStagingDatParsed.Revision = od.Revision;
                            newStagingDatParsed.ProjectName = od.ProjectName;
                            newStagingDatParsed.Deleted = od.Deleted;
                            newStagingDatParsed.FileName = od.FileName;
                            newStagingDatParsed.Language = od.Language;

                            newStagingDatParsed.Result = value.result;
                            newStagingDatParsed.ResultDate = DateTime.ParseExact(value.date, "yyyyMMddHHmmss", null);
                            newStagingDatParsed.ResultLoggedBy = value.who;
                            newStagingDatParsed.BugNumber = null;
                            steelHeadDataResultList.Add(newStagingDatParsed);
                        }
                    }
                    else
                    {
                        //through the observation it turns out the number of bugid is matching with the number of "failed" items
                        List<Rv> failedResult = resultGroup.ToList();
                        int failedResultNum = failedResult.Count();
                        if (failedResult.Count() > bugIds.Count() || failedResult.Count() < bugIds.Count())
                        {
                            failedResultNum = Math.Min(failedResult.Count(), bugIds.Count());
                        }
                        for (int i = 0; i < failedResultNum; i++)
                        {
                            newStagingDatParsed = new Staging_FabricBackup_SteelheadDataParsed();
                            newStagingDatParsed.SymbolicName = od.SymbolicName;
                            newStagingDatParsed.ParserIdentifier = od.ParserIdentifier;
                            newStagingDatParsed.Revision = od.Revision;
                            newStagingDatParsed.ProjectName = od.ProjectName;
                            newStagingDatParsed.Deleted = od.Deleted;
                            newStagingDatParsed.FileName = od.FileName;
                            newStagingDatParsed.Language = od.Language;

                            newStagingDatParsed.Result = failedResult[i].result;
                            newStagingDatParsed.ResultDate = DateTime.ParseExact(failedResult[i].date, "yyyyMMddHHmmss", null);
                            newStagingDatParsed.ResultLoggedBy = failedResult[i].who;
                            newStagingDatParsed.BugNumber = bugIds[i].id;
                            steelHeadDataResultList.Add(newStagingDatParsed);
                        }
                    }
                }
            }
            return steelHeadDataResultList;
        }
    }
}
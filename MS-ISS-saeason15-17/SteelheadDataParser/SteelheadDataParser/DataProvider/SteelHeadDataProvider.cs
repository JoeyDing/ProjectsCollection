using SteelheadDataParser.Core;
using SteelheadDataParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.DataProvider
{
    public class SteelHeadDataProvider
    {
        /// <summary>
        /// Get data from table Staging_FabricBackup_SteelheadDataParsed and save it into class Staging_FabricBackup_SteelheadDataParsed
        /// </summary>
        /// <returns></returns>
        public List<Staging_FabricBackup_SteelheadDataParsed> GetOriginalSteelHeadData()
        {
            List<Staging_FabricBackup_SteelheadDataParsed> originalSteelData = null;
            using (var db = new Staging_SkypeLocalizationDataWEntities())
            {
                originalSteelData = db.GetStagingFabricBackupSteelheadData().ToList().Select(x => new Staging_FabricBackup_SteelheadDataParsed
                {
                    SymbolicName = x.SymbolicName,
                    ParserIdentifier = x.ParserIdentifier,
                    SteelheadData = x.SteelheadData,
                    Revision = x.Revision,
                    ProjectName = x.ProjectName,
                    Language = x.Language,
                    FileName = x.FileName,
                    Deleted = x.Deleted,
                    ResourceIdentity = x.ResourceIdentity
                }).ToList();
            }
            return originalSteelData;
        }

        /// <summary>
        /// Get the data from destination table Staging_FabricBackup_SteelheadDataParsed
        /// </summary>
        /// <returns></returns>
        public List<Staging_FabricBackup_SteelheadDataParsed> GetDestinationSteelHeadData()
        {
            List<Staging_FabricBackup_SteelheadDataParsed> destinationSteelData = null;
            using (var db = new Staging_SkypeLocalizationDataWEntities())
            {
                destinationSteelData = db.Staging_FabricBackup_SteelheadDataParsed.ToList();
            }
            return destinationSteelData;
        }

        /// <summary>
        /// Get the data should be removed from destination table Staging_FabricBackup_SteelheadDataParsed
        /// </summary>
        /// <param name="originalSteelHeadData">Data list from table table Staging_SteelheadData</param>
        /// <param name="destinationSteelHeadData">Data list from table table Staging_FabricBackup_SteelheadDataParsed</param>
        /// <returns></returns>
        public HashSet<Staging_FabricBackup_SteelheadDataParsed> PrepareOnlyDestinationData(List<Staging_FabricBackup_SteelheadDataParsed> originalSteelHeadData, List<Staging_FabricBackup_SteelheadDataParsed> destinationSteelHeadData)
        {
            var onlyDestinationData = new HashSet<Staging_FabricBackup_SteelheadDataParsed>(destinationSteelHeadData, new SteelHeadDataComparer());
            onlyDestinationData.ExceptWith(originalSteelHeadData);
            return onlyDestinationData;
        }

        /// <summary>
        /// Get the data should be added from destination table Staging_FabricBackup_SteelheadDataParsed
        /// </summary>
        /// <param name="originalSteelHeadData">Data list from table table Staging_SteelheadData</param>
        /// <param name="destinationSteelHeadData">Data list from table table Staging_FabricBackup_SteelheadDataParsed</param>
        /// <returns></returns>
        public HashSet<Staging_FabricBackup_SteelheadDataParsed> PrepareOnlyOriginalData(List<Staging_FabricBackup_SteelheadDataParsed> originalSteelHeadData, List<Staging_FabricBackup_SteelheadDataParsed> destinationSteelHeadData)
        {
            var onlyOriginalData = new HashSet<Staging_FabricBackup_SteelheadDataParsed>(originalSteelHeadData, new SteelHeadDataComparer());
            onlyOriginalData.ExceptWith(destinationSteelHeadData);
            return onlyOriginalData;
        }
    }
}
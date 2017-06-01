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
        //public HashSet<Staging_SteelheadDataParsed> GetOiginalSteelHeadData()
        //{
        //    //HashSet<Core.Staging_SteelheadDataParsed> temp;
        //    List<Staging_SteelheadDataParsed> originalSteelData = null;
        //    using (var db = new Staging_SkypeLocalizationDataWEntities())
        //    {
        //        originalSteelData = db.GetStagingFabricBackupSteelheadData().Select(x => new Core.Staging_SteelheadDataParsed()
        //        {
        //            SymbolicName = x.SymbolicName,
        //            Revision = x.Revision,
        //            SteelheadData = x.SteelheadData,
        //            ProjectName = x.ProjectName,
        //            ParserIdentifier = x.ParserIdentifier,
        //            Language = x.Language,
        //            FileName = x.FileName,
        //            Deleted = x.Deleted
        //        }).ToList();
        //    }
        //    return originalSteelData;
        //}

        public HashSet<Staging_SteelheadDataParsed> GetDestinationSteelHeadData()
        {
            HashSet<Staging_SteelheadDataParsed> destinationSteelData = null;
            using (var db = new Staging_SkypeLocalizationDataWEntities())
            {
                db.GetStagingFabricBackupSteelheadData();
            }
            return destinationSteelData;
        }

        /// <summary>
        /// Remove items from destination db
        /// </summary>
        /// <param name="originalSteelHeadData"></param>
        /// <param name="destinationSteelHeadData"></param>
        /// <returns></returns>
        public HashSet<Staging_SteelheadDataParsed> PrepareRemovedDbItems(HashSet<Staging_SteelheadDataParsed> originalSteelHeadData, HashSet<Staging_SteelheadDataParsed> destinationSteelHeadData)
        {
            var tempList = new HashSet<Staging_SteelheadDataParsed>(destinationSteelHeadData, new SteelHeadDataComparer());
            //get the data should be removed from destination db
            tempList.ExceptWith(originalSteelHeadData);
            return tempList;
        }

        /// <summary>
        /// /// Remove items from destination db
        /// </summary>
        /// <param name="originalSteelHeadData"></param>
        /// <param name="destinationSteelHeadData"></param>
        /// <returns></returns>
        public HashSet<Staging_SteelheadDataParsed> PrepareAddedDbItems(List<Staging_SteelheadDataParsed> originalSteelHeadData, List<Staging_SteelheadDataParsed> destinationSteelHeadData)
        {
            //custom properties as a creteria
            var tempList = new HashSet<Staging_SteelheadDataParsed>(originalSteelHeadData, new SteelHeadDataComparer());
            tempList.ExceptWith(destinationSteelHeadData);
            return tempList;
        }
    }
}
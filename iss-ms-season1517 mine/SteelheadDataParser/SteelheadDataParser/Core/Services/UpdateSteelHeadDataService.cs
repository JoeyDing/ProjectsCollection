using SteelheadDataParser.Model;
using SteelheadDataParser.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.Core.Services
{
    public class UpdateSteelHeadDataService
    {
        /// <summary>
        /// Add new item from original tabel Staging_SteelheadData
        /// Remove items that no longer exists in tabel Staging_SteelheadData
        /// </summary>
        /// <param name="onlyDestinationData"></param>
        /// <param name="onlyOriginalData"></param>
        ///

        public void UpdateSteelHeadData(HashSet<Staging_FabricBackup_SteelheadDataParsed> onlyDestinationData, HashSet<Staging_FabricBackup_SteelheadDataParsed> onlyOriginalData)
        {
            using (var db = new Staging_SkypeLocalizationDataWEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        Logger.LogStart();
                        if (onlyOriginalData.Count() != 0)
                        {
                            foreach (var removedData in onlyDestinationData)
                            {
                                db.Staging_FabricBackup_SteelheadDataParsed.Attach(removedData);
                                db.Staging_FabricBackup_SteelheadDataParsed.Remove(removedData);
                            }
                            db.SaveChanges();
                        }
                        BulkInsertService bulkInsertService = new BulkInsertService();
                        if (onlyOriginalData.Count() != 0)
                        {
                            bulkInsertService.InsertData_Bulk(db, onlyOriginalData, "Staging_FabricBackup_SteelheadDataParsed", dbContextTransaction.UnderlyingTransaction as SqlTransaction);
                        }
                        //foreach (var addedData in onlyOriginalData)
                        //{
                        //    db.Staging_FabricBackup_SteelheadDataParsed.Add(addedData);
                        //}
                        dbContextTransaction.Commit();
                        Logger.LogEnd();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
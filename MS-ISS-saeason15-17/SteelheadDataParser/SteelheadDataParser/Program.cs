using SteelheadDataParser.Core;
using SteelheadDataParser.Core.Services;
using SteelheadDataParser.DataProvider;
using SteelheadDataParser.Model;
using SteelheadDataParser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Program.ProcessSteelHeadData();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void ProcessSteelHeadData()
        {
            //1  Get the original steelheaddata(not parsed) and destination steelhead(parsed)
            SteelHeadDataProvider dataProvider = new SteelHeadDataProvider();

            List<Staging_FabricBackup_SteelheadDataParsed> originalSteelHeadData = dataProvider.GetOriginalSteelHeadData();

            List<Staging_FabricBackup_SteelheadDataParsed> destinationSteelHeadData = dataProvider.GetDestinationSteelHeadData();

            //2  Get the parsed original steelheaddata
            UpdateOriginalDataService updateOriginalDataService = new UpdateOriginalDataService();

            List<Staging_FabricBackup_SteelheadDataParsed> parsedOriginalData;
            parsedOriginalData = updateOriginalDataService.ParseOriginalData(originalSteelHeadData);

            //3  Prepare data to be updated to db
            HashSet<Staging_FabricBackup_SteelheadDataParsed> onlyDestinationData = dataProvider.PrepareOnlyDestinationData(parsedOriginalData, destinationSteelHeadData);

            HashSet<Staging_FabricBackup_SteelheadDataParsed> onlyOriginalData = dataProvider.PrepareOnlyOriginalData(parsedOriginalData, destinationSteelHeadData);

            //4  Update db
            UpdateSteelHeadDataService updateService = new UpdateSteelHeadDataService();

            updateService.UpdateSteelHeadData(onlyDestinationData, onlyOriginalData);
        }
    }
}
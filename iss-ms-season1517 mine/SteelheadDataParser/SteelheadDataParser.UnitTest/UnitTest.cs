using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteelheadDataParser.Core;
using SteelheadDataParser.Core.Services;
using SteelheadDataParser.DataProvider;
using SteelheadDataParser.Model;
using SteelheadDataParser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        /// <summary>
        /// Passed, Failed, BugId
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType01()

        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "﻿<TR><Rvs><Rv date=\"20160901174706\" result=\"Passed\" who=\"Inge\"/><Rv date=\"20160901175014\" result=\"Failed\" who=\"Inge\"/></Rvs><Bugs><Bug id=\"683292\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Failed, BugId
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType02()
        {
            ///Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "<TR><Rvs><Rv date=\"20161019212602\" result=\"Failed\" who=\"Maribel\"/></Rvs><Bugs><Bug id=\"742440\"/></Bugs></TR>"
            });

            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();

            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(1, result.Count);
        }

        /// <summary>
        /// Failed, Failed, Passed, Failed, BugId, BugId, BugId
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType03()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "<TR><Rvs><Rv date=\"20161026151719\" result=\"Failed\" who=\"Sandra\"/><Rv date=\"20161026152004\" result=\"Failed\" who=\"Sandra\"/><Rv date=\"20161026152925\" result=\"Passed\" who=\"Sandra\"/><Rv date=\"20161026152927\" result=\"Failed\" who=\"Sandra\"/></Rvs><Bugs><Bug id=\"750963\"/><Bug id=\"750964\"/><Bug id=\"750969\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(4, result.Count);
        }

        /// <summary>
        /// Failed, Failed, BugId, BugId
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType04()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "<TR><Rvs><Rv date=\"20161019124925\" result=\"Failed\" who=\"ando\"/><Rv date=\"20161020094824\" result=\"Failed\" who=\"ando\"/></Rvs><Bugs><Bug id=\"741743\"/><Bug id=\"742968\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Failed, Passed, Failed, BugId, BugId
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType05()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "﻿<TR><Rvs><Rv date=\"20170124130954\" result=\"Failed\" who=\"Maribel\"/><Rv date=\"20170124131044\" result=\"Passed\" who=\"Maribel\"/><Rv date=\"20170124131050\" result=\"Failed\" who=\"Maribel\"/></Rvs><Bugs><Bug id=\"846647\"/><Bug id=\"846649\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// Passed, Failed, Passed, Passed, Failed,BugID,BugID
        /// </summary>
        [TestMethod]
        public void ParseSteelHeadDataType06()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "﻿<TR><Rvs><Rv date=\"20161019171852\" result=\"Passed\" who=\"Administrator\"/><Rv date=\"20161019172031\" result=\"Failed\" who=\"Administrator\"/><Rv date=\"20161020100835\" result=\"Passed\" who=\"Administrator\"/><Rv date=\"20161020100837\" result=\"Passed\" who=\"Administrator\"/><Rv date=\"20161020100941\" result=\"Failed\" who=\"Administrator\"/></Rvs><Bugs><Bug id=\"741941\"/><Bug id=\"742899\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(5, result.Count);
        }

        /// Failed, Failed, Failed, BugId, BugId, BugId
        [TestMethod]
        public void ParseSteelHeadDataType07()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "<TR><Rvs><Rv date=\"20170128115423\" result=\"Failed\" who=\"EETLAMA\"/><Rv date=\"20170128121314\" result=\"Failed\" who=\"EETLAMA\"/><Rv date=\"20170128121635\" result=\"Failed\" who=\"EETLAMA\"/></Rvs><Bugs><Bug id=\"853004\"/><Bug id=\"853008\"/><Bug id=\"853009\"/></Bugs></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(3, result.Count);
        }

        /// Passed, No bugID
        [TestMethod]
        public void ParseSteelHeadDataType08()
        {
            //Setting
            HashSet<Staging_FabricBackup_SteelheadDataParsed> originalDataList = new HashSet<Staging_FabricBackup_SteelheadDataParsed>();
            originalDataList.Add(new Staging_FabricBackup_SteelheadDataParsed
            {
                SteelheadData = "﻿<TR><Rvs><Rv date=\"20160718204258\" result=\"Passed\" who=\"yoko winther\"/></Rvs><Bugs/></TR>"
            });
            //execute
            string xmlString = originalDataList.FirstOrDefault().SteelheadData.ToString();
            SteelheadColumnData parserResult = DeserializeService.Deserialize<SteelheadColumnData>(xmlString);
            UpdateOriginalDataService updateOriginal = new UpdateOriginalDataService();
            var result = updateOriginal.ParseOriginalData(originalDataList.ToList());
            //assert
            Assert.AreEqual(1, result.ToList().Count);
        }

        /// <summary>
        /// Test PrepareOnlyOriginalData and PrepareOnlyDestinationData method
        /// </summary>
        [TestMethod]
        public void GetDataOnlyExistInOriginalTableAndDestinationTable()
        {
            //Setting
            List<Staging_FabricBackup_SteelheadDataParsed> originalSteelHeadData = new List<Staging_FabricBackup_SteelheadDataParsed> {
                new Staging_FabricBackup_SteelheadDataParsed
                {
                   BugNumber=1,
                   Deleted=false,
                   FileName="a1",
                   Language="b1",
                   ParserIdentifier="c1",
                   ProjectName="d1",
                   Revision=1,
                   SteelheadData="<TR><Rvs><Rv date=\"20160901174706\" result=\"Passed\" who=\"Inge\"/><Rv date=\"20160901175014\" result=\"Failed\" who=\"Inge\"/></Rvs><Bugs><Bug id=\"683292\"/></Bugs></TR>",
                   SymbolicName="e1"
                }
                };

            List<Staging_FabricBackup_SteelheadDataParsed> destinationSteelHeadData = new List<Staging_FabricBackup_SteelheadDataParsed>{
                new Staging_FabricBackup_SteelheadDataParsed
                {
                    BugNumber=1,
                    Deleted=false,
                    FileName="a1",
                    Language="b1",
                    ParserIdentifier="c1",
                    ProjectName="d1",
                    Revision=1,
                    SteelheadData="<TR><Rvs><Rv date=\"20160901174706\" result=\"Passed\" who=\"Inge\"/><Rv date=\"20160901175014\" result=\"Failed\" who=\"Inge\"/></Rvs><Bugs><Bug id=\"683292\"/></Bugs></TR>",
                    SymbolicName="e1",
                    Result="f1",
                    ResultDate=System.DateTime.Now,
                    SteelHeadDataParsedKey=1,
                    ResultLoggedBy="g1"
                }
                };
            //execute
            SteelHeadDataProvider steelHeadDataProvider = new SteelHeadDataProvider();
            HashSet<Staging_FabricBackup_SteelheadDataParsed> dataOnlyInOrigianlTable = steelHeadDataProvider.PrepareOnlyOriginalData(originalSteelHeadData, destinationSteelHeadData);
            HashSet<Staging_FabricBackup_SteelheadDataParsed> dataOnlyInDestinationTable = steelHeadDataProvider.PrepareOnlyDestinationData(originalSteelHeadData, destinationSteelHeadData);
            //assert
            Assert.AreEqual(0, dataOnlyInOrigianlTable.Count);
            Assert.AreEqual(0, dataOnlyInDestinationTable.Count);
        }
    }
}
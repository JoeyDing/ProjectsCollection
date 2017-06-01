using ExtensibleDataExtraction.Lib.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Tests.UnitTests
{
    [TestClass]
    public class Unit_ConvertFieldsAndValuesToDataTableService
    {
        [TestMethod]
        public void ConvertFieldsAndValuesToDT()
        {
            //arrange
            ConvertFieldsAndValuesToDataTableService convertfvToDataTableService = new ConvertFieldsAndValuesToDataTableService();

            string[] fields = new string[] { "Col_A", "Col_B", "Col_C" };

            HashSet<string[]> values = new HashSet<string[]> { new[] { "1", "2", "3" } };

            //act
            DataTable dataTable = convertfvToDataTableService.ConvertFieldsAndValuesToDataTable(fields, values);

            //assert
            Assert.AreEqual(3, dataTable.Columns.Count);
            Assert.AreEqual("Col_A", dataTable.Columns[0].ColumnName);
            Assert.AreEqual("Col_B", dataTable.Columns[1].ColumnName);
            Assert.AreEqual("Col_C", dataTable.Columns[2].ColumnName);
            Assert.AreEqual("1", dataTable.Rows[0][0]);
            Assert.AreEqual("2", dataTable.Rows[0][1]);
            Assert.AreEqual("3", dataTable.Rows[0][2]);
        }
    }
}
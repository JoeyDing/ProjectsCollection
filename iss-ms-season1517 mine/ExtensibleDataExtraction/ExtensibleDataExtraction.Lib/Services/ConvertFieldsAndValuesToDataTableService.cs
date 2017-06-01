using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Services
{
    public class ConvertFieldsAndValuesToDataTableService
    {
        public DataTable ConvertFieldsAndValuesToDataTable(string[] fields, HashSet<string[]> values)
        {
            //insert data
            var dataTable = new DataTable();

            foreach (var columnName in fields)
            {
                var column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = columnName;
                dataTable.Columns.Add(column);
            }

            if (values.Any())
            {
                foreach (var valueArray in values)
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        row[fields[i]] = valueArray[i];
                    }
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }
    }
}
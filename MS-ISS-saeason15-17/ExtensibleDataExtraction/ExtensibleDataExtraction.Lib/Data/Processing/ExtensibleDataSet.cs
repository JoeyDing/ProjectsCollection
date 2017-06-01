using ExtensibleDataExtraction.Lib.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Processing
{
    public class ExtensibleDataSet
    {
        public Mapping Mapping { get; set; }

        public List<ExtensibleDataRow> Rows { get; set; }
    }
}
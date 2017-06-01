using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public class ExtensibleItem
    {
        public string ConnectionString { get; set; }

        public JsonEndPoint JsonEndPoint { get; set; }

        public Mapping Mapping { get; set; }
    }
}
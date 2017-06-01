using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public class GlobalConfig
    {
        public string ConnectionString { get; set; }
        public Params Params { get; set; }
        public string LogName { get; set; }
    }
}
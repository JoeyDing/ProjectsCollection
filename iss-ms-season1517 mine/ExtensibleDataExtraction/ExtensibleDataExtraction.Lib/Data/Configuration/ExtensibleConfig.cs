using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    [XmlRoot("ExtensibleConfig")]
    public class ExtensibleConfig
    {
        [XmlElement("GlobalConfig")]
        public GlobalConfig GlobalConfigItem { get; set; }

        [XmlElement("ExtensibleItems")]
        public ExtensibleItems Items { get; set; }
    }
}
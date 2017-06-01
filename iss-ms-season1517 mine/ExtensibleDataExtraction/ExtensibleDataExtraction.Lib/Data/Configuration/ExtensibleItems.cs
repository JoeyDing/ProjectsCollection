using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    //[XmlRoot("ExtensibleItems")]
    public class ExtensibleItems
    {
        [XmlElement("ExtensibleItem")]
        public List<ExtensibleItem> Items { get; set; }
    }
}
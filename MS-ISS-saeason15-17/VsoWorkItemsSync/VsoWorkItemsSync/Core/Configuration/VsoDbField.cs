using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VsoWorkItemsSync.Core.Configuration
{
    [Serializable]
    public class VsoDbField
    {
        [XmlAttribute]
        public string DataType { get; set; }

        [XmlAttribute]
        public string VsoField { get; set; }

        [XmlAttribute]
        public string DbField { get; set; }
    }
}
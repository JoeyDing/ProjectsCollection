using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VsoWorkItemsSync.Core.Configuration
{
    [Serializable]
    public class VsoDbMapping
    {
        [XmlAttribute]
        public string VsoWorkItemName { get; set; }

        [XmlAttribute]
        public string DbTableName { get; set; }

        [XmlArray("Fields")]
        [XmlArrayItem("Field", typeof(VsoDbField))]
        public VsoDbField[] Fields { get; set; }
    }
}
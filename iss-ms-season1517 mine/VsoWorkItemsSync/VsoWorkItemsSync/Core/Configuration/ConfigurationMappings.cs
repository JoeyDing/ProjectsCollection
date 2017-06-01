using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VsoWorkItemsSync.Core.Configuration
{
    [Serializable]
    public class ConfigurationMappings
    {
        [XmlAttribute]
        public string DbConnectionString { get; set; }

        [XmlAttribute]
        public string VsoRootAccount { get; set; }

        [XmlAttribute]
        public string VsoPrivateKey { get; set; }

        [XmlAttribute]
        public string VsoProjectName { get; set; }

        [XmlArray("Mappings")]
        [XmlArrayItem("Mapping", typeof(VsoDbMapping))]
        public VsoDbMapping[] Mappings { get; set; }
    }
}
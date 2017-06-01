using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public enum RequestType
    {
        [XmlEnum("GET")]
        GET,

        [XmlEnum("POST")]
        POST
    }
}
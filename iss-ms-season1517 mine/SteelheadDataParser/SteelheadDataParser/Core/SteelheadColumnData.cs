using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteelheadDataParser.Core
{
    public class Rv
    {
        [XmlAttribute(AttributeName = "date")]
        public string date { get; set; }

        [XmlAttribute(AttributeName = "result")]
        public string result { get; set; }

        [XmlAttribute(AttributeName = "who")]
        public string who { get; set; }
    }

    public class Bug
    {
        [XmlAttribute(AttributeName = "id")]
        public int id { get; set; }
    }

    public class Rvs
    {
        [XmlElement(ElementName = "Rv")]
        public List<Rv> Rv { get; set; }
    }

    [XmlRoot(ElementName = "Bugs")]
    public class Bugs
    {
        [XmlElement(ElementName = "Bug")]
        public List<Bug> Bug { get; set; }
    }

    [XmlRoot(ElementName = "TR")]
    public class SteelheadColumnData
    {
        [XmlElement(ElementName = "Rvs")]
        public Rvs Rvs { get; set; }

        [XmlElement(ElementName = "Bugs")]
        public Bugs Bugs { get; set; }
    }
}
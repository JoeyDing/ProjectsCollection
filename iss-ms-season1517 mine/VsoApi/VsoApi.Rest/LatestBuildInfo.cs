using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VsoApi.Rest
{
    [XmlRoot(ElementName = "com.pmease.quickbuild.model.Build")]
    public class LatestBuildInfo
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "configuration")]
        public string Configuration { get; set; }

        [XmlElement(ElementName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "scheduled")]
        public string Scheduled { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "statusDate")]
        public string StatusDate { get; set; }

        [XmlElement(ElementName = "beginDate")]
        public string BeginDate { get; set; }

        [XmlElement(ElementName = "duration")]
        public string Duration { get; set; }

        [XmlElement(ElementName = "stepRuntimes")]
        public string StepRuntimes { get; set; }

        [XmlElement(ElementName = "repositoryRuntimes")]
        public string RepositoryRuntimes { get; set; }

        [XmlElement(ElementName = "secretAwareVariableValues")]
        public string SecretAwareVariableValues { get; set; }
    }
}
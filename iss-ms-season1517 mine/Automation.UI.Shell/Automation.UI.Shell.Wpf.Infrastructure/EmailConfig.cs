using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Automation.UI.Shell.Wpf.Infrastructure
{
    [XmlRoot(ElementName = "Setting")]
    public class EmailSetting
    {
        [XmlElement(ElementName = "EmailServer")]
        public string EmailServer { get; set; }

        [XmlElement(ElementName = "EmailFrom")]
        public string EmailFrom { get; set; }

        [XmlElement(ElementName = "EmailToList")]
        public EmailToList EmailToList { get; set; }

        [XmlElement(ElementName = "EmailCcList")]
        public EmailCcList EmailCcList { get; set; }
    }

    [XmlRoot(ElementName = "EmailToList")]
    public class EmailToList
    {
        [XmlElement(ElementName = "EmailTo")]
        public List<string> EmailTo { get; set; }
    }

    [XmlRoot(ElementName = "EmailCcList")]
    public class EmailCcList
    {
        [XmlElement(ElementName = "EmailCc")]
        public List<string> EmailCc { get; set; }
    }
}
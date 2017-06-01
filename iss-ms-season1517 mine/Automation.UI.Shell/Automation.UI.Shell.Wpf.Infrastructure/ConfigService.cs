using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Automation.UI.Shell.Wpf.Infrastructure
{
    [XmlRoot(ElementName = "ConfigService")]
    public class ConfigService
    {
        [XmlElement(ElementName = "TextBoxItem")]
        public List<string> TextBoxItem { get; set; }
    }
}
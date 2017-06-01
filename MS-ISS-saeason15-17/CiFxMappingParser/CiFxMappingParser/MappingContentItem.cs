using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class MappingContentItem
    {
        public string Text { get; set; }
        public string Accessibility_id { get; set; }
        public string Id { get; set; }
        public string Command { get; set; }
        public string Xpath { get; set; }
    }
}
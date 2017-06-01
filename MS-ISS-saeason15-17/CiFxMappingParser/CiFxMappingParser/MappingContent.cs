using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class MappingContent
    {
        //key is the original mapping key 
        public Dictionary<string, MappingContentItem> Items { get; set; }
    }
}
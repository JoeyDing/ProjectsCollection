using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class ResourceContent
    {
        public string Culture { get; set; }
        public Dictionary<string, Dictionary<string, string>> Content { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public enum LocType
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals,
        Regex,
        NoPattern
    }

    public class MatchingPattern
    {
        //text or command or Accessibility_Id
        public string PropertyName { get; set; }

        //key is LocType, value is the regex value 
        public List<MatchingPatternItem> Values { get; set; }
    }

    public class MatchingPatternItem
    {
        public LocType LocType { get; set; }
        public string SourceValue { get; set; }
    }
}
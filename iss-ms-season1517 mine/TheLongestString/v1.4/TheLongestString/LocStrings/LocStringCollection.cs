using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheLongestString
{
    /// <summary>
    /// List of LocString translations of a given Id, and some aggregate properties.
    /// </summary>
    public class LocStringCollection : List<LocString>
    {
        public string Id { get; set; }
        public double MaximumWidth { get; set; }
        public double AverageWidth { get; set; }
        public double SourceCultureWidth { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Content
{
    /// <summary>
    /// translation could not be found from the translation file
    /// </summary>
    public class CultureValueUnfoundContent
    {
        public string UiMappingKey { get; set; }
        public List<CultureValueUnfoundContentItem> Values { get; set; }

        public CultureValueUnfoundContent()
        {
            this.Values = new List<CultureValueUnfoundContentItem>();
        }
    }

    public class CultureValueUnfoundContentItem
    {
        public string ResourceId { get; set; }
        public string Value { get; set; }

        public string PropertyName { get; set; }
    }
}
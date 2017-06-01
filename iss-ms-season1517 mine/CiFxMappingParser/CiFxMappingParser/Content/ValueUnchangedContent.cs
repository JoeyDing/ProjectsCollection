using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Content
{
    /// <summary>
    /// string collection with "NoPattern"
    /// </summary>
    public class ValueUnchangedContent
    {
        public string UiMappingKey { get; set; }
        public List<ValueUnchangedContentItem> Values { get; set; }

        public ValueUnchangedContent()
        {
            this.Values = new List<ValueUnchangedContentItem>();
        }
    }

    public class ValueUnchangedContentItem
    {
        public string Value { get; set; }
        public string PropertyName { get; set; }
    }
}
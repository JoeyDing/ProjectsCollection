using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Content
{
    /// <summary>
    /// the mapping resourceid of sourceMappingKey could be be found in the english file
    /// </summary>
    public class ValueUnfoundContent
    {
        public string UiMappingKey { get; set; }
        public List<ValueUnfoundContentItem> Values { get; set; }

        public ValueUnfoundContent()
        {
            this.Values = new List<ValueUnfoundContentItem>();
        }
    }

    public class ValueUnfoundContentItem
    {
        public string Value { get; set; }
        public string PropertyName { get; set; }
    }
}
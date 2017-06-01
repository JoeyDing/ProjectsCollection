using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class DuplicatedContent
    {
        public string UiMappingKey { get; set; }

        //key is propertyName
        public Dictionary<string, DuplicatedContentItem> Values { get; set; }

        public DuplicatedContent()
        {
            this.Values = new Dictionary<string, DuplicatedContentItem>();
        }
    }

    public class DuplicatedContentItem
    {
        public string EnglishValue { get; set; }

        public string PropertyName { get; set; }

        //key is resourceid and value is the english value(i.e. regex contains "start with")
        public List<KeyValuePair<string, string>> ResourceItems { get; set; }

        public DuplicatedContentItem()
        {
            this.ResourceItems = new List<KeyValuePair<string, string>>();
        }
    }
}
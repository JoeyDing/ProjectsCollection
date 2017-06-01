using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Writer
{
    public class SourceMappingSerializer
    {
        public string Serialize(MappingContent mappingContent)
        {
            var jObject = new JObject();
            if (mappingContent.Items != null)
            {
                foreach (var item in mappingContent.Items)
                {
                    var subObject = new JObject();
                    var properties = typeof(MappingContentItem).GetProperties();
                    foreach (var prop in properties)
                    {
                        string propValue = prop.GetValue(item.Value) as string;
                        if (propValue != null)
                            subObject.Add(prop.Name.ToLower(), propValue);
                    }

                    jObject.Add(item.Key, subObject);
                }
            }
        
            return jObject.ToString();
        }
    }
}
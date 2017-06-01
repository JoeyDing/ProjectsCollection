using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Reader
{
    public class ResourceReader
    {
        public ResourceContent Read(string filePath, string culture)
        {
            //1 read file
            string jsonString = File.ReadAllText(filePath);

            //2 parse to JObject
            JObject jobject = JObject.Parse(jsonString);

            //3 create result object
            var result = new ResourceContent();
            result.Culture = culture;
            result.Content = new Dictionary<string, Dictionary<string, string>>();

            foreach (var prop in jobject.Properties())
            {
                var dict = new Dictionary<string, string>();
                if (prop.HasValues && prop.Value is JObject)
                {
                    var subObjectProps = (prop.Value as JObject).Properties();
                    foreach (var subProp in subObjectProps)
                    {
                        dict[subProp.Name] = subProp.HasValues ? subProp.Value.ToString() : null;
                    }
                }
                result.Content.Add(prop.Name, dict);
            }

            return result;
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CiFxMappingParser.Reader
{
    public class SourceMappingReader
    {
        public MappingContent Read(string filePath)
        {
            //1 read file
            string jsonString = File.ReadAllText(filePath);

            //2 parse to JObject
            JObject jobject = JObject.Parse(jsonString);

            //3 create result object
            var result = new MappingContent();
            result.Items = new Dictionary<string, MappingContentItem>();

            foreach (var prop in jobject.Properties())
            {
                var contentItem = new MappingContentItem();
                if (prop.HasValues && prop.Value is JObject)
                {
                    var subObjectProps = (prop.Value as JObject).Properties();
                    foreach (var subProp in subObjectProps)
                    {
                        var value = subProp.HasValues ? subProp.Value.ToString() : null;
                        switch (subProp.Name.ToLower())
                        {
                            case "text":
                                contentItem.Text = value;
                                break;

                            case "id":
                                contentItem.Id = value;
                                break;

                            case "accessibility_id":
                                contentItem.Accessibility_id = value;
                                break;

                            case "command":
                                contentItem.Command = value;
                                break;

                            case "xpath":
                                contentItem.Xpath = value;
                                break;

                            default:
                                break;
                        }
                    }
                }
                result.Items.Add(prop.Name, contentItem);
            }

            return result;
        }
    }
}
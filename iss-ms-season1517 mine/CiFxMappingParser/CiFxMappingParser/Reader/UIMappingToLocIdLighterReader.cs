using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Reader
{
    public class UIMappingToLocIdLighterReader
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
                //if (prop.HasValues && prop.Value is JObject)
                if (prop.HasValues && prop.Value is JArray)
                {
                    foreach (var subProp in prop.Value)
                    {
                        string propertyName = subProp["Property"].ToString();
                        string resourceID = subProp["ResourceId"].ToString();
                        switch (propertyName)
                        {
                            case "text":
                                contentItem.Text = resourceID;
                                break;

                            case "id":
                                contentItem.Id = resourceID;
                                break;

                            case "accessibility_id":
                                contentItem.Accessibility_id = resourceID;
                                break;

                            case "command":
                                contentItem.Command = resourceID;
                                break;

                            case "xpath":
                                contentItem.Xpath = resourceID;
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
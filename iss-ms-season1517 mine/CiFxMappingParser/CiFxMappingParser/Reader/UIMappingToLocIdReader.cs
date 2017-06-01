using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Reader
{
    public class UIMappingToLocIdReader
    {
        /// <summary>
        /// {
        ///     mappId:{
        ///         'text':{
        ///            MappingSource: .*value.*,
        ///            MatchedBy: Regex,
        ///            ResourcesIds: {
        ///               resource1: value1,
        ///               resource2: value2
        ///            }
        ///         }
        ///
        ///     }
        /// }
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public UIMappingToLocIdContent Read(string filePath)
        {
            //1 read file
            string jsonString = File.ReadAllText(filePath);

            //2 parse to JObject
            JObject rootObject = JObject.Parse(jsonString);

            //3 create result object
            var result = new UIMappingToLocIdContent();
            //result.Mapping = new Dictionary<string, string>();
            result.Mapping = new Dictionary<string, Dictionary<string, UIMappingToLocIdContentItem>>();
            foreach (var prop in rootObject.Properties())
            {
                var propertiesDict = new Dictionary<string, UIMappingToLocIdContentItem>();

                if (prop.HasValues && prop.Value is JObject)
                {
                    foreach (var subProp1 in (prop.Value as JObject).Properties())
                    {
                        var contentItem = new UIMappingToLocIdContentItem();
                        foreach (var subProp2 in (subProp1.Value as JObject).Properties())
                        {
                            var value = subProp2.HasValues ? subProp2.Value.ToString() : null;
                            switch (subProp2.Name.ToLower())
                            {
                                case "matchedby":
                                    contentItem.MatchedBy = (MatchedBy)Enum.Parse(typeof(MatchedBy), value);
                                    break;

                                case "mappingsource":
                                    contentItem.MappingSource = value;
                                    break;

                                case "resourcesids":
                                    if (subProp2.Value is JObject)
                                    {
                                        contentItem.ResourceIds = new List<UIMappingToLocIdContentItemResource>();
                                        foreach (var resourceId in (subProp2.Value as JObject).Properties())
                                        {
                                            contentItem.ResourceIds.Add(new UIMappingToLocIdContentItemResource
                                            {
                                                ResourceId = resourceId.Name,
                                                Value = resourceId.HasValues ? (string)resourceId.Value : null
                                            });
                                        }
                                    }
                                    break;
                            }
                        }
                        propertiesDict.Add(subProp1.Name, contentItem);
                    }
                }
                result.Mapping.Add(prop.Name, propertiesDict);
            }
            return result;
        }
    }
}
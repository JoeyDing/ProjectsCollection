using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace CiFxMappingParser.Serializer
{
    public class UIMappingToLocIdContentSerializer
    {
        public string Serialize(UIMappingToLocIdContent uiMappingToLocIdContent)
        {
            var jObject = new JObject();

            if (uiMappingToLocIdContent.Mapping != null)
            {
                foreach (var item in uiMappingToLocIdContent.Mapping)
                {
                    if (item.Value.Count != 0)
                    {
                        List<JObject> proList = new List<JObject>();
                        foreach (var itemTemp in item.Value)
                        {
                            var subObject = new JObject();
                            List<UIMappingToLocIdContentItemResource> resourceIds = itemTemp.Value.ResourceIds;
                            if (resourceIds.Count != 0 && resourceIds[0] != null)
                            {
                                subObject.Add("Property", itemTemp.Key);
                                subObject.Add("ResourceId", resourceIds[0].ResourceId);
                            }
                            proList.Add(subObject);
                        }
                        jObject.Add(item.Key.ToLower(), JToken.FromObject(proList));
                    }
                }
            }
            return jObject.ToString();
        }
    }
}
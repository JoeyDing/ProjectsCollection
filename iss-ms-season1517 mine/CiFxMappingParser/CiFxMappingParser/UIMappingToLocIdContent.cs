using System.Collections.Generic;

namespace CiFxMappingParser
{
    public enum MatchedBy
    {
        Value,
        Regex
    }

    public class UIMappingToLocIdContent
    {
        //key = sourceMapping key, value ={ dictionary of key = propertyName, value = resource id infos}
        public Dictionary<string, Dictionary<string, UIMappingToLocIdContentItem>> Mapping { get; set; }
    }

    public class UIMappingToLocIdContentItem
    {
        public MatchedBy MatchedBy { get; set; }
        public List<UIMappingToLocIdContentItemResource> ResourceIds { get; set; }

        public string MappingSource { get; set; }
    }

    public class UIMappingToLocIdContentItemResource
    {
        public string ResourceId { get; set; }
        public string Value { get; set; }
    }
}
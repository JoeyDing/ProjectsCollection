using CiFxMappingParser.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class MappingResult
    {
        //key = culture, Dictionary<string,string> = sourceMappingContent in the culture
        public Dictionary<string, MappingContent> LocalizedMappingsList { get; set; }

        public UIMappingToLocIdContent UIMappingToLocIdContent { get; set; }

        //key = MappingKey, value = DuplicatedContent 
        public Dictionary<string, DuplicatedContent> TotalDuplicatedEnglishValues { get; set; }

        //key = MappingKey, value = ValueNotFoundContent
        public Dictionary<string, ValueUnfoundContent> UnFoundSourceValues { get; set; }

        //key = MappingKey, value = ValueUnchangedContent
        public Dictionary<string, ValueUnchangedContent> UnChangedSourceValues { get; set; }

        //key = MappingKey, value = CultureValueNotFoundContent
        public Dictionary<string, CultureValueUnfoundContent> UnFoundTranslationValues { get; set; }
    }
}
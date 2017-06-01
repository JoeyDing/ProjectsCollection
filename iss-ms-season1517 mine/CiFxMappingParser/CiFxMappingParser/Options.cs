using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser
{
    public class Options
    {
        [Option('s', "sourceMappingPath", Required = true, HelpText = "")]
        public string SourceMappingPath { get; set; }

        [Option('c', "sourceConfigMappingPath", Required = false, HelpText = "")]
        public string SourceConfigMappingPath { get; set; }

        [Option('r', "englishResourcePath", Required = true, HelpText = "")]
        public string EnglishResourcePath { get; set; }

        [Option('m', "mappingToLocIdPath", Required = false, HelpText = "")]
        public string MappingToLocIdPath { get; set; }

        [Option('t', "translatedResourceFolderPath", Required = false, HelpText = "")]
        public string TranslatedResourceFolderPath { get; set; }

        [Option('d', "DestinationPath", Required = true, HelpText = "")]
        public string DestinationPath { get; set; }
    }
}
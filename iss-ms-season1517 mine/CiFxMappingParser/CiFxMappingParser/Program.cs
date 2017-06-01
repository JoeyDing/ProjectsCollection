using CiFxMappingParser.Reader;
using CiFxMappingParser.Serializer;
using CiFxMappingParser.Service;
using CiFxMappingParser.Writer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CiFxMappingParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //1 read parameters
                var resourceReader = new ResourceReader();
                var mappingReader = new SourceMappingReader();
                var cultureReader = new CultureReader();
                UIMappingToLocIdReader uiMappingToLocIdReader = new UIMappingToLocIdReader();
                UIMappingToLocIdLighterReader uiMappingToLocIdLighterReader = new UIMappingToLocIdLighterReader();
                var sourceMapping = mappingReader.Read(options.SourceMappingPath);
                var englishResource = resourceReader.Read(options.EnglishResourcePath, "en-us");
                MappingContent sourceConfigMappingContent = null;
                if (File.Exists(options.SourceConfigMappingPath))
                    sourceConfigMappingContent = mappingReader.Read(options.SourceConfigMappingPath);
                UIMappingToLocIdContent mappingToLocId = null;
                MappingContent mappingToLocIdLighter = null;
                if (File.Exists(options.MappingToLocIdPath))
                { //for writing
                    mappingToLocId = uiMappingToLocIdReader.Read(options.MappingToLocIdPath);
                    //for reading
                    mappingToLocIdLighter = uiMappingToLocIdLighterReader.Read(options.MappingToLocIdPath);
                }
                //2 read translated resource
                var translatedResources = new Dictionary<string, ResourceContent>();
                if (Directory.Exists(options.TranslatedResourceFolderPath))
                {
                    foreach (var filePath in Directory.EnumerateFiles(options.TranslatedResourceFolderPath))
                    {
                        var culture = cultureReader.Read(filePath);
                        var cultureResource = resourceReader.Read(filePath, culture);
                        translatedResources.Add(cultureResource.Culture, cultureResource);
                    }
                }

                //3 generate result
                MappingResult result = null;
                var regexService = new RegexService();
                result = new MappingParserService(regexService).GetLocalizedMapping(sourceMapping, englishResource, translatedResources, sourceConfigMappingContent, mappingToLocId, mappingToLocIdLighter);

                //4 write "ui to resource id mapping" file
                var serializerUiToResourceID = new UIMappingToLocIdContentSerializer();
                var uiToResourceIDPath = Path.Combine(new string[] {
                    options.DestinationPath,
                    "UiToResourceID.json",
                });
                var uiMappingToLocIdContent = serializerUiToResourceID.Serialize(result.UIMappingToLocIdContent);
                if (!Directory.Exists(Directory.GetParent(uiToResourceIDPath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(uiToResourceIDPath).FullName);

                File.WriteAllText(uiToResourceIDPath, uiMappingToLocIdContent);

                //5 write UnfoundAndUnchangedSourceValues log
                var builder = new StringBuilder();
                string logPath = "";

                //builder.AppendLine("TotalStrings, TotalFound, TotalNotFound, TotalNotChanged");
                builder.AppendLine("Total found by value, Total not found by value, Total not changed");
                var props = typeof(MappingContentItem).GetProperties();
                int count = 0;
                if (result.LocalizedMappingsList.Count() != 0)
                    count = result.LocalizedMappingsList.Last().Value.Items.Select(x =>
                    {
                        int total = 0;
                        foreach (var prop in props)
                        {
                            var value = prop.GetValue(x.Value);
                            if (value != null)
                                total++;
                        }
                        return total;
                    }).Sum();

                int totalFound = result.UIMappingToLocIdContent.Mapping.Values.Select(c => c.Count).Sum();
                var notFoundCount = result.UnFoundSourceValues.Select(x => x.Value.Values.Count).Sum();
                var notChangedCount = result.UnChangedSourceValues.Select(x => x.Value.Values.Count).Sum();
                builder.AppendLine(string.Format("{0},{1},{2}", totalFound, notFoundCount, notChangedCount));
                builder.AppendLine();
                builder.AppendLine("Not found deatails");
                builder.AppendLine("sourceMappingKey,Property,Value");
                foreach (var item in result.UnFoundSourceValues)
                {
                    foreach (var subItem in item.Value.Values)
                    {
                        var row = new string[] { item.Key, subItem.PropertyName, string.Format("\"{0}\"", subItem.Value.Replace("\"", "\"\"")) };
                        builder.AppendLine(row.Aggregate((a, b) => a + "," + b));
                    }
                }

                builder.AppendLine();
                builder.AppendLine("Not changed deatails");
                builder.AppendLine("sourceMappingKey,Property,Value");
                foreach (var item in result.UnChangedSourceValues)
                {
                    foreach (var subItem in item.Value.Values)
                    {
                        var row = new string[] { item.Key, subItem.PropertyName, string.Format("\"{0}\"", subItem.Value.Replace("\"", "\"\"")) };
                        builder.AppendLine(row.Aggregate((a, b) => a + "," + b));
                    }
                }

                logPath = string.Format(@"{0}\Reports\{1}_UnfoundAndUnchangedSourceValues.csv", options.DestinationPath, Path.GetFileNameWithoutExtension(options.SourceMappingPath));
                if (!Directory.Exists(Directory.GetParent(logPath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(logPath).FullName);
                File.WriteAllText(logPath, builder.ToString());

                //  those two files below will be generated if the TranslatedResourceFolderPath's not provided
                if (Directory.Exists(options.TranslatedResourceFolderPath))
                {
                    //6 write "localized source" files.
                    var serializerSourceMapping = new SourceMappingSerializer();
                    foreach (var item in result.LocalizedMappingsList)
                    {
                        var resourcePath = Path.Combine(new string[] {
                        options.DestinationPath,
                        "mappings",
                        string.Format("{0}_{1}.json",Path.GetFileNameWithoutExtension(options.SourceMappingPath), item.Key)
                    });
                        var content = serializerSourceMapping.Serialize(item.Value);
                        if (!Directory.Exists(Directory.GetParent(resourcePath).FullName))
                            Directory.CreateDirectory(Directory.GetParent(resourcePath).FullName);

                        File.WriteAllText(resourcePath, content);
                    }

                    //7  write NotFoundTranslationValues log
                    builder = new StringBuilder();
                    builder.AppendLine("sourceMappingKey,Property,Value");
                    foreach (var item in result.UnFoundTranslationValues)
                    {
                        foreach (var subItem in item.Value.Values)
                        {
                            var row = new string[] { item.Key, subItem.PropertyName, string.Format("\"{0}\"", subItem.Value.Replace("\"", "\"\"")) };
                            builder.AppendLine(row.Aggregate((a, b) => a + "," + b));
                        }
                    }

                    logPath = string.Format(@"{0}\Reports\{1}_NotFoundTranslationValues.csv", options.DestinationPath, Path.GetFileNameWithoutExtension(options.SourceMappingPath));
                    File.WriteAllText(logPath, builder.ToString());
                }
            }
        }
    }
}
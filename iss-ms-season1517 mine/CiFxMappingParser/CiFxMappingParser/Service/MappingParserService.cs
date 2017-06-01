using CiFxMappingParser.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiFxMappingParser.Service
{
    public class MappingParserService
    {
        public RegexService RegexService { get; set; }

        public MappingParserService(RegexService regexService)
        {
            this.RegexService = regexService;
        }

        public MappingResult GetLocalizedMapping(MappingContent sourceMappingContent, ResourceContent englishResourceContent, Dictionary<string, ResourceContent> localizedResourceContent, MappingContent sourceConfigMappingContent, UIMappingToLocIdContent uiMappingToLocIdContent = null, MappingContent mappingToLocIdLighter = null)
        {
            if (!sourceMappingContent.Items.Any())
                throw new ArgumentNullException("The sourceMappingContent should not be empty");

            MappingResult mappingResult = new MappingResult();
            mappingResult.LocalizedMappingsList = new Dictionary<string, MappingContent>();
            if (uiMappingToLocIdContent != null)
                mappingResult.UIMappingToLocIdContent = uiMappingToLocIdContent;
            else
            {
                mappingResult.UIMappingToLocIdContent = new UIMappingToLocIdContent();
                mappingResult.UIMappingToLocIdContent.Mapping = new Dictionary<string, Dictionary<string, UIMappingToLocIdContentItem>>();
            }

            mappingResult.TotalDuplicatedEnglishValues = new Dictionary<string, DuplicatedContent>();
            mappingResult.UnFoundSourceValues = new Dictionary<string, ValueUnfoundContent>();
            mappingResult.UnChangedSourceValues = new Dictionary<string, ValueUnchangedContent>();
            mappingResult.UnFoundTranslationValues = new Dictionary<string, CultureValueUnfoundContent>();

            foreach (var cultureFile in localizedResourceContent)
            {
                var content = new MappingContent();
                content.Items = new Dictionary<string, MappingContentItem>();
                foreach (var item in sourceMappingContent.Items)
                {
                    content.Items.Add(item.Key, new MappingContentItem
                    {
                        Accessibility_id = item.Value.Accessibility_id,
                        Id = item.Value.Id,
                        Text = item.Value.Text,
                        Command = item.Value.Command,
                        Xpath = item.Value.Xpath
                    });
                }
                mappingResult.LocalizedMappingsList.Add(cultureFile.Key, content);
            }

            bool existLocalizedFiles = localizedResourceContent.Count == 0 ? false : true;

            //1. for each mapping key object in the source mapping file with a "text" property
            List<string> sourceMappingKeysList = sourceMappingContent.Items.Keys.ToList();

            foreach (string sourceMappingKey in sourceMappingKeysList)
            {
                if (uiMappingToLocIdContent == null)
                {
                    MappingContentItem sourceItem = sourceMappingContent.Items[sourceMappingKey];

                    List<MatchingPattern> mappingPatternList = null;
                    MappingContentItem mappingContentItem = null;
                    MappingContentItem configMappingContentItem = null;

                    //only used for android/ios/web, not for common
                    if (sourceConfigMappingContent != null)
                    {
                        configMappingContentItem = sourceConfigMappingContent.Items[sourceMappingKey];
                        mappingPatternList = this.GetMatchingPattern(sourceItem, configMappingContentItem);
                    }
                    //UiToResourceID should be stored by running any step below

                    if (configMappingContentItem == null)
                    {
                        mappingContentItem = sourceMappingContent.Items[sourceMappingKey];
                        this.SearchLocalizedValue(existLocalizedFiles, sourceMappingKey, mappingContentItem, englishResourceContent, englishResourceContent, mappingResult.UIMappingToLocIdContent, mappingPatternList, mappingResult, mappingToLocIdLighter);
                    }
                    else
                    {
                        this.SearchLocalizedValue(existLocalizedFiles, sourceMappingKey, configMappingContentItem, englishResourceContent, englishResourceContent, mappingResult.UIMappingToLocIdContent, mappingPatternList, mappingResult, mappingToLocIdLighter);
                    }
                }

                foreach (string culture in localizedResourceContent.Keys)
                {
                    MappingContentItem mappingContentItem = sourceMappingContent.Items[sourceMappingKey];
                    List<MatchingPattern> mappingPatternList = null;

                    if (sourceConfigMappingContent != null)
                    {
                        MappingContentItem configMappingContentItem = sourceConfigMappingContent.Items[sourceMappingKey];
                        mappingPatternList = this.GetMatchingPattern(mappingContentItem, configMappingContentItem);
                    }

                    //The following localizedvalue should be found based on the resourcids from "UiToResourceID" file
                    MappingContentItem localizedResourceItem = this.SearchLocalizedValue(existLocalizedFiles, sourceMappingKey, mappingContentItem, englishResourceContent, localizedResourceContent[culture], mappingResult.UIMappingToLocIdContent, mappingPatternList, mappingResult, mappingToLocIdLighter);

                    mappingResult.LocalizedMappingsList[culture].Items[sourceMappingKey] = localizedResourceItem;
                }
            }

            return mappingResult;
        }

        /// <summary>
        /// multiple resource ids might contain the same english value
        /// </summary>
        /// <param name="englishResourceContent"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<KeyValuePair<string[], string>> GetCombinationKeyByValue(ResourceContent englishResourceContent, string value)
        {
            var result = new List<KeyValuePair<string[], string>>();
            foreach (string key1 in englishResourceContent.Content.Keys)
            {
                var inner = new string[2];
                foreach (string key2 in englishResourceContent.Content[key1].Keys)
                {
                    if (englishResourceContent.Content[key1][key2] == value)
                    {
                        inner[0] = key1;
                        inner[1] = key2;
                        result.Add(new KeyValuePair<string[], string>(inner, value));
                    }
                }
            }
            return result;
        }

        public MappingContentItem GetConfigMappingContentItem(MappingContentItem sourceItem)
        { return null; }

        public List<MatchingPattern> GetMatchingPattern(MappingContentItem sourceItem, MappingContentItem mappingConfigItem)
        {
            var result = new List<MatchingPattern>();
            Dictionary<string, LocType> locTypeMapping = new Dictionary<string, LocType>
            {
                { "[contains]",LocType.Contains },
                { "[startswith]",LocType.StartsWith },
                { "[endswith]" ,LocType.EndsWith},
                { "[equals]",LocType.Equals},
                { "[regex]",LocType.Regex },
                { "[nopattern]",LocType.NoPattern }
            };
            List<string> locTypes = new List<string> { "[contains]", "[startswith]", "[endswith]", "[equals]", "[regex]", "[nopattern]" };
            var regService = new RegexService();
            LoadMatchingPatternBasedOnLocType(result, "command", locTypes, mappingConfigItem.Command, sourceItem.Command, locTypeMapping, regService);
            LoadMatchingPatternBasedOnLocType(result, "text", locTypes, mappingConfigItem.Text, sourceItem.Text, locTypeMapping, regService);
            LoadMatchingPatternBasedOnLocType(result, "xpath", locTypes, mappingConfigItem.Xpath, sourceItem.Xpath, locTypeMapping, regService);
            LoadMatchingPatternBasedOnLocType(result, "id", locTypes, mappingConfigItem.Id, sourceItem.Id, locTypeMapping, regService);
            LoadMatchingPatternBasedOnLocType(result, "accessibility_id", locTypes, mappingConfigItem.Accessibility_id, sourceItem.Accessibility_id, locTypeMapping, regService);

            return result;
        }

        //Process source and config
        private MappingContentItem SearchLocalizedValue(bool existLocalizedFiles, string sourceMappingKey, MappingContentItem sourceItem, ResourceContent englishResource, ResourceContent localizedResource, UIMappingToLocIdContent uiMappingToLocIdContent, List<MatchingPattern> matchedPatterns, MappingResult mappingResult, MappingContent mappingToLocIdLighter)
        {
            // by default assign all the values from the source
            var result = new MappingContentItem
            {
                Accessibility_id = sourceItem.Accessibility_id,
                Command = sourceItem.Command,
                Id = sourceItem.Id,
                Text = sourceItem.Text,
                Xpath = sourceItem.Xpath
            };

            if (matchedPatterns == null)
            {
                //common file properties
                var propsToSearch = new Dictionary<string, string> {
                    { nameof(sourceItem.Text), sourceItem.Text },
                    { nameof(sourceItem.Accessibility_id),sourceItem.Accessibility_id },
                    { nameof(sourceItem.Xpath),sourceItem.Xpath }
            };

                foreach (var propToSearch in propsToSearch)
                {
                    if (propToSearch.Value != null)
                    {
                        //the resourceid should be read from "UiToResourceID" file
                        KeyValuePair<string[], string>? resourceID = GetResourcedId(sourceMappingKey, propToSearch.Key.ToLower(), propToSearch.Value, englishResource, uiMappingToLocIdContent, mappingResult, mappingToLocIdLighter);

                        //skip the step to get the translation if existLocalizedFiles is true.
                        //1.2 Get Translation value
                        if (existLocalizedFiles)
                        {
                            if (resourceID.HasValue)
                            {
                                if (localizedResource.Content.ContainsKey(resourceID.Value.Key[0])
                                               && localizedResource.Content[resourceID.Value.Key[0]].ContainsKey(resourceID.Value.Key[1]))
                                {
                                    var translation = localizedResource.Content[resourceID.Value.Key[0]][resourceID.Value.Key[1]];
                                    var propInfo = (typeof(MappingContentItem)).GetProperty(propToSearch.Key);
                                    propInfo.SetValue(result, translation);
                                }
                                else
                                {
                                    //1.2.1 in the case of multiple english value can not be found
                                    var cultureMapKey = localizedResource.Culture + "\\" + sourceMappingKey;
                                    CultureValueUnfoundContent missingValues = null;
                                    if (!mappingResult.UnFoundTranslationValues.ContainsKey(cultureMapKey))
                                    {
                                        missingValues = new CultureValueUnfoundContent();
                                        mappingResult.UnFoundTranslationValues.Add(cultureMapKey, missingValues);
                                        missingValues.UiMappingKey = cultureMapKey;
                                    }
                                    else
                                        missingValues = mappingResult.UnFoundTranslationValues[cultureMapKey];

                                    missingValues.Values.Add(new CultureValueUnfoundContentItem
                                    {
                                        ResourceId = propToSearch.Value,
                                        Value = resourceID.Value.Value,
                                        PropertyName = resourceID.Value.Key[0] + "\\" + resourceID.Value.Key[1]
                                    });
                                }
                            }
                        }
                    }
                }
            }
            else if (matchedPatterns.Any())
            {
                //use regex to search for matching resource ids (regex will be applied to translations) in the resource file
                foreach (var matchedProp in matchedPatterns)
                {
                    foreach (var matchValue in matchedProp.Values)
                    {
                        if (matchValue.SourceValue != null)
                        {
                            string regexPattern = null;
                            switch (matchValue.LocType)
                            {
                                case LocType.Contains:
                                    regexPattern = string.Format(".*{0}.*", matchValue.SourceValue);
                                    break;

                                case LocType.StartsWith:
                                    regexPattern = string.Format(".*{0}", matchValue.SourceValue);
                                    break;

                                case LocType.EndsWith:
                                    regexPattern = string.Format(".*{0}", matchValue.SourceValue);
                                    break;

                                case LocType.Regex:
                                    regexPattern = string.Format("{0}", matchValue.SourceValue);
                                    break;
                            }
                            List<KeyValuePair<string[], string>> resourceIds = null;

                            //populate the uitoresourceid mapping file
                            if (regexPattern != null)
                                resourceIds = this.GetResourcedIdsByRegex(sourceMappingKey, matchedProp.PropertyName, matchValue.SourceValue, englishResource, uiMappingToLocIdContent, mappingResult, regexPattern);
                            //for string with ["Equals"]
                            else if (matchValue.LocType == LocType.Equals)
                            {
                                var rId = this.GetResourcedId(sourceMappingKey, matchedProp.PropertyName, matchValue.SourceValue, englishResource, uiMappingToLocIdContent, mappingResult, mappingToLocIdLighter);

                                if (rId.HasValue)
                                {
                                    resourceIds = new List<KeyValuePair<string[], string>> { rId.Value };
                                }
                            }
                            //string with "No patterns", insert these kinds of data into report as well
                            else
                            {
                                if (!mappingResult.UnChangedSourceValues.ContainsKey(sourceMappingKey))
                                    mappingResult.UnChangedSourceValues[sourceMappingKey] = new ValueUnchangedContent { UiMappingKey = sourceMappingKey };

                                if (!mappingResult.UnChangedSourceValues[sourceMappingKey].Values.Any(c => c.PropertyName == matchedProp.PropertyName))
                                {
                                    mappingResult.UnChangedSourceValues[sourceMappingKey].Values.Add(new ValueUnchangedContentItem
                                    {
                                        Value = matchValue.SourceValue,
                                        PropertyName = matchedProp.PropertyName
                                    });
                                }
                            }

                            //skip the step to get the translation if existLocalizedFiles is true.
                            if (existLocalizedFiles)
                            {
                                if (resourceIds != null)
                                {
                                    //set first character to upper case
                                    var firstChar = matchedProp.PropertyName[0].ToString().ToUpper();
                                    matchedProp.PropertyName = matchedProp.PropertyName.Remove(0, 1).Insert(0, firstChar);
                                    var propInfo = (typeof(MappingContentItem)).GetProperty(matchedProp.PropertyName);

                                    var translation = resourceIds
                                        .Where(c => localizedResource.Content.ContainsKey(c.Key[0]) && localizedResource.Content[c.Key[0]].ContainsKey(c.Key[1]))
                                        .Select(c => localizedResource.Content[c.Key[0]][c.Key[1]])
                                        .Aggregate((a, b) => a + "|" + b);

                                    var originalValue = propInfo.GetValue(result).ToString();
                                    translation = originalValue.Replace(matchValue.SourceValue, translation);

                                    propInfo.SetValue(result, translation);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private KeyValuePair<string[], string>? GetResourcedId(string sourceMappingKey, string propertyName, string sourceMappingValue, ResourceContent englishResource, UIMappingToLocIdContent uiMappingToLocIdContent, MappingResult mappingResult, MappingContent uiMappingToLocIdLighterVersion)
        {
            KeyValuePair<string[], string>? resourceIDandValue = null;
            //1 Get Resource ID
            if (uiMappingToLocIdContent != null && uiMappingToLocIdContent.Mapping.ContainsKey(sourceMappingKey) && uiMappingToLocIdContent.Mapping[sourceMappingKey].ContainsKey(propertyName))
            {
                //1.1 if the mapping key is inside the uiMappingToLocID file, then use the corresponding value as the resourceID to find the corresponding translation in the localized resource file
                if (uiMappingToLocIdLighterVersion != null && uiMappingToLocIdLighterVersion.Items.ContainsKey(sourceMappingKey))
                {
                    //if (uiMappingToLocIdContent.Mapping[sourceMappingKey][propertyName].ResourceIds[0].ResourceId.Split('|') != null && sourceMappingValue != null)
                    //{
                    if (uiMappingToLocIdLighterVersion.Items[sourceMappingKey].GetType().GetProperty(UppercaseFirst(propertyName)).GetValue(uiMappingToLocIdLighterVersion.Items[sourceMappingKey]) != null)
                    {
                        resourceIDandValue = new KeyValuePair<string[], string>(uiMappingToLocIdLighterVersion.Items[sourceMappingKey].GetType().GetProperty(UppercaseFirst(propertyName)).GetValue(uiMappingToLocIdLighterVersion.Items[sourceMappingKey]).ToString().Split('|'), sourceMappingValue);
                    }
                    //}
                }
            }
            else
            {
                //1.2 otherewise match the english value in the english resource file to find the resourceid
                var resourceIdsHierarchy = this.GetCombinationKeyByValue(englishResource, sourceMappingValue);

                if (resourceIdsHierarchy.Count < 1)
                {
                    //1.2.1 in the case of multiple english value can not be found
                    if (!mappingResult.UnFoundSourceValues.ContainsKey(sourceMappingKey))
                        mappingResult.UnFoundSourceValues[sourceMappingKey] = new ValueUnfoundContent { UiMappingKey = sourceMappingKey };

                    if (!mappingResult.UnFoundSourceValues[sourceMappingKey].Values.Any(c => c.PropertyName == propertyName))
                    {
                        mappingResult.UnFoundSourceValues[sourceMappingKey].Values.Add(new ValueUnfoundContentItem
                        {
                            Value = sourceMappingValue,
                            PropertyName = propertyName
                        });
                    }

                    return null;
                }

                if (resourceIdsHierarchy.Count > 1)
                {
                    resourceIDandValue = resourceIdsHierarchy[0];
                    //1.2.2 In the case of multiple resource id with the same English value in the English file:
                    //pick the first match resource id and Increment the list of TotalDuplicatedEnglishValue in the result.
                    foreach (KeyValuePair<string[], string> resourceids in resourceIdsHierarchy)
                    {
                        if (!mappingResult.TotalDuplicatedEnglishValues.ContainsKey(sourceMappingKey))
                            mappingResult.TotalDuplicatedEnglishValues[sourceMappingKey] = new DuplicatedContent { UiMappingKey = sourceMappingKey };

                        if (!mappingResult.TotalDuplicatedEnglishValues[sourceMappingKey].Values.ContainsKey(propertyName))
                            mappingResult.TotalDuplicatedEnglishValues[sourceMappingKey].Values[propertyName] = new DuplicatedContentItem
                            {
                                EnglishValue = sourceMappingValue,
                                PropertyName = propertyName,
                            };

                        mappingResult.TotalDuplicatedEnglishValues[sourceMappingKey].Values[propertyName].ResourceItems.Add(new KeyValuePair<string, string>(resourceids.Key[0] + "|" + resourceids.Key[1], sourceMappingValue));
                    }
                }
                else
                {
                    resourceIDandValue = resourceIdsHierarchy[0];
                    //add the matched resource id in UiMappingToLocId if there's only one match
                    if (!uiMappingToLocIdContent.Mapping.ContainsKey(sourceMappingKey))
                        uiMappingToLocIdContent.Mapping[sourceMappingKey] = new Dictionary<string, UIMappingToLocIdContentItem>();

                    uiMappingToLocIdContent.Mapping[sourceMappingKey].Add(propertyName, new UIMappingToLocIdContentItem
                    {
                        MatchedBy = MatchedBy.Value,
                        ResourceIds = new List<UIMappingToLocIdContentItemResource>
                        {
                            new UIMappingToLocIdContentItemResource {
                                ResourceId = resourceIDandValue.Value.Key[0] + "|" + resourceIDandValue.Value.Key[1],
                                Value = resourceIDandValue.Value.Value
                            },
                        },
                        MappingSource = sourceMappingValue
                    });
                }
            }
            return resourceIDandValue;
        }

        private List<KeyValuePair<string[], string>> GetResourcedIdsByRegex(string sourceMappingKey, string propertyName, string sourceMappingValue, ResourceContent englishResource, UIMappingToLocIdContent uiMappingToLocIdContent, MappingResult mappingResult, string regexPattern)
        {
            List<KeyValuePair<string[], string>> resouceIDsAndValues = null;
            //1 Get Resource ID
            if (uiMappingToLocIdContent != null && uiMappingToLocIdContent.Mapping.ContainsKey(sourceMappingKey) && uiMappingToLocIdContent.Mapping[sourceMappingKey].ContainsKey(propertyName))
            {
                //1.1 if the mapping key is inside the uiMappingToLocID file, then use the corresponding value as the resourceID to find the corresponding translation in the localized resource file
                resouceIDsAndValues = uiMappingToLocIdContent.Mapping[sourceMappingKey][propertyName].ResourceIds
                    .Select(c => new KeyValuePair<string[], string>(c.ResourceId.Split('|'), c.Value)).ToList();
            }
            else
            {
                //1.2 otherewise match the english value in the english resource file to find the resourceid
                var resourceIdsHierarchy = new Dictionary<string, KeyValuePair<string[], string>>();
                foreach (var rootItem in englishResource.Content)
                {
                    foreach (var childItem in rootItem.Value)
                    {
                        var matched = this.RegexService.GetValues(childItem.Value, regexPattern);
                        if (matched.Any())
                            resourceIdsHierarchy[rootItem.Key + "|" + childItem.Key] = new KeyValuePair<string[], string>(
                                new string[] { rootItem.Key, childItem.Key },
                                childItem.Value);
                    }
                }

                if (resourceIdsHierarchy.Count < 1)
                {
                    //1.2.1 in the case of multiple english value can not be found
                    if (!mappingResult.UnFoundSourceValues.ContainsKey(sourceMappingKey))
                        mappingResult.UnFoundSourceValues[sourceMappingKey] = new ValueUnfoundContent { UiMappingKey = sourceMappingKey };

                    if (!mappingResult.UnFoundSourceValues[sourceMappingKey].Values.Any(c => c.PropertyName == propertyName))
                    {
                        mappingResult.UnFoundSourceValues[sourceMappingKey].Values.Add(new ValueUnfoundContentItem
                        {
                            Value = sourceMappingValue,
                            PropertyName = propertyName
                        });
                    }

                    return null;
                }
                else
                {
                    //add the matched resource id in UiMappingToLocId if there's only one match
                    if (!uiMappingToLocIdContent.Mapping.ContainsKey(sourceMappingKey))
                        uiMappingToLocIdContent.Mapping[sourceMappingKey] = new Dictionary<string, UIMappingToLocIdContentItem>();

                    uiMappingToLocIdContent.Mapping[sourceMappingKey].Add(propertyName, new UIMappingToLocIdContentItem
                    {
                        MatchedBy = MatchedBy.Value,
                        ResourceIds = resourceIdsHierarchy.Keys.Select(x =>
                        new UIMappingToLocIdContentItemResource { ResourceId = x, Value = "" }).ToList(),
                        MappingSource = sourceMappingValue
                    });
                }

                resouceIDsAndValues = resourceIdsHierarchy.Values.ToList();
            }
            return resouceIDsAndValues;
        }

        private string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static void LoadMatchingPatternBasedOnLocType(List<MatchingPattern> result, string propertyName, List<string> locTypes, string confgPropertyItem, string sourcePropertyItem, Dictionary<string, LocType> locTypeMapping, RegexService regService)
        {
            if (confgPropertyItem != null && confgPropertyItem.ToLower().Contains("[nopattern]"))
            {
                result.Add(new MatchingPattern
                {
                    PropertyName = propertyName,
                    Values = new List<MatchingPatternItem> { new MatchingPatternItem { LocType = LocType.NoPattern, SourceValue = null } }
                });
                return;
            }
            if ((propertyName == "command"
                || propertyName == "text"
                || propertyName == "accessibility_id") && confgPropertyItem != null)
            {
                foreach (var locType in locTypes)
                {
                    if (confgPropertyItem.ToLower().Contains(locType))
                    {
                        var regexString = confgPropertyItem.Replace("\\", @"\\")
                            .Replace("|", "\\|")
                            .Replace("(", "\\(")
                            .Replace(")", "\\)")
                            .Replace("[", "\\[")
                            .Replace("]", "\\]")
                            .Replace(".", "\\.")
                            .Replace("*", "\\*");

                        //reset locType back because of first escaping
                        regexString = regexString.Replace("\\" + locType.Replace("]", "\\]"), locType);
                        //replace with the global pattern
                        regexString = regexString.Replace(locType, @"([^|]+)");
                        var regexResult = regService.GetValues(sourcePropertyItem, regexString);
                        if (regexResult != null)
                        {
                            result.Add(new MatchingPattern
                            {
                                PropertyName = propertyName,
                                Values = regexResult.Select(c => new MatchingPatternItem { LocType = locTypeMapping[locType], SourceValue = c }).ToList()
                            });
                            return;
                        }
                    }
                }
            }
            if (sourcePropertyItem != null)
                result.Add(new MatchingPattern
                {
                    PropertyName = propertyName,
                    Values = new List<MatchingPatternItem> { new MatchingPatternItem { LocType = LocType.NoPattern, SourceValue = sourcePropertyItem } }
                });
        }
    }
}
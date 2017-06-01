using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheLongestString.LocFileProvider
{
    class JSLocDocument : ILocDocument
    {
        private readonly string source;
        private readonly bool isEnglishCulture;

        public static IEnumerable<string> AllowedExtensions
        {
            get
            {
                return new List<string>
                {
                    ".js",
                };
            }
        }

        public string Name { get; private set; }

        public string SourceCultureName { get; private set; }

        public string SourceCultureEnglishName { get; private set; }

        public string TargetCultureName { get; private set; }

        public string TargetCultureEnglishName { get; private set; }

        private JSLocDocument(string filePath)
        {
            var cultureName = GetCultureName(filePath);
            this.source = filePath;
            this.Name = filePath;

            this.TargetCultureName = cultureName;
            this.TargetCultureEnglishName = cultureName;

            if (cultureName.ToLower() == "en-us")
            {
                cultureName = "en-US";
                this.SourceCultureName = cultureName;
                this.SourceCultureEnglishName = cultureName;
                this.isEnglishCulture = true;
            }
        }

        public IDictionary<string, LocString> GetDocumentLocStrings(LocDictionary parent, bool isSource)
        {
            var locStrings = new Dictionary<string, LocString>();

            //1 load .strings file
            var jsDocument = File.ReadLines(source);

            var stringList = new Dictionary<string, string>();
            const string resourceRegexString = "([^:\\s]+): \\\"(.+)\\\"";
            const string commentRegexString = "(/\\*([^*]|[\r\n]|(\\*+([^*/]|[\r\n])))*\\*+/)|(//.*)";

            Regex resourceRegex = new Regex(resourceRegexString);
            Regex commentRegex = new Regex(commentRegexString);

            foreach (string line in jsDocument)
            {
                // Store the node's key and value (app string)
                string key = string.Empty;
                string value = string.Empty;
                if (commentRegex.IsMatch(line))
                {
                    continue;
                }
                if (resourceRegex.IsMatch(line))
                {
                    GroupCollection matchGroups = resourceRegex.Match(line).Groups;

                    key = matchGroups[1].Value;
                    value = matchGroups[2].Value;

                    stringList.Add(key, value);
                }
            }

            //2 create LocString items from .strings file
            foreach (var dict in stringList)
            {
                //3 set if parent was loaded with source

                if (isSource)
                {
                    if (isEnglishCulture)
                        locStrings.Add(dict.Key.ToString(), new LocString(parent, dict.Value.ToString()));
                    else
                        locStrings.Add(dict.Key.ToString(), new LocString(parent, "", false));
                }

                else if (!isEnglishCulture)
                    locStrings.Add(dict.Key.ToString(), new LocString(parent, dict.Value.ToString()));
            }

            parent.WithSourceStrings = parent.WithSourceStrings == false ? isEnglishCulture : parent.WithSourceStrings;

            return locStrings;
        }

        private string GetCultureName(string filePath)
        {
            string result = "n/a";
            List<char> temp = new List<char>();

            for (int i = filePath.Length - 1; i >= 0; i--)
            {
                if (filePath[i] != '\\' && filePath[i] != '.')
                {
                    temp.Add(filePath[i]);
                }
                else
                {
                    var stringPart = new string(ReverseArray(temp.ToArray()));
                    if (stringPart.Contains("-"))
                        return stringPart.ToLower();
                    else
                        temp.Clear();
                }
            }

            return result;
        }

        private char[] ReverseArray(char[] array)
        {
            for (int i = 0; i < array.Length / 2; i++)
            {
                var temp = array[i];
                array[i] = array[array.Length - 1 - i];
                array[array.Length - 1 - i] = temp;
            }

            return array;
        }

        internal static ILocDocument Create(string file)
        {
            return new JSLocDocument(file);
        }
    }
}

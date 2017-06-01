using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TheLongestString.LocFileProvider
{
    class JsonLocDocument : ILocDocument
    {
        private readonly string source;
        private readonly bool isEnglishCulture;

        public static IEnumerable<string> AllowedExtensions
        {
            get
            {
                return new List<string>
                {
                    ".json",
                };
            }
        }

        public string Name { get; private set; }

        public string SourceCultureName { get; private set; }

        public string SourceCultureEnglishName { get; private set; }

        public string TargetCultureName { get; private set; }

        public string TargetCultureEnglishName { get; private set; }

        private JsonLocDocument(string filePath)
        {
            var cultureName = GetCultureName(filePath);
            this.source = filePath;
            this.Name = filePath;

            this.TargetCultureName = cultureName;
            this.TargetCultureEnglishName = cultureName;

            if (cultureName.ToLower() == "en")
            {
                cultureName = "en";
                this.SourceCultureName = cultureName;
                this.SourceCultureEnglishName = cultureName;
                this.isEnglishCulture = true;
            }
        }

        /// <summary>
        /// Tranverse Json string, connect the property Name with "_" until reach the end as key,  property value as value
        /// </summary>
        /// <param name="token"></param>
        /// <param name="propName"></param>
        /// <param name="stringsList"></param>
        private void tranversJToken(JToken token,  string propName,ref Dictionary<string, string> stringsList)
        {
            var prop = token as JProperty;
            if (prop != null)
            {
                propName = propName + "_" + prop.Name;
            }
            if (prop != null && prop.Value.GetType().Name.ToLower().Equals("jvalue"))
            {

                string _propName = propName.Substring(1);
                string _prop = prop.Value.ToString();
                stringsList[_propName] = _prop;
                return;
            }
            
            foreach (JToken child in token.Children())
            {
                tranversJToken(child, propName,ref stringsList);
            }
        }

        public IDictionary<string, LocString> GetDocumentLocStrings(LocDictionary parent, bool isSource)
        {
            var locStrings = new Dictionary<string, LocString>();
            var stringList = new Dictionary<string, string>();


            //1 load .json file
            var content = File.ReadAllText(source);
            var objects = JArray.Parse("["+content+"]"); // parse as array  
            JObject o = JObject.Parse(content);
            foreach (JToken child in o.Children())
            {
                tranversJToken(child, "", ref stringList);
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
            string cultureName = "";

            int i0 = filePath.LastIndexOf("\\");
            int i1 = filePath.IndexOf("_", i0);
            int i2 = filePath.LastIndexOf(".");
            if (i1 != -1) {
                cultureName = filePath.Substring(i1 + 1,i2-i1-1);
            }

            return cultureName;
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
            return new JsonLocDocument(file);
        }
    }
}

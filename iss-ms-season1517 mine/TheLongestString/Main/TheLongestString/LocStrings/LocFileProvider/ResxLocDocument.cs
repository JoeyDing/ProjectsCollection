﻿using Microsoft.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheLongestString.LocFileProvider
{
    public class ResxLocDocument : ILocDocument
    {
        private readonly string source;
        private readonly bool isEnglishCulture;

        public static IEnumerable<string> AllowedExtensions
        {
            get
            {
                return new List<string>
                {
                    ".resx",
                    ".resw",
                };
            }
        }

        public string Name { get; private set; }

        public string SourceCultureName { get; private set; }

        public string SourceCultureEnglishName { get; private set; }

        public string TargetCultureName { get; private set; }

        public string TargetCultureEnglishName { get; private set; }

        private ResxLocDocument(string filePath)
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

        public static ResxLocDocument Create(string filePath)
        {
            return new ResxLocDocument(filePath);
        }

        public IDictionary<string, LocString> GetDocumentLocStrings(LocDictionary parent, bool isSource)
        {
            var locStrings = new Dictionary<string, LocString>();

            //1 load resx
            var resx = new ResXResourceReader(source);
            IDictionaryEnumerator dict = resx.GetEnumerator();
            //2 create LocString items from resx
            while (dict.MoveNext())
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
    }
}
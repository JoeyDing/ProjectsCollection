using Microsoft.Localization;
using Microsoft.Localization.Lcx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLongestString.LocFileProvider
{
    public class LcxLocDocument : ILocDocument
    {
        private readonly LocDocument source;

        public static IEnumerable<string> AllowedExtensions
        {
            get
            {
                return new List<string>
                {
                    ".lct",
                    ".lcl"
                };
            }
        }

        public string Name { get; private set; }

        public string SourceCultureName { get; private set; }

        public string SourceCultureEnglishName { get; private set; }

        public string TargetCultureName { get; private set; }

        public string TargetCultureEnglishName { get; private set; }

        private LcxLocDocument(string path)
        {
            using (var reader = new LcxReaderWriter(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                this.source = reader.Load();
            }

            this.Name = this.source.Name;
            this.SourceCultureName = this.source.SourceCulture.Name;
            this.SourceCultureEnglishName = this.source.SourceCulture.EnglishName;
            this.TargetCultureName = this.source.TargetCulture.Name;
            this.TargetCultureEnglishName = this.source.TargetCulture.EnglishName;
        }

        public static LcxLocDocument Create(string path)
        {
            return new LcxLocDocument(path);
        }

        public IDictionary<string, LocString> GetDocumentLocStrings(LocDictionary parent, bool isSource)
        {
            parent.WithSourceStrings = isSource;
            var locStrings = new Dictionary<string, LocString>();
            this.AddToStrings(locStrings, source.Items, parent, isSource);
            return locStrings;
        }

        /// <summary>
        /// Recursively search through the document tree and add all the valid loc strings.
        /// </summary>
        /// <param name="locItemList"></param>
        private void AddToStrings(IDictionary<string, LocString> locStrings, LocItemList locItemList, LocDictionary parent, bool isSource)
        {
            foreach (var i in locItemList)
            {
                if (i.HasChildren)
                {
                    AddToStrings(locStrings, i.Children, parent, isSource);
                }

                try
                {
                    if (IsValidLocItemWithString(i))
                    {
                        var newString = GetStringFromLocItem(i, isSource, parent);
                        if (i.Id.ItemId.HasStringId)
                            locStrings.Add(i.Id.ItemId.StringId, newString);
                        else
                            locStrings.Add(i.Id.ItemId.NumericId.ToString(), newString);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

        private LocString GetStringFromLocItem(LocItem item, bool useSource, LocDictionary parent)
        {
            string newString = null;

            if (useSource)
            {
                newString = item.String.RawString;
            }
            else if (item.String.TargetString != null)
            {
                newString = item.String.TargetString.RawString;
            }

            var locString = new LocString(parent, newString);

            return locString;
        }

        /// <summary>
        /// True if this LocItem has a valid string.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool IsValidLocItemWithString(LocItem item)
        {
            if (item.HasString && item.Display.LocTable)
            {
                if (item.String != null && !item.String.DevLocked && !String.IsNullOrWhiteSpace(item.String.RawString))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
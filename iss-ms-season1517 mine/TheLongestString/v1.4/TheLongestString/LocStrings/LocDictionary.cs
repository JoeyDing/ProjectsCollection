using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Localization;

namespace TheLongestString
{
    /// <summary>
    /// Summarizes a LocDocument into a basic dictionary of Id and translation.
    /// </summary>
    public class LocDictionary
    {
        /// <summary>
        /// Dictionary of LocId and the translation string.
        /// </summary>
        private Dictionary<string, LocString> _strings = new Dictionary<string, LocString>();

        /// <summary>
        /// Using a LocDocument, creates a Dictionary to easily access a list of the LocIds and their corressponding translated strings.
        /// </summary>
        /// <param name="document">LocDocument with the strings.</param>
        /// <param name="useSource">If true, will create a dictionary with the source culture (e.g. en-US) instead of the target culture.</param>
        public LocDictionary(LocDocument document, bool useSource = false)
        {
            IsSource = useSource;

            if (useSource)
            {
                CultureName = document.SourceCulture.Name;
                CultureEnglishName = document.SourceCulture.EnglishName;
            }
            else
            {
                CultureName = document.TargetCulture.Name;
                CultureEnglishName = document.TargetCulture.Name;
            }
            

            // doc stores translated strings in subitems, recurse through them
            AddToStrings(document.Items);
        }

        /// <summary>
        /// Get the translation for a given LocId.
        /// </summary>
        /// <param name="locId">Localization ID, e.g. IDS_COLLAB_UNSUPPORTED_ANNOTATIONS</param>
        /// <returns></returns>
        public LocString GetLocString(string locId)
        {
            if (_strings.ContainsKey(locId))
            {
                return _strings[locId];
            }

            return null;
        }

        public IEnumerable<string> LocIds
        {
            get
            {
                return _strings.Keys;
            }
        }

        public IDictionary<string, LocString> Strings
        {
            get { return _strings; }
        }

        /// <summary>
        /// Recursively search through the document tree and add all the valid loc strings.
        /// </summary>
        /// <param name="locItemList"></param>
        private void AddToStrings(LocItemList locItemList)
        {
            foreach (var i in locItemList)
            {
                if (i.HasChildren)
                {
                    AddToStrings(i.Children);
                }

                try
                {
                    if (IsValidLocItemWithString(i))
                    {
                        var newString = GetStringFromLocItem(i, IsSource);
                        _strings.Add(i.Id.ItemId.StringId, newString);

                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

        private LocString GetStringFromLocItem(LocItem item, bool useSource)
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

            var locString = new LocString(this, newString);

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

        public string CultureName { get; private set; }

        public string CultureEnglishName { get; private set; }

        /// <summary>
        /// True if this is the original source language that we're translating FROM.
        /// </summary>
        public bool IsSource { get; private set; }

        public override string ToString()
        {
            return CultureName;
        }


        public void CalculateWidths(System.Windows.Media.Typeface renderTypeface, double fontSize)
        {
            foreach (var pair in _strings)
            {
                pair.Value.Width = LocString.CalculateTextWidth(pair.Value, renderTypeface, fontSize);
            }
        }
    }
}

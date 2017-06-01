using Microsoft.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLongestString.LocFileProvider;

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
       
        public LocDictionary()
        {
        }
        /// <summary>
        /// Using a list of LocDocuments, creates a source Dictionary to easily access a list of the LocIds and their corressponding source strings.
        /// </summary>
        /// <param name="documents">List of LocDocuments with the strings.</param>
        /// <param name="useSource">If true, will create a dictionary with the source culture (e.g. en-US) instead of the target culture.</param>
        public LocDictionary(IEnumerable<ILocDocument> documents)
        {
            IsSource = true;
            var firstDocWithCulture = documents.FirstOrDefault(d => d.SourceCultureName != null);
            CultureName = firstDocWithCulture == null ? null : firstDocWithCulture.SourceCultureName;
            CultureEnglishName = firstDocWithCulture == null ? null : firstDocWithCulture.SourceCultureEnglishName;

            //"OrderByDescending" is there to start document loading with the english culture file
            foreach (var document in documents.OrderByDescending(c => c.SourceCultureName))
            {
                // doc stores source strings in subitems, recurse through them
                AddToStrings(document.GetDocumentLocStrings(this, this.IsSource));
            }
        }

        /// <summary>
        /// Using a LocDocument, creates a Dictionary to easily access a list of the LocIds and their corressponding translated strings.
        /// </summary>
        /// <param name="document">LocDocument with the strings.</param>
        /// <param name="useSource">If true, will create a dictionary with the source culture (e.g. en-US) instead of the target culture.</param>
        public LocDictionary(ILocDocument document, string filePath, bool useSource = false)
        {
            IsSource = useSource;
            this.FileName = document.Name;

            if (useSource)
            {
                CultureName = document.SourceCultureName;
                CultureEnglishName = document.SourceCultureEnglishName;
            }
            else
            {
                CultureName = document.TargetCultureName;
                CultureEnglishName = document.TargetCultureEnglishName;
            }

            // doc stores translated strings in subitems, recurse through them
            AddToStrings(document.GetDocumentLocStrings(this, this.IsSource));
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

        public List<string> LocIds
        {
            get
            {
                return _strings.Keys.ToList();
            }
        }

        public Dictionary<string, LocString> Strings
        {
            get { return _strings; }
            set {
                _strings = value;
            }
        }

        private void AddToStrings(IDictionary<string, LocString> items)
        {
            foreach (var item in items)
            {
                //only load non-added strings and don't override already added strings
                //otherwise some english strings might be overriden with empty value
                if (!_strings.ContainsKey(item.Key))
                {
                    _strings[item.Key] = item.Value;
                }
            }
        }

        public string CultureName { get; set; }

        public string CultureEnglishName { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// True if this is the original source language that we're translating FROM.
        /// </summary>
        public bool IsSource { get; set; }

        public bool WithSourceStrings { get; set; }

        public override string ToString()
        {
            return CultureName;
        }

        public void CalculateWidthsAndHeights(System.Windows.Media.Typeface renderTypeface, double fontSize)
        {
            foreach (var pair in _strings)
            {
                pair.Value.Width = LocString.CalculateTextWidth(pair.Value, renderTypeface, fontSize);
                pair.Value.Height = LocString.CalculateTextHeight(pair.Value, renderTypeface, fontSize);
            }
        }
    }
}
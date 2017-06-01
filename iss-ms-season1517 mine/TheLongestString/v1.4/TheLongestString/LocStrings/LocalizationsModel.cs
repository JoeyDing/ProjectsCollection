using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;

using Microsoft.Localization;
using Microsoft.Localization.Lcx;

namespace TheLongestString
{
    public class LocalizationsModel
    {
        private List<LocDictionary> _dictionaries;

        public LocDictionary SourceDictionary { get; private set; }

        /// <summary>
        /// Create a new dictionary of Loc ids and their associated translations for a given document.
        /// </summary>
        /// <param name="doc">Document to create the dictionary from.</param>
        /// <param name="renderTypeface">Typeface to use to calculate the string width in pixels.</param>
        /// <param name="renderFontSize">Font size to use to calculate the string width in pixels.</param>
        /// <param name="useSourceDictionary">If true, will use the source culture instead of the target culture, e.g. en-US.</param>
        /// <returns></returns>
        private LocDictionary CreateLocDictionary(LocDocument doc, Typeface renderTypeface = null, double renderFontSize = 0, bool useSourceDictionary = false)
        {
            System.Diagnostics.Debug.WriteLine("[{0}] Processing {1}...", System.Threading.Thread.CurrentThread.ManagedThreadId, doc.TargetCulture);

            var dict = new LocDictionary(doc, useSourceDictionary);

            if (renderTypeface != null && renderFontSize > 0)
            {
                dict.CalculateWidths(renderTypeface, renderFontSize);
            }

            return dict;
        }

        public void LoadFilesInParallel(
            IList<string> filePaths,
            Typeface renderTypeface,
            double renderFontSize)
        {
            if (filePaths.Count <= 0)
            {
                return;
            }

            // load the files up
            var docs = new List<LocDocument>();
            foreach (string file in filePaths)
            {
                try
                {
                    using (var reader = new LcxReaderWriter(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        docs.Add(reader.Load());
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading {0}: {1}", file, e);
                }
            }

            // list of tasks, add 1 for the source dictionary (e.g. en-US)
            var tasks = new Task<LocDictionary>[docs.Count];

            // parse the source dictionary
            var sourceDictionaryTask = Task<LocDictionary>.Factory.StartNew(() => CreateLocDictionary(docs[0], renderTypeface, renderFontSize, true));

            // parse all the translations
            for (int i = 0; i < docs.Count; ++i)
            {
                var doc = docs[i];
                tasks[i] = Task<LocDictionary>.Factory.StartNew(() => CreateLocDictionary(doc, renderTypeface, renderFontSize));
            }

            try
            {
                // wait for everything to finish
                Task.WaitAll(sourceDictionaryTask);
                Task.WaitAll(tasks);

                // commit changes
                SourceDictionary = sourceDictionaryTask.Result;

                _dictionaries = new List<LocDictionary>(filePaths.Count + 1);
                _dictionaries.Add(SourceDictionary);

                foreach (var t in tasks)
                {
                    _dictionaries.Add(t.Result);
                }
            }
            catch (AggregateException ae)
            {
                System.Diagnostics.Debug.WriteLine("Aggregate Exception: {0}", ae.InnerExceptions.Count);

                for (int i = 0; i < ae.InnerExceptions.Count; ++i)
                {
                    System.Diagnostics.Debug.WriteLine("Inner exception #{0}: {1}", i + 1, ae.InnerExceptions[i]);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Load Files exception: {0}", e);
            }
        }

        /// <summary>
        /// Gets the collection of translations for a given id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public LocStringCollection GetLocStrings(string itemId)
        {
            var col = new LocStringCollection();

            col.Id = itemId;

            col.MaximumWidth = 0;

            foreach (var d in _dictionaries)
            {
                LocString s = null;
                try
                {
                    s = d.GetLocString(itemId);

                    if (s != null)
                    {
                        col.Add(s);
                        col.MaximumWidth = Math.Max(col.MaximumWidth, s.Width);

                        if (s.Dictionary == SourceDictionary)
                        {
                            col.SourceCultureWidth = s.Width;
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }

            // mark the max width strings
            col.ForEach(s => s.IsMaxWidth = s.Width == col.MaximumWidth);

            // calculate the average
            col.AverageWidth = col.Average(s => s.Width);

            return col;
        }

        /// <summary>
        ///  Update all related font information for already loaded files, if the files have not been loaded yet, an exception will be thrown
        /// </summary>
        /// <param name="renderTypeFace"></param>
        /// <param name="renderFontSize"></param>
        public void UpdateModelFontInfo(Typeface renderTypeFace, double renderFontSize)
        {
            if (renderTypeFace != null && renderFontSize > 0)
            {
                if (_dictionaries != null)
                {
                    foreach (var dict in _dictionaries)
                    {
                        dict.CalculateWidths(renderTypeFace, renderFontSize);
                    }
                }
                else
                {
                    throw new ArgumentException("Files must be loaded first");
                }
            }
            else
            {
                throw new ArgumentException("Invalid parameters: typeface cannot be null/fontsize cannot be <= 0");
            }
        }
    }
}

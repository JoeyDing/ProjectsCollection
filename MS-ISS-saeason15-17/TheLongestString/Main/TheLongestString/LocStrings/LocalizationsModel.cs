using Microsoft.Localization;
using Microsoft.Localization.Lcx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TheLongestString.LocFileProvider;
using TheLongestString.Model;

namespace TheLongestString
{
    public class LocalizationsModel
    {
        private List<LocDictionary> _dictionaries;

        public List<LocDictionary> Dictionaries
        {
            get
            {
                return _dictionaries;
            }
            set
            {
                _dictionaries = value;
            }
        }

        public LocDictionary SourceDictionary { get; set; }

        /// <summary>
        /// Create a new dictionary of Loc ids and their associated translations for a given document.
        /// </summary>
        /// <param name="doc">Document to create the dictionary from.</param>
        /// <param name="renderTypeface">Typeface to use to calculate the string width in pixels.</param>
        /// <param name="renderFontSize">Font size to use to calculate the string width in pixels.</param>
        /// <param name="useSourceDictionary">If true, will use the source culture instead of the target culture, e.g. en-US.</param>
        /// <returns></returns>
        private LocDictionary CreateLocDictionary(ILocDocument doc, string path, Typeface renderTypeface = null, double renderFontSize = 0, bool useSourceDictionary = false)
        {
            System.Diagnostics.Debug.WriteLine("[{0}] Processing {1}...", System.Threading.Thread.CurrentThread.ManagedThreadId, doc.TargetCultureName);

            var dict = new LocDictionary(doc, path, useSourceDictionary);

            if (renderTypeface != null && renderFontSize > 0)
            {
                dict.CalculateWidthsAndHeights(renderTypeface, renderFontSize);
            }

            return dict;
        }

        private LocDictionary CreateGlobalSourceLocDictionary(IEnumerable<ILocDocument> docs, Typeface renderTypeface = null, double renderFontSize = 0)
        {
            //get list of distincts doc
            var uniqueDocs = docs.GroupBy(x => new FileInfo(x.Name).Name + x.SourceCultureName).Select(g => g.First()).ToList();
            //get first matches
            var dict = new LocDictionary(uniqueDocs);
            //foreach (var doc in uniqueDocs)
            //{
            //    System.Diagnostics.Debug.WriteLine("[{0}] Processing {1}...", System.Threading.Thread.CurrentThread.ManagedThreadId, doc.TargetCulture);

            //}

            if (renderTypeface != null && renderFontSize > 0)
            {
                dict.CalculateWidthsAndHeights(renderTypeface, renderFontSize);
            }

            return dict;
        }

        public bool LoadFilesInParallel(
            IList<string> filePaths,
            Typeface renderTypeface,
            double renderFontSize)
        {
            if (filePaths.Count <= 0)
            {
                return false;
            }

            //1. iterate through file paths, and create lis of LocDocument, which
            //save the file name, source & target culture information, and majorly
            //implement the method GetDocumentLocStrings to get Dictionary<Ids,LocString[LocDictionary its belongs to, font information, transltation]>
            //So that you can iterate different type of file with ILocDocument interface
            var docs = new Dictionary<string, ILocDocument>();
            foreach (string file in filePaths)
            {
                try
                {
                    if (LcxLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, LcxLocDocument.Create(file));
                    }
                    else if (ResxLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, ResxLocDocument.Create(file));
                    }
                    else if (XmlLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, XmlLocDocument.Create(file));
                    }
                    else if (StringsLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, StringsLocDocument.Create(file));
                    }
                    else if (JSLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, JSLocDocument.Create(file));
                    }
                    else if (JsonLocDocument.AllowedExtensions.Any(e => file.EndsWith(e)))
                    {
                        docs.Add(file, JsonLocDocument.Create(file));
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading {0}: {1}", file, e);
                }
            }

            //2. Iterate throught List<LocDocument> to read LocDictionary and save it to _dictionaries
            //and caculate font information
            if (docs.Count > 0)
            {
                try
                {
                    _dictionaries = new List<LocDictionary>(filePaths.Count + 1);
                    // parse the source dictionary
                    var sourceDictionaryTask = Task<LocDictionary>.Factory.StartNew(() => CreateGlobalSourceLocDictionary(docs.Values, renderTypeface, renderFontSize));

                    var translationsTask = sourceDictionaryTask.ContinueWith((antecendent) =>
                    {
                        // commit changes
                        SourceDictionary = antecendent.Result;
                        _dictionaries.Add(SourceDictionary);

                        // list of tasks, add 1 for the source dictionary (e.g. en-US)
                        var tasks = new Task<LocDictionary>[docs.Count];
                        // parse all the translations
                        int x = 0;
                        foreach (var doc in docs)
                        {
                            tasks[x] = Task<LocDictionary>.Factory.StartNew(() => CreateLocDictionary(doc.Value, doc.Key, renderTypeface, renderFontSize));
                            x++;
                        }
                        // wait for everything to finish
                        Task.WaitAll(sourceDictionaryTask);
                        foreach (var t in tasks)
                        {
                            _dictionaries.Add(t.Result);
                        }
                    });

                    Task.WaitAll(translationsTask);

                    return true;
                }
                catch (AggregateException ae)
                {
                    System.Diagnostics.Debug.WriteLine("Aggregate Exception: {0}", ae.InnerExceptions.Count);

                    for (int i = 0; i < ae.InnerExceptions.Count; ++i)
                    {
                        System.Diagnostics.Debug.WriteLine("Inner exception #{0}: {1}", i + 1, ae.InnerExceptions[i]);
                    }
                    return false;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Load Files exception: {0}", e);
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Gets the collection of translations for a given id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public LocStringCollection GetLocStrings(string itemId, ref List<FontConfigModel> cultureFonts, string defaultFontName, double defaultFontSize, List<LocString> source = null, bool allowNonSupportedFont = true)
        {
            List<LocString> items = new List<LocString>();
            LocStringCollection col = new LocStringCollection(items);

            if (source != null)
            {
                items = source;
                col = new LocStringCollection(items);

                //col.MaximumWidth = items.Max(s => s.Width);
                //col.MaximumHeight = items.Max(s => s.Height);
                foreach (var s in items)
                {
                    col.MaximumWidth = Math.Max(col.MaximumWidth, s.Width);
                    col.MaximumHeight = Math.Max(col.MaximumHeight, s.Height);
                }
            }
            else
            {
                items = new List<LocString>();
                col = new LocStringCollection(items);

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
                            //update loc string based on font
                            CultureFontUtil.UpdateLocStringFromCultureFont(ref cultureFonts, s, defaultFontName, defaultFontSize);

                            if (allowNonSupportedFont || s.IsFontSupported)
                            {
                                if (s != null && s.HasValue)
                                {
                                    col.MaximumWidth = Math.Max(col.MaximumWidth, s.Width);
                                    col.MaximumHeight = Math.Max(col.MaximumHeight, s.Height);

                                    if (s.Dictionary == SourceDictionary)
                                    {
                                        col.SourceCultureWidth = s.Width;
                                        col.SourceCultureHeight = s.Height;
                                    }

                                    items.Add(s);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
            }

            // mark the max width/height strings
            double totalWidth = 0;
            double totalHeight = 0;
            items.ForEach(s =>
                {
                    s.IsMaxWidth = s.Width == col.MaximumWidth;
                    s.IsMaxHeight = s.Height == col.MaximumHeight;
                    totalWidth += s.Width;
                    totalHeight += s.Height;
                });

            // calculate the average
            // col.AverageWidth = items.Average(s => s.Width);
            //col.AverageHeight = items.Average(s => s.Height);
            col.AverageWidth = totalWidth / items.Count;
            col.AverageHeight = totalHeight / items.Count;

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
                        dict.CalculateWidthsAndHeights(renderTypeFace, renderFontSize);
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
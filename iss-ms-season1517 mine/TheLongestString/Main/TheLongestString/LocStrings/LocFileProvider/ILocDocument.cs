using Microsoft.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLongestString.LocFileProvider
{
    public interface ILocDocument
    {
        string Name { get; }
        /// <summary>
        /// Specified [Source Ids] Source Culture Name(customized, e.g en-US)
        /// </summary>
        string SourceCultureName { get; }

        /// <summary>
        /// Specified [Source Ids] Source Culture english Name
        /// </summary>
        string SourceCultureEnglishName { get; }

        /// <summary>
        ///  Specified current document's target culture Name
        /// </summary>
        string TargetCultureName { get; }

        /// <summary>
        /// Specified current document's target culture english Name
        /// </summary>
        string TargetCultureEnglishName { get; }

        /// <summary>
        /// Read key,value pair from loc files, and save as [Resource Ids]-LocString pair(contains fonts etc information and used to bind view model)
        /// LocDictionary contains all Recource Ids for specific culture(probably contains multiple files)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="isSource"></param>
        /// <returns></returns>
        IDictionary<string, LocString> GetDocumentLocStrings(LocDictionary parent, bool isSource);
    }
}
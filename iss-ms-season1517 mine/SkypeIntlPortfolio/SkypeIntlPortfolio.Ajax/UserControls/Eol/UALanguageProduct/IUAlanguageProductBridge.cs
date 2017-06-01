using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol.UALanguageProduct
{
    public interface IUAlanguageProductBridge : IUALanguageBridgeCommon
    {
        event Func<string, int, int, List<UALanguage_Base>> GetUALanguageUnderPrudctByType;

        event Func<string, int> GetTotalCountOfUALanguageUnderPrudctByType;

        event Func<string, int, int, int, List<ResourceFile>> GetFilesUnderLanguage;

        event Func<string, int, int> GetTotalCountOfFilesUnderLanguage;

        event Func<List<ResourceFile>> GetFilesListUnderProductForInsert;

        event Action<int> DeleteUALanguage;

        event Func<string, List<BasicLanguageList_Base>> GetFilteredLanguageListForProductLevel;

        event Func<List<Products_New>> GetAllProducts;

        event Action<string, int, string> InsertFromAnotherProject;

        event Action<string, string> MoveEolFromOneTypeToAnother;
    }
}
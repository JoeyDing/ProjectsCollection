using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol.UILanguageProduct
{
    public interface IUIlanguageProductBridge : IUILanguageBridgeCommon
    {
        event Func<string, int, int, List<UILanguage_Base>> GetUILanguageUnderPrudctByType;

        event Func<string, int> GetTotalCountOfUILanguageUnderPrudctByType;

        event Func<string, int, int, int, List<ResourceFile>> GetFilesUnderLanguage;

        event Func<string, int, int> GetTotalCountOfFilesUnderLanguage;

        event Func<List<ResourceFile>> GetFilesListUnderProductForInsert;

        event Action<int> DeleteUiLanguage;

        event Func<string, List<BasicLanguageList_Base>> GetFilteredLanguageListForProductLevel;

        event Func<List<Products_New>> GetAllProducts;

        event Action<int, string> InsertFromAnotherProject;

        event Action<string, string> MoveEolFromOneTypeToAnother;
    }
}
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface IUILanguageBridge : IUILanguageBridgeCommon
    {
        event Func<int, int, List<GetUILanguageFileOfProductByPage_Result>> onGetUILanguageFileOfProduct;

        event Func<int> onGetTotalRecord;

        event Func<int, string, int> onGetTotalRecordPerFile;

        event Func<int, string, int, int, List<UILanguage_Base>> onGetUILanguagePerFile;

        event Func<int, List<BasicLanguageList>> GetFilteredLanguageListForFileLevel;
    }
}
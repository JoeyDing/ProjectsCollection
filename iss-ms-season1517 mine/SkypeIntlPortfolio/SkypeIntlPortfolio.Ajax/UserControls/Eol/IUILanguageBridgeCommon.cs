using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface IUILanguageBridgeCommon
    {
        event EventHandler<UILanguage> onUpdateLanguage;

        event Action<bool, List<UILanguage_Base>> onInsertLanguage;

        event Action<int, int> onDeleteFiles;

        event EventHandler<List<BasicLanguageList>> onGetBasicLanguage;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface ISpokenInputOutputView : ICommonInputOutputBridge
    {
        Dictionary<int, List<GetSpokenInputOutputOfProduct_Result>> DictSpokenInputOutputs_Result { get; set; }

        event Func<int, List<GetSpokenInputOutputOfProduct_Result>> GetSpokenInputOutput;

        event EventHandler<SpokenInputOutput> onUpdateInputOutput;

        event EventHandler<SpokenInputOutput> onInsertInputOutput;

        event EventHandler<SpokenInputOutput> onDeleteInputOutput;
    }
}
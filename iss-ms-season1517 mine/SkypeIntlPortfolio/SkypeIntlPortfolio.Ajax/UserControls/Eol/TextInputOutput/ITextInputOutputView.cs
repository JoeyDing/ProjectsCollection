using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface ITextInputOutputBridge : ICommonInputOutputBridge
    {
        Dictionary<int, Dictionary<int, GetTextInputOutputOfProduct_Result>> DictTextInputOutputs_Result { get; set; }

        event Func<int, List<GetTextInputOutputOfProduct_Result>> GetTextInputOutput;

        event EventHandler<TextInputOutput> onUpdateInputOutput;

        event EventHandler<TextInputOutput> onInsertInputOutput;

        event EventHandler<TextInputOutput> onDeleteInputOutput;
    }
}
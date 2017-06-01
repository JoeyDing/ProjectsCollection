using Automation.UI.Shell.Wpf.Infrastructure.ConfigPopUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts
{
    public interface ILoadDictForPopup
    {
        void LoadControlsRecurrsively(Grid dynamicGrid, object parentObject, ref int rowIndex, Dictionary<string, List<string>> dict);
    }
}
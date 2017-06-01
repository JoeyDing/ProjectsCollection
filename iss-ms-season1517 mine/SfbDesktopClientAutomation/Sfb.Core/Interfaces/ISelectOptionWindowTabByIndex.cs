using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Interfaces
{
    public interface ISelectOptionWindowTabByIndex
    {
        UIItemCollection SelectOptionWindowTabByIndex(int index, Window options_Window);
    }
}
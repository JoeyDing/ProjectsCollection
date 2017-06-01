using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace Sfb.Core.Services
{
    public class SelectOptionWindowTabByIndexService : ISelectOptionWindowTabByIndex
    {
        public UIItemCollection SelectOptionWindowTabByIndex(int index, Window options_Window)
        {
            Tree tree = options_Window.Get<Tree>(SearchCriteria.ByControlType(ControlType.Tree));
            tree.KeyIn(KeyboardInput.SpecialKeys.HOME);
            //move to SKype Meetings tab
            for (int i = 0; i <= index; i++)
            {
                tree.KeyIn(KeyboardInput.SpecialKeys.DOWN);
            }

            return options_Window.Items;
        }
    }
}
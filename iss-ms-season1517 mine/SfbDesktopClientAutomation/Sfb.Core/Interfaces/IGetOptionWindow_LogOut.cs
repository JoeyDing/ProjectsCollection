using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Interfaces
{
    public interface IGetOptionWindow_LogOut
    {
        Window GetOptionWindow_LogOut(Window sfbClient, Application app);
    }
}
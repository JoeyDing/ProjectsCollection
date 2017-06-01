using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Sfb.Core.Interfaces
{
    public interface ITraverseItemNative
    {
        Tuple<string, Node<AutomationElement>> TraverseItemNative(AutomationElement node, int depth = 0, string index = "root");
    }
}
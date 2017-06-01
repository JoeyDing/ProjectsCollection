using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Sfb.Core.Services
{
    public class TraverseItemNativeService : ITraverseItemNative
    {
        public Tuple<string, Node<AutomationElement>> TraverseItemNative(AutomationElement node, int depth = 0, string index = "root")
        {
            string stringResult = "";
            var itemResult = new Node<AutomationElement>()
            {
                Element = node,
                Items = new List<Node<AutomationElement>>()
            };
            string indent = "";
            for (int i = 0; i < depth; i++)
                indent += "----";

            itemResult.Element = node;
            stringResult = string.Format("{0}{1}({2}){3}", indent, node.Current.Name, index, Environment.NewLine);
            var tt = node.FindAll(TreeScope.Children, Condition.TrueCondition);
            if (tt != null)
            {
                for (int i = 0; i < tt.Count; i++)
                {
                    var child = tt[i];
                    itemResult.Items.Add(new Node<AutomationElement> { Element = child, Items = new List<Node<AutomationElement>>() });
                    string currentIndex = index + "," + i;
                    var childResult = TraverseItemNative(child, depth + 1, currentIndex);
                    stringResult += childResult.Item1;
                    foreach (var item in childResult.Item2.Items)
                    {
                        itemResult.Items[i].Items.Add(item);
                    }
                }
            }
            return new Tuple<string, Node<AutomationElement>>(stringResult, itemResult);
        }
    }
}
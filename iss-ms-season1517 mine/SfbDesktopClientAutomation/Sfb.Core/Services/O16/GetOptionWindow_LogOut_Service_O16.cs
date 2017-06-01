using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services.O16
{
    public class GetOptionWindow_LogOut_Service_O16 : IGetOptionWindow_LogOut
    {
        private readonly ITraverseItemNative traverse;

        public GetOptionWindow_LogOut_Service_O16(ITraverseItemNative _traverse)
        {
            traverse = _traverse;
        }

        public Window GetOptionWindow_LogOut(Window sfbClient, Application app)
        {
            //get all panes
            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = true;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 3;

            #region Log out

            //var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            ////get contact pane
            //var pane_contact = panes[2] as Panel;
            ////get Show Menu Arrow inside pane
            ////var button_location = pane_contact.Items[1] as Button;
            //var button_location = pane_contact.Items[4] as Button;
            //button_location.Click();
            //Thread.Sleep(1000);

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[1] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[6] as Button;
            button_location.Click();
            Thread.Sleep(5000);

            //click "File"
            var traverseItemNative = traverse.TraverseItemNative(pane_contact.AutomationElement);
            //var traverseItemNativeFile = traverseItemNative.Item2.Items[13].Items[2].Items[0].Items[0].Items[0];
            var traverseItemNativeFile = traverseItemNative.Item2.Items[11].Items[2].Items[0].Items[0].Items[0];
            //create wrapper for element
            var fileWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeFile.Element, pane_contact.ActionListener);
            fileWrapper.Click();
            Thread.Sleep(3000);
            //click "Sign Out",traverseItemNative has been changed since file's been clicked
            traverseItemNative = traverse.TraverseItemNative(pane_contact.AutomationElement);
            //var traverseItemNativeSignout = traverseItemNative.Item2.Items[11].Items[2].Items[0].Items[0].Items[0].Items[1].Items[0].Items[0].Items[0];
            var traverseItemNativeSignout = traverseItemNative.Item2.Items[11].Items[2].Items[0].Items[0].Items[0].Items[0].Items[0].Items[0].Items[0];
            //create wrapper for element
            var signout_wrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeSignout.Element, pane_contact.ActionListener);
            signout_wrapper.Click();

            #endregion Log out

            // only open the option modal window
            var panes1 = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            (panes1[1] as Panel).Items[0].Click();

            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }
    }
}
using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services.O15
{
    public class GetOptionWindowService_O15 : IGetOptionWindow
    {
        public Window GetOptionWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            var pane_contact = panes[3] as Panel;

            //get option button inside pane
            var button_option = pane_contact.Items[0] as Button;
            // only open the option modal window
            button_option.Click();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }
    }
}
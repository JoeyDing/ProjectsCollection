using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services
{
    public class Sign_InService : ISign_In
    {
        public void Sign_In(Window sfbClient)
        {
            // only open the option modal window

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            //var pane_contact = panes[1] as Panel;
            var pane_contact = panes[0] as Panel;

            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[12] as Button;
            //var button_location = pane_contact.Items[6] as Button;
            //var button_location = pane_contact.Items[7] as Button;
            button_location.Click();
        }
    }
}
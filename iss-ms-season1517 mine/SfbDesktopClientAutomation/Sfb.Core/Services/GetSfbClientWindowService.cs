using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services
{
    public class GetSfbClientWindowService : IGetSfbClientWindow
    {
        private readonly ICloseAllModals closeAll;

        public GetSfbClientWindowService(ICloseAllModals _closeAll)
        {
            closeAll = _closeAll;
        }

        public Window GetSfbClientWindow(Application app)
        {
            Thread.Sleep(3000);
            Window window = app.GetWindows().First(c =>
            {
                bool found = false;
                try
                {
                    var item = c.Get(SearchCriteria.ByAutomationId("idBuddyListTab"));
                    found = true;
                }
                catch (Exception)
                {
                }

                return c.IsModal == false && found;
            });

            if (app.GetWindows().Count > 1)
            {
                closeAll.CloseAllModals(window, app);
            }
            window.DisplayState = DisplayState.Restored;
            window.Focus();
            return window;
        }
    }
}
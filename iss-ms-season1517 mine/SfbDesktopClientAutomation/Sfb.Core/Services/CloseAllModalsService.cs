using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services
{
    public class CloseAllModalsService : ICloseAllModals
    {
        public void CloseAllModals(Window sfbClient, Application app)
        {
            // open window
            //// close any open windows
            sfbClient.Focus();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            foreach (var item in windows)
            {
                item.Close();
            }
            Thread.Sleep(3000);
        }
    }
}
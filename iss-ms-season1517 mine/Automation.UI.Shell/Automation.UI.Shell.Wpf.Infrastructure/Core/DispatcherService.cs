using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core
{
    public class DispatcherService : IDispatcher
    {
        private Dispatcher dispatcher;

        public DispatcherService()
        {
            dispatcher = Application.Current.Dispatcher;
        }

        public void Invoke(Action action)
        {
            dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            dispatcher.BeginInvoke(action);
        }
    }
}
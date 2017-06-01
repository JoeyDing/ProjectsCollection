using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core
{
    public abstract class ShellViewModelBase : ViewModelBase
    {
        protected readonly IDispatcher dispatcher;

        public ShellViewModelBase(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
    }
}
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts
{
    public interface IExpandStatusGroupView
    {
        bool ExpandGroup(string groupName);
    }
}
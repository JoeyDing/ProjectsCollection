using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.TestRunnerModule.ModuleA
{
    public interface IModuleAView
    {
        TestRunnerControl RunnerControl { get; }
    }
}
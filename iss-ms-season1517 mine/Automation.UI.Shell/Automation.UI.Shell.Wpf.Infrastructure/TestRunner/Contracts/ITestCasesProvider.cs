using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts
{
    public interface ITestCasesProvider
    {
        ObservableCollection<TestCase> GetTestCasesList();
    }
}
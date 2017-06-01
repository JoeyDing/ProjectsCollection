using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.TestRunnerModule.ModuleB
{
    public class ModuleBTestCasesProvider : ITestCasesProvider
    {
        public ObservableCollection<TestCase> GetTestCasesList()
        {
            var testCase = new TestCase { Name = "TestRunnerModule - Test", IsChecked = true };
            testCase.TestAction += (lang) =>
            {
                return "Test run from TestRunnerModule for language " + lang.CultureName;
            };

            return new ObservableCollection<TestCase> { testCase };
        }
    }
}
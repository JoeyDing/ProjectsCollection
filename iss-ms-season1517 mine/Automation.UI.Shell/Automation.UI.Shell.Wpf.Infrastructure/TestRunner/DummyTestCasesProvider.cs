using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public class DummyTestCasesProvider : ITestCasesProvider
    {
        public ObservableCollection<TestCase> GetTestCasesList()
        {
            //for test only
            var result = new ObservableCollection<TestCase>();
            foreach (var index in Enumerable.Range(1, 5))
            {
                var testcase = new TestCase
                {
                    Name = "Test " + index,
                    IsChecked = true,
                };
                testcase.TestAction += (language) =>
                {
                    Thread.Sleep(1000);
                    return string.Format("{0} done.", testcase.Name);
                };

                result.Add(testcase);
            }
            return result;
        }
    }
}
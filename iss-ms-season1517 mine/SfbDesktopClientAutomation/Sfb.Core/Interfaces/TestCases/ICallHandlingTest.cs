using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface ICallHandlingTest
    {
        bool Options_CallHandling(string languageShortName, TestCase testcase);
    }
}
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface IStatusTest
    {
        bool Options_Status(string languageShortName, TestCase testcase);
    }
}
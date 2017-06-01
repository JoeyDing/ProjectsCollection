using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface IRecordingTest
    {
        bool Options_Recording(string languageShortName, TestCase testcase);
    }
}
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface IRecordingPublishRecordingTest
    {
        bool Recording_Publish_Recording(string languageShortName, TestCase testcase);
    }
}
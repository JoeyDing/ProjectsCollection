using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface IConferenceJoinMeetingNowTest
    {
        bool ConferenceJoin_MeetNow(string languageShortName, TestCase testcase);
    }
}
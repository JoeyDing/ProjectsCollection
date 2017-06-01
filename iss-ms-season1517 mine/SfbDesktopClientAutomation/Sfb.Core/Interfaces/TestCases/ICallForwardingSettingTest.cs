using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Sfb.Core.Interfaces
{
    public interface ICallForwardingSettingTest
    {
        bool Call_Forwarding_Setting(string languageShortName, TestCase testCase);
    }
}
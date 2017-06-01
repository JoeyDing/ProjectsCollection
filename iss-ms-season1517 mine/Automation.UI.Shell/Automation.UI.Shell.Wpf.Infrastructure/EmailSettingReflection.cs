using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure
{
    [Serializable]
    public class EmailSettingReflection
    {
        public TestCasesOptions TSoptions { get; set; }

        public string AccountName { get; set; }
        public string EmailServer { get; set; }

        public string EmailFrom { get; set; }

        public List<string> EmailToList { get; set; }

        public List<string> EmailCcList { get; set; }

        public bool IsSent { get; set; }
    }

    public enum TestCasesOptions { SignIn, SendIM, LeaveConversation }
}
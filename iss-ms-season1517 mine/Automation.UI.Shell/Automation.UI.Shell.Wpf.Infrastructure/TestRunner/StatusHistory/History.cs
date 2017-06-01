using Automation.UI.Shell.Wpf.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    [Serializable]
    public enum HistoryStatus
    {
        Passed,
        Failed,
        Waiting
    }

    [Serializable]
    public class History : PropertyChangedHandler
    {
        private string group;

        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        private Language language;

        public Language Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged("Language");
            }
        }

        private TestCase testcase;

        public TestCase Testcase
        {
            get { return testcase; }
            set
            {
                testcase = value;
                OnPropertyChanged("Testcase");
            }
        }

        private HistoryStatus status;

        public HistoryStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private string msg;

        public string Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
                OnPropertyChanged("Msg");
            }
        }

        private DateTime runTime;

        public DateTime RunTime
        {
            get
            {
                return runTime;
            }
            set
            {
                runTime = value;
                OnPropertyChanged("RunTime");
            }
        }

        private string guid;

        public string Guid
        {
            get { return guid; }
            set
            {
                guid = value;
                OnPropertyChanged("Guid");
            }
        }
    }
}
using System;

namespace Sfb.LanguageInstaller.Wpf
{
    [Serializable]
    public class InstallerHistory : ModelBase
    {
        public enum HistoryStatus
        {
            Passed,
            Failed,
            Waiting
        }

        public string Group { get; set; }
        public string Language { get; set; }

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

        public string Msg { get; set; }

        public DateTime RunTime { get; set; }

        private Operation op;

        public Operation Op
        {
            get { return op; }
            set
            {
                op = value;
                OnPropertyChanged("Op");
            }
        }

        public InstallerHistory()
        { }

        public InstallerHistory(string language, HistoryStatus status, string msg, DateTime dateTime)
        {
            this.Language = language;
            this.Status = status;
            this.Msg = msg;
            this.RunTime = dateTime;
        }
    }
}
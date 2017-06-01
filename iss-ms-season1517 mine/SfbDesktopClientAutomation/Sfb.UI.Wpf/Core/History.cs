using System;

namespace Sfb.UI.Wpf
{
    [Serializable]
    public class History : ViewModelBase
    {
        public string grp { get; set; }
        public string apk { get; set; }

        public Language Language { get; set; }

        public TestCase Testcase { get; set; }

        // public string Status { get; set; }

        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        public string Msg { get; set; }

        public DateTime runTime { get; set; }

        public string guid { get; set; }

        public History()
        { }

        public History(string apk, string language, string textcase, string status, string msg, DateTime dateTime)
        {
            this.apk = apk;
            //this.Language = language;
            //this.Testcase = textcase;
            this.Status = status;
            this.Msg = msg;
            this.runTime = dateTime;
        }

        public string testCaseCmd { get; set; }
    }
}
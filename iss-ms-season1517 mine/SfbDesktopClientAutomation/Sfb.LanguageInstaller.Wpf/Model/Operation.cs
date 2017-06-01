using System;

namespace Sfb.LanguageInstaller.Wpf
{
    [Serializable]
    public class Operation : ModelBase
    {
        private string operationName;
        private string version;
        private string buildVersion;
        private string language;

        public string OperationName
        {
            get { return operationName; }
            set
            {
                operationName = value;
                OnPropertyChanged("OperationName");
            }
        }

        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        public string BuildVersion
        {
            get { return buildVersion; }
            set
            {
                buildVersion = value;
                OnPropertyChanged("BuildVersion");
            }
        }

        public string Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged("Language");
            }
        }

        [field: NonSerialized]
        public event Func<string, string> Action;

        public string RunAction()
        {
            string result = "";
            if (this.Action != null)
                result = this.Action(language);

            return result;
        }
    }
}
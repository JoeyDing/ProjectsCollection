using System.ComponentModel;

namespace Sfb.UI.Wpf
{
    public class ApkInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _IsToInstall;

        public bool IsToInstall
        {
            get { return _IsToInstall; }
            set
            {
                _IsToInstall = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsToInstall"));
                }
            }
        }

        public string Location
        {
            get;
            set;
        }
    }
}
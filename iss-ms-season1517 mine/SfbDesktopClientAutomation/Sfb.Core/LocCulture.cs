using Sfb.Installer.Core;

namespace Sfb.Core
{
    public class LocCulture : NotifyPropertyChanged
    {
        private string cultureName;

        public string CultureName
        {
            get { return cultureName; }
            set
            {
                cultureName = value;
                OnPropertyChanged("CultureName");
            }
        }

        private string englishName;

        public string EnglishName
        {
            get { return englishName; }
            set
            {
                englishName = value;
                OnPropertyChanged("EnglishName");
            }
        }

        private int lcid;

        public int Lcid
        {
            get { return lcid; }
            set
            {
                lcid = value;
                OnPropertyChanged("Lcid");
            }
        }

        private bool isLip;

        public bool IsLip
        {
            get { return isLip; }
            set
            {
                isLip = value;
                OnPropertyChanged("IsLip");
            }
        }
    }
}
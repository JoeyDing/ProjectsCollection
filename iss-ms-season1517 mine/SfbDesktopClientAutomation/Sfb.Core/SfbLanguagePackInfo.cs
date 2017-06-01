using Sfb.Installer.Core;

namespace Sfb.Core
{
    public class SfbLanguagePackInfo : NotifyPropertyChanged
    {
        private string languagePackFolderPath;
        private string languagePackInstallationFileName;
        private LocCulture language;
        private bool isChecked;

        public string LanguagePackFolderPath
        {
            get { return languagePackFolderPath; }
            set
            {
                languagePackFolderPath = value;
                OnPropertyChanged("LanguagePackFolderPath");
            }
        }

        public string LanguagePackInstallationFileName
        {
            get { return languagePackInstallationFileName; }
            set
            {
                languagePackInstallationFileName = value;
                OnPropertyChanged("LanguagePackInstallationFileName");
            }
        }

        public LocCulture Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged("Language");
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
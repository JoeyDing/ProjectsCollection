using System.Collections.ObjectModel;

namespace Sfb.UI.Wpf
{
    public class ApkInfoCollection
    {
        private ObservableCollection<ApkInfo> info = new ObservableCollection<ApkInfo>();

        public ObservableCollection<ApkInfo> Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }
    }
}
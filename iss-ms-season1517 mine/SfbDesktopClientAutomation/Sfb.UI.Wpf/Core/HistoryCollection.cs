using System.Collections.ObjectModel;

namespace Sfb.UI.Wpf
{
    public class HistoryCollection
    {
        private ObservableCollection<History> info = new ObservableCollection<History>();

        public ObservableCollection<History> Info
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
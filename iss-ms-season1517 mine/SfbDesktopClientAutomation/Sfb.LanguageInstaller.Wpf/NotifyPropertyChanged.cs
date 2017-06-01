using System;
using System.ComponentModel;

namespace Sfb.UI.Wpf
{
    [Serializable]
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyPropertyChanged()
        {
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
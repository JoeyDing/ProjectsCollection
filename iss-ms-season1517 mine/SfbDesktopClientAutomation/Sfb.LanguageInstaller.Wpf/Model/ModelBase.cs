using System;
using System.ComponentModel;

namespace Sfb.LanguageInstaller.Wpf
{
    [Serializable]
    public class ModelBase : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public ModelBase()
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
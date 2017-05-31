using System;
using System.ComponentModel;

namespace WpfApplication001A.Models
{
    //core of how WPF does the data binding, subsribe to events, like property changed to update
    public class Customer : INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// INITIALZE A NEW INSTANCE OF THE CUSTMER CLASS
        /// </summary>
        public Customer(string customerName)
        {
            Name = customerName;
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                //want to implement inotify propertychanged
                OnPropertyChanged("Name");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        # endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get;
            private set;
        }

        //error setting
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (String.IsNullOrWhiteSpace(Name))
                    {
                        Error = "Name cannot be null or empty";
                    }
                    else
                    {
                        Error = null;
                    }
                }
                return Error;
            }
        }

        # endregion
    }
}
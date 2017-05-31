using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApplicationExercise.Models
{
    public class Customer : IDataErrorInfo, INotifyPropertyChanged
    {
        private string customerName;

        #region IDataErrorInfo Member Info

        //indexer
        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return GetValidationError(propertyName);
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        #endregion IDataErrorInfo Member Info

        #region Validation

        //in order to track properties that aree validated
        private static readonly string[] ValidatedProperties = { "CustomerName" };

        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;
                return true;
            }
        }

        private string ValidateCustomerName()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                return "Customer name cannot be empty.";
            }
            return null;
        }

        private string GetValidationError(string propertyName)
        {
            //1. call indexer,pass the proname
            string error = null;
            switch (propertyName)
            {
                case "CustomerName":
                    error = ValidateCustomerName();
                    break;
            }
            //2. execute the validation
            return error;
        }

        #endregion Validation

        public string CustomerName

        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                NotifyPropertyChanged("CustomerName");
            }
        }

        #region INotifyPropertyChangedregion MembersInfo

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChangedregion MembersInfo
    }
}
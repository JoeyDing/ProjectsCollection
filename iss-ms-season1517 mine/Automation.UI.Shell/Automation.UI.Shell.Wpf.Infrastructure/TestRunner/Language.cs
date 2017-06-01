using Automation.UI.Shell.Wpf.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    [Serializable]
    public class Language : PropertyChangedHandler
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

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
    }
}
using Automation.UI.Shell.Wpf.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    //TODO: + ITakeScreen property
    [Serializable]
    public class TestCase : PropertyChangedHandler
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

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [field: NonSerialized]
        public event Func<Language, string> TestAction;

        public string RunAction(Language language)
        {
            string result = "";
            if (this.TestAction != null)
                result = this.TestAction(language);

            return result;
        }
    }
}
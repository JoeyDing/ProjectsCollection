using System;

namespace Sfb.UI.Wpf
{
    [Serializable]
    public class TestCase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
    }
}
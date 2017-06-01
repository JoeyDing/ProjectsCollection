using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Mvp
{
    [Serializable]
    public class CheckableItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
}
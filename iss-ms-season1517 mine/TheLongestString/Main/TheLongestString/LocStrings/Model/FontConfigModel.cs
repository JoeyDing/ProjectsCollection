using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLongestString.Model
{
    [Serializable]
    public class FontConfigModel
    {
        public string Culture { get; set; }
        public string FontFamily { get; set; }
        public double FontSize { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TheLongestString
{
    public class LocString : BindableBase
    {
        private string _locString = null;
        private double _width = -1;
        private bool _isMaxWidth = false;

        private LocDictionary _locDictionary = null;

        public LocString(LocDictionary locDictionary, string locString)
        {
            _locDictionary = locDictionary;
            _locString = locString;
        }

        public string String
        {
            get { return _locString; }
        }

        /// <summary>
        /// Pixel width of the string once it's been rendered in a given typeface and font size.
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { SetProperty<double>(ref _width, value); }
        }

        /// <summary>
        /// Dictionary that this string belongs to, has details on the culture.
        /// </summary>
        public LocDictionary Dictionary
        {
            get { return _locDictionary; }
        }

        /// <summary>
        /// True if this string is the longest of all the other translations for this Id.
        /// </summary>
        public bool IsMaxWidth
        {
            get { return _isMaxWidth; }
            set { SetProperty<bool>(ref _isMaxWidth, value); }
        }

        /// <summary>
        /// Renders a string in a specific font and measures the pixel width of it.
        /// </summary>
        /// <param name="s">String to measure.</param>
        /// <param name="typeface">Typeface to render, e.g. Segoe UI.</param>
        /// <param name="fontSize">Font size to render at.</param>
        /// <returns>Width of the text, in pixels</returns>
        public static double CalculateTextWidth(LocString s, Typeface typeface, double fontSize)
        {
            return CalculateTextWidth(s.String, typeface, fontSize);
        }

        /// <summary>
        /// Renders a string in a specific font and measures the pixel width of it.
        /// </summary>
        /// <param name="s">String to measure.</param>
        /// <param name="typeface">Typeface to render, e.g. Segoe UI.</param>
        /// <param name="fontSize">Font size to render at.</param>
        /// <returns>Width of the text, in pixels</returns>
        public static double CalculateTextWidth(string s, Typeface typeface, double fontSize)
        {
            if (String.IsNullOrEmpty(s))
            {
                return 0;
            }

            var ft = new FormattedText(s, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            return ft.WidthIncludingTrailingWhitespace;
        }

        public override string ToString()
        {
            return _locString;
        }
    }
}

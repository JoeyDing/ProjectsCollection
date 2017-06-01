using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TheLongestString
{
    public class LocString : BindableBase
    {
        private string _locString = null;
        private double _height = -1;
        private double _width = -1;
        private bool _isMaxHeight = false;
        private bool _isMaxWidth = false;
        private bool _isFontSupported;
        private bool? _isSelected;
        private string _fontFamilyName;
        private double? _fontSize;
        private FontFamily _fontFamily;

        public bool HasValue { get; private set; }

        private LocDictionary _locDictionary = null;

        public LocString(LocDictionary locDictionary, string locString, bool hasValue = true)
        {
            this.HasValue = hasValue;
            _locDictionary = locDictionary;
            _locString = locString;
        }

        public string String
        {
            get { return _locString; }
        }

        /// <summary>
        /// Dictionary that this string belongs to, has details on the culture.
        /// </summary>
        public LocDictionary Dictionary
        {
            get { return _locDictionary; }
            set { _locDictionary = value; }
        }

        #region Width Calculation

        /// <summary>
        /// Pixel width of the string once it's been rendered in a given typeface and font size.
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { SetProperty<double>(ref _width, value); }
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

        #endregion Width Calculation

        #region Height Calculation

        /// <summary>
        /// Pixel _height of the string once it's been rendered in a given typeface and font size.
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { SetProperty<double>(ref _height, value); }
        }

        /// <summary>
        /// True if this string is the tallest of all the other translations for this Id.
        /// </summary>
        public bool IsMaxHeight
        {
            get { return _isMaxHeight; }
            set { SetProperty<bool>(ref _isMaxHeight, value); }
        }

        /// <summary>
        /// Renders a string in a specific font and measures the pixel height of it.
        /// </summary>
        /// <param name="s">String to measure.</param>
        /// <param name="typeface">Typeface to render, e.g. Segoe UI.</param>
        /// <param name="fontSize">Font size to render at.</param>
        /// <returns>Height of the text, in pixels</returns>
        public static double CalculateTextHeight(LocString s, Typeface typeface, double fontSize)
        {
            bool isFontSupported;
            double textHeight = CalculateTextHeight(s.String, typeface, fontSize, out isFontSupported);
            s.IsFontSupported = isFontSupported;
            return textHeight;
        }

        /// <summary>
        /// Renders a string in a specific font and measures the pixel height of it.
        /// </summary>
        /// <param name="s">String to measure.</param>
        /// <param name="typeface">Typeface to render, e.g. Segoe UI.</param>
        /// <param name="fontSize">Font size to render at.</param>
        /// <returns>Height of the text, in pixels</returns>
        public static double CalculateTextHeight(string s, Typeface typeface, double fontSize, out bool isFontSupported)
        {
            isFontSupported = true;
            if (String.IsNullOrEmpty(s))
            {
                return 0;
            }

            //var ft = new FormattedText(s, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);
            //ft.Trimming = TextTrimming.None;
            //return ft.Height;

            double height = 0;

            GlyphTypeface glyphTypeface;
            if (typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                for (int n = 0; n < s.Length; n++)
                {
                    if (glyphTypeface.CharacterToGlyphMap.ContainsKey(s[n]))
                    {
                        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[s[n]];

                        var glyphHeight = (glyphTypeface.Height - glyphTypeface.TopSideBearings[glyphIndex]
                                                 - glyphTypeface.BottomSideBearings[glyphIndex]) * fontSize;
                        if (glyphHeight > height)
                        {
                            height = glyphHeight;
                        }
                    }
                    else
                    {
                        isFontSupported = false;
                        return new FormattedText(s, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black).Height;
                    }
                }
            }
            else
                isFontSupported = false;

            return height;
        }

        #endregion Height Calculation

        public override string ToString()
        {
            return _locString;
        }

        public int CharacterCount
        {
            get
            {
                return this.String.Length;
            }
        }

        public bool IsFontSupported
        {
            get { return _isFontSupported; }
            set
            {
                SetProperty<bool>(ref _isFontSupported, value);
            }
        }
        public bool? IsSelected {
            get { return _isSelected; }
            set
            {
                SetProperty<bool?>(ref _isSelected, value);
            }
        }

        public string FontFamilyName
        {
            get { return _fontFamilyName; }
            set
            {
                SetProperty<string>(ref _fontFamilyName, value);
            }
        }

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set
            {
                SetProperty<FontFamily>(ref _fontFamily, value);
            }
        }

        public double? FontSize
        {
            get { return _fontSize; }
            set
            {
                SetProperty<double?>(ref _fontSize, value);
            }
        }

    }
}
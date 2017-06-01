using System;
using System.Globalization;
using System.Windows.Data;

namespace Sfb.UI.Wpf
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string status = value.ToString();
            if (status.Equals("F"))
            {
                return "error.png";
            }
            else if (status.Equals("S"))
            {
                return "complete.png";
            }
            else if (status.Equals("P"))
            {
                return "notice.png";
            }

            return this;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from visibility to bool
            return null;
        }
    }
}
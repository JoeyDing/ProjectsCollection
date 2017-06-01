using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TheLongestString
{
    /// <summary>
    /// Returns True if value is null. Can be inverted.
    /// </summary>
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool answer = value == null;

            // flip the bit if a parameter is set
            if (parameter != null)
            {
                answer = !answer;
            }

            return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

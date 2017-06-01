using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TheLongestString
{
    public class ConvertFromPoint : IValueConverter
    {
        /// <summary>
        /// Convert point value to device independent unit
        /// </summary>
        /// <param name="points">point value</param>
        /// <returns>point value converted to dip (device Independent Unit)</returns>
        public static double PointsToDIP(double points)
        {
            return points * (96.0 / 72.0);
        }

        public static double DIPToPoints(double dip)
        {
            return dip * (72.0 / 96.0);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result;
            if (value != null && double.TryParse(value.ToString(), out result))
            {
                return Math.Round(DIPToPoints(result));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double result;
            if (value != null && double.TryParse(value.ToString(), out result))
            {
                return PointsToDIP(result);
            }

            return value;
        }
    }
}
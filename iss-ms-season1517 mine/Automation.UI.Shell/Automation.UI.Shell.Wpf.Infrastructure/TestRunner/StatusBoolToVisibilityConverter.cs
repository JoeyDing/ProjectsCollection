using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public class StatusBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string pictureName = null;
            string assetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestRunner\StatusHistory\assets");
            HistoryStatus status = (HistoryStatus)value;
            switch (status)
            {
                case HistoryStatus.Passed:
                    pictureName = Path.Combine(assetFolder, @"complete.png");
                    break;

                case HistoryStatus.Failed:
                    pictureName = Path.Combine(assetFolder, @"error.png");
                    break;

                case HistoryStatus.Waiting:
                    pictureName = Path.Combine(assetFolder, @"notice.png");
                    break;
            }

            return pictureName;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from visibility to bool
            return null;
        }
    }
}
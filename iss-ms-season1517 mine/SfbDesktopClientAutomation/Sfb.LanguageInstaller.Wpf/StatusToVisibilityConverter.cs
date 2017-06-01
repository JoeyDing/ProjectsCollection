using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Sfb.LanguageInstaller.Wpf
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string pictureName = null;
            string assetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\assets");
            InstallerHistory.HistoryStatus status = (InstallerHistory.HistoryStatus)value;
            switch (status)
            {
                case InstallerHistory.HistoryStatus.Passed:
                    pictureName = Path.Combine(assetFolder, @"complete.png");
                    break;

                case InstallerHistory.HistoryStatus.Failed:
                    pictureName = Path.Combine(assetFolder, @"error.png");
                    break;

                case InstallerHistory.HistoryStatus.Waiting:
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
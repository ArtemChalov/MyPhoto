using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace FolderContentPresenter.Converters
{
    public class PathToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filePath = value as string;

            return Path.GetFileName(filePath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

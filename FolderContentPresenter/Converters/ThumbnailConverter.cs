using System;
using System.Globalization;
using System.Windows.Data;
using WriteableBitmapEx;

namespace FolderContentPresenter.Converters
{
    class ThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filePath = value as string;

            return BitmapImageFactory.CreateFromFile(filePath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

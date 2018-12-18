using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WriteableBitmapEx;

namespace FolderContentPresenter.Converters
{
    class ThumbnailConverterToolTip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filePath = value as string;

            BitmapImage image = null;

            if (!String.IsNullOrEmpty(filePath))
                image = BitmapImageFactory.CreateThumbnailFromFile(filePath, 256, DesiredSize.Width);

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

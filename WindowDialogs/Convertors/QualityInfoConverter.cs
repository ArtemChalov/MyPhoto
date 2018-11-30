using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowDialogs.Convertors
{
    public class QualityInfoConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            if (val > 3) {
                if (val > 5) {
                    if (val > 7) {
                        if (val > 8) {
                            if (val > 9)
                                return "Отличное";
                            else
                                return "Хорошое";
                        }
                        else
                            return "Нормальное";
                    }
                    else
                        return "Среднее";
                }
                else
                    return "Плохое";
            }
            else
                return "Очень плохое";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

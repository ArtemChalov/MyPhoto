using System;
using System.Globalization;
using System.Windows.Data;

namespace WindowDialogs.Convertors
{
    /// <summary>
    /// Class that implements the IValueConverter interface
    /// </summary>
    public class QualityInfoConverter: IValueConverter
    {
        /// <summary>
        /// Converts a double value to a string.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
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

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

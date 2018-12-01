using System;
using System.Globalization;
using System.Windows.Controls;

namespace WindowDialogs.ValidationRules
{
    /// <summary>
    /// Class that implements ValidationRule.
    /// </summary>
    public class IntegerValidationRules: ValidationRule
    {
        private int _min = 0;
        private int _max = int.MaxValue;

        /// <summary>
        /// Minimum boundary value.
        /// </summary>
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Maximum boundary value.
        /// </summary>
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="cultureInfo">The culture to use in the ValidationRule.</param>
        /// <returns>ValidationResult object</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int parameter = 0;

            try {
                if (((string)value).Length > 0)
                    parameter = Int32.Parse((String)value);
            }
            catch (Exception e) {
                return new ValidationResult(false, e.Message);
            }

            if ((parameter < Min) || (parameter > Max)) {
                return new ValidationResult(false,
                  "Пожалуйста введите значение из диапазона: " + Min + " - " + Max + ".");
            }
            else {
                return new ValidationResult(true, null);
            }
        }
    }
}

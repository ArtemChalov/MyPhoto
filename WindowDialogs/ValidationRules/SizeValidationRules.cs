using System;
using System.Globalization;
using System.Windows.Controls;

namespace WindowDialogs.ValidationRules
{
    public class IntegerValidationRules: ValidationRule
    {
        private int _min = 0;
        private int _max = int.MaxValue;

        public IntegerValidationRules(){}

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

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

using System;
using System.Globalization;
using System.Windows.Data;
using StroopApp.Models;

namespace StroopApp.Converters
{
    public class CalculationModeToBooleanConverter : IValueConverter
    {
        // Convertit une valeur CalculationMode en booléen selon le paramètre.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            string modeParameter = parameter.ToString();
            if (Enum.TryParse(typeof(CalculationMode), modeParameter, out object modeValue))
            {
                return value.Equals(modeValue);
            }
            return false;
        }

        // Si le radio button est coché, retourne la valeur indiquée par ConverterParameter.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter != null)
            {
                string modeParameter = parameter.ToString();
                if (Enum.TryParse(typeof(CalculationMode), modeParameter, out object modeValue))
                {
                    return modeValue;
                }
            }
            return Binding.DoNothing;
        }
    }
}

using System.Globalization;
using System.Windows.Data;
using StroopApp.Models;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ProfileEditorWindow"/>.
    /// Converts between a <see cref="CalculationMode"/> and a boolean value for selection logic.
    /// </summary>

    public class CalculationModeToBooleanConverter : IValueConverter
    {
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

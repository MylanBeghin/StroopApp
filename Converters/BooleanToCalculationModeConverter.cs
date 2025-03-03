using System.Globalization;
using System.Windows.Data;
using StroopApp.Models;

namespace StroopApp.Converters
{
    public class BooleanToCalculationModeConverter : IValueConverter
    {
        // Convert bool to CalculationMode
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CalculationMode mode)
            {
                return mode == CalculationMode.WordCount;
            }
            return false;
        }

        // Convert back from bool to CalculationMode
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isWordCount)
            {
                return isWordCount ? CalculationMode.WordCount : CalculationMode.TaskDuration;
            }
            return CalculationMode.TaskDuration;
        }
    }
}

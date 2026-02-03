using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converts a boolean to Visibility (Visible if true, Collapsed if false) and vice versa.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility v)
                return v == Visibility.Visible;
            return false;
        }
    }
}

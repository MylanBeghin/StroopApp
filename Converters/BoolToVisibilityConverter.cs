using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Convertit un booléen en Visibility (Visible si true, Collapsed si false) et inversement.
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

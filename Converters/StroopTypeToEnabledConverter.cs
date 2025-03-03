using System;
using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    public class StroopTypeToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Retourne true uniquement si StroopType vaut "Amorce"
            return (value as string)?.Equals("Amorce", StringComparison.OrdinalIgnoreCase) == true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

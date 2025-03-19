using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    public class HeightToMaxHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
                return height * 0.4;
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}

using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ProfileManagementView"/>.
    /// Converts a duration in milliseconds to the number of complete hours.
    /// </summary>
    public class MillisecondsToHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int milliseconds)
            {
                int hours = milliseconds / 3600000;
                return hours;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

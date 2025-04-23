using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ProfileManagementView"/>.
    /// Converts a duration in milliseconds to the number of seconds, excluding complete minutes.
    /// </summary>
    public class MillisecondsToSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int milliseconds)
            {
                int seconds = (milliseconds % 60000) / 1000;
                return seconds;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

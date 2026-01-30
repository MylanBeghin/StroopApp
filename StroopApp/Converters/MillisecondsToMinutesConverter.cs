using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ProfileManagementView"/>.
    /// Divide the total duration (in ms) into minutes after subtracting the entire hours.
    /// </summary>
    public class MillisecondsToMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int milliseconds)
            {
                int minutes = (milliseconds % 3600000) / 60000;
                return minutes;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ParticipantManagementView"/>.
    /// Converts a height value into a maximum height by applying a scaling factor.
    /// Used to adapt UI responsiveness.
    /// </summary>

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

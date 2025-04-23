using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="KeyMappingView"/>.
    /// Converts a <see cref="KeyMapping.Key"/> to its string representation for display purposes.
    /// </summary>
    public class KeyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Key key)
            {
                return key.ToString();
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && Enum.TryParse(s, out Key key))
            {
                return key;
            }
            return Binding.DoNothing;
        }
    }
}

using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    /// <summary>
    /// Converter associated with <see cref="ProfileEditorWindow"/>.
    /// Converts a StroopType string into a boolean indicating if the value is "Amorce".
    /// Used to enable or disable UI elements conditionally.
    /// </summary>

    public class StroopTypeToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.Equals("Amorce", StringComparison.OrdinalIgnoreCase) == true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

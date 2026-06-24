using StroopApp.Models.Simon;
using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
    public class StimulusPositionToGridColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is StimulusPosition.Left ? 0 : 1;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
            
    }
}
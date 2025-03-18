using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace StroopApp.Converters
{
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

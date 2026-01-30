using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
	public class BoolToStringConverter : IValueConverter
	{
        /// <summary>
        /// Converts a boolean to a string based on a parameter format "TrueValue|FalseValue".
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var param = (parameter as string)?.Split('|');
			if (param == null || param.Length < 2)
				return value?.ToString() ?? "";
			return (bool)value ? param[0] : param[1];
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}

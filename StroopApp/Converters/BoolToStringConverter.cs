using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
	public class BoolToStringConverter : IValueConverter
	{
		public string ConverterParameter
		{
			get; set;
		} // Format: "Vrai|Faux"
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

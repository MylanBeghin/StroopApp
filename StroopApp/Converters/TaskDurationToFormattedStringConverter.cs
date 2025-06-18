using System.Globalization;
using System.Windows.Data;

namespace StroopApp.Converters
{
	public class TaskDurationToFormattedStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int ms = (int)value;
			TimeSpan ts = TimeSpan.FromMilliseconds(ms);
			if (ts.TotalHours >= 1)
				return $"{(int)ts.TotalHours}h {ts.Minutes}m {ts.Seconds}s";
			if (ts.TotalMinutes >= 1)
				return $"{ts.Minutes}m {ts.Seconds}s";
			return $"{ts.Seconds}s";
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}

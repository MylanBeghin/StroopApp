using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StroopApp.Converters
{
	/// <summary>
	/// Converts a boolean to Visibility (Visible if false, Collapsed if true) and vice versa.
	/// </summary>
	public class BoolToVisibilityInvertedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b && b)
				return Visibility.Collapsed;
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility v)
				return v == Visibility.Collapsed;
			return false;
		}
	}
}

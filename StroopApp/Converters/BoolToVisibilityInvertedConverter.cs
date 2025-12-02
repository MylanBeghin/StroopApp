using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StroopApp.Converters
{
	/// <summary>
	/// Convertit un booléen en Visibility (Visible si false, Collapsed si true) et inversement.
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

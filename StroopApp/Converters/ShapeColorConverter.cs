using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StroopApp.Converters
{
	public class ShapeColorConverter : IValueConverter
	{
		// Utilisé pour colorer la forme dominante dans le slider (■ ou ●)
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dominantForm = value as string;
			var targetForm = parameter as string;

			// Si la forme courante est la forme dominante, couleur bleue, sinon gris
			if (dominantForm != null && targetForm != null)
				return dominantForm == targetForm ? Brushes.DodgerBlue : Brushes.LightGray;

			return Brushes.LightGray;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

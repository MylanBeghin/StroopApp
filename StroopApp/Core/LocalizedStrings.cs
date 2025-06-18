using System.ComponentModel;
using System.Globalization;
using System.Threading;

using Xceed.Wpf.AvalonDock.Properties;

namespace StroopApp.Core
{
	public class LocalizedStrings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public string this[string key] =>
			Resources.Strings.ResourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture) ?? $"!!{key}!!";

		public void ChangeCulture(string culture)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
		}
	}
}

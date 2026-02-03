using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace StroopApp.Core
{
    /// <summary>
    /// Provides localized string resources with culture change support and UI notification.
    /// </summary>
    public class LocalizedStrings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public string this[string key] =>
			Resources.Strings.ResourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture) ?? $"!!{key}!!";

        /// <summary>
        /// Changes the current UI culture and notifies bindings to refresh all localized strings.
        /// </summary>
        public void ChangeCulture(string culture)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
		}
	}
}

using System;
using System.ComponentModel;
using System.Globalization;

namespace StroopApp
{
    public class LocalizedStrings : INotifyPropertyChanged
    {
        private CultureInfo _culture = new CultureInfo("en-US");

        public string this[string key] => Resources.Strings.ResourceManager.GetString(key, _culture) ?? key;

        public CultureInfo CurrentCulture => _culture;

        public void SetCulture(string cultureName)
        {
            _culture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentUICulture = _culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

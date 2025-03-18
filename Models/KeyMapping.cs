using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StroopApp.Models
{
    public class KeyMapping : INotifyPropertyChanged
    {
        private Key _key;
        public string Color { get; set; }
        public Key Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged();
                }
            }
        }

        public KeyMapping(string color, Key key)
        {
            Color = color;
            _key = key;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

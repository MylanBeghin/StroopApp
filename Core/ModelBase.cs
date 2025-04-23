using ModernWpf.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.Core
{
    /// <summary>
    /// Base class for all Models, implementing <see cref="INotifyPropertyChanged"/>.
    /// Provides a method for property change notifications and a utility method to show error dialogs.
    /// </summary>

    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

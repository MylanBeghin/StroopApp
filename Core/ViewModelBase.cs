using ModernWpf.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StroopApp.Core
{
    /// <summary>
    /// Base class for all ViewModels, implementing <see cref="INotifyPropertyChanged"/>.
    /// Provides a method for property change notifications and a utility method to show error dialogs.
    /// </summary>

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected async void ShowErrorDialog(string message)
        {
            var loc = App.Current.Resources["Loc"] as StroopApp.Core.LocalizedStrings;
            var dialog = new ContentDialog
            {
                Title = loc?["Error_Title"],
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
    }
}

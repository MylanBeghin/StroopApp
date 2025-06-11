using System.ComponentModel;
using System.Runtime.CompilerServices;

using ModernWpf.Controls;

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
		protected async virtual void ShowErrorDialog(string message)
		{
			var dialog = new ContentDialog
			{
				Title = "Erreur",
				Content = message,
				CloseButtonText = "OK"
			};

			await dialog.ShowAsync();
		}
	}
}

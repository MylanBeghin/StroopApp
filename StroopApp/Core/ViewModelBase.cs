using System.ComponentModel;
using System.Runtime.CompilerServices;

using ModernWpf.Controls;

using StroopApp.Resources;

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
				Title = Strings.Error_Title,
				Content = message,
				CloseButtonText = Strings.Button_OK
			};

			await dialog.ShowAsync();
		}
		public async Task<bool> ShowConfirmationDialog(string title, string message)
		{
			var dlg = new ContentDialog
			{
				Title = title,
				Content = message,
				PrimaryButtonText = Strings.Button_Confirm,
				CloseButtonText = Strings.Button_Cancel
			};
			return await dlg.ShowAsync() == ContentDialogResult.Primary;
		}
	}
}

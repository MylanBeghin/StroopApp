using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

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
		public event PropertyChangedEventHandler? PropertyChanged;
		
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Affiche une boîte de dialogue d'erreur de manière asynchrone.
		/// </summary>
		protected virtual async Task ShowErrorDialogAsync(string message)
		{
			try
			{
				var dialog = new ContentDialog
				{
					Title = Strings.Error_Title,
					Content = message,
					CloseButtonText = Strings.Button_OK
				};

				await dialog.ShowAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error showing dialog: {ex.Message}");
			}
		}

		/// <summary>
		/// Affiche une boîte de dialogue de confirmation de manière asynchrone.
		/// </summary>
		public virtual async Task<bool> ShowConfirmationDialogAsync(string title, string message)
		{
			try
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
			catch (Exception ex)
			{
				Debug.WriteLine($"Error showing confirmation dialog: {ex.Message}");
				return false;
			}
		}
	}
}

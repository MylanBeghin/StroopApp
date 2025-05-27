using ModernWpf.Controls;

namespace StroopApp.Notifiers
{
	public class DefaultNotifier : IUserNotifier
	{
		public async Task NotifyAsync(string title, string message)
		{
			var dlg = new ContentDialog
			{
				Title = title,
				Content = message,
				CloseButtonText = "OK"
			};
			await dlg.ShowAsync();
		}
	}
}

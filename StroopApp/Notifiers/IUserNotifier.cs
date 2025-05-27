namespace StroopApp.Notifiers
{
	public interface IUserNotifier
	{
		Task NotifyAsync(string title, string message);
	}
}

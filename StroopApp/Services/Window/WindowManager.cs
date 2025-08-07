using StroopApp.Models;
using StroopApp.Views;

namespace StroopApp.Services.Window
{
	public class WindowManager : IWindowManager
	{
		private ParticipantWindow _participantWindow;

		public void ShowParticipantWindow(ExperimentSettings settings)
		{
			if (_participantWindow == null)
			{
				_participantWindow = new ParticipantWindow(settings);
				_participantWindow.Closed += (_, _) => _participantWindow = null;
				_participantWindow.Show();
			}
			else
			{
				_participantWindow.Reset();
				_participantWindow.Activate();

			}
		}
		public void CloseParticipantWindow()
		{
			_participantWindow?.Close();
			_participantWindow = null;
		}
	}
}

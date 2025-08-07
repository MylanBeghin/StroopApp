using StroopApp.Models;
using StroopApp.Views;

namespace StroopApp.Services.Window
{
	public interface IWindowManager
	{
		void ShowParticipantWindow(ExperimentSettings settings);
		void CloseParticipantWindow();
	}
}

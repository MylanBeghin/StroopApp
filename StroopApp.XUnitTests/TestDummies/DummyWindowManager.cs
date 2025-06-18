using StroopApp.Models;
using StroopApp.Services.Window;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyWindowManager : IWindowManager
	{
		public bool ShowCalled;
		public void ShowParticipantWindow(ExperimentSettings settings) => ShowCalled = true;
		public void CloseParticipantWindow()
		{
		}
	}
}

using StroopApp.Models;

namespace StroopApp.XUnitTests.Models
{
	public class TestTrial : StroopTrial
	{
		public void RaiseTrialNumberChanged()
		{
			OnPropertyChanged(nameof(TrialNumber));
		}
	}

}

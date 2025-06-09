namespace StroopApp.XUnitTests.Models
{
	using StroopApp.Models;

	using Xunit;

	public class SharedExperimentData_HandlersTests
	{
		private class TestTrial : StroopTrial
		{
			public void RaiseTrialNumberChanged()
			{
				OnPropertyChanged(nameof(TrialNumber));
			}
		}

		[Fact]
		public void CurrentTrial_AttachHandler_RaisesCurrentTrialChanged()
		{
			// Arrange
			var data = new SharedExperimentData();
			var trial = new TestTrial();
			data.CurrentTrial = trial;

			bool handlerCalled = false;
			data.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(data.CurrentTrial)) handlerCalled = true; };

			// Act
			trial.RaiseTrialNumberChanged();

			// Assert
			Assert.True(handlerCalled);
		}

		[Fact]
		public void CurrentTrial_DetachHandler_PreventsNotificationFromOldTrial()
		{
			// Arrange
			var data = new SharedExperimentData();
			var oldTrial = new TestTrial();
			var newTrial = new TestTrial();
			data.CurrentTrial = oldTrial;
			data.CurrentTrial = newTrial;

			int count = 0;
			data.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(data.CurrentTrial)) count++; };

			// Act
			oldTrial.RaiseTrialNumberChanged(); // Ne doit rien faire
			newTrial.RaiseTrialNumberChanged(); // Doit notifier

			// Assert
			Assert.Equal(1, count);
		}

		[Fact]
		public void CurrentTrial_SetSameInstance_NoDoubleAttachment()
		{
			// Arrange
			var data = new SharedExperimentData();
			var trial = new TestTrial();
			data.CurrentTrial = trial;
			data.CurrentTrial = trial; // même instance, pas de double abonné

			int callCount = 0;
			data.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(data.CurrentTrial)) callCount++; };

			// Act
			trial.RaiseTrialNumberChanged();
			trial.RaiseTrialNumberChanged();

			// Assert
			Assert.Equal(2, callCount);
		}

		[Fact]
		public void CurrentTrial_SetToNull_DetachesHandler()
		{
			// Arrange
			var data = new SharedExperimentData();
			var trial = new TestTrial();
			data.CurrentTrial = trial;
			data.CurrentTrial = null;

			int callCount = 0;
			data.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(data.CurrentTrial)) callCount++; };

			// Act
			trial.RaiseTrialNumberChanged();

			// Assert
			Assert.Equal(0, callCount); // Pas de notification (handler détaché)
		}
	}
}

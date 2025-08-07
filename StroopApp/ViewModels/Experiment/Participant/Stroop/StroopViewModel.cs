using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using DocumentFormat.OpenXml.Wordprocessing;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;
using StroopApp.Views.Experiment.Participant.Stroop;

public class StroopViewModel : ViewModelBase
{
	private ExperimentSettings _settings;
	public ExperimentSettings Settings
	{
		get => _settings;
		set
		{
			_settings = value;
			OnPropertyChanged();
		}
	}

	private UserControl _currentControl;
	public UserControl CurrentControl
	{
		get => _currentControl;
		set
		{
			_currentControl = value;
			OnPropertyChanged();
		}
	}

	private TaskCompletionSource<double> _inputTcs;
	private readonly Stopwatch _responseTime;
	private readonly Stopwatch _wordTimer;
	private readonly INavigationService _participantWindowNavigationService;
	private readonly Random random = new Random();
	private CancellationTokenSource _cancellationTokenSource;
	private bool _isDisposed = false;

	public StroopViewModel(ExperimentSettings settings, INavigationService participantWindowNavigationService)
	{
		Settings = settings;
		_participantWindowNavigationService = participantWindowNavigationService;
		_responseTime = new Stopwatch();
		_wordTimer = new Stopwatch();
		_cancellationTokenSource = new CancellationTokenSource();

		StartTrials();
	}

	public void StartResponseTimer()
	{
		_responseTime.Restart();
		_wordTimer.Restart();
		_inputTcs = new TaskCompletionSource<double>();
	}

	public async void StartTrials()
	{
		try
		{
			foreach (var trial in Settings.ExperimentContext.CurrentBlock.TrialRecords)
			{
				if (Settings.ExperimentContext.IsTaskStopped || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					HandleTaskStopped();
					return;
				}
				Settings.ExperimentContext.CurrentTrial = trial;

				CurrentControl = new FixationCrossControl();
				await Task.Delay(Settings.CurrentProfile.FixationDuration, _cancellationTokenSource.Token);
				if (Settings.ExperimentContext.IsTaskStopped || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					HandleTaskStopped();
					return;
				}
				if (Settings.CurrentProfile.IsAmorce)
				{
					CurrentControl = new AmorceControl(trial.Amorce);
					await Task.Delay(Settings.CurrentProfile.AmorceDuration, _cancellationTokenSource.Token);

					if (Settings.ExperimentContext.IsTaskStopped || _cancellationTokenSource.Token.IsCancellationRequested)
					{
						HandleTaskStopped();
						return;
					}
				}

				var wordControl = new WordControl(trial.Stimulus.Text, trial.Stimulus.Color);
				CurrentControl = wordControl;

				await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
				{
					if (!_isDisposed)
					{
						_responseTime.Restart();
						_wordTimer.Restart();
						_inputTcs = new TaskCompletionSource<double>();
					}
				}), DispatcherPriority.Render);

				if (Settings.ExperimentContext.IsTaskStopped || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					HandleTaskStopped();
					return;
				}

				var delayTask = Task.Delay(Settings.CurrentProfile.MaxReactionTime, _cancellationTokenSource.Token);
				var completed = await Task.WhenAny(_inputTcs.Task, delayTask);

				if (completed == _inputTcs.Task && !_inputTcs.Task.IsCanceled)
				{
					_responseTime.Stop();
					trial.ReactionTime = _inputTcs.Task.Result;
					Settings.ExperimentContext.CurrentBlock.TrialTimes.Add(trial.ReactionTime);
					Settings.ExperimentContext.ReactionPoints.Add(new ReactionTimePoint(trial.TrialNumber, trial.ReactionTime, trial.IsValidResponse));
					CurrentControl = new FixationCrossControl();
				}
				else if (!delayTask.IsCanceled)
				{
					_responseTime.Stop();
					_inputTcs.TrySetCanceled();
					Settings.ExperimentContext.CurrentBlock.TrialTimes.Add(null);
					Settings.ExperimentContext.ReactionPoints.Add(new ReactionTimePoint(trial.TrialNumber, double.NaN, null));
				}

				if (Settings.ExperimentContext.IsTaskStopped || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					HandleTaskStopped();
					return;
				}

				double remaining = Settings.CurrentProfile.MaxReactionTime - _wordTimer.Elapsed.TotalMilliseconds;
				if (remaining > 0)
					await Task.Delay((int)remaining, _cancellationTokenSource.Token);

				_wordTimer.Stop();
			}

			EndBlock();
		}
		catch (OperationCanceledException)
		{
			// Stop
			HandleTaskStopped();
		}
	}
	public void StopTask()
	{
		Settings.ExperimentContext.IsTaskStopped = true;
		_cancellationTokenSource?.Cancel();
	}

	public void Dispose()
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			StopTask();
			_cancellationTokenSource?.Dispose();
		}
	}
	private void HandleTaskStopped()
	{
		_responseTime.Stop();
		_wordTimer.Stop();
		_inputTcs?.TrySetCanceled();

		if (Settings.ExperimentContext.CurrentBlock != null)
		{
			Settings.ExperimentContext.CurrentBlock.CalculateValues();
		}

		Settings.ExperimentContext.CurrentTrial = null;
		Settings.ExperimentContext.IsBlockFinished = true;
	}
	public void EndBlock()
	{
		Settings.ExperimentContext.CurrentBlock.CalculateValues();
		Settings.ExperimentContext.CurrentTrial = null;
		Settings.ExperimentContext.IsBlockFinished = true;
	}

	public void ProcessInput(Key key)
	{
		if (_inputTcs == null || _inputTcs.Task.IsCompleted)
			return;

		string? answer = key == Settings.KeyMappings.Red.Key ? Settings.KeyMappings.Red.Color
					   : key == Settings.KeyMappings.Blue.Key ? Settings.KeyMappings.Blue.Color
					   : key == Settings.KeyMappings.Green.Key ? Settings.KeyMappings.Green.Color
					   : key == Settings.KeyMappings.Yellow.Key ? Settings.KeyMappings.Yellow.Color
					   : null;

		if (answer != null)
		{
			var trial = Settings.ExperimentContext.CurrentTrial;
			trial.GivenAnswer = answer;
			trial.IsValidResponse = string.Equals(trial.ExpectedAnswer, answer, StringComparison.OrdinalIgnoreCase);
			_inputTcs.TrySetResult(_responseTime.Elapsed.TotalMilliseconds);
		}
	}
}

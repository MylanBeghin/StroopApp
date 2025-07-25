﻿// StroopViewModel.cs
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
	private readonly INavigationService _navigationService;
	private readonly Random random = new Random();

	public StroopViewModel(ExperimentSettings settings, INavigationService navigationService)
	{
		Settings = settings;
		_navigationService = navigationService;
		_responseTime = new Stopwatch();
		_wordTimer = new Stopwatch();
		GenerateTrials();
		StartTrials();
	}

	public void StartResponseTimer()
	{
		_responseTime.Restart(); // démarrage après rendu
		_wordTimer.Restart();
		_inputTcs = new TaskCompletionSource<double>();
	}
	private List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage)
	{
		int switchCount = (count - 1) * switchPercentage / 100;
		int noSwitchCount = (count - 1) - switchCount;

		var switches = new List<bool>();
		switches.AddRange(Enumerable.Repeat(true, switchCount));
		switches.AddRange(Enumerable.Repeat(false, noSwitchCount));
		switches = switches.OrderBy(_ => random.Next()).ToList();

		var sequence = new List<AmorceType>();
		var current = random.Next(0, 2) == 0 ? AmorceType.Round : AmorceType.Square;
		sequence.Add(current);

		foreach (var isSwitch in switches)
		{
			if (isSwitch)
				current = current == AmorceType.Round ? AmorceType.Square : AmorceType.Round;

			sequence.Add(current);
		}

		return sequence;
	}

	private void GenerateTrials()
	{
		var wordColors = new[] { "Blue", "Red", "Green", "Yellow" };
		var wordTexts = new[] { "Blue", "Red", "Green", "Yellow" };
		int total = Settings.CurrentProfile.WordCount;

		int congruentCount = total * Settings.CurrentProfile.CongruencePercent / 100;
		int incongruentCount = total - congruentCount;
		var congruenceFlags = new List<bool>();
		congruenceFlags.AddRange(Enumerable.Repeat(true, congruentCount));      // true = congruent
		congruenceFlags.AddRange(Enumerable.Repeat(false, incongruentCount));   // false = incongruent
		congruenceFlags = congruenceFlags.OrderBy(_ => random.Next()).ToList();

		List<AmorceType>? amorceSequence = null;
		if (Settings.CurrentProfile.IsAmorce)
		{
			amorceSequence = GenerateAmorceSequence(total, Settings.CurrentProfile.DominantPercent);
		}

		for (int i = 0; i < total; i++)
		{
			int type = random.Next(0, 1);
			var trial = new StroopTrial
			{
				TrialNumber = i + 1,
				Block = Settings.Block,
				ParticipantId = Settings.Participant.Id,
				IsAmorce = Settings.CurrentProfile.IsAmorce,
				SwitchPercent = Settings.CurrentProfile.DominantPercent,
				CongruencePercent = Settings.CurrentProfile.CongruencePercent,
			};
			bool isCongruent = congruenceFlags[i];
			if (isCongruent)
			{
				int idx = random.Next(0, wordColors.Length);
				trial.Stimulus = new Word(wordColors[idx], wordTexts[idx]);
				trial.IsCongruent = true;
			}
			else
			{
				var indices = Enumerable.Range(0, wordColors.Length).OrderBy(_ => random.Next()).Take(2).ToArray();
				trial.Stimulus = new Word(wordColors[indices[0]], wordTexts[indices[1]]);
				trial.IsCongruent = false;
			}

			if (amorceSequence != null)
			{
				trial.Amorce = amorceSequence[i];
			}
			trial.DetermineExpectedAnswer();
			Settings.ExperimentContext.CurrentBlock.TrialRecords.Add(trial);
		}
	}

	public async void StartTrials()
	{
		foreach (var trial in Settings.ExperimentContext.CurrentBlock.TrialRecords)
		{
			Settings.ExperimentContext.CurrentTrial = trial;

			CurrentControl = new FixationCrossControl();
			await Task.Delay(Settings.CurrentProfile.FixationDuration);
			if (Settings.CurrentProfile.IsAmorce)
			{
				CurrentControl = new AmorceControl(trial.Amorce);
				await Task.Delay(Settings.CurrentProfile.AmorceDuration);
			}

			var wordControl = new WordControl(trial.Stimulus.Text, trial.Stimulus.Color);
			CurrentControl = wordControl;

			await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				_responseTime.Restart();
				_wordTimer.Restart();
				_inputTcs = new TaskCompletionSource<double>();
			}), DispatcherPriority.Render);

			var delayTask = Task.Delay(Settings.CurrentProfile.MaxReactionTime);
			var completed = await Task.WhenAny(_inputTcs.Task, delayTask);

			if (completed == _inputTcs.Task)
			{
				_responseTime.Stop();
				trial.ReactionTime = _inputTcs.Task.Result;
				Settings.ExperimentContext.CurrentBlock.TrialTimes.Add(trial.ReactionTime);
				Settings.ExperimentContext.ReactionPoints.Add(new ReactionTimePoint(trial.TrialNumber, trial.ReactionTime, trial.IsValidResponse));
				CurrentControl = new FixationCrossControl();
			}
			else
			{
				_responseTime.Stop();
				_inputTcs.TrySetCanceled();
				Settings.ExperimentContext.CurrentBlock.TrialTimes.Add(null);
				Settings.ExperimentContext.ReactionPoints.Add(new ReactionTimePoint(trial.TrialNumber, double.NaN, null));
			}

			double remaining = Settings.CurrentProfile.MaxReactionTime - _wordTimer.Elapsed.TotalMilliseconds;
			if (remaining > 0)
				await Task.Delay((int)remaining);

			_wordTimer.Stop();
		}

		EndBlock();
	}

	public void EndBlock()
	{
		Settings.ExperimentContext.CurrentBlock.CalculateValues();
		Settings.ExperimentContext.CurrentTrial = null;
		_navigationService.NavigateTo(() => new EndInstructionsPage());
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

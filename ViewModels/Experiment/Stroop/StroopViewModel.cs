using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StroopApp.Models;
using StroopApp.Views.Experiment.Participant.Stroop;

public class StroopViewModel : INotifyPropertyChanged
{
    private ExperimentSettings _settings;
    public ExperimentSettings Settings
    {
        get => _settings;
        set { _settings = value; OnPropertyChanged(); }
    }
    private readonly Random random = new Random();
    private UserControl _currentControl;
    public UserControl CurrentControl
    {
        get => _currentControl;
        set { _currentControl = value; OnPropertyChanged(); }
    }
    private TaskCompletionSource<long> _inputTcs;
    private Stopwatch _currentStopwatch;
    private StroopTrial _currentTrial;
    public StroopTrial CurrentTrial
    {
        get => _currentTrial;
        set { _currentTrial = value; OnPropertyChanged(); }
    }
    public StroopViewModel(ExperimentSettings settings)
    {
        Settings = settings;
        GenerateTrials();
        StartTrials();
    }

    private void GenerateTrials()
    {
        var wordColors = new[] { "Blue", "Red", "Green", "Yellow" };
        var wordTexts = new[] { "Bleu", "Rouge", "Vert", "Jaune" };

        Settings.ExperimentContext.TrialRecords.Clear();
        for (int i = 0; i < Settings.CurrentProfile.WordCount; i++)
        {
            var trial = new StroopTrial
            {
                TrialNumber = i + 1,
                StroopType = Settings.CurrentProfile.StroopType,
                Block = 1,
                ParticipantId = Settings.Participant.Id
            };

            if (Settings.CurrentProfile.StroopType == "Congruent")
            {
                int index = random.Next(0, 4);
                trial.Stimulus = new Word(wordColors[index], wordTexts[index]);
            }
            else
            {
                int[] indices = Enumerable.Range(0, 4)
                                          .OrderBy(n => random.Next())
                                          .Take(2)
                                          .ToArray();
                int firstIndex = indices[0];
                int secondIndex = indices[1];
                trial.Stimulus = new Word(wordColors[firstIndex], wordTexts[secondIndex]);
                if (string.Equals(Settings.CurrentProfile.StroopType, "Amorce", StringComparison.OrdinalIgnoreCase))
                {
                    trial.Amorce = random.Next(0, 2) == 0 ? AmorceType.Round : AmorceType.Square;
                }
            }
            trial.DetermineExpectedAnswer();
            Settings.ExperimentContext.TrialRecords.Add(trial);
        }
    }

    public async void StartTrials()
    {
        foreach (var trial in Settings.ExperimentContext.TrialRecords)
        {
            CurrentTrial = trial;
            Settings.ExperimentContext.CurrentTrialNumber = trial.TrialNumber;
            _currentStopwatch = Stopwatch.StartNew();
            _inputTcs = new TaskCompletionSource<long>();
            CurrentControl = new FixationCrossControl();
            await Task.Delay(_settings.CurrentProfile.FixationDuration);
            if (trial.StroopType=="Amorce")
            {
                CurrentControl = new AmorceControl(trial.Amorce);
                await Task.Delay(_settings.CurrentProfile.AmorceDuration);
            }
            var wordControl = new WordControl(trial.Stimulus.Text, trial.Stimulus.Color);
            CurrentControl = wordControl;
            var delayTask = Task.Delay(_settings.CurrentProfile.WordDuration);
            var completedTask = await Task.WhenAny(_inputTcs.Task, delayTask);
            long reactionTime = 0;
            if (completedTask == _inputTcs.Task)
            {
                reactionTime = _inputTcs.Task.Result - _settings.CurrentProfile.FixationDuration - _settings.CurrentProfile.AmorceDuration ;
                long remaining = _settings.CurrentProfile.WordDuration - reactionTime;
                if (remaining > 0)
                {
                    CurrentControl = new FixationCrossControl();
                    await Task.Delay((int)remaining);
                }
            }
            else
            {
                reactionTime = _settings.CurrentProfile.WordDuration;
            }
            _currentStopwatch.Stop();
            trial.ReactionTime = reactionTime;
        }
    }
    public void ProcessInput(Key key)
    {
        if (_inputTcs != null && !_inputTcs.Task.IsCompleted)
        {
            string answer = null;
            if (key == Settings.KeyMappings.Red.Key)
                answer = Settings.KeyMappings.Red.Color;
            else if (key == Settings.KeyMappings.Blue.Key)
                answer = Settings.KeyMappings.Blue.Color;
            else if (key == Settings.KeyMappings.Green.Key)
                answer = Settings.KeyMappings.Green.Color;
            else if (key == Settings.KeyMappings.Yellow.Key)
                answer = Settings.KeyMappings.Yellow.Color;

            if (answer != null)
            {
                CurrentTrial.GivenAnswer = answer;
                CurrentTrial.IsValidResponse = string.Equals(CurrentTrial.ExpectedAnswer, answer, StringComparison.OrdinalIgnoreCase);
                _inputTcs.TrySetResult(_currentStopwatch.ElapsedMilliseconds);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

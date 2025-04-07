using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using StroopApp.Models;
using StroopApp.ViewModels.Experiment;
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

    private TaskCompletionSource<double> _inputTcs;

    private Stopwatch _wordTimer;

    private Stopwatch _reactionTimeTimer;

    private StroopTrial _currentTrial;
    public StroopTrial CurrentTrial
    {
        get => _currentTrial;
        set { _currentTrial = value; OnPropertyChanged(); }
    }

    private double _wordTimerValue;
    public double WordTimerValue
    {
        get => _wordTimerValue;
        set { _wordTimerValue = value; OnPropertyChanged(); }
    }

    private double _reactionTimeTimerValue;
    public double ReactionTimeTimerValue
    {
        get => _reactionTimeTimerValue;
        set { _reactionTimeTimerValue = value; OnPropertyChanged(); }
    }

    public StroopViewModel(ExperimentSettings settings)
    {
        Settings = settings;
        _wordTimer = new Stopwatch();
        _reactionTimeTimer = new Stopwatch();
        GenerateTrials();
        StartTrials();
        new GraphViewModel(Settings);
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
            Settings.ExperimentContext.CurrentTrial = trial;
            _wordTimer.Reset();
            _reactionTimeTimer.Reset();
            _wordTimer.Start();

            var debugTimer = new DispatcherTimer();
            debugTimer.Interval = TimeSpan.FromMilliseconds(10);
            debugTimer.Tick += (s, e) =>
            {
                WordTimerValue = _wordTimer.Elapsed.TotalMilliseconds;
                ReactionTimeTimerValue = _reactionTimeTimer.IsRunning ? _reactionTimeTimer.Elapsed.TotalMilliseconds : ReactionTimeTimerValue;
            };
            debugTimer.Start();

            CurrentControl = new FixationCrossControl();
            await Task.Delay((int)Settings.CurrentProfile.FixationDuration);

            if (trial.StroopType == "Amorce")
            {
                CurrentControl = new AmorceControl(trial.Amorce);
                await Task.Delay((int)Settings.CurrentProfile.AmorceDuration);
            }

            CurrentControl = new WordControl(trial.Stimulus.Text, trial.Stimulus.Color);
            _reactionTimeTimer.Start();
            _inputTcs = new TaskCompletionSource<double>();


            var delayTask = Task.Delay((int)Settings.CurrentProfile.MaxReactionTime);
            var completedTask = await Task.WhenAny(_inputTcs.Task, delayTask);
            double reactionTime = 0;
            if (completedTask == _inputTcs.Task)
            {
                _reactionTimeTimer.Stop();
                reactionTime = _inputTcs.Task.Result;
                CurrentControl = new FixationCrossControl();
            }
            else
            {
                _reactionTimeTimer.Stop();
                reactionTime = _reactionTimeTimer.Elapsed.TotalMilliseconds;
            }

            double remaining = Settings.CurrentProfile.WordDuration - _wordTimer.Elapsed.TotalMilliseconds;
            if (remaining > 0)
            {
                await Task.Delay((int)remaining);
            }
            _wordTimer.Stop();

            trial.ReactionTime = reactionTime;
            Settings.ExperimentContext.CurrentTrial = trial;
            debugTimer.Stop();
            WordTimerValue = _wordTimer.Elapsed.TotalMilliseconds;
            ReactionTimeTimerValue = _reactionTimeTimer.Elapsed.TotalMilliseconds;
            OnPropertyChanged(nameof(WordTimerValue));
            OnPropertyChanged(nameof(ReactionTimeTimerValue));
            _wordTimer.Reset();
            _reactionTimeTimer.Reset();
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
                Settings.ExperimentContext.CurrentTrial.GivenAnswer = answer;
                Settings.ExperimentContext.CurrentTrial.IsValidResponse = string.Equals(Settings.ExperimentContext.CurrentTrial.ExpectedAnswer, answer, StringComparison.OrdinalIgnoreCase);
                _inputTcs.TrySetResult(_reactionTimeTimer.Elapsed.TotalMilliseconds);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

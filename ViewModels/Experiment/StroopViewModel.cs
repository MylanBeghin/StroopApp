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
    private readonly ExperimentSettings _settings;
    private readonly Random random = new Random();

    // Liste des trials générés
    public ObservableCollection<StroopTrialRecord> Trials { get; set; } = new ObservableCollection<StroopTrialRecord>();

    // UserControl actuellement affiché
    private UserControl _currentControl;
    public UserControl CurrentControl
    {
        get => _currentControl;
        set { _currentControl = value; OnPropertyChanged(); }
    }

    // Pour gérer la saisie
    private TaskCompletionSource<long> _inputTcs;
    private Stopwatch _currentStopwatch;

    // On conserve le trial courant pour mise à jour
    private StroopTrialRecord _currentTrial;

    public StroopViewModel(ExperimentSettings settings)
    {
        _settings = settings;
        GenerateTrials();
        // Lancer la séquence sans attendre (peut être lancé par la vue si besoin)
        StartTrials();
    }

    private void GenerateTrials()
    {
        // Par exemple, on utilise une logique similaire aux versions précédentes
        var wordColors = new[] { "Blue", "Red", "Green", "Yellow" };
        var wordTexts = new[] { "Bleu", "Rouge", "Vert", "Jaune" };

        Trials.Clear();
        for (int i = 0; i < _settings.CurrentProfile.WordCount; i++)
        {
            var trial = new StroopTrialRecord
            {
                TrialNumber = i + 1,
                StroopType = _settings.CurrentProfile.StroopType, // "Congruent", "Incongruent" ou "Amorce"
                Block = 1
            };

            if (_settings.CurrentProfile.StroopType == "Congruent")
            {
                int index = random.Next(0, 4);
                trial.Stimulus = new Word(wordColors[index], wordTexts[index]);
            }
            else // Pour "Incongruent" ou "Amorce"
            {
                int[] indices = Enumerable.Range(0, 4)
                                          .OrderBy(n => random.Next())
                                          .Take(2)
                                          .ToArray();
                int firstIndex = indices[0];
                int secondIndex = indices[1];
                trial.Stimulus = new Word(wordColors[firstIndex], wordTexts[secondIndex]);
                if (string.Equals(_settings.CurrentProfile.StroopType, "Amorce", StringComparison.OrdinalIgnoreCase))
                {
                    trial.Amorce = random.Next(0, 2) == 0 ? AmorceType.Round : AmorceType.Square;
                }
            }
            trial.DetermineExpectedAnswer();
            Trials.Add(trial);
        }
    }

    public async void StartTrials()
    {
        foreach (var trial in Trials)
        {
            _currentTrial = trial;
            // Démarrage du chronomètre pour la phase mot
            _currentStopwatch = Stopwatch.StartNew();
            _inputTcs = new TaskCompletionSource<long>();
            // Affichage de la croix de fixation pendant FixationDuration
            CurrentControl = new FixationCrossControl();
            await Task.Delay(_settings.CurrentProfile.FixationDuration);

            // Si le trial est de type "Amorce", afficher l'amorce pendant AmorceDuration
            if (trial.StroopType=="Amorce")
            {
                CurrentControl = new AmorceControl(trial.Amorce);
                await Task.Delay(_settings.CurrentProfile.AmorceDuration);
            }

            // Affichage du mot avec le WordControl
            // Ici, on affiche toujours le texte du stimulus ; la couleur affichée dépend du design souhaité.
            // Par exemple, pour Incongruent, on peut afficher le mot dans la couleur indiquée par Stimulus.Color.
            var wordControl = new WordControl(trial.Stimulus.Text, trial.Stimulus.Color);
            CurrentControl = wordControl;

            // On attend soit l'input, soit l'expiration de WordDuration
            var delayTask = Task.Delay(_settings.CurrentProfile.WordDuration);
            var completedTask = await Task.WhenAny(_inputTcs.Task, delayTask);
            long reactionTime = 0;
            if (completedTask == _inputTcs.Task)
            {
                reactionTime = _inputTcs.Task.Result;
                // En cas de réponse anticipée, on affiche à nouveau la fixation pour le temps restant
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

        // À ce stade, vous pouvez déclencher l'export des résultats, etc.
    }

    // Méthode appelée par la vue lors d'un KeyDown
    public void ProcessInput(Key key)
    {
        if (_inputTcs != null && !_inputTcs.Task.IsCompleted)
        {
            string answer = null;
            if (key == _settings.KeyMappings.Red.Key)
                answer = _settings.KeyMappings.Red.Color;
            else if (key == _settings.KeyMappings.Blue.Key)
                answer = _settings.KeyMappings.Blue.Color;
            else if (key == _settings.KeyMappings.Green.Key)
                answer = _settings.KeyMappings.Green.Color;
            else if (key == _settings.KeyMappings.Yellow.Key)
                answer = _settings.KeyMappings.Yellow.Color;

            if (answer != null)
            {
                _currentTrial.GivenAnswer = answer;
                _currentTrial.IsValidResponse = string.Equals(_currentTrial.ExpectedAnswer, answer, StringComparison.OrdinalIgnoreCase);
                _inputTcs.TrySetResult(_currentStopwatch.ElapsedMilliseconds);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

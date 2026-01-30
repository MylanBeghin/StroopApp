using System.Globalization;

using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Language;

namespace StroopApp.Services.Trial
{
    /// <summary>
    /// Service for generating randomized Stroop trial sequences with localized stimuli and optional visual cues.
    /// </summary>
    public class TrialGenerationService : ITrialGenerationService
    {
		private readonly Random _random = new Random();
		private readonly ILanguageService _languageService;

		public TrialGenerationService(ILanguageService languageService)
		{
			_languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
		}

		/// <summary>
		/// Legacy method for backward compatibility.
		/// Delegates to the interface-based overload via adapter.
		/// </summary>
		public List<StroopTrial> GenerateTrials(ExperimentSettings settings)
		{
			if (settings?.CurrentProfile == null)
				throw new ArgumentException("Settings and  CurrentProfile cannot be null", nameof(settings));

			var config = new ExperimentSettingsTrialConfigurationAdapter(settings);
			return GenerateTrials(config);
		}

		/// <summary>
		/// Generates trials based on minimal configuration interface.
		/// This is the primary implementation.
		/// </summary>
		public List<StroopTrial> GenerateTrials(ITrialConfiguration config)
		{
			if (config == null)
				throw new ArgumentNullException(nameof(config));

			var trials = new List<StroopTrial>();
			var wordColors = new[] { "Blue", "Red", "Green", "Yellow" };

			var taskCultureCode = string.IsNullOrWhiteSpace(config.TaskLanguage)
								? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName
								: config.TaskLanguage;

			var taskCulture = new CultureInfo(taskCultureCode);

			var wordTexts = new[]
			{
				_languageService.GetLocalizedString("Word_BLUE", taskCultureCode),
				_languageService.GetLocalizedString("Word_RED", taskCultureCode),
				_languageService.GetLocalizedString("Word_GREEN", taskCultureCode),
				_languageService.GetLocalizedString("Word_YELLOW", taskCultureCode)
			};

			int total = config.WordCount;
			int congruentCount = total * config.CongruencePercent / 100;
			int incongruentCount = total - congruentCount;

			var congruenceFlags = new List<bool>();
			congruenceFlags.AddRange(Enumerable.Repeat(true, congruentCount));
			congruenceFlags.AddRange(Enumerable.Repeat(false, incongruentCount));
			congruenceFlags = congruenceFlags.OrderBy(_ => _random.Next()).ToList();

			List<VisualCueType>? amorceSequence = null;
			if (config.IsAmorce)
				amorceSequence = GenerateAmorceSequence(total, config.DominantPercent);

			for (int i = 0; i < total; i++)
			{
				var trial = new StroopTrial
				{
					TrialNumber = i + 1,
					Block = config.Block,
					ParticipantId = config.ParticipantId,
					IsAmorce = config.IsAmorce,
					SwitchPercent = config.DominantPercent,
					CongruencePercent = config.CongruencePercent,
				};

				bool isCongruent = congruenceFlags[i];

				if (isCongruent)
				{
					int idx = _random.Next(wordColors.Length);
					trial.Stimulus = new Word(wordColors[idx], wordColors[idx], wordTexts[idx]);
					trial.IsCongruent = true;
				}
				else
				{
					var indices = Enumerable.Range(0, wordColors.Length)
						.OrderBy(_ => _random.Next())
						.Take(2)
						.ToArray();
					trial.Stimulus = new Word(wordColors[indices[0]], wordColors[indices[1]], wordTexts[indices[1]]);
					trial.IsCongruent = false;
				}

				if (amorceSequence != null)
					trial.VisualCue = amorceSequence[i];
				trial.DetermineExpectedAnswer();

				trials.Add(trial);
			}

			return trials;
		}

		public List<VisualCueType> GenerateAmorceSequence(int count, int switchPercentage)
		{
			if (count <= 0)
				throw new ArgumentException("Count of visual cues must be positive", nameof(count));

			int switchCount = (count - 1) * switchPercentage / 100;
			int noSwitchCount = (count - 1) - switchCount;

			var switches = new List<bool>();
			switches.AddRange(Enumerable.Repeat(true, switchCount));
			switches.AddRange(Enumerable.Repeat(false, noSwitchCount));
			switches = switches.OrderBy(_ => _random.Next()).ToList();

			var sequence = new List<VisualCueType>();
			var current = _random.Next(0, 2) == 0 ? VisualCueType.Round : VisualCueType.Square;
			sequence.Add(current);

			foreach (var isSwitch in switches)
			{
				if (isSwitch)
					current = current == VisualCueType.Round ? VisualCueType.Square : VisualCueType.Round;

				sequence.Add(current);
			}

			return sequence;
		}
	}
}
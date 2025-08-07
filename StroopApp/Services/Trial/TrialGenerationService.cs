using System.Globalization;

using StroopApp.Core;
using StroopApp.Models;

namespace StroopApp.Services.Trial
{
	public class TrialGenerationService : ITrialGenerationService
	{
		private readonly Random _random = new Random();

		public List<StroopTrial> GenerateTrials(ExperimentSettings settings)
		{
			if (settings?.CurrentProfile == null)
				throw new ArgumentException("Settings et CurrentProfile ne peuvent pas être null", nameof(settings));

			var trials = new List<StroopTrial>();
			var wordColors = new[] { "Blue", "Red", "Green", "Yellow" };

			// ✅ Configuration de la culture pour les mots localisés
			var culture = new CultureInfo(settings.CurrentProfile.TaskLanguage ?? "en");
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;

			var loc = new LocalizedStrings();
			var wordTexts = new[]
			{
				loc["Word_BLUE"],
				loc["Word_RED"],
				loc["Word_GREEN"],
				loc["Word_YELLOW"]
			};

			int total = settings.CurrentProfile.WordCount;
			int congruentCount = total * settings.CurrentProfile.CongruencePercent / 100;
			int incongruentCount = total - congruentCount;

			// ✅ Génération des flags de congruence de manière aléatoire
			var congruenceFlags = new List<bool>();
			congruenceFlags.AddRange(Enumerable.Repeat(true, congruentCount));
			congruenceFlags.AddRange(Enumerable.Repeat(false, incongruentCount));
			congruenceFlags = congruenceFlags.OrderBy(_ => _random.Next()).ToList();

			// ✅ Génération de la séquence d'amorces si nécessaire
			List<AmorceType>? amorceSequence = null;
			if (settings.CurrentProfile.IsAmorce)
				amorceSequence = GenerateAmorceSequence(total, settings.CurrentProfile.DominantPercent);

			// ✅ Génération des trials
			for (int i = 0; i < total; i++)
			{
				var trial = new StroopTrial
				{
					TrialNumber = i + 1,
					Block = settings.Block,
					ParticipantId = settings.Participant.Id,
					IsAmorce = settings.CurrentProfile.IsAmorce,
					SwitchPercent = settings.CurrentProfile.DominantPercent,
					CongruencePercent = settings.CurrentProfile.CongruencePercent,
				};

				bool isCongruent = congruenceFlags[i];

				if (isCongruent)
				{
					// Trial congruent : même couleur et même mot
					int idx = _random.Next(wordColors.Length);
					trial.Stimulus = new Word(wordColors[idx], wordColors[idx], wordTexts[idx]);
					trial.IsCongruent = true;
				}
				else
				{
					// Trial incongruent : couleur différente du mot
					var indices = Enumerable.Range(0, wordColors.Length)
						.OrderBy(_ => _random.Next())
						.Take(2)
						.ToArray();
					trial.Stimulus = new Word(wordColors[indices[0]], wordColors[indices[1]], wordTexts[indices[1]]);
					trial.IsCongruent = false;
				}

				// ✅ Assigner l'amorce si nécessaire
				if (amorceSequence != null)
					trial.Amorce = amorceSequence[i];

				// ✅ Déterminer la réponse attendue
				trial.DetermineExpectedAnswer();

				trials.Add(trial);
			}

			return trials;
		}

		public List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage)
		{
			if (count <= 0)
				throw new ArgumentException("Le nombre d'amorces doit être positif", nameof(count));

			int switchCount = (count - 1) * switchPercentage / 100;
			int noSwitchCount = (count - 1) - switchCount;

			var switches = new List<bool>();
			switches.AddRange(Enumerable.Repeat(true, switchCount));
			switches.AddRange(Enumerable.Repeat(false, noSwitchCount));
			switches = switches.OrderBy(_ => _random.Next()).ToList();

			var sequence = new List<AmorceType>();
			var current = _random.Next(0, 2) == 0 ? AmorceType.Round : AmorceType.Square;
			sequence.Add(current);

			foreach (var isSwitch in switches)
			{
				if (isSwitch)
					current = current == AmorceType.Round ? AmorceType.Square : AmorceType.Round;

				sequence.Add(current);
			}

			return sequence;
		}
	}
}
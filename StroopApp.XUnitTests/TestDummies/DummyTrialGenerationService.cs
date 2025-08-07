using System.Collections.Generic;

using StroopApp.Models;
using StroopApp.Services.Trial;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyTrialGenerationService : ITrialGenerationService
	{
		public bool GenerateTrialsCalled { get; private set; }
		public bool GenerateAmorceSequenceCalled { get; private set; }
		public int LastTrialCount { get; private set; }

		public List<StroopTrial> GenerateTrials(ExperimentSettings settings)
		{
			GenerateTrialsCalled = true;

			if (settings?.CurrentProfile == null)
				return new List<StroopTrial>();

			var trials = new List<StroopTrial>();
			int trialCount = settings.CurrentProfile.WordCount;
			LastTrialCount = trialCount;

			// Générer des trials de test simples
			for (int i = 0; i < trialCount; i++)
			{
				trials.Add(new StroopTrial
				{
					TrialNumber = i + 1,
					Block = settings.Block,
					ParticipantId = settings.Participant?.Id ?? "TestParticipant",
					IsCongruent = i % 2 == 0, // Alternance congruent/incongruent
					ExpectedAnswer = "TestAnswer",
					Stimulus = new Word("Red", "Red", "RED")
				});
			}

			return trials;
		}

		public List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage)
		{
			GenerateAmorceSequenceCalled = true;

			var sequence = new List<AmorceType>();
			for (int i = 0; i < count; i++)
			{
				sequence.Add(i % 2 == 0 ? AmorceType.Square : AmorceType.Round);
			}

			return sequence;
		}
	}
}
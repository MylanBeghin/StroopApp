using System.Collections.Generic;

using StroopApp.Models;

namespace StroopApp.Services.Trial
{
	public interface ITrialGenerationService
	{
		/// <summary>
		/// Génère une liste de trials basée sur les paramètres d'expérience
		/// </summary>
		/// <param name="settings">Paramètres de l'expérience</param>
		/// <returns>Liste des trials générés</returns>
		List<StroopTrial> GenerateTrials(ExperimentSettings settings);

		/// <summary>
		/// Génère une séquence d'amorces (optionnel)
		/// </summary>
		/// <param name="count">Nombre d'amorces à générer</param>
		/// <param name="switchPercentage">Pourcentage de changement d'amorce</param>
		/// <returns>Séquence d'amorces</returns>
		List<AmorceType> GenerateAmorceSequence(int count, int switchPercentage);
	}
}

using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

using StroopApp.Models;

namespace StroopApp.Services.Profile
{
	public class ProfileService : IProfileService
	{
		private readonly string _configDir;
		private readonly string _profilesPath;
		private readonly string _lastProfileFile;

		public ProfileService(string configDir)
		{
			_configDir = configDir;
			_profilesPath = Path.Combine(_configDir, "profiles.json");
			_lastProfileFile = "lastProfile.txt";
		}

		public ObservableCollection<ExperimentProfile> LoadProfiles()
		{
			if (!File.Exists(_profilesPath))
				return new ObservableCollection<ExperimentProfile>();

			var json = File.ReadAllText(_profilesPath);
			return JsonSerializer.Deserialize<ObservableCollection<ExperimentProfile>>(json)
				   ?? new ObservableCollection<ExperimentProfile>();
		}

		public void SaveProfiles(ObservableCollection<ExperimentProfile> profiles)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_profilesPath, json);
		}

		public void AddProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
		{
			profiles.Add(profile);
			SaveProfiles(profiles);
		}
		public ObservableCollection<ExperimentProfile> UpsertProfile(ExperimentProfile profile)
		{
			// On charge la liste actuelle depuis le JSON
			var allProfiles = LoadProfiles();

			// On recherche si ce profil existe déjà
			var existing = allProfiles.FirstOrDefault(p => p.Id == profile.Id);

			// S’il n’existe pas, on l’ajoute
			if (existing == null)
			{
				if (profile.Id == Guid.Empty)
					profile.Id = Guid.NewGuid();
				allProfiles.Add(profile);
			}
			else
			{
				// Sinon, on met à jour ses propriétés
				existing.ProfileName = profile.ProfileName;
				existing.Hours = profile.Hours;
				existing.Minutes = profile.Minutes;
				existing.Seconds = profile.Seconds;
				existing.WordDuration = profile.WordDuration;
				existing.FixationDuration = profile.FixationDuration;
				existing.AmorceDuration = profile.AmorceDuration;
				existing.IsAmorce = profile.IsAmorce;
				existing.GroupSize = profile.GroupSize;
				existing.TaskDuration = profile.TaskDuration;
				existing.WordCount = profile.WordCount;
				existing.MaxReactionTime = profile.MaxReactionTime;
				existing.CalculationMode = profile.CalculationMode;
				existing.DominantPercent = profile.DominantPercent;
				existing.CongruencePercent = profile.CongruencePercent;
				existing.SwitchPercent = profile.SwitchPercent;
				existing.UpdateDerivedValues();
			}

			// Sauvegarde de la liste
			SaveProfiles(allProfiles);

			// Renvoi de la liste actualisée
			return LoadProfiles();
		}
		public void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
		{
			if (profiles.Contains(profile))
			{
				profiles.Remove(profile);
				SaveProfiles(profiles);
			}
		}

		public Guid? LoadLastSelectedProfile()
		{
			if (File.Exists(_lastProfileFile))
			{
				var text = File.ReadAllText(_lastProfileFile);
				if (Guid.TryParse(text, out var id))
					return id;
			}
			return null;
		}

		public void SaveLastSelectedProfile(ExperimentProfile profile)
		{
			File.WriteAllText(_lastProfileFile, profile.Id.ToString());
		}
		public ExperimentProfile UpsertProfileWithoutReload(ExperimentProfile profile, ObservableCollection<ExperimentProfile> localProfiles)
		{

			var allProfiles = LoadProfiles();


			var existing = allProfiles.FirstOrDefault(p => p.Id == profile.Id);


			if (existing == null)
			{
				if (profile.Id == Guid.Empty)
					profile.Id = Guid.NewGuid();
				allProfiles.Add(profile);
			}
			else
			{

				existing.ProfileName = profile.ProfileName;
				existing.WordDuration = profile.WordDuration;
				// ... etc.
			}
			SaveProfiles(allProfiles);

			// Mise à jour côté mémoire, sans rafraîchir toute la liste
			var localExisting = localProfiles.FirstOrDefault(p => p.Id == profile.Id);

			if (localExisting == null)
			{
				// Ajout local
				localProfiles.Add(profile);
			}
			else
			{
				// Mise à jour locale
				localExisting.ProfileName = profile.ProfileName;
				localExisting.WordDuration = profile.WordDuration;
				// ... etc.
			}

			// On renvoie le profil (ou la référence “finale” après mise à jour)
			return profile;
		}
	}
}

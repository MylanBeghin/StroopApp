using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

using StroopApp.Models;

namespace StroopApp.Services.Profile
{
    /// <summary>
    /// Service for managing experiment profile persistence (CRUD operations) and last selected profile tracking.
    /// </summary>
    public class ProfileService : IProfileService
	{
		private readonly string _configDir;
		private readonly string _profilesPath;
		private readonly string _lastProfileFile;

		public ProfileService(string configDir)
		{
			_configDir = configDir ?? throw new ArgumentNullException(nameof(configDir));
			_profilesPath = Path.Combine(_configDir, "profiles.json");
			_lastProfileFile = Path.Combine(_configDir, "lastProfile.json");
		}

        /// <summary>
        /// Loads all experiment profiles from JSON configuration file.
        /// </summary>
        public ObservableCollection<ExperimentProfile> LoadProfiles()
		{
			if (!File.Exists(_profilesPath))
				return new ObservableCollection<ExperimentProfile>();

			var json = File.ReadAllText(_profilesPath);
			return JsonSerializer.Deserialize<ObservableCollection<ExperimentProfile>>(json)
				   ?? new ObservableCollection<ExperimentProfile>();
		}

        /// <summary>
        /// Saves all experiment profiles to JSON configuration file.
        /// </summary>
        public void SaveProfiles(ObservableCollection<ExperimentProfile> profiles)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_profilesPath, json);
		}

        /// <summary>
        /// Inserts a new profile or updates an existing one by ID, then returns the refreshed collection.
        /// </summary>
        public ObservableCollection<ExperimentProfile> UpsertProfile(ExperimentProfile profile)
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
				existing.TaskLanguage = profile.TaskLanguage;
				existing.UpdateDerivedValues();
			}


			SaveProfiles(allProfiles);


			return LoadProfiles();
		}

        /// <summary>
        /// Deletes a profile from the collection and persists changes.
        /// </summary>
        public void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
		{
			if (profiles.Contains(profile))
			{
				profiles.Remove(profile);
				SaveProfiles(profiles);
			}
		}

        /// <summary>
        /// Loads the last selected profile ID from configuration, or null if not found.
        /// </summary>
        public Guid? LoadLastSelectedProfile()
		{
			if (File.Exists(_lastProfileFile))
			{
				var text = File.ReadAllText(_lastProfileFile);
				try
				{
					var guidString = JsonSerializer.Deserialize<string>(text);
					if (Guid.TryParse(guidString, out var id))
						return id;
				}
				catch (JsonException)
				{
					if (Guid.TryParse(text, out var id))
						return id;
				}
			}
			return null;
		}

        /// <summary>
        /// Saves the currently selected profile ID to configuration.
        /// </summary>
        public void SaveLastSelectedProfile(ExperimentProfile profile)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(profile.Id, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_lastProfileFile, json);
		}
	}
}

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
			_lastProfileFile = Path.Combine(_configDir, "lastProfile.json");
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
				existing.UpdateDerivedValues();
			}


			SaveProfiles(allProfiles);


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
			Directory.CreateDirectory(_configDir);
			File.WriteAllText(_lastProfileFile, profile.Id.ToString());
		}
	}
}

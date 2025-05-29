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
		public void UpdateProfileById(ExperimentProfile modifiedProfile, Guid profileId, ObservableCollection<ExperimentProfile> profiles)
		{
			var target = profiles.FirstOrDefault(p => p.Id == profileId);
			if (target == null)
				return;

			target.ProfileName = modifiedProfile.ProfileName;
			target.Hours = modifiedProfile.Hours;
			target.Minutes = modifiedProfile.Minutes;
			target.Seconds = modifiedProfile.Seconds;
			target.WordDuration = modifiedProfile.WordDuration;
			target.FixationDuration = modifiedProfile.FixationDuration;
			target.AmorceDuration = modifiedProfile.AmorceDuration;
			target.IsAmorce = modifiedProfile.IsAmorce;
			target.GroupSize = modifiedProfile.GroupSize;
			target.TaskDuration = modifiedProfile.TaskDuration;
			target.WordCount = modifiedProfile.WordCount;
			target.MaxReactionTime = modifiedProfile.MaxReactionTime;
			target.CalculationMode = modifiedProfile.CalculationMode;
			target.DominantPercent = modifiedProfile.DominantPercent;
			target.CongruencePourcentage = modifiedProfile.CongruencePourcentage;
			target.UpdateDerivedValues();
			SaveProfiles(profiles);
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
	}
}

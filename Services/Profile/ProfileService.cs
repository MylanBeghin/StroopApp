using StroopApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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

        public void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles)
        {
            if (profiles.Contains(profile))
            {
                profiles.Remove(profile);
                SaveProfiles(profiles);
            }
        }

        public string? LoadLastSelectedProfile()
        {
            if (File.Exists(_lastProfileFile))
            {
                return File.ReadAllText(_lastProfileFile);
            }
            return null;
        }

        public void SaveLastSelectedProfile(ExperimentProfile profile)
        {
            File.WriteAllText(_lastProfileFile, profile.ProfileName);
        }
    }
}

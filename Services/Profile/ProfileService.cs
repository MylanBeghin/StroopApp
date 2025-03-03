using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using StroopApp.Models;

namespace StroopApp.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private const string ProfilesFilePath = "profiles.json";
        private const string LastProfileFile = "lastProfile.txt";

        public ObservableCollection<ExperimentProfile> LoadProfiles()
        {
            if (File.Exists(ProfilesFilePath))
            {
                var json = File.ReadAllText(ProfilesFilePath);
                return JsonSerializer.Deserialize<ObservableCollection<ExperimentProfile>>(json)
                       ?? new ObservableCollection<ExperimentProfile>();
            }
            return new ObservableCollection<ExperimentProfile>();
        }

        public void SaveProfiles(ObservableCollection<ExperimentProfile> profiles)
        {
            var json = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ProfilesFilePath, json);
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
            if (File.Exists(LastProfileFile))
            {
                return File.ReadAllText(LastProfileFile);
            }
            return null;
        }

        public void SaveLastSelectedProfile(ExperimentProfile profile)
        {
            File.WriteAllText(LastProfileFile, profile.ProfileName);
        }
    }
}

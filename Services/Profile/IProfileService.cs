using StroopApp.Models;
using System.Collections.ObjectModel;

namespace StroopApp.Services
{
    public interface IProfileService
    {
        ObservableCollection<ExperimentProfile> LoadProfiles();
        void SaveProfiles(ObservableCollection<ExperimentProfile> profiles);
        void AddProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles);
        void UpdateProfileById(ExperimentProfile modifiedProfile, Guid profileId, ObservableCollection<ExperimentProfile> profiles);
        void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles);
        Guid? LoadLastSelectedProfile();
        void SaveLastSelectedProfile(ExperimentProfile profile);
    }
}

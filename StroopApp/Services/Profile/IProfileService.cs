using System.Collections.ObjectModel;

using StroopApp.Models;

namespace StroopApp.Services
{
	public interface IProfileService
	{
		ObservableCollection<ExperimentProfile> LoadProfiles();
		void SaveProfiles(ObservableCollection<ExperimentProfile> profiles);
		void AddProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles);
		ObservableCollection<ExperimentProfile> UpsertProfile(ExperimentProfile profile);
		void DeleteProfile(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles);
		Guid? LoadLastSelectedProfile();
		void SaveLastSelectedProfile(ExperimentProfile profile);
	}
}

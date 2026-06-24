using StroopApp.Models;
using StroopApp.Services.Profile;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public partial class SimonProfileEditorViewModel : ProfileEditorViewModelBase
    {
        public SimonProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService) : base(profile, profiles, profileService)
        {
        }
    }
}

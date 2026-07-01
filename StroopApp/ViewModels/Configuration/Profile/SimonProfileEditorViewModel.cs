using CommunityToolkit.Mvvm.ComponentModel;
using StroopApp.Resources;
using StroopApp.Models;
using StroopApp.Models.Simon;
using StroopApp.Services.Profile;
using System.Collections.ObjectModel;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public partial class SimonProfileEditorViewModel : ProfileEditorViewModelBase
    {

        public List<EnumOption<SimonStimulusPositionMode>> StimulusPositionModes { get; } =
            [
            new () { Value= SimonStimulusPositionMode.LeftRight, DisplayName= Strings.SimonStimulusPositionMode_LeftRight},
            new () { Value= SimonStimulusPositionMode.Center, DisplayName= Strings.SimonStimulusPositionMode_Center},
            ];

        [ObservableProperty]
        private EnumOption<SimonStimulusPositionMode> _selectedStimulusPositionMode;

        protected override void InitializeTaskSpecificFromProfile()
        {
            var profile = (SimonProfile)Profile;
            SelectedStimulusPositionMode = StimulusPositionModes.First(o => o.Value == profile.StimulusPositionMode);
        }

        protected override void SyncTaskSpecificToProfile()
        {
            ((SimonProfile)Profile).StimulusPositionMode = SelectedStimulusPositionMode.Value;
        }
        public SimonProfileEditorViewModel(SimonProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService) : base(profile, profiles, profileService)
        { }
    }   
}

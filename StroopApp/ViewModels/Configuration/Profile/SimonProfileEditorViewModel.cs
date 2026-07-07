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
        public List<EnumOption<SimonAnswerMode>> AnswerModes { get; } =
            [
            new () { Value= SimonAnswerMode.GoNoGo, DisplayName= Strings.SimonAnswerMode_GoNoGo},
            new () { Value= SimonAnswerMode.LeftRight, DisplayName= Strings.SimonAnswerMode_LeftRight},
            ];

        [ObservableProperty]
        private EnumOption<SimonStimulusPositionMode> _selectedStimulusPositionMode;

        [ObservableProperty]
        private EnumOption<SimonAnswerMode> _selectedAnswerMode;

        protected override void InitializeTaskSpecificFromProfile()
        {
            var profile = (SimonProfile)Profile;
            SelectedStimulusPositionMode = StimulusPositionModes.First(positionMode => positionMode.Value == profile.StimulusPositionMode);
            SelectedAnswerMode = AnswerModes.First(answer => answer.Value == profile.AnswerMode);
        }

        protected override void SyncTaskSpecificToProfile()
        {
            ((SimonProfile)Profile).StimulusPositionMode = SelectedStimulusPositionMode.Value;
            ((SimonProfile)Profile).AnswerMode = SelectedAnswerMode.Value;
        }
        public SimonProfileEditorViewModel(SimonProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService) : base(profile, profiles, profileService)
        { }
    }   
}

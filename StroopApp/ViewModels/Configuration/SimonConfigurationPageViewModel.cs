using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Resources;
using StroopApp.Services.Navigation;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.ViewModels.State;
using StroopApp.Views;

namespace StroopApp.ViewModels.Configuration
{
    public partial class SimonConfigurationPageViewModel : ViewModelBase
    {

        public ProfileManagementViewModel ProfileViewModel { get; }
        public ParticipantManagementViewModel ParticipantViewModel { get; }
        public SimonResponseMappingViewModel SimonKeyMappingViewModel { get;  }
        public ExportFolderSelectorViewModel ExportFolderSelectorViewModel { get; }


        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        private readonly ITrialGenerationService _trialGenerationService;
        private readonly ExperimentSettingsViewModel _settings;


        public SimonConfigurationPageViewModel(
            ExperimentSettingsViewModel settings,
            ProfileManagementViewModel profileViewModel,
            ParticipantManagementViewModel participantViewModel,
            SimonResponseMappingViewModel simonKeyMappingViewModel,
            ExportFolderSelectorViewModel exportFolderSelectorViewModel,
            INavigationService experimenterNavigationService,
            IWindowManager windowManager,
            ITrialGenerationService trialGenerationService
            )
        {
            _settings = settings;
            ProfileViewModel = profileViewModel;
            ParticipantViewModel = participantViewModel;
            SimonKeyMappingViewModel = simonKeyMappingViewModel;
            ExportFolderSelectorViewModel = exportFolderSelectorViewModel;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _trialGenerationService = trialGenerationService;
        }

        [RelayCommand]
        private async Task LaunchExperimentAsync()
        {
            try
            {
                string? error = ValidateRequiredSelection() ;
                if(error != null)
                {
                    await ShowErrorDialogAsync(error);
                    return;
                }

                _settings.CurrentProfile = ProfileViewModel.CurrentProfile;
                _settings.Participant = ParticipantViewModel.SelectedParticipant;
                _settings.KeyMappings.Simon = SimonKeyMappingViewModel.Mappings;


                PrepareContext();
                PrepareTrials();
                ShowExperimentWindow();
            }
            catch(Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }
        
        private string? ValidateRequiredSelection()
        {
            if (ProfileViewModel.CurrentProfile == null) return Strings.Error_SelectProfile;
            if (ParticipantViewModel.SelectedParticipant == null) return Strings.Error_SelectParticipant;
            return null;
        }

        private void PrepareContext()
        {
            _settings.ExperimentContext.IsTaskStopped = false;
            _settings.ExperimentContext.IsBlockFinished = false;
            _settings.ExperimentContext.NewColumnSerie();
            _settings.ExperimentContext.AddNewSerie(_settings);
        }

        private void PrepareTrials()
        {
            if (_settings.ExperimentContext.CurrentBlock is null)
                throw new InvalidOperationException("CurrentBlock was not initialized after AddNewSerie");

            var trials = _trialGenerationService.GenerateTrials(_settings);
            foreach (var trial in trials)
                _settings.ExperimentContext.CurrentBlock.TrialRecords.Add(trial);
           
        }

        private void ShowExperimentWindow()
        {
            _experimenterNavigationService.NavigateTo<ExperimentDashBoardPage>();
            _windowManager.ShowSimonParticipantWindow(_settings);
        }

    }
}

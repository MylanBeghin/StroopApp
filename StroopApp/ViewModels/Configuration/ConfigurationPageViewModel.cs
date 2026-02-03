using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration
{
    /// <summary>
    /// ViewModel for the main configuration page, coordinating profile, participant, and key mapping selection.
    /// Handles experiment launch with validation and initialization of trial sequences.
    /// </summary>
    public class ConfigurationPageViewModel : ViewModelBase
    {
        private readonly ProfileManagementViewModel _profileViewModel;
        private readonly ParticipantManagementViewModel _participantViewModel;
        private readonly KeyMappingViewModel _keyMappingViewModel;
        private readonly INavigationService _experimenterNavigationService;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;
        private readonly ITrialGenerationService _trialGenerationService;

        private ExperimentSettings _settings;
        public ICommand LaunchExperimentCommand { get; }

        public ConfigurationPageViewModel(ExperimentSettings settings,
                                  ProfileManagementViewModel profileViewModel,
                                  ParticipantManagementViewModel participantViewModel,
                                  KeyMappingViewModel keyMappingViewModel,
                                  INavigationService experimenterNavigationService,
                                  IWindowManager windowManager,
                                  ITrialGenerationService trialGenerationService,
                                  ILanguageService languageService)
        {
            _profileViewModel = profileViewModel;
            _participantViewModel = participantViewModel;
            _keyMappingViewModel = keyMappingViewModel;
            _experimenterNavigationService = experimenterNavigationService;
            _windowManager = windowManager;
            _trialGenerationService = trialGenerationService;
            _languageService = languageService;
            _settings = settings;
            LaunchExperimentCommand = new RelayCommand(async _ => await LaunchExperimentAsync());
        }

        private async Task LaunchExperimentAsync()
        {
            try
            {
                _settings.CurrentProfile = _profileViewModel.CurrentProfile;
                _settings.Participant = _participantViewModel.SelectedParticipant;
                _settings.KeyMappings = _keyMappingViewModel.Mappings;

                if (_settings.CurrentProfile == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectProfile);
                    return;
                }

                if (_settings.Participant == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectParticipant);
                    return;
                }

                _settings.ExperimentContext.IsTaskStopped = false;
                _settings.ExperimentContext.IsBlockFinished = false;
                _settings.ExperimentContext.NewColumnSerie();
                _settings.ExperimentContext.AddNewSerie(_settings);

                if (_settings.ExperimentContext.CurrentBlock is null)
                    throw new InvalidOperationException("CurrentBlock was not initialized after AddNewSerie");

                var trials = _trialGenerationService.GenerateTrials(_settings);

                foreach (var trial in trials)
                {
                    _settings.ExperimentContext.CurrentBlock.TrialRecords.Add(trial);
                }

                _experimenterNavigationService.NavigateTo(() =>
                    new ExperimentDashBoardPage(_settings, _experimenterNavigationService, _windowManager, _languageService));
                _windowManager.ShowParticipantWindow(_settings);
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }
    }
}

using System.IO;
using System.Windows.Controls;

using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.KeyMapping;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Participant;
using StroopApp.Services.Profile;
using StroopApp.Services.Trial;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views.Configuration;
using StroopApp.Views.KeyMapping;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;

namespace StroopApp.Views
{
	public partial class ConfigurationPage : Page
	{
		public ConfigurationPage(ExperimentSettings settings, INavigationService experimentNavigationService, IWindowManager windowManager, ILanguageService languageService)
		{
			InitializeComponent();
			var configDir = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"StroopApp");
			// Instanciation des services
			var profileService = new ProfileService(configDir);
			var exportFolderStorageService = new ExportationService(settings, languageService, configDir);
			var participantService = new ParticipantService(configDir, settings);
			var keyMappingService = new KeyMappingService(configDir);
			var trialGenerationService = new TrialGenerationService(languageService);

			// Instanciation unique des ViewModels
			var profileViewModel = new ProfileManagementViewModel(profileService);
			var participantViewModel = new ParticipantManagementViewModel(participantService, settings.ExperimentContext.IsParticipantSelectionEnabled);
			var keyMappingViewModel = new KeyMappingViewModel(keyMappingService);
			var exportFolderSelectorViewModel = new ExportFolderSelectorViewModel(settings, exportFolderStorageService);

			// Instanciation du ViewModel principal et liaison au DataContext
			DataContext = new ConfigurationPageViewModel(settings, profileViewModel, participantViewModel, keyMappingViewModel, experimentNavigationService, windowManager, trialGenerationService, languageService);

			// Instanciation des vues en injectant les ViewModels partagés
			var profileManagementView = new ProfileManagementView(profileViewModel);
			var participantManagementView = new ParticipantManagementView(participantViewModel);
			var keyMappingView = new KeyMappingView(keyMappingViewModel);
			var exportFolderView = new ExportFolderSelectorView(exportFolderSelectorViewModel);

			// Ajout dynamique des vues dans le conteneur (ici MainGrid)
			// Supposons que MainGrid possède déjà les lignes prévues dans le XAML
			MainGrid.Children.Add(profileManagementView);
			Grid.SetRow(profileManagementView, 1);
			KeyMappingContainer.Children.Add(keyMappingView);
			Grid.SetColumn(keyMappingView, 0);
			KeyMappingContainer.Children.Add(exportFolderView);
			Grid.SetColumn(exportFolderView, 2);
			MainGrid.Children.Add(participantManagementView);
			Grid.SetRow(participantManagementView, 5);
		}
	}
}


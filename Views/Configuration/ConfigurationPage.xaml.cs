using StroopApp.Services.KeyMapping;
using StroopApp.Services.Navigation;
using StroopApp.Services.Participant;
using StroopApp.Services.Profile;
using StroopApp.ViewModels.Configuration;
using StroopApp.ViewModels.Configuration.Participant;
using StroopApp.ViewModels.Configuration.Profile;
using StroopApp.Views.KeyMapping;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ConfigurationPage : Page
    {
        public ConfigurationPage(INavigationService navigationService)
        {
            InitializeComponent();

            // Instanciation des services
            var profileService = new ProfileService();
            var participantService = new ParticipantService();
            var keyMappingService = new KeyMappingService();

            // Instanciation unique des ViewModels
            var profileViewModel = new ProfileManagementViewModel(profileService);
            var participantViewModel = new ParticipantManagementViewModel(participantService);
            var keyMappingViewModel = new KeyMappingViewModel(keyMappingService);

            // Instanciation du ViewModel principal et liaison au DataContext
            DataContext = new ConfigurationPageViewModel(profileViewModel, participantViewModel, keyMappingViewModel, navigationService);

            // Instanciation des vues en injectant les ViewModels partagés
            var profileManagementView = new ProfileManagementView(profileViewModel);
            var participantManagementView = new ParticipantManagementView(participantViewModel);
            var keyMappingView = new KeyMappingView(keyMappingViewModel);

            // Ajout dynamique des vues dans le conteneur (ici MainGrid)
            // Supposons que MainGrid possède déjà les lignes prévues dans le XAML
            MainGrid.Children.Add(profileManagementView);
            Grid.SetRow(profileManagementView, 1);

            MainGrid.Children.Add(keyMappingView);
            Grid.SetRow(keyMappingView, 3);

            MainGrid.Children.Add(participantManagementView);
            Grid.SetRow(participantManagementView, 5);
        }
    }
}


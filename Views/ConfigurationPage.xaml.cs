using System.Windows.Controls;
using StroopApp.Views.Participant;
using StroopApp.Views.Profile;
using StroopApp.Views.KeyMapping;
using StroopApp.ViewModels;

namespace StroopApp.Views
{
    public partial class ConfigurationPage : Page
    {
        public ProfileManagementView profileManagementView;
        public KeyMappingView keyMappingView;
        public ParticipantManagementView participantManagementView;
        public ConfigurationPageViewModel ConfigurationPageViewModel;

        public ConfigurationPage()
        {
            InitializeComponent();
            profileManagementView = new ProfileManagementView();
            keyMappingView = new KeyMappingView();
            participantManagementView = new ParticipantManagementView();
            DataContext = ConfigurationPageViewModel = new ConfigurationPageViewModel(profileManagementView, participantManagementView, keyMappingView);
        }
    }
}

using StroopApp.Views.Profile;
using StroopApp.Views.Participant;
namespace StroopApp.Views
{
    public partial class ConfigurationPage : System.Windows.Controls.Page
    {
        public ProfileManagementView profileManagementView;
        public ParticipantManagementView participantManagementView;
        public ConfigurationPage()
        {
            InitializeComponent();
            profileManagementView = new ProfileManagementView();
            participantManagementView = new ParticipantManagementView();
        }
    }
}

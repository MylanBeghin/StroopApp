using ModernWpf.Controls; // Assurez-vous que ModernWpf est référencé
using StroopApp.Views.Profile;
namespace StroopApp.Views
{
    public partial class ConfigurationPage : System.Windows.Controls.Page
    {
        public ProfileManagementView profileManagementView;
        public ParticipantManagementPage participantManagementPage;
        public ConfigurationPage()
        {
            InitializeComponent();
            profileManagementView = new ProfileManagementView();
        }
    }
}

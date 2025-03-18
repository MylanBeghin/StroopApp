using System.Windows.Controls;
using StroopApp.Services.Participant;
using StroopApp.ViewModels;

namespace StroopApp.Views.Participant
{
    public partial class ParticipantManagementView : UserControl
    {
        public ParticipantManagementView()
        {
            InitializeComponent();
            var participantService = new ParticipantService();
            DataContext = new ParticipantManagementViewModel(participantService);
        }
    }
}

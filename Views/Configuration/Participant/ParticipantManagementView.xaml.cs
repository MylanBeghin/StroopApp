using System.Windows.Controls;
using StroopApp.Services.Participant;
using StroopApp.ViewModels.Configuration.Participant;

namespace StroopApp.Views.Participant
{
    public partial class ParticipantManagementView : UserControl
    {
        public ParticipantManagementView(ParticipantManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}

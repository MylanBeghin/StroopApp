using StroopApp.ViewModels.Configuration.Participant;
using System.Windows.Controls;

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

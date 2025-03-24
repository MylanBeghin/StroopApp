using System.Windows.Controls;
using StroopApp.ViewModels;
using StroopApp.Services.Participant;

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

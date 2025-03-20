using StroopApp.Models;
using StroopApp.Services.Participant;
using System.Collections.ObjectModel;
using System.Windows;
using StroopApp.ViewModels;

namespace StroopApp.Views.Participant
{
    public partial class ParticipantEditorWindow : Window
    {
        public ParticipantEditorWindow(ParticipantModel Participant, ObservableCollection<ParticipantModel> Participants, IParticipantService participantService)
        {
            InitializeComponent();
            var viewmodel = new ParticipantEditorViewModel(Participant, Participants, participantService);
            DataContext = viewmodel;
            viewmodel.CloseAction = () =>
            {
                DialogResult = viewmodel.DialogResult;
                Close();
            };
        }

    }
}

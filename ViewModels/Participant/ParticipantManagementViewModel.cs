using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Participant;
using StroopApp.Views.Participant;

namespace StroopApp.ViewModels
{
    public class ParticipantManagementViewModel : INotifyPropertyChanged
    {
        private readonly IParticipantService _IparticipantService;
        public ObservableCollection<ParticipantModel> Participants { get; set; }

        private ParticipantModel _selectedParticipant;
        public ParticipantModel SelectedParticipant
        {
            get => _selectedParticipant;
            set { _selectedParticipant = value; OnPropertyChanged(); }
        }

        public ICommand CreateParticipantCommand { get; }
        public ICommand ModifyParticipantCommand { get; }
        public ICommand DeleteParticipantCommand { get; }

        public ParticipantManagementViewModel(IParticipantService participantService)
        {
            _IparticipantService = participantService;
            Participants = _IparticipantService.LoadParticipants();
            if (Participants.Any())
                SelectedParticipant = Participants.First();

            CreateParticipantCommand = new RelayCommand(CreateParticipant);
            ModifyParticipantCommand = new RelayCommand(ModifyParticipant);
            DeleteParticipantCommand = new RelayCommand(DeleteParticipant);
        }

        private void CreateParticipant()
        {
            // Ici, vous pouvez ouvrir une fenêtre d'édition de participant. Pour l'instant, on crée un participant vierge.
            var newParticipant = new ParticipantModel
            {
                Id = _IparticipantService.GetNextParticipantId()
            };
            var participantWindow = new ParticipantEditorWindow(newParticipant, Participants, _IparticipantService);
            participantWindow.ShowDialog();
            if(participantWindow.DialogResult == true)
            {
                Participants.Add(newParticipant);
                _IparticipantService.SaveParticipants(Participants);
                SelectedParticipant = newParticipant;
            }
        }

        private void ModifyParticipant()
        {
            if (SelectedParticipant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant à modifier !");
                return;
            }
            var participantWindow = new ParticipantEditorWindow(SelectedParticipant, Participants, _IparticipantService);
            participantWindow.ShowDialog();
            if (participantWindow.DialogResult == true)
            {
                Participants.Add(SelectedParticipant);
                _IparticipantService.SaveParticipants(Participants);
                SelectedParticipant = SelectedParticipant;
            }
        }

        private void DeleteParticipant()
        {
            if (SelectedParticipant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant à supprimer !");
                return;
            }
                
            _IparticipantService.DeleteParticipant(SelectedParticipant, Participants);
            if (Participants.Any())
                SelectedParticipant = Participants.First();
            else
                SelectedParticipant = null;
        }
        private async void ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Erreur",
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

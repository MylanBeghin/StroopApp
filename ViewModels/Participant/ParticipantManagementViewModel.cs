using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.ViewModels
{
    public class ParticipantManagementViewModel : INotifyPropertyChanged
    {
        private readonly IParticipantService _participantService;
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
        public ICommand SelectParticipantCommand { get; }

        public ParticipantManagementViewModel(IParticipantService participantService)
        {
            _participantService = participantService;
            Participants = _participantService.LoadParticipants();
            if (Participants.Any())
                SelectedParticipant = Participants.First();

            CreateParticipantCommand = new RelayCommand(CreateParticipant);
            ModifyParticipantCommand = new RelayCommand(ModifyParticipant);
            DeleteParticipantCommand = new RelayCommand(DeleteParticipant);
            SelectParticipantCommand = new RelayCommand(SelectParticipant);
        }

        private void CreateParticipant()
        {
            // Ici, vous pouvez ouvrir une fenêtre d'édition de participant. Pour l'instant, on crée un participant vierge.
            var newParticipant = new ParticipantModel
            {
                Id = _participantService.GetNextParticipantId()
            };

            // Optionnel : ouvrir une fenêtre d'édition pour saisir les détails.
            Participants.Add(newParticipant);
            _participantService.SaveParticipants(Participants);
            SelectedParticipant = newParticipant;
        }

        private void ModifyParticipant()
        {
            if (SelectedParticipant == null)
                return;

            // Ouvrir une fenêtre d'édition (ParticipantEditorWindow) pour modifier le participant.
            // Par exemple :
            // var editor = new ParticipantEditorWindow(SelectedParticipant);
            // if (editor.ShowDialog() == true) { ... }

            // Pour l'instant, on suppose que les modifications sont effectuées et on enregistre.
            _participantService.SaveParticipants(Participants);
            OnPropertyChanged(nameof(SelectedParticipant));
        }

        private void DeleteParticipant()
        {
            if (SelectedParticipant == null)
                return;
            _participantService.DeleteParticipant(SelectedParticipant, Participants);
            if (Participants.Any())
                SelectedParticipant = Participants.First();
            else
                SelectedParticipant = null;
        }

        private void SelectParticipant()
        {
            if (SelectedParticipant == null)
                return;

            // Lors du lancement de l'expérience, créer un dossier dans "Résultats" avec l'ID du participant.
            string resultsDirectory = "Résultats";
            if (!Directory.Exists(resultsDirectory))
            {
                Directory.CreateDirectory(resultsDirectory);
            }

            string participantFolder = Path.Combine(resultsDirectory, SelectedParticipant.Id.ToString());
            if (!Directory.Exists(participantFolder))
            {
                Directory.CreateDirectory(participantFolder);
            }

            // Vous pouvez maintenant lancer l'expérience avec SelectedParticipant et enregistrer les résultats dans participantFolder.
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

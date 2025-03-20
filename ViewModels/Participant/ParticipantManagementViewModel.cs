using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
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

        // ICollectionView pour le filtrage
        public ICollectionView ParticipantsView { get; set; }

        private ParticipantModel _selectedParticipant;
        public ParticipantModel SelectedParticipant
        {
            get => _selectedParticipant;
            set { _selectedParticipant = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ParticipantsView.Refresh();
            }
        }

        public ICommand CreateParticipantCommand { get; }
        public ICommand ModifyParticipantCommand { get; }
        public ICommand DeleteParticipantCommand { get; }
        public ICommand OpenResultsCommand { get; }

        public ParticipantManagementViewModel(IParticipantService participantService)
        {
            _participantService = participantService;
            Participants = _participantService.LoadParticipants();
            if (Participants.Any())
                SelectedParticipant = Participants.First();

            // Création de la vue pour le filtrage
            ParticipantsView = CollectionViewSource.GetDefaultView(Participants);
            ParticipantsView.Filter = FilterParticipants;

            CreateParticipantCommand = new RelayCommand(CreateParticipant);
            ModifyParticipantCommand = new RelayCommand(ModifyParticipant);
            DeleteParticipantCommand = new RelayCommand(DeleteParticipant);
            OpenResultsCommand = new RelayCommand<ParticipantModel>(OpenResults);
        }

        private bool FilterParticipants(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            var participant = obj as ParticipantModel;
            // Vous pouvez personnaliser le filtre selon vos besoins
            return participant.Id.ToString().Contains(SearchText)
                || (participant.Age?.ToString().Contains(SearchText) ?? false)
                || (participant.Height?.ToString().Contains(SearchText) ?? false)
                || (participant.Weight?.ToString().Contains(SearchText) ?? false)
                || participant.SexAssigned.ToString().Contains(SearchText)
                || participant.Gender.ToString().Contains(SearchText);
        }

        private void CreateParticipant()
        {
            var newParticipant = new ParticipantModel
            {
                Id = _participantService.GetNextParticipantId()
            };
            var participantWindow = new Views.Participant.ParticipantEditorWindow(newParticipant, Participants, _participantService);
            participantWindow.ShowDialog();
            if (participantWindow.DialogResult == true)
            {
                Participants.Add(newParticipant);
                _participantService.SaveParticipants(Participants);
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
            var participantWindow = new Views.Participant.ParticipantEditorWindow(SelectedParticipant, Participants, _participantService);
            participantWindow.ShowDialog();
            if (participantWindow.DialogResult == true)
            {
                OnPropertyChanged(nameof(SelectedParticipant));
                _participantService.SaveParticipants(Participants);
            }
        }

        private void DeleteParticipant()
        {
            if (SelectedParticipant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant à supprimer !");
                return;
            }
            _participantService.DeleteParticipant(SelectedParticipant, Participants);
            if (Participants.Any())
                SelectedParticipant = Participants.First();
            else
                SelectedParticipant = null;
        }

        private void OpenResults(ParticipantModel participant)
        {
            if (participant == null)
                return;
            string resultsFolder = Path.Combine("Résultats", participant.Id.ToString());
            if (!Directory.Exists(resultsFolder))
            {
                Directory.CreateDirectory(resultsFolder);
            }
            System.Diagnostics.Process.Start("explorer.exe", resultsFolder);
        }

        private async void ShowErrorDialog(string message)
        {
            var dialog = new ModernWpf.Controls.ContentDialog
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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Participant;
using StroopApp.Views.Participant;

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
            // Instanciation du ViewModel éditeur
            var viewModel = new ParticipantEditorViewModel(newParticipant, Participants, _participantService);
            var participantWindow = new ParticipantEditorWindow(viewModel);
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
            // Instanciation du ViewModel éditeur avec le participant sélectionné
            var viewModel = new ParticipantEditorViewModel(SelectedParticipant, Participants, _participantService);
            var participantWindow = new ParticipantEditorWindow(viewModel);
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
            SelectedParticipant = Participants.FirstOrDefault();
        }

        private void OpenResults(ParticipantModel participant)
        {
            if (participant == null)
                return;

            string resultsFolder = System.IO.Path.Combine("Résultats", participant.Id.ToString());
            if (!System.IO.Directory.Exists(resultsFolder))
            {
                System.IO.Directory.CreateDirectory(resultsFolder);
            }
            System.Diagnostics.Process.Start("explorer.exe", resultsFolder);
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

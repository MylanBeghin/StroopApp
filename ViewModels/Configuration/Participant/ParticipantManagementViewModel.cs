using System.Windows.Input;
using System.Windows.Data;
using System.Collections.ObjectModel;
using ModernWpf.Controls;
using StroopApp.Core;
using StroopApp.Services.Participant;
using StroopApp.Views.Participant;
using System.ComponentModel;
using System.IO;

namespace StroopApp.ViewModels.Configuration.Participant
{
    public class ParticipantManagementViewModel : ViewModelBase
    {
        readonly IParticipantService _participantService;
        public ObservableCollection<Models.Participant> Participants { get; set; }
        public ICollectionView ParticipantsView { get; set; }

        private Models.Participant _selectedParticipant;
        public Models.Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set { _selectedParticipant = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ParticipantsView.Refresh(); }
        }

        public ICommand CreateParticipantCommand { get; }
        public ICommand ModifyParticipantCommand { get; }
        public ICommand DeleteParticipantCommand { get; }
        public ICommand OpenResultsCommand { get; }

        public ParticipantManagementViewModel(IParticipantService participantService)
        {
            _participantService = participantService;
            Participants = _participantService.LoadParticipants();
            if (Participants.Any()) SelectedParticipant = Participants.First();

            ParticipantsView = CollectionViewSource.GetDefaultView(Participants);
            ParticipantsView.Filter = FilterParticipants;

            CreateParticipantCommand = new RelayCommand(CreateParticipant);
            ModifyParticipantCommand = new RelayCommand(ModifyParticipant);
            DeleteParticipantCommand = new RelayCommand(async () => await DeleteParticipant());
            OpenResultsCommand = new RelayCommand<Models.Participant>(OpenResults);
        }

        bool FilterParticipants(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            var p = obj as Models.Participant;
            return p.Id.ToString().Contains(SearchText)
                || p.Age?.ToString().Contains(SearchText) == true
                || p.Height?.ToString().Contains(SearchText) == true
                || p.Weight?.ToString().Contains(SearchText) == true
                || p.SexAssigned.ToString().Contains(SearchText)
                || p.Gender.ToString().Contains(SearchText);
        }

        void CreateParticipant()
        {
            var newP = new Models.Participant { Id = _participantService.GetNextParticipantId() };
            var vm = new ParticipantEditorViewModel(newP, Participants, _participantService);
            var win = new ParticipantEditorWindow(vm);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                Participants.Add(newP);
                _participantService.SaveParticipants(Participants);
                SelectedParticipant = newP;
            }
        }

        void ModifyParticipant()
        {
            if (SelectedParticipant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant à modifier !");
                return;
            }
            var vm = new ParticipantEditorViewModel(SelectedParticipant, Participants, _participantService);
            var win = new ParticipantEditorWindow(vm);
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                _participantService.SaveParticipants(Participants);
                OnPropertyChanged(nameof(SelectedParticipant));
            }
        }

        async System.Threading.Tasks.Task DeleteParticipant()
        {
            if (SelectedParticipant == null)
            {
                ShowErrorDialog("Veuillez sélectionner un participant à supprimer !");
                return;
            }

            var dlg = new ContentDialog
            {
                Title = "Confirmation de suppression",
                Content = "Voulez-vous vraiment supprimer ce participant ? Ses données seront archivées.",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler"
            };
            if (await dlg.ShowAsync() != ContentDialogResult.Primary) return;

            _participantService.DeleteParticipant(SelectedParticipant.Id);
            Participants.Remove(SelectedParticipant);
            SelectedParticipant = Participants.FirstOrDefault();
        }

        void OpenResults(Models.Participant p)
        {
            if (p == null) return;
            var folder = Path.Combine("Résultats", p.Id.ToString());
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            System.Diagnostics.Process.Start("explorer.exe", folder);
        }
    }
}

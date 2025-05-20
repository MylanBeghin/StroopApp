using ModernWpf.Controls;
using StroopApp.Core;
using StroopApp.Services.Participant;
using StroopApp.Views.Participant;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace StroopApp.ViewModels.Configuration.Participant
{
    public class ParticipantManagementViewModel : ViewModelBase
    {
        readonly IParticipantService _participantService;
        public ObservableCollection<Models.Participant> Participants
        {
            get; set;
        }
        public ICollectionView ParticipantsView
        {
            get; set;
        }

        private Models.Participant _selectedParticipant;
        public Models.Participant SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                _selectedParticipant = value;
                OnPropertyChanged();
            }
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
        private bool _isParticipantSelectionEnabled;

        public bool IsParticipantSelectionEnabled
        {
            get => _isParticipantSelectionEnabled;
            set
            {
                if (_isParticipantSelectionEnabled != value)
                {
                    _isParticipantSelectionEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand CreateParticipantCommand
        {
            get;
        }
        public ICommand ModifyParticipantCommand
        {
            get;
        }
        public ICommand DeleteParticipantCommand
        {
            get;
        }
        public ParticipantManagementViewModel(IParticipantService participantService, bool isParticipantSelectionEnabled)
        {
            IsParticipantSelectionEnabled = isParticipantSelectionEnabled;
            _participantService = participantService;
            Participants = _participantService.LoadParticipants();
            if (Participants.Any())
                SelectedParticipant = Participants.First();

            ParticipantsView = CollectionViewSource.GetDefaultView(Participants);
            ParticipantsView.Filter = FilterParticipants;

            CreateParticipantCommand = new RelayCommand(CreateParticipant);
            ModifyParticipantCommand = new RelayCommand(ModifyParticipant);
            DeleteParticipantCommand = new RelayCommand(async () => await DeleteParticipant());
        }

        bool FilterParticipants(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;
            var p = obj as Models.Participant;
            return p.Id.ToString().Contains(SearchText);
        }

        void CreateParticipant()
        {
            var newP = new Models.Participant { Id = _participantService.GetNextParticipantId() };
            var vm = new ParticipantEditorViewModel(newP, Participants, _participantService);
            var win = new ParticipantEditorWindow(vm);
            win.ShowDialog();
            if (win.DialogResult == true)
            {

                _participantService.AddParticipant(Participants, newP);
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
            var viewModel = new ParticipantEditorViewModel(SelectedParticipant, Participants, _participantService);
            var participantWindow = new ParticipantEditorWindow(viewModel);
            participantWindow.ShowDialog();
            if (participantWindow.DialogResult == true)
            {
                _participantService.UpdateParticipantById(
                    SelectedParticipant.Id,
                    SelectedParticipant,
                    Participants);
                OnPropertyChanged(nameof(SelectedParticipant));
            }
        }

        async Task DeleteParticipant()
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
            if (await dlg.ShowAsync() != ContentDialogResult.Primary)
                return;

            _participantService.DeleteParticipant(Participants, SelectedParticipant.Id);
            SelectedParticipant = Participants.FirstOrDefault();
        }
    }
}

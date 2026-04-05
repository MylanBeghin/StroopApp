using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StroopApp.Core;
using StroopApp.Resources;
using StroopApp.Services.Participant;
using StroopApp.Views.Participant;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ParticipantModel = StroopApp.Models.Participant;

namespace StroopApp.ViewModels.Configuration.Participant
{
    /// <summary>
    /// ViewModel for managing participants list with search, create, edit, and delete operations.
    /// Uses IParticipantService for persistence and supports participant selection control.
    /// </summary>
    public partial class ParticipantManagementViewModel : ViewModelBase
    {
        private readonly IParticipantService _participantService;

        public ObservableCollection<ParticipantModel> Participants { get; }
        public ICollectionView ParticipantsView { get; }

        [ObservableProperty]
        private ParticipantModel? _selectedParticipant;

        [ObservableProperty]
        private string _searchText = string.Empty;

        partial void OnSearchTextChanged(string value)
        {
            ParticipantsView.Refresh();
        }

        [ObservableProperty]
        private bool _isParticipantSelectionEnabled;

        public ParticipantManagementViewModel(IParticipantService participantService, bool isParticipantSelectionEnabled)
        {
            _participantService = participantService;
            IsParticipantSelectionEnabled = isParticipantSelectionEnabled;

            Participants = _participantService.LoadParticipants();
            if (Participants.Any())
                SelectedParticipant = Participants.First();

            ParticipantsView = CollectionViewSource.GetDefaultView(Participants);
            ParticipantsView.Filter = FilterParticipants;
        }

        private bool FilterParticipants(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            if (obj is not ParticipantModel p || string.IsNullOrWhiteSpace(p.Id))
                return false;

            return p.Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        [RelayCommand]
        private async Task CreateParticipantAsync()
        {
            try
            {
                var newParticipant = new ParticipantModel();
                var vm = new ParticipantEditorViewModel(newParticipant, Participants);
                var win = new ParticipantEditorWindow(vm);
                win.ShowDialog();

                if (win.DialogResult == true)
                {
                    _participantService.AddParticipant(Participants, newParticipant);
                    SelectedParticipant = newParticipant;
                    ParticipantsView.Refresh();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task ModifyParticipantAsync()
        {
            try
            {
                if (SelectedParticipant == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectParticipantToModify);
                    return;
                }

                var viewModel = new ParticipantEditorViewModel(SelectedParticipant, Participants);
                var participantWindow = new ParticipantEditorWindow(viewModel);
                participantWindow.ShowDialog();

                if (viewModel.DialogResult == true && viewModel.OriginalParticipant != null)
                {
                    _participantService.UpdateParticipant(
                        viewModel.OriginalParticipant,
                        viewModel.Participant,
                        Participants);

                    ParticipantsView.Refresh();
                    OnPropertyChanged(nameof(SelectedParticipant));
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteParticipantAsync()
        {
            try
            {
                if (SelectedParticipant == null)
                {
                    await ShowErrorDialogAsync(Strings.Error_SelectParticipantToDelete);
                    return;
                }

                if (await ShowConfirmationDialogAsync(Strings.Title_DeleteConfirmation, Strings.Message_DeleteParticipantConfirmation))
                {
                    _participantService.DeleteParticipant(Participants, SelectedParticipant.Id);
                    SelectedParticipant = Participants.FirstOrDefault();
                    ParticipantsView.Refresh();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }
    }
}

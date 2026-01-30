using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Resources;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ParticipantModel = StroopApp.Models.Participant;

namespace StroopApp.ViewModels.Configuration.Participant
{
    /// <summary>
    /// ViewModel for creating or editing a participant in a modal dialog.
    /// Handles validation, cloning for safe editing, and dialog result communication.
    /// </summary>
    public class ParticipantEditorViewModel : ViewModelBase
    {
        private ParticipantModel _participant;
        public ParticipantModel Participant
        {
            get => _participant;
            set
            {
                if (_participant != value)
                {
                    _participant = value;
                    OnPropertyChanged();
                }
            }
        }
        public ParticipantModel? OriginalParticipant { get; }
        public bool IsEditing => OriginalParticipant == null;

        public ObservableCollection<ParticipantModel> Participants
        {
            get;
        }
        public IEnumerable<SexAssignedAtBirth> SexAssignedValues
        {
            get;
        }
        public IEnumerable<Gender> GenderValues
        {
            get;
        }
        public ICommand SaveCommand
        {
            get;
        }
        public ICommand CancelCommand
        {
            get;
        }
        public bool? DialogResult
        {
            get; private set;
        }
        public Action? CloseAction
        {
            get; set;
        }

        public ParticipantEditorViewModel(ParticipantModel participant, ObservableCollection<ParticipantModel> participants)
        {
            Participants = participants;

            var isExisting = Participants.Contains(participant);
            OriginalParticipant = isExisting ? participant : null;
            Participant = isExisting ? CloneParticipant(participant) : participant;

            SexAssignedValues = (SexAssignedAtBirth[])Enum.GetValues(typeof(SexAssignedAtBirth));
            GenderValues = (Gender[])Enum.GetValues(typeof(Gender));

            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        private ParticipantModel CloneParticipant(ParticipantModel p)
        {
            return new ParticipantModel
            {
                Id = p.Id,
                Age = p.Age,
                Weight = p.Weight,
                Height = p.Height,
                SexAssigned = p.SexAssigned,
                Gender = p.Gender
            };
        }

        private async Task SaveAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Participant.Id))
                {
                    await ShowErrorDialogAsync(Strings.Error_FillIdField);
                    return;
                }
                if (Participants.Any(p => p.Id == Participant.Id && p != OriginalParticipant))
                {
                    await ShowErrorDialogAsync(Strings.Error_ParticipantIdAlreadyUsed);
                    return;
                }
                DialogResult = true;
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"{Strings.Error_Title}: {ex.Message}");
            }
        }

        private void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }
    }
}

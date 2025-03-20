using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.ViewModels
{
    public class ParticipantEditorViewModel : INotifyPropertyChanged
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
        public IEnumerable<SexAssignedAtBirth> SexAssignedValues { get; }
        public IEnumerable<Gender> GenderValues { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IParticipantService _IparticipantService;
        public bool? DialogResult { get; private set; }
        public Action? CloseAction { get; set; }

        public ParticipantEditorViewModel(ParticipantModel participant, ObservableCollection<ParticipantModel> Participants, IParticipantService participantService)
        {
            Participant = participant;
            _IparticipantService = participantService;
            SexAssignedValues = (SexAssignedAtBirth[])Enum.GetValues(typeof(SexAssignedAtBirth));
            GenderValues = (Gender[])Enum.GetValues(typeof(Gender));

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        private async void Save()
        {
            if (!Participant.Age.HasValue ||
                !Participant.Weight.HasValue ||
                !Participant.Height.HasValue ||
                double.IsNaN(Participant.Weight.Value) ||
                double.IsInfinity(Participant.Weight.Value) ||
                double.IsNaN(Participant.Height.Value) ||
                double.IsInfinity(Participant.Height.Value))
            {
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez remplir correctement tous les champs obligatoires.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }
            DialogResult = true;
            CloseAction?.Invoke();
        }

        private void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

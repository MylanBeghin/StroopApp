using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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

        // Expose les valeurs possibles pour les énumérations
        public IEnumerable<SexAssignedAtBirth> SexAssignedValues { get; }
        public IEnumerable<Gender> GenderValues { get; }

        // Commandes pour Sauvegarder et Annuler
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

        // Méthode appelée lors de la sauvegarde
        private void Save()
        {
            DialogResult = true;
            CloseAction?.Invoke();
        }

        // Méthode appelée lors de l'annulation
        private void Cancel()
        {
            // Implémentez ici la logique d'annulation, par exemple, en fermant la fenêtre avec DialogResult = false.
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

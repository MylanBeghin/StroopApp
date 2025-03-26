// ParticipantEditorViewModel.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.ViewModels.Configuration.Participant
{
    public class ParticipantEditorViewModel : INotifyPropertyChanged
    {
        private ParticipantModel _participant;
        /// <summary>
        /// Participant utilisé pour l'édition dans la fenêtre.
        /// Pour une modification, il s'agit d'une copie du participant original.
        /// </summary>
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

        /// <summary>
        /// Référence au participant original (non cloné) en cas de modification.
        /// </summary>
        private ParticipantModel _originalParticipant;

        public ObservableCollection<ParticipantModel> Participants { get; }
        public IEnumerable<SexAssignedAtBirth> SexAssignedValues { get; }
        public IEnumerable<Gender> GenderValues { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IParticipantService _participantService;
        public bool? DialogResult { get; private set; }
        public Action? CloseAction { get; set; }

        public ParticipantEditorViewModel(ParticipantModel participant, ObservableCollection<ParticipantModel> participants, IParticipantService participantService)
        {
            _participantService = participantService;
            Participants = participants;

            // Si le participant existe déjà dans la collection, on considère qu'il s'agit d'une modification.
            // On clone alors le participant pour éviter la mise à jour immédiate des valeurs.
            if (Participants.Contains(participant))
            {
                _originalParticipant = participant;
                Participant = CloneParticipant(participant);
            }
            else
            {
                // Pour la création, on utilise directement l'objet.
                Participant = participant;
            }

            SexAssignedValues = (SexAssignedAtBirth[])Enum.GetValues(typeof(SexAssignedAtBirth));
            GenderValues = (Gender[])Enum.GetValues(typeof(Gender));

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
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
                Gender = p.Gender,
                Results = p.Results
            };
        }

        private async void Save()
        {
            // Validation des champs obligatoires
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
            // Vérification d'unicité de l'ID
            if (Participants.Any(p => p.Id == Participant.Id && p != (_originalParticipant ?? Participant)))
            {
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Cet identifiant est déjà utilisé pour un autre participant",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            // Pour une modification, on met à jour le participant original avec les nouvelles valeurs.
            if (_originalParticipant != null)
            {
                _originalParticipant.Id = Participant.Id;
                _originalParticipant.Age = Participant.Age;
                _originalParticipant.Weight = Participant.Weight;
                _originalParticipant.Height = Participant.Height;
                _originalParticipant.SexAssigned = Participant.SexAssigned;
                _originalParticipant.Gender = Participant.Gender;
                _originalParticipant.Results = Participant.Results;
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

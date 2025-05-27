using System.Collections.ObjectModel;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Participant;

namespace StroopApp.ViewModels.Configuration.Participant
{
	public class ParticipantEditorViewModel : ViewModelBase
	{
		private Models.Participant _participant;
		/// <summary>
		/// Participant utilisé pour l'édition dans la fenêtre.
		/// Pour une modification, il s'agit d'une copie du participant original.
		/// </summary>
		public Models.Participant Participant
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
		private Models.Participant ModifiedParticipant => Participant;

		public ObservableCollection<Models.Participant> Participants
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

		private readonly IParticipantService _participantService;
		public bool? DialogResult
		{
			get; private set;
		}
		public Action? CloseAction
		{
			get; set;
		}

		public ParticipantEditorViewModel(Models.Participant participant, ObservableCollection<Models.Participant> participants, IParticipantService participantService)
		{
			_participantService = participantService;
			Participants = participants;

			Participant = Participants.Contains(participant) ? CloneParticipant(participant) : participant;

			SexAssignedValues = (SexAssignedAtBirth[])Enum.GetValues(typeof(SexAssignedAtBirth));
			GenderValues = (Gender[])Enum.GetValues(typeof(Gender));

			SaveCommand = new RelayCommand(Save);
			CancelCommand = new RelayCommand(Cancel);
		}

		private Models.Participant CloneParticipant(Models.Participant p)
		{
			return new Models.Participant
			{
				Id = p.Id,
				Age = p.Age,
				Weight = p.Weight,
				Height = p.Height,
				SexAssigned = p.SexAssigned,
				Gender = p.Gender
			};
		}

		private void Save()
		{
			if (!Participant.Age.HasValue ||
				!Participant.Weight.HasValue ||
				!Participant.Height.HasValue ||
				double.IsNaN(Participant.Weight.Value) ||
				double.IsInfinity(Participant.Weight.Value) ||
				double.IsNaN(Participant.Height.Value) ||
				double.IsInfinity(Participant.Height.Value))
			{
				ShowErrorDialog("Veuillez remplir correctement tous les champs obligatoires.");
				return;
			}
			if (Participants.Any(p => p.Id == Participant.Id && p != Participant))
			{
				ShowErrorDialog("Cet identifiant est déjà utilisé pour un autre participant");
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
	}
}

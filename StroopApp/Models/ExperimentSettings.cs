using DocumentFormat.OpenXml.Wordprocessing;

using StroopApp.Core;
namespace StroopApp.Models
{
	/// <summary>
	/// Holds the current configuration state for an experiment, including the selected participant,
	/// preset profile, key mappings, shared context, and current block index.
	/// Refactored to delegate responsibilities to specialized context objects.
	/// </summary>

	public class ExperimentSettings : ModelBase
	{
		// Internal context objects (composition)
		private readonly ExperimentConfiguration _configuration;
		private readonly ParticipantContext _participantContext;
		private readonly ExperimentRunState _runState;

		// Legacy: private fields for backward compatibility with property change notifications
		private int block;

		public int Block
		{
			get => _runState.Block;
			set
			{
				if (value != _runState.Block)
				{
					_runState.Block = value;
					block = value; // Keep legacy field in sync
					OnPropertyChanged();
				}

			}
		}
		private Participant _participant;
		public Participant Participant
		{
			get => _participantContext.Participant;
			set
			{
				_participantContext.Participant = value;
				_participant = value; // Keep legacy field in sync
				OnPropertyChanged();
			}
		}

		private ExperimentProfile _currentProfile;
		public ExperimentProfile CurrentProfile
		{
			get => _configuration.Profile;
			set
			{
				_configuration.Profile = value;
				_currentProfile = value; // Keep legacy field in sync
				OnPropertyChanged();
			}
		}

		private KeyMappings _keyMappings;
		public KeyMappings KeyMappings
		{
			get => _configuration.KeyMappings;
			set
			{
				_configuration.KeyMappings = value;
				_keyMappings = value; // Keep legacy field in sync
				OnPropertyChanged();
			}
		}
		private SharedExperimentData _experimentContext;
		public SharedExperimentData ExperimentContext
		{
			get => _runState.ExperimentContext;
			set
			{
				_runState.ExperimentContext = value;
				_experimentContext = value; // Keep legacy field in sync
				OnPropertyChanged();
			}
		}
		private string _exportFolderPath;

		public string ExportFolderPath
		{
			get => _configuration.ExportFolderPath;
			set
			{
				if (_configuration.ExportFolderPath != value)
				{
					_configuration.ExportFolderPath = value;
					_exportFolderPath = value; // Keep legacy field in sync
					OnPropertyChanged(nameof(ExportFolderPath));
				}
			}
		}
		public void Reset()
		{
			// Preserve exact order of execution from characterization tests
			ExperimentContext.Reset();
			Block = 1;
			ExperimentContext.IsBlockFinished = true;
			ExperimentContext.IsParticipantSelectionEnabled = true;
			ExperimentContext.HasUnsavedExports = true;
			OnPropertyChanged(string.Empty);
		}
		public ExperimentSettings()
		{
			// Initialize internal contexts
			_configuration = new ExperimentConfiguration();
			_participantContext = new ParticipantContext();
			_runState = new ExperimentRunState();

			// Keep legacy fields in sync for backward compatibility
			_currentProfile = _configuration.Profile;
			_keyMappings = _configuration.KeyMappings;
			_experimentContext = _runState.ExperimentContext;
			_exportFolderPath = _configuration.ExportFolderPath;
			block = _runState.Block;
			_participant = _participantContext.Participant;
		}
	}
}

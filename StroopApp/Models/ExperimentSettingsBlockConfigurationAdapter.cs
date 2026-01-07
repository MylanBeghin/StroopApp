namespace StroopApp.Models
{
	/// <summary>
	/// Adapter that projects ExperimentSettings to IBlockConfiguration.
	/// This is a temporary bridge to decouple SharedExperimentData and Block from the god object.
	/// </summary>
	public class ExperimentSettingsBlockConfigurationAdapter : IBlockConfiguration
	{
		private readonly ExperimentSettings _settings;

		public ExperimentSettingsBlockConfigurationAdapter(ExperimentSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
			
			if (_settings.CurrentProfile == null)
				throw new ArgumentException("CurrentProfile cannot be null", nameof(settings));
		}

		public int Block => _settings.Block;

		public int WordCount => _settings.CurrentProfile.WordCount;

		public string ProfileName => _settings.CurrentProfile.ProfileName;

		public int CongruencePercent => _settings.CurrentProfile.CongruencePercent;

		public int? SwitchPercent => _settings.CurrentProfile.SwitchPercent;

		public bool IsAmorce => _settings.CurrentProfile.IsAmorce;
	}
}

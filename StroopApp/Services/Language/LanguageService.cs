using System.Globalization;
using System.IO;
using System.Text.Json;

namespace StroopApp.Services.Language
{
	public class LanguageService : ILanguageService
	{
		private const string ConfigFileName = "language.json";

		private readonly string _configPath;
		private AppConfig _config;

		public LanguageService()
		{
			_configPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"StroopApp",
				ConfigFileName);

			_config = LoadConfig();
			ApplyCulture(_config.Language);
		}

		public string CurrentLanguageCode => _config.Language;

		public void SetLanguage(string languageCode)
		{
			if (string.IsNullOrWhiteSpace(languageCode))
				return;

			ApplyCulture(languageCode);
			_config.Language = languageCode;
			SaveConfig();
		}

		private void ApplyCulture(string languageCode)
		{
			var culture = new CultureInfo(languageCode);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		private AppConfig LoadConfig()
		{
			if (!File.Exists(_configPath))
				return new AppConfig();

			try
			{
				var json = File.ReadAllText(_configPath);
				return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
			}
			catch
			{
				return new AppConfig();
			}
		}

		private void SaveConfig()
		{
			Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
			var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_configPath, json);
		}

		private class AppConfig
		{
			public string Language { get; set; } = "en";
		}
	}
}

using System.IO;
using System.Text.Json;

using StroopApp.Models;

namespace StroopApp.Services.KeyMapping
{
    /// <summary>
    /// Service for loading and saving key mapping configuration from/to JSON files.
    /// </summary>
    public class KeyMappingService : IKeyMappingService
	{
		private readonly string _configDir;
		private readonly string _keyMappingPath;

		public KeyMappingService(string configDir)
		{
			_configDir = configDir ?? throw new ArgumentNullException(nameof(configDir));
			_keyMappingPath = Path.Combine(_configDir, "keymappings.json");
		}
        /// <summary>
        /// Loads key mappings from configuration file, or returns default mappings if not found.
        /// </summary>
        public async Task<KeyMappings> LoadKeyMappings()
		{
			if (!File.Exists(_keyMappingPath))
				return new KeyMappings();

			var json = await File.ReadAllTextAsync(_keyMappingPath);
			return JsonSerializer.Deserialize<KeyMappings>(json)
				   ?? new KeyMappings();
		}
        /// <summary>
        /// Saves key mappings to configuration file.
        /// </summary>
        public async Task SaveKeyMappings(KeyMappings keyMappings)
		{
			Directory.CreateDirectory(_configDir);
			var json = JsonSerializer.Serialize(keyMappings, new JsonSerializerOptions { WriteIndented = true });
			await File.WriteAllTextAsync(_keyMappingPath, json);
		}
	}
}

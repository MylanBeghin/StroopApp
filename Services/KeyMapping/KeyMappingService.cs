using StroopApp.Models;
using System.IO;
using System.Text.Json;

namespace StroopApp.Services.KeyMapping
{
    public class KeyMappingService : IKeyMappingService
    {
        private readonly string _configDir;
        private readonly string _keyMappingPath;

        public KeyMappingService(string configDir)
        {
            _configDir = configDir;
            _keyMappingPath = Path.Combine(_configDir, "keymappings.json");
        }
        public async Task<KeyMappings> LoadKeyMappings()
        {
            if (!File.Exists(_keyMappingPath))
                return new KeyMappings();

            var json = await File.ReadAllTextAsync(_keyMappingPath);
            return JsonSerializer.Deserialize<KeyMappings>(json)
                   ?? new KeyMappings();
        }
        public async Task SaveKeyMappings(KeyMappings keyMappings)
        {
            Directory.CreateDirectory(_configDir);
            var json = JsonSerializer.Serialize(keyMappings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_keyMappingPath, json);
        }
    }
}

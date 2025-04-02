using StroopApp.Models;
using StroopApp.Services.KeyMapping;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StroopApp.Services.KeyMapping
{
    public class KeyMappingService : IKeyMappingService
    {
        private readonly string _filePath = "keymappings.json";

        public async Task<KeyMappings> LoadKeyMappingsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new KeyMappings();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<KeyMappings>(json);
        }

        public async Task SaveKeyMappingsAsync(KeyMappings mappings)
        {
            var json = JsonSerializer.Serialize(mappings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}

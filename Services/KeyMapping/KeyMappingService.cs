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
        private readonly JsonSerializerOptions _jsonOptions;

        public KeyMappingService()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task<KeyMappings> LoadKeyMappingsAsync()
        {
            if (!File.Exists(_filePath))
            {
                // Retourne le mapping par défaut si le fichier n'existe pas
                return new KeyMappings();
            }

            string json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<KeyMappings>(json, _jsonOptions);
        }

        public async Task SaveKeyMappingsAsync(KeyMappings mappings)
        {
            string json = JsonSerializer.Serialize(mappings, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}

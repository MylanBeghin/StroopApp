using StroopApp.Models;

namespace StroopApp.Services.KeyMapping
{
    public interface IKeyMappingService
    {
        Task<KeyMappings> LoadKeyMappings();
        Task SaveKeyMappings(KeyMappings mappings);
    }
}

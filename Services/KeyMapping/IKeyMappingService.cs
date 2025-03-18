using StroopApp.Models;
using System.Threading.Tasks;

namespace StroopApp.Services.KeyMapping
{
    public interface IKeyMappingService
    {
        Task<KeyMappings> LoadKeyMappingsAsync();
        Task SaveKeyMappingsAsync(KeyMappings mappings);
    }
}

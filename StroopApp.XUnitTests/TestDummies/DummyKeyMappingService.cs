using StroopApp.Models;
using StroopApp.Services.KeyMapping;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyKeyMappingService : IKeyMappingService
	{
		public Task<KeyMappings> LoadKeyMappings() => Task.FromResult(new KeyMappings());
		public Task SaveKeyMappings(KeyMappings mappings) => Task.CompletedTask;
	}
}

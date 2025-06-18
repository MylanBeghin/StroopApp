using System.ComponentModel;

using StroopApp.Services.Exportation;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyExportationService : IExportationService
	{
		public void OnProfileExportPathChanged(object sender, PropertyChangedEventArgs e)
		{
		}
		public string LoadExportFolderPath() => string.Empty;
		public void SaveExportFolderPath(string path)
		{
		}
		public Task<string> ExportDataAsync() => Task.FromResult("dummy.xlsx");
		public void Dispose()
		{
		}
	}
}

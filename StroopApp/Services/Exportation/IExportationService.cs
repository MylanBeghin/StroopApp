namespace StroopApp.Services.Exportation
{
    public interface IExportationService
    {
        void OnProfileExportPathChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);
        string LoadExportFolderPath();
        void SaveExportFolderPath(string path);
        Task<string> ExportDataAsync();
        void Dispose();
    }
}
namespace StroopApp.Services.Exportation
{
    /// <summary>
    /// Defines contract for experiment data exportation to Excel files.
    /// </summary>
    public interface IExportationService
    {
        /// <summary>
        /// Loads the export folder path from configuration.
        /// </summary>
        string LoadExportFolderPath();

        /// <summary>
        /// Saves the export folder path to configuration.
        /// </summary>
        void SaveExportFolderPath(string path);

        /// <summary>
        /// Exports all experiment data to an Excel file.
        /// </summary>
        /// <returns>Full path to the generated file.</returns>
        Task<string> ExportDataAsync();
    }
}
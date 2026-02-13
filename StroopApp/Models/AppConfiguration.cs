namespace StroopApp.Models
{
    /// <summary>
    /// Application-wide configuration settings.
    /// Registered as a singleton in the DI container.
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// The directory where application configuration files are stored.
        /// Typically %AppData%\StroopApp on Windows.
        /// </summary>
        public required string ConfigDirectory { get; init; }
    }
}

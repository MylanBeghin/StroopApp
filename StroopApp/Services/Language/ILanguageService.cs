namespace StroopApp.Services.Language
{
    /// <summary>
    /// Defines contract for language management and localized string retrieval.
    /// </summary>
    public interface ILanguageService
    {
        /// <summary>
        /// Gets the current language code (e.g., "en", "fr").
        /// </summary>
        string CurrentLanguageCode { get; }

        /// <summary>
        /// Retrieves a localized string for the given resource key.
        /// </summary>
        string GetLocalizedString(string resourceKey, string? cultureCode = null);

        /// <summary>
        /// Sets the application language.
        /// </summary>
        void SetLanguage(string languageCode);
    }
}

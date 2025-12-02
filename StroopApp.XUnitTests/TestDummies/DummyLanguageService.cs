using StroopApp.Services.Language;

namespace StroopApp.XUnitTests.TestDummies
{
    public class DummyLanguageService : ILanguageService
    {
        private string _currentLanguage = "en";

        public string CurrentLanguageCode => _currentLanguage;

        public void SetLanguage(string languageCode)
        {
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                _currentLanguage = languageCode;
            }
        }
        public string GetLocalizedString(string resourceKey, string? cultureCode = null)
        {
            // Pour les tests, on retourne simplement la clé de ressource
            // ou une valeur par défaut en anglais
            return resourceKey switch
            {
                "Word_RED" => "RED",
                "Word_BLUE" => "BLUE",
                "Word_GREEN" => "GREEN",
                "Word_YELLOW" => "YELLOW",
                _ => resourceKey // Par défaut, retourne la clé elle-même
            };
        }
    }
}
namespace StroopApp.Services.Language
{
	public interface ILanguageService
	{
		string CurrentLanguageCode { get; }
		public string GetLocalizedString(string resourceKey, string? cultureCode = null);
		void SetLanguage(string languageCode);
	}
}

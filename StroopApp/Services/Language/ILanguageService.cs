namespace StroopApp.Services.Language
{
	public interface ILanguageService
	{
		string CurrentLanguageCode { get; }
		void SetLanguage(string languageCode);
	}
}

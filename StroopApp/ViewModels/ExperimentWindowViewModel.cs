using System.Windows;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Views;

namespace StroopApp.ViewModels
{
	/// <summary>
	/// Root ViewModel for the main experiment window.
	/// Manages language selection and initial navigation to configuration page.
	/// </summary>
	public class ExperimentWindowViewModel : ViewModelBase
	{
		public bool IsEnglishSelected => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en";
		public bool IsFrenchSelected => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr";
		private readonly ILanguageService _languageService;
		public ICommand ChangeLanguageCommand
		{
			get;
		}
		public ExperimentWindowViewModel(INavigationService experimentNavigationService, ILanguageService languageService)
		{
			_languageService = languageService;
			experimentNavigationService.NavigateTo<ConfigurationPage>();
			ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
		}
		private void ChangeLanguage(string lang)
		{
			if (Application.Current.Resources["Loc"] is LocalizedStrings loc)
				loc.ChangeCulture(lang);

			_languageService.SetLanguage(lang);

			OnPropertyChanged(nameof(IsEnglishSelected));
			OnPropertyChanged(nameof(IsFrenchSelected));
		}
	}
}

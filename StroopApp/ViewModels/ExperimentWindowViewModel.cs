﻿using System.Windows;
using System.Windows.Input;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;

namespace StroopApp.ViewModels
{
	public class ExperimentWindowViewModel : ViewModelBase
	{
		public bool IsEnglishSelected => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en";
		public bool IsFrenchSelected => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr";

		public ICommand ChangeLanguageCommand
		{
			get;
		}
		public ExperimentWindowViewModel(ExperimentSettings settings, INavigationService experimentNavigationService, IWindowManager windowManager)
		{
			experimentNavigationService.NavigateTo(() => new ConfigurationPage(settings, experimentNavigationService, windowManager));
			ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
		}
		private void ChangeLanguage(string lang)
		{
			if (Application.Current.Resources["Loc"] is LocalizedStrings loc)
				loc.ChangeCulture(lang);

			OnPropertyChanged(nameof(IsEnglishSelected));
			OnPropertyChanged(nameof(IsFrenchSelected));
		}
	}
}

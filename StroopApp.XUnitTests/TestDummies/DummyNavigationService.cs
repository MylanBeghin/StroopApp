using System;
using System.Windows.Controls;

using StroopApp.Services.Navigation;

namespace StroopApp.XUnitTests.TestDummies
{
	public class DummyNavigationService : INavigationService
	{
		public bool Navigated;
		void INavigationService.NavigateTo<T>(object parameter)
		{
			Navigated = true;
		}
		void INavigationService.NavigateTo(Func<Page> pageFactory)
		{
			Navigated = true;
		}
	}
}

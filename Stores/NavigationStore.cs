using StroopApp.Core;

namespace StroopApp.Stores
{
    public class NavigationStore
    {
		private ViewModelBase _currentViewModel;

		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set => _currentViewModel = value;
		}

	}
}

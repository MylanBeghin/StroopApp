using StroopApp.Models;
using StroopApp.Services.Language;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.ViewModels.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter;
using StroopApp.Views.Experiment.Experimenter.Graphs;
using System.Windows.Controls;

namespace StroopApp.Views
{
    public partial class ExperimentDashBoardPage : Page, INavigationAware
    {
        private readonly ExperimentSettings _settings;
        private readonly IWindowManager _windowManager;
        private readonly ILanguageService _languageService;

        public INavigationService NavigationService
        {
            set => Initialize(value);
        }

        public ExperimentDashBoardPage(ExperimentSettings settings, IWindowManager windowManager, ILanguageService languageService)
        {
            InitializeComponent();
            _settings = settings;
            _windowManager = windowManager;
            _languageService = languageService;
        }

        private void Initialize(INavigationService navigationService)
        {
            var experimentProfileView = new ExperimentProgressView(_settings);
            var graphsView = new GraphsView(_settings);
            DataContext = new ExperimentDashBoardPageViewModel(_settings, navigationService, _windowManager, _languageService);
            MainGrid.Children.Add(experimentProfileView);
            Grid.SetRow(experimentProfileView, 1);
            MainGrid.Children.Add(graphsView);
            Grid.SetRow(graphsView, 3);
        }
    }
}

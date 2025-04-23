using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;

namespace StroopApp.ViewModels.Experiment.Participant
{
    public class InstructionsPageViewModel
    {
        private readonly ExperimentSettings _settings;
        private readonly INavigationService _navigationService;
        private int _currentPageIndex;
        private const int TotalPages = 3;
        private readonly string _stroopType;
        public UIElement CurrentInstruction { get; private set; }
        public ICommand NextCommand { get; }
        public event EventHandler InstructionChanged;
        public StroopPage StroopPage { get; set; }
        public InstructionsPageViewModel(ExperimentSettings settings, INavigationService navigationService)
        {
            _currentPageIndex = 0;
            _settings = settings;
            _navigationService = navigationService;
            _stroopType = _settings.CurrentProfile.StroopType;
            NextCommand = new RelayCommand(_ => NextPage());
            CurrentInstruction = GenerateInstructionPage(_currentPageIndex);

        }
        private void NextPage()
        {
            _currentPageIndex++;
            if (_currentPageIndex < TotalPages)
            {
                CurrentInstruction = GenerateInstructionPage(_currentPageIndex);
                InstructionChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                StroopPage = new StroopPage(_navigationService, _settings);
                _navigationService.NavigateTo(() => StroopPage);
            }
        }
        private UIElement GenerateInstructionPage(int index)
        {
            var tb = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.Black,
                Foreground = Brushes.White,
                FontSize = 36,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            if (_stroopType.Equals("Amorce", StringComparison.OrdinalIgnoreCase))
            {
                switch (index)
                {
                    case 0:
                        tb.Inlines.Add(new Run("Nous allons maintenant réaliser une première tâche mentale.\r\n\r\n"));
                        tb.Inlines.Add(new Run("A chaque essai, vous verrez apparaître une croix de fixation '+' au milieu de l'écran, suivie d'une amorce et d'un nom de couleur : 'ROUGE', 'VERT', 'BLEU', 'JAUNE'.\r\n\r\n"));
                        tb.Inlines.Add(new Run("Ces noms seront toujours écrits dans une couleur différente du sens du mot.\r\n\r\n"));
                        tb.Inlines.Add(new Run("Par exemple :\r\n\r\n"));
                        tb.Inlines.Add(new Run("ROUGE")
                        {
                            Foreground = Brushes.Blue,
                            FontSize = 78,
                        });
                        break;
                    case 1:
                        tb.Inlines.Add(new Run("L'amorce, que vous verrez avant le nom, sera soit un carré, soit un cercle.\r\n\r\n"));
                        tb.Inlines.Add(new Run("Si vous avez vu un carré, vous devrez lire le mot.\r\n"));
                        tb.Inlines.Add(new Run("Si vous avez vu un cercle, vous devrez donner la couleur de l'encre.\r\n\r\n"));
                        tb.Inlines.Add(new Run("Par exemple :\r\n"));
                        var sp = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(40)
                        };
                        var gridShape = new Grid
                        {
                            Width = 200,
                            Height = 200,
                            Margin = new Thickness(0, 0, 80, 0)
                        };
                        var ellipse = new System.Windows.Shapes.Ellipse
                        {
                            Width = 200,
                            Height = 200,
                            Stroke = Brushes.White,
                            StrokeThickness = 6
                        };
                        gridShape.Children.Add(ellipse);
                        var crossGrid = new Grid
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        crossGrid.Children.Add(new System.Windows.Shapes.Rectangle { Width = 50, Height = 6, Fill = Brushes.White });
                        crossGrid.Children.Add(new System.Windows.Shapes.Rectangle { Width = 6, Height = 50, Fill = Brushes.White });
                        gridShape.Children.Add(crossGrid);
                        sp.Children.Add(gridShape);
                        sp.Children.Add(new TextBlock
                        {
                            Text = "ROUGE",
                            Foreground = Brushes.Blue,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(10, 0, 0, 0),
                            FontSize = 78,
 
                        });
                        tb.Inlines.Add(new InlineUIContainer(sp));
                        break;
                    case 2:
                        tb.Inlines.Add(new Run("Avez-vous des questions avant de commencer ?"));
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0:
                        tb.Inlines.Add(new Run("Nous allons maintenant réaliser une première tâche mentale.\r\n\r\n"));
                        tb.Inlines.Add(new Run("A chaque essai, vous verrez apparaître une croix de fixation '+' au milieu de l'écran, suivie d'un nom de couleur : 'ROUGE', 'VERT', 'BLEU', 'JAUNE'.\r\n\r\n"));
                        tb.Inlines.Add(new Run(_stroopType.Equals("Congruent", StringComparison.OrdinalIgnoreCase)
                            ? "Ces noms seront toujours écrits dans la même couleur que le sens du mot.\r\n\r\n"
                            : "Ces noms seront toujours écrits dans une couleur différente du sens du mot.\r\n\r\n"));
                        tb.Inlines.Add(new Run("Par exemple :\r\n\r\n"));
                        tb.Inlines.Add(new Run("ROUGE")
                        {
                            Foreground = _stroopType.Equals("Congruent", StringComparison.OrdinalIgnoreCase) ? Brushes.Red : Brushes.Blue,
                            FontSize = 78,
                        });
                        break;
                    case 1:
                        tb.Inlines.Add(new Run("Vous devrez appuyer sur le bouton correspondant à la couleur du mot le plus rapidement possible."));
                        break;
                    case 2:
                        tb.Inlines.Add(new Run("Avez-vous des questions avant de commencer ?"));
                        break;
                }
            }
            return tb;
        }
    }
}

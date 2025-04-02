using StroopApp.Models;
using StroopApp.Services.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using StroopApp.Views.Experiment.Participant;   

namespace StroopApp.Views
{
    public partial class InstructionsPage : Page
    {
        private readonly ExperimentSettings _settings;
        private readonly string _stroopType;
        private int _currentPageIndex = 0;
        private const int TotalPages = 3;
        private readonly INavigationService _navigationService;
        public InstructionsPage(ExperimentSettings settings, INavigationService navigationService)
        {
            InitializeComponent();
            _settings = settings;
            _stroopType = settings.CurrentProfile.StroopType;
            _navigationService = navigationService;
            ShowPage(_currentPageIndex);
            Loaded += (s, e) => Focus();
        }

        private void ShowPage(int index)
        {
            InstructionTextBlock.Inlines.Clear();
            if (_stroopType.Equals("Amorce", StringComparison.OrdinalIgnoreCase))
            {
                switch (index)
                {
                    case 0:
                        InstructionTextBlock.Inlines.Add(new Run("Nous allons maintenant réaliser une première tâche mentale.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("A chaque essai, vous verrez apparaître une croix de fixation '+' au milieu de l'écran, suivie d'une amorce et d'un nom de couleur : 'ROUGE', 'VERT', 'BLEU', 'JAUNE'.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Ces noms seront toujours écrits dans une couleur différente du sens du mot.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Par exemple :\r\n\r\n"));
                        var exampleRunAmorce = new Run("ROUGE") { Foreground = Brushes.Blue };
                        InstructionTextBlock.Inlines.Add(exampleRunAmorce);
                        break;
                    case 1:
                        InstructionTextBlock.Inlines.Add(new Run("L'amorce, que vous verrez avant le nom, sera soit un carré, soit un cercle.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Si vous avez vu un carré, vous devrez lire le mot.\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Si vous avez vu un cercle, vous devrez donner la couleur de l'encre.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Par exemple :\r\n"));
                        var horizontalPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(40)
                        };
                        var shapeContainer = new Grid
                        {
                            Width = 200,
                            Height = 200,
                            Margin = new Thickness(0,0,80,0)
                        };
                        var ellipse = new Ellipse
                        {
                            Width = 200,
                            Height = 200,
                            Stroke = Brushes.White
                        };
                        shapeContainer.Children.Add(ellipse);
                        var crossGrid = new Grid
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        var horizontalRect = new Rectangle
                        {
                            Width = 40,
                            Height = 3,
                            Fill = Brushes.White
                        };
                        var verticalRect = new Rectangle
                        {
                            Width = 3,
                            Height = 40,
                            Fill = Brushes.White
                        };

                        crossGrid.Children.Add(horizontalRect);
                        crossGrid.Children.Add(verticalRect);
                        shapeContainer.Children.Add(crossGrid);
                        horizontalPanel.Children.Add(shapeContainer);
                        var rougeTextBlock = new TextBlock
                        {
                            Text = "ROUGE",
                            Foreground = Brushes.Blue,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(10, 0, 0, 0),
                            FontSize = 52
                        };
                        horizontalPanel.Children.Add(rougeTextBlock);
                        InstructionTextBlock.Inlines.Add(new InlineUIContainer(horizontalPanel));
                        break;
                    case 2:
                        InstructionTextBlock.Inlines.Add(new Run("Avez-vous des questions avant de commencer ?"));
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0:
                        InstructionTextBlock.Inlines.Add(new Run("Nous allons maintenant réaliser une première tâche mentale.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("A chaque essai, vous verrez apparaître une croix de fixation '+' au milieu de l'écran, suivie d'un nom de couleur : 'ROUGE', 'VERT', 'BLEU', 'JAUNE'.\r\n\r\n"));
                        if (_stroopType.Equals("Congruent", StringComparison.OrdinalIgnoreCase))
                            InstructionTextBlock.Inlines.Add(new Run("Ces noms seront toujours écrits dans la même couleur que le sens du mot.\r\n\r\n"));
                        else
                            InstructionTextBlock.Inlines.Add(new Run("Ces noms seront toujours écrits dans une couleur différente du sens du mot.\r\n\r\n"));
                        InstructionTextBlock.Inlines.Add(new Run("Par exemple :\r\n\r\n"));
                        var exampleRun = new Run("ROUGE")
                        {
                            Foreground = (_stroopType.Equals("Congruent", StringComparison.OrdinalIgnoreCase)) ? Brushes.Red : Brushes.Blue
                        };
                        InstructionTextBlock.Inlines.Add(exampleRun);
                        break;
                    case 1:
                        InstructionTextBlock.Inlines.Add(new Run("Vous devrez appuyer sur le bouton correspondant à la couleur du mot le plus rapidement possible."));
                        break;
                    case 2:
                        InstructionTextBlock.Inlines.Add(new Run("Avez-vous des questions avant de commencer ?"));
                        break;
                }
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                _currentPageIndex++;
                if (_currentPageIndex < TotalPages)
                    ShowPage(_currentPageIndex);
                else
                {
                    _navigationService.NavigateTo<StroopPage>(_settings);
                }
            }
        }
    }
}

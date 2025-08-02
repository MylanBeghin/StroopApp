using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Navigation;
using StroopApp.Views.Experiment.Participant;

namespace StroopApp.ViewModels.Experiment.Participant
{
	public class InstructionsPageViewModel : ViewModelBase
	{
		private readonly ExperimentSettings _settings;
		private readonly INavigationService _navigationService;
		private int _currentPageIndex;
		private const int TotalPages = 3;

		public UIElement CurrentInstruction { get; private set; }
		public ICommand NextCommand { get; }
		public event EventHandler InstructionChanged;
		public StroopPage StroopPage { get; set; }

		public InstructionsPageViewModel(ExperimentSettings settings, INavigationService navigationService)
		{
			_currentPageIndex = 0;
			_settings = settings;
			_navigationService = navigationService;
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

		private UIElement GenerateInstructionPage(int page)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(_settings.CurrentProfile.TaskLanguage ?? "en");
			var loc = new LocalizedStrings();
			var profile = _settings.CurrentProfile;
			var congruence = profile.CongruencePercent;
			bool hasCue = profile.IsAmorce;
			string key = hasCue switch
			{
				false when congruence == 0 => "Case1",
				false when congruence == 100 => "Case2",
				false => "Case3",
				true when congruence == 0 => "Case4",
				true when congruence == 100 => "Case5",
				true => "Case6",
			};

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

			switch (page)
			{
				case 0:
				tb.Inlines.Add(new Run(loc["Page1_Intro"]) { FontWeight = FontWeights.Bold });
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new Run(loc["Page1_Display"]));
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new Run(loc["Page1_WordsList"]));
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new Run(loc[$"{key}_P1_Congruence"]));
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new Run(loc["Page1_Example"]));
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				var exampleColor = (key == "Case2" || key == "Case5") ? Brushes.Red : Brushes.Blue;
				tb.Inlines.Add(new Run(loc["Word_RED"]) { Foreground = exampleColor, FontSize = 78 });
				break;
				case 1:
				tb.Inlines.Add(new Run(loc[$"{key}_P2_Instructions"]));
				tb.Inlines.Add(new LineBreak());
				tb.Inlines.Add(new LineBreak());
				if (hasCue)
				{
					tb.Inlines.Add(new Run(loc["Page2_CueExplanation"]));
					tb.Inlines.Add(new LineBreak());
					tb.Inlines.Add(new LineBreak());

					var panel = new StackPanel
					{
						Orientation = Orientation.Horizontal,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
					};

					// Exemple 1 : carré/croix + mot en rouge
					var squareExample = new StackPanel
					{
						Orientation = Orientation.Horizontal,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center

					};

					// Croisillon blanc centré
					var crossGrid = new Grid
					{
						Width = 250,
						Height = 250,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
					};

					// Barre verticale
					crossGrid.Children.Add(new Rectangle
					{
						Width = 6,
						Height = 50,
						Fill = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					});

					// Barre horizontale
					crossGrid.Children.Add(new Rectangle
					{
						Width = 50,
						Height = 6,
						Fill = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					});
					crossGrid.Children.Add(new Rectangle
					{
						Width = 150,
						Height = 150,
						Stroke = Brushes.White,
						StrokeThickness = 6,
						Fill = Brushes.Transparent,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					});

					// Ajout de la forme dans le StackPanel
					squareExample.Children.Add(crossGrid);

					// Mot en dessous
					squareExample.Children.Add(new TextBlock
					{
						Text = loc["Word_RED"],
						Foreground = Brushes.Red,
						FontSize = 78,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					});

					// Ajout à ton panel principal
					panel.Children.Add(squareExample);


					tb.Inlines.Add(new InlineUIContainer(panel) { BaselineAlignment = BaselineAlignment.Center });
					tb.Inlines.Add(new LineBreak());
					tb.Inlines.Add(new LineBreak());
					tb.Inlines.Add(new Run(loc[$"{key}_P2_Example"]));
				}

				break;
				case 2:
				tb.Inlines.Add(new Run(loc["Page3_Questions"]));
				break;
			}

			return tb;
		}
	}
}

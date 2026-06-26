using StroopApp.Core;
using StroopApp.Services.Navigation;
using StroopApp.ViewModels.State;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StroopApp.ViewModels.Experiment.Participant.Instructions
{
    public partial class SimonInstructionsViewModel : InstructionsViewModelBase
    {
        protected override int TotalPages => 4;

        public SimonInstructionsViewModel(ExperimentSettingsViewModel settings,
                                          INavigationService participantWindowNavigationService,
                                          Func<Page> nextPageFactory)
            : base(settings, participantWindowNavigationService, nextPageFactory)
        { }

        /// <summary>
        /// Generates localized instruction content based on page index and experiment configuration.
        /// Handles dynamic instruction based on congruence percentage and key mappings.
        /// </summary>
        protected override UIElement GenerateInstructionPage(int page)
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                var targetCulture = new CultureInfo(_settings.CurrentProfile.TaskLanguage ?? "en");
                Thread.CurrentThread.CurrentCulture = targetCulture;
                Thread.CurrentThread.CurrentUICulture = targetCulture;

                var loc = new LocalizedStrings();
                var simon = _settings.KeyMappings.Simon;

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
                        tb.Inlines.Add(new Run(loc["Simon_Page1_Intro"]) { FontWeight = FontWeights.Bold });
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(new Run(loc["Simon_Page1_Display"]));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(new Run(loc[$"Simon_Page1_Display2"]));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(new Run(string.Format(loc["Simon_Page1_LeftKey"], simon.Left.Key)));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(new Run(string.Format(loc["Simon_Page1_RightKey"], simon.Right.Key)));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());
                        break;

                    case 1:
                        tb.Inlines.Add(new Run(loc["Simon_Circle_Instruction"]));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(CreateEllipse(simon.Left.Color));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(new Run(loc["Simon_LeftKey_Instruction"] + $" {simon.Left.Key})"));
                        break;

                    case 2:
                        tb.Inlines.Add(new Run(loc["Simon_Circle_Instruction"]));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());

                        tb.Inlines.Add(CreateEllipse(simon.Right.Color));
                        tb.Inlines.Add(new LineBreak()); tb.Inlines.Add(new LineBreak());
                        
                        tb.Inlines.Add(new Run(loc["Simon_RightKey_Instruction"] + $" {simon.Right.Key})"));
                        break;
                    
                    case 3:
                        tb.Inlines.Add(new Run(loc["Page3_Questions"]));
                        break;
                }
                return tb;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }
        private Ellipse CreateEllipse(string color = "#FFFFFF")
        {
            return new Ellipse
            {
                Width = 120,
                Height = 120,
                Fill = (Brush)new BrushConverter().ConvertFromString(color)!,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}

using RealTimeGraphX.DataPoints;
using RealTimeGraphX.Renderers;
using RealTimeGraphX.WPF;
using System.Diagnostics;
using System.Windows.Media;
using StroopApp.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel;
namespace StroopApp.ViewModels.Experiment
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private ExperimentSettings _settings;
        public ExperimentSettings Settings
        {
            get => _settings;
            set { _settings = value; OnPropertyChanged(); }
        }
        public WpfGraphController<Int32DataPoint, DoubleDataPoint> Controller { get; set; }

        public GraphViewModel(ExperimentSettings settings)
        {
            Settings = settings;
            Controller = new WpfGraphController<Int32DataPoint, DoubleDataPoint>();
            Controller.Renderer = new ScrollingLineRenderer<WpfGraphDataSeries>();
            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Temps de réaction du participant",
                Stroke = Colors.Red,
            });
            Start();

        }

        private void Start()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    var x = Settings.ExperimentContext.CurrentTrial.TrialNumber;
                    var y = Settings.ExperimentContext.CurrentTrial.ReactionTime;
                    Controller.PushData(x, y);

                    Thread.Sleep(30);
                }
            });
            thread.Start();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

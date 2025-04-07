using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using RealTimeGraphX.DataPoints;
using RealTimeGraphX.Renderers;
using RealTimeGraphX.WPF;
using StroopApp.Models;
using StroopApp.Services.Navigation;

namespace StroopApp.ViewModels.Experiment
{
    public class ExperimentDashBoardPageViewModel : INotifyPropertyChanged
    {
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }
        public ExperimentDashBoardPageViewModel(INavigationService navigationService, ExperimentSettings settings)
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Renderer = new ScrollingLineRenderer<WpfGraphDataSeries>();
            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series Name",
                Stroke = Colors.Red,
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using SkiaSharp;

using StroopApp.Core;

namespace StroopApp.Models
{
	/// <summary>
	/// Defines the next action to take after a block ends: Restart, Start new experiment, or Quit.
	/// </summary>
	public enum ExperimentAction
	{
		None,
		RestartBlock,
		NewExperiment,
		Quit
	}
	/// <summary>
	/// Centralized data container used during the experiment to store trials, reaction times, and chart series.
	/// Also tracks current trial and experiment flow control state.
	/// </summary>
	public class SharedExperimentData : ModelBase
	{

		public ObservableCollection<Block> Blocks
		{
			get;
		}
		public ObservableCollection<ISeries> BlockSeries
		{
			get;
		}

		private Block? _currentBlock;
		public Block? CurrentBlock
		{
			get => _currentBlock;
			set
			{
				if (_currentBlock != value)
				{
					_currentBlock = value;
					OnPropertyChanged();
				}
			}
		}
		public ObservableCollection<ISeries>? ColumnSerie
		{
			get; set;
		}
		public ObservableCollection<ReactionTimePoint> ReactionPoints
		{
			get; set;
		}
		public ObservableCollection<RectangularSection> Sections
		{
			get; set;
		}
		public int currentBlockStart;
		public int currentBlockEnd;

		private StroopTrial? _currentTrial;
		public StroopTrial? CurrentTrial
		{
			get => _currentTrial;
			set
			{
				if (_currentTrial != value)
				{
					if (_currentTrial != null)
						_currentTrial.PropertyChanged -= CurrentTrial_PropertyChanged;
					_currentTrial = value;
					OnPropertyChanged(nameof(CurrentTrial));
					if (_currentTrial != null)
						_currentTrial.PropertyChanged += CurrentTrial_PropertyChanged;
				}
			}
		}
		private void CurrentTrial_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(StroopTrial.TrialNumber))
			{
				OnPropertyChanged(nameof(CurrentTrial));

			}
		}

		private bool _isBlockFinished = false;
		public bool IsBlockFinished
		{
			get => _isBlockFinished;
			set
			{
				if (_isBlockFinished != value)
				{
					_isBlockFinished = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _isParticipantSelectionEnabled = true;
		public bool IsParticipantSelectionEnabled
		{
			get => _isParticipantSelectionEnabled;
			set
			{
				if (_isParticipantSelectionEnabled != value)
				{
					_isParticipantSelectionEnabled = value;
					OnPropertyChanged();
				}
			}
		}
		ExperimentAction _nextAction;
		public ExperimentAction NextAction
		{
			get => _nextAction;
			set
			{
				_nextAction = value;
				OnPropertyChanged();
			}
		}

		private readonly SKColor[] _palette = { SKColors.CornflowerBlue, SKColors.OrangeRed, SKColors.MediumSeaGreen, SKColors.Goldenrod };

		public int _colorIndex;
		public SharedExperimentData()
		{
			Blocks = new ObservableCollection<Block>();
			BlockSeries = new ObservableCollection<ISeries>();
			Sections = new ObservableCollection<RectangularSection>();
			ReactionPoints = new ObservableCollection<ReactionTimePoint>();
			NewColumnSerie();
			currentBlockStart = 1;
		}

		private void NewColumnSerie()
		{
			ColumnSerie =
			new ObservableCollection<ISeries>
			{
				new ColumnSeries<ReactionTimePoint>
				{
					Values = ReactionPoints,
					DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
					DataLabelsSize = 16,
					DataLabelsPaint = new SolidColorPaint(SKColors.Black),
					DataLabelsFormatter = point => point.Coordinate.SecondaryValue.Equals(double.NaN) ? "Aucune réponse" : point.Coordinate.PrimaryValue.ToString("N0"),
					XToolTipLabelFormatter = point =>
					{
						var trial = (int)(point.Coordinate.SecondaryValue + 1);
						return $"Essai n°{trial}";
					},
					YToolTipLabelFormatter = point =>
					{
						var value = point.Coordinate.PrimaryValue;
						return double.IsNaN(value)
							? "Pas de réponse"
							: $"Temps de réponse : {value:0.#####} ms";
					},
					Mapping = (point, index) => new Coordinate( point.TrialNumber-1,point.ReactionTime != null ? point.ReactionTime.Value : double.NaN)
				}.OnPointCreated(p =>
					{
						if (p.Visual is null) return;
						var model = p.Model;
						if (model!= null && model.IsValidResponse.HasValue)
						{
							if(model.IsValidResponse.Value)
							{
                            // Bonne réponse : point vert
                            p.Visual.Fill = new SolidColorPaint(SKColors.Green);
							p.Visual.Stroke = new SolidColorPaint(SKColors.Green);
							}
							else
							{ 
                            // Mauvaise réponse : point rouge
                            p.Visual.Fill = new SolidColorPaint(SKColors.Red);
							p.Visual.Stroke = new SolidColorPaint(SKColors.Red);
							}
						}
					}
				)
			};
		}

		public void AddNewSerie(ExperimentSettings _settings)
		{
			CurrentBlock = new Block(_settings.Block, _settings.CurrentProfile.ProfileName);
			Blocks.Add(CurrentBlock);
			var color = _palette[_colorIndex % _palette.Length];
			var fillColor = color.WithAlpha(50);

			var start = currentBlockStart;
			var count = _settings.CurrentProfile.WordCount;
			var end = start + count - 1;
			currentBlockEnd = end;
			BlockSeries.Add(new LineSeries<double?>
			{
				Values = CurrentBlock.TrialTimes,
				Stroke = new SolidColorPaint(SKColors.Black, 2),
				Fill = new SolidColorPaint(SKColors.Black.WithAlpha(60)),
				LineSmoothness = 0.4f,
				GeometrySize = 6,
				GeometryStroke = new SolidColorPaint(SKColors.Black, 2),
				GeometryFill = new SolidColorPaint(SKColors.White),
				Mapping = (pt, idx) => new Coordinate(start + idx, pt.Value)
			});
			Sections.Add(new RectangularSection
			{
				Xi = start,
				Xj = end,
				Fill = new SolidColorPaint(fillColor),
				Label = $"Bloc n°{_settings.Block}",
				LabelSize = 16,
				LabelPaint = new SolidColorPaint(SKColors.Black)
			});
			_colorIndex++;
			currentBlockStart = end + 1;
		}
		public void Reset()
		{
			Blocks.Clear();
			BlockSeries.Clear();
			Sections.Clear();
			ReactionPoints.Clear();
			_colorIndex = 0;
			currentBlockStart = 1;
			currentBlockEnd = 0;
			CurrentBlock = null;
			IsBlockFinished = false;
			NextAction = ExperimentAction.None;
			NewColumnSerie();
		}
	}
}

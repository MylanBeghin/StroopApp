﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using SkiaSharp;

using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.Exportation;
using StroopApp.Services.Navigation;
using StroopApp.Services.Window;
using StroopApp.Views;

namespace StroopApp.ViewModels.Experiment.Experimenter
{
	public class EndExperimentViewModel : ViewModelBase
	{
		public ExperimentSettings Settings
		{
			get;
		}
		public ObservableCollection<Block> Blocks
		{
			get;
		}
		private readonly IExportationService _exportationService;
		private readonly INavigationService _experimenterNavigationService;
		private readonly IWindowManager _windowManager;

		public ICommand ContinueCommand
		{
			get;
		}
		public ICommand RestartCommand
		{
			get;
		}
		public ICommand QuitCommand
		{
			get;
		}
		private string _currentParticipant;
		public string CurrentParticipant
		{
			get => _currentParticipant;
			set
			{
				_currentParticipant = value;
				OnPropertyChanged();
			}
		}
		private string _currentProfile;
		public string CurrentProfile
		{
			get => _currentProfile;
			set
			{
				_currentProfile = value;
				OnPropertyChanged();
			}
		}
		public EndExperimentViewModel(ExperimentSettings settings,
								  IExportationService exportationService,
								  INavigationService experimenterNavigationService,
								  IWindowManager windowManager)
		{
			Settings = settings;
			_exportationService = exportationService;
			_experimenterNavigationService = experimenterNavigationService;
			_windowManager = windowManager;
			ContinueCommand = new RelayCommand(Continue);
			RestartCommand = new RelayCommand(Restart);
			QuitCommand = new RelayCommand(Quit);
			Blocks = Settings.ExperimentContext.Blocks;
			CurrentParticipant = "Participant n° " + Settings.Participant.Id.ToString();
			CurrentProfile = "Tâche : " + Settings.CurrentProfile.ProfileName;
			UpdateBlock();
		}

		private void UpdateBlock()
		{

			Settings.ExperimentContext.ReactionPoints = new ObservableCollection<ReactionTimePoint>();
			Settings.ExperimentContext.ColumnSerie = new ObservableCollection<ISeries>
			{
				new ColumnSeries<ReactionTimePoint>
				{
					Values = Settings.ExperimentContext.ReactionPoints,
					DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
					DataLabelsSize = 16,
					DataLabelsPaint = new SolidColorPaint(SKColors.Black),
					DataLabelsFormatter = point =>
				point.Coordinate.PrimaryValue.Equals(null)
					? "Aucune réponse"
					: point.Coordinate.PrimaryValue.ToString("N0"),
					Mapping = (point, index) => new Coordinate(
						point.TrialNumber-1,
						point.ReactionTime != null
							? point.ReactionTime.Value
							: double.NaN
					)
				}.OnPointCreated(p =>
	{
		if (p.Visual is null) return;
		var model = p.Model;
		if (model!= null && model.IsValidResponse.HasValue)
						{
							// Orange (wrong answer)
							var orange = new SKColor(255, 166, 0);      // #FFA600
							// Violet (right answer)
							var violet = new SKColor(91, 46, 255);      // #5B2EFF
							if(model.IsValidResponse.Value)
							{
								p.Visual.Fill = new SolidColorPaint(violet);
								p.Visual.Stroke = new SolidColorPaint(violet);
							}
							else
							{
								p.Visual.Fill = new SolidColorPaint(orange);
								p.Visual.Stroke = new SolidColorPaint(orange);
							}
						}
			})
			};

		}

		private void Continue()
		{
			Settings.Block++;
			Settings.ExperimentContext.IsBlockFinished = false;
			Settings.ExperimentContext.IsParticipantSelectionEnabled = false;
			_experimenterNavigationService.NavigateTo(() => new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager));
		}
		private async void Restart()
		{
			await _exportationService.ExportDataAsync();
			Settings.ExperimentContext.IsBlockFinished = false;
			Settings.ExperimentContext.IsParticipantSelectionEnabled = true;
			Settings.Reset();
			_experimenterNavigationService.NavigateTo(
				() => new ConfigurationPage(Settings, _experimenterNavigationService, _windowManager));
		}

		private async void Quit()
		{
			await _exportationService.ExportDataAsync();
			Application.Current.Shutdown();
		}
	}
}

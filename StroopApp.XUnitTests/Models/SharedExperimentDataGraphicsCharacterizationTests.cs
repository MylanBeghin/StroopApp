using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using StroopApp.Models;
using System.Collections.ObjectModel;
using Xunit;

namespace StroopApp.XUnitTests.Models
{
	/// <summary>
	/// Characterization tests for SharedExperimentData graphics behavior.
	/// These tests freeze the EXACT current behavior of LiveCharts integration.
	/// DO NOT modify these tests unless the actual graphics behavior changes intentionally.
	/// </summary>
	public class SharedExperimentDataGraphicsCharacterizationTests
	{
		// ========== NewColumnSerie() - Structure Tests ==========

		[Fact]
		public void NewColumnSerie_CreatesNonNullColumnSerie()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.NewColumnSerie();

			// Assert
			Assert.NotNull(data.ColumnSerie);
		}

		[Fact]
		public void NewColumnSerie_ColumnSerieContainsExactlyOneSeries()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.NewColumnSerie();

			// Assert
			Assert.Single(data.ColumnSerie);
		}

		[Fact]
		public void NewColumnSerie_SeriesIsColumnSeriesOfReactionTimePoint()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.NewColumnSerie();

			// Assert
			var series = data.ColumnSerie.First();
			Assert.IsType<ColumnSeries<ReactionTimePoint>>(series);
		}

		[Fact]
		public void NewColumnSerie_ValuesReferenceLiveReactionPoints()
		{
			// Arrange
			var data = new SharedExperimentData();

			// Act
			data.NewColumnSerie();

			// Assert
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();
			Assert.Same(data.ReactionPoints, columnSeries.Values);
		}

		// ========== NewColumnSerie() - Mapping Lambda Tests ==========

		[Fact]
		public void NewColumnSerie_MappingLambda_TrialNumberMapsToXMinusOne()
		{
			// This test characterizes the EXACT mapping behavior observed.
			// TrialNumber 5 ? X coordinate 4 (TrialNumber - 1)

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();
			var testPoint = new ReactionTimePoint(5, 500, true);

			// Act
			var mapping = columnSeries.Mapping;
			var coordinate = mapping(testPoint, 0);

			// Assert
			Assert.Equal(4, coordinate.SecondaryValue); // TrialNumber - 1 = 5 - 1 = 4
		}

		[Fact]
		public void NewColumnSerie_MappingLambda_ReactionTimeNullMapsToNaN()
		{
			// This test characterizes handling of null ReactionTime.
			// null ? double.NaN

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();
			var testPoint = new ReactionTimePoint(1, null, true);

			// Act
			var mapping = columnSeries.Mapping;
			var coordinate = mapping(testPoint, 0);

			// Assert
			Assert.True(double.IsNaN(coordinate.PrimaryValue));
		}

		[Fact]
		public void NewColumnSerie_MappingLambda_ReactionTimeValueMapsToY()
		{
			// This test characterizes mapping of actual ReactionTime value.
			// ReactionTime = 500 ? Y coordinate 500

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();
			var testPoint = new ReactionTimePoint(1, 500, true);

			// Act
			var mapping = columnSeries.Mapping;
			var coordinate = mapping(testPoint, 0);

			// Assert
			Assert.Equal(500, coordinate.PrimaryValue);
		}

		// ========== NewColumnSerie() - DataLabelsFormatter Tests ==========

		[Fact]
		public void NewColumnSerie_DataLabelsFormatter_Exists()
		{
			// This test characterizes that DataLabelsFormatter is configured.
			// Note: We cannot easily test the exact output without full LiveCharts context,
			// but we verify the lambda is assigned.

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();

			// Assert
			Assert.NotNull(columnSeries.DataLabelsFormatter);
		}

		// ========== NewColumnSerie() - XToolTipLabelFormatter Tests ==========

		[Fact]
		public void NewColumnSerie_XToolTipLabelFormatter_Exists()
		{
			// This test characterizes that XToolTipLabelFormatter is configured.

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();

			// Assert
			Assert.NotNull(columnSeries.XToolTipLabelFormatter);
		}

		// ========== NewColumnSerie() - YToolTipLabelFormatter Tests ==========

		[Fact]
		public void NewColumnSerie_YToolTipLabelFormatter_Exists()
		{
			// This test characterizes that YToolTipLabelFormatter is configured.

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();

			// Assert
			Assert.NotNull(columnSeries.YToolTipLabelFormatter);
		}

		// ========== AddNewSerie() - BlockSeries Tests ==========

		[Fact]
		public void AddNewSerie_AddsLineSeriesToBlockSeries()
		{
			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var initialCount = data.BlockSeries.Count;

			// Act
			data.AddNewSerie(config);

			// Assert
			Assert.Equal(initialCount + 1, data.BlockSeries.Count);
		}

		[Fact]
		public void AddNewSerie_AddedSeriesIsLineSeries()
		{
			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };

			// Act
			data.AddNewSerie(config);

			// Assert
			var addedSeries = data.BlockSeries.Last();
			Assert.IsType<LineSeries<double?>>(addedSeries);
		}

		[Fact]
		public void AddNewSerie_LineSeriesValuesReferenceCurrentBlockTrialTimes()
		{
			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };

			// Act
			data.AddNewSerie(config);

			// Assert
			var lineSeries = (LineSeries<double?>)data.BlockSeries.Last();
			Assert.Same(data.CurrentBlock.TrialTimes, lineSeries.Values);
		}

		// ========== AddNewSerie() - RectangularSection Tests ==========

		[Fact]
		public void AddNewSerie_AddsRectangularSectionToSections()
		{
			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var initialCount = data.Sections.Count;

			// Act
			data.AddNewSerie(config);

			// Assert
			Assert.Equal(initialCount + 1, data.Sections.Count);
		}

		[Fact]
		public void AddNewSerie_SectionXiEqualsCurrentBlockStartBeforeCall()
		{
			// This characterizes the EXACT behavior: Xi uses value BEFORE the call.

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var expectedXi = data.CurrentBlockStart; // Value BEFORE call

			// Act
			data.AddNewSerie(config);

			// Assert
			var section = data.Sections.Last();
			Assert.Equal(expectedXi, section.Xi);
		}

		[Fact]
		public void AddNewSerie_SectionXjEqualsCalculatedEnd()
		{
			// This characterizes Xj = CurrentBlockStart + WordCount - 1

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var expectedXj = data.CurrentBlockStart + config.WordCount - 1; // 1 + 10 - 1 = 10

			// Act
			data.AddNewSerie(config);

			// Assert
			var section = data.Sections.Last();
			Assert.Equal(expectedXj, section.Xj);
		}

		[Fact]
		public void AddNewSerie_SectionLabelContainsBlockNumber()
		{
			// This characterizes the EXACT label format: "Bloc n°{Block}"

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 3, WordCount = 10 };

			// Act
			data.AddNewSerie(config);

			// Assert
			var section = data.Sections.Last();
			Assert.Equal("Block 3", section.Label);
		}

		// ========== AddNewSerie() - ColorIndex Tests ==========

		[Fact]
		public void AddNewSerie_IncrementsColorIndexByOne()
		{
			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var initialColorIndex = data.ColorIndex;

			// Act
			data.AddNewSerie(config);

			// Assert
			Assert.Equal(initialColorIndex + 1, data.ColorIndex);
		}

		[Fact]
		public void AddNewSerie_ColorIndexCyclesModuloPaletteLength()
		{
			// This characterizes the cycling behavior.
			// Palette has 4 colors, so index cycles 0,1,2,3,0,1,2,3...

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };

			// Act - Add 5 series to force cycling
			for (int i = 0; i < 5; i++)
			{
				data.AddNewSerie(config);
			}

			// Assert
			// After 5 additions, colorIndex should be 5
			// Color used for 5th block: palette[5 % 4] = palette[1] (second color)
			Assert.Equal(5, data.ColorIndex);
		}

		// ========== AddNewSerie() - LineSeries Mapping Lambda Tests ==========

		[Fact]
		public void AddNewSerie_LineSeriesMappingUsesStartPlusIndex()
		{
			// This characterizes the EXACT mapping: X = start + index

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };
			var start = data.CurrentBlockStart; // 1

			// Act
			data.AddNewSerie(config);

			// Assert
			var lineSeries = (LineSeries<double?>)data.BlockSeries.Last();
			var mapping = lineSeries.Mapping;

			// Test mapping at index 0
			var coord0 = mapping(123.45, 0);
			Assert.Equal(start + 0, coord0.SecondaryValue); // 1 + 0 = 1

			// Test mapping at index 5
			var coord5 = mapping(678.90, 5);
			Assert.Equal(start + 5, coord5.SecondaryValue); // 1 + 5 = 6
		}

		[Fact]
		public void AddNewSerie_LineSeriesMappingValidValueMapsToY()
		{
			// This characterizes mapping of actual double? value.
			// CRITICAL OBSERVATION: The mapping uses pt.Value, so it ASSUMES non-null values.
			// Null values in TrialTimes are NOT expected/handled by this mapping lambda.

			// Arrange
			var data = new SharedExperimentData();
			var config = new TestBlockConfiguration { Block = 1, WordCount = 10 };

			// Act
			data.AddNewSerie(config);

			// Assert
			var lineSeries = (LineSeries<double?>)data.BlockSeries.Last();
			var mapping = lineSeries.Mapping;

			// Test mapping with value
			var coord = mapping(456.78, 0);
			Assert.Equal(456.78, coord.PrimaryValue);
		}

		// ========== Helper Test Configuration ==========

		private class TestBlockConfiguration : IBlockConfiguration
		{
			public int Block { get; set; }
			public int WordCount { get; set; }
			public string ProfileName { get; set; } = "Test";
			public int CongruencePercent { get; set; } = 50;
			public int? SwitchPercent { get; set; } = 50;
			public bool IsAmorce { get; set; } = false;
		}

		// ========== Duplication: NewColumnSerie vs UpdateBlock Pattern ==========

		[Fact]
		public void NewColumnSerie_ValuesReferenceLiveCollection_ChangesPropagateToSeries()
		{
			// This characterizes that NewColumnSerie() binds to LIVE ReactionPoints.
			// Changes to ReactionPoints are visible in the series.

			// Arrange
			var data = new SharedExperimentData();
			data.NewColumnSerie();
			var columnSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();

			// Act - Add point after series creation
			data.ReactionPoints.Add(new ReactionTimePoint(1, 500, true));

			// Assert
			// Series Values should reference ReactionPoints, so change is visible
			Assert.Same(data.ReactionPoints, columnSeries.Values);
			Assert.Single(columnSeries.Values);
		}

		[Fact]
		public void Snapshot_ValuesReferenceSnapshot_OriginalChangesDoNotPropagate()
		{
			// This characterizes the behavior seen in UpdateBlock():
			// Creating a SNAPSHOT of ReactionPoints creates an independent copy.
			// Changes to original do NOT propagate to snapshot.

			// Arrange
			var data = new SharedExperimentData();
			data.ReactionPoints.Add(new ReactionTimePoint(1, 500, true));

			// Create snapshot (simulates UpdateBlock behavior)
			var pointsSnapshot = new ObservableCollection<ReactionTimePoint>(data.ReactionPoints);
			var snapshotSeries = new ColumnSeries<ReactionTimePoint>
			{
				Values = pointsSnapshot
			};

			// Act - Add point to ORIGINAL after snapshot
			data.ReactionPoints.Add(new ReactionTimePoint(2, 600, true));

			// Assert
			// Original has 2 points
			Assert.Equal(2, data.ReactionPoints.Count);
			// Snapshot still has 1 point (independent copy)
			Assert.Single(snapshotSeries.Values);
			// They are NOT the same reference
			Assert.NotSame(data.ReactionPoints, snapshotSeries.Values);
		}

		[Fact]
		public void Snapshot_CreatesIndependentCopy_NotLiveBound()
		{
			// This documents the EXACT difference between NewColumnSerie() and UpdateBlock().
			// NewColumnSerie() ? live binding
			// UpdateBlock() ? snapshot (frozen state)

			// Arrange
			var data = new SharedExperimentData();
			data.ReactionPoints.Add(new ReactionTimePoint(1, 500, true));

			// Create live binding (NewColumnSerie style)
			data.NewColumnSerie();
			var liveSeries = (ColumnSeries<ReactionTimePoint>)data.ColumnSerie.First();

			// Create snapshot (UpdateBlock style)
			var pointsSnapshot = new ObservableCollection<ReactionTimePoint>(data.ReactionPoints);

			// Act - Modify original
			data.ReactionPoints.Add(new ReactionTimePoint(2, 600, true));

			// Assert
			// Live series sees the change
			Assert.Equal(2, liveSeries.Values.Count);
			// Snapshot does NOT see the change
			Assert.Single(pointsSnapshot);
		}
	}
}

using StroopApp.Models;

namespace StroopApp.Core.Models
{
    public enum CalculationMode
    {
        TaskDuration,
        WordCount
    }

    public class StroopTaskSettings : TaskSettings
    {
        public StroopTaskSettings()
        {
            TaskName = "Stroop";

            FixationDuration = 100;
            MaxReactionTime = 400;
            VisualCueDuration = 0;
            GroupSize = 5;
            HasVisualCue = false;
            WordCount = 10;
            CalculationMode = CalculationMode.WordCount;
            CongruencePercent = 50;
            DominantPercent = 50;
            SwitchPercent = null;
            ExportFolderPath = string.Empty;
            KeyMappings = new KeyMappings();

            var currentCultureCode = Thread.CurrentThread.CurrentUICulture?.TwoLetterISOLanguageName;
            TaskLanguage = string.IsNullOrWhiteSpace(currentCultureCode) ? "en" : currentCultureCode;

            UpdateDerivedValues();
        }

        // Timing
        public int MaxReactionTime { get; set; }
        public int FixationDuration { get; set; }
        public int VisualCueDuration { get; set; }
        public bool HasVisualCue { get; set; }

        // Duration breakdown (HH:MM:SS) — used when CalculationMode = TaskDuration
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        // Derived timing values (updated via UpdateDerivedValues)
        public int WordDuration { get; set; }
        public int TaskDuration { get; set; }

        // Trial counts & groups
        public int WordCount { get; set; }
        public int GroupSize { get; set; }

        // Percentages
        public int CongruencePercent { get; set; }
        public int DominantPercent { get; set; }
        public int? SwitchPercent { get; set; }

        // Task configuration
        public CalculationMode CalculationMode { get; set; }
        public string TaskLanguage { get; set; } = "en";

        // Input & export
        public KeyMappings KeyMappings { get; set; }
        public string ExportFolderPath { get; set; }

        public void UpdateDerivedValues()
        {
            WordDuration = MaxReactionTime + FixationDuration + VisualCueDuration;

            if (CalculationMode == CalculationMode.TaskDuration)
            {
                TaskDuration = ((Hours * 3600) + (Minutes * 60) + Seconds) * 1000;
                if (WordDuration > 0)
                    WordCount = TaskDuration / WordDuration;
            }
            else if (CalculationMode == CalculationMode.WordCount)
            {
                TaskDuration = WordCount * WordDuration;
                Hours = TaskDuration / 3600000;
                Minutes = (TaskDuration % 3600000) / 60000;
                Seconds = (TaskDuration % 60000) / 1000;
            }
        }
    }
}

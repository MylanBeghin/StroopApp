namespace StroopApp.Models
{
    public enum CalculationMode
    {
        TaskDuration,
        WordCount
    }

    public class ExperimentProfile
    {
        public ExperimentProfile()
        {
            Id = Guid.NewGuid();
            ProfileName = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName switch
            {
                "fr" => "Nouveau profil",
                "en" => "New profile",
                _ => "New profile"
            };
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
            UpdateDerivedValues();

            var currentCultureCode = Thread.CurrentThread.CurrentUICulture?.TwoLetterISOLanguageName;
            TaskLanguage = string.IsNullOrWhiteSpace(currentCultureCode) ? "en" : currentCultureCode;
        }

        public Guid Id { get; set; }
        public string ProfileName { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int WordDuration { get; set; }
        public int FixationDuration { get; set; }
        public int VisualCueDuration { get; set; }
        public bool HasVisualCue { get; set; }
        public int GroupSize { get; set; }
        public int TaskDuration { get; set; }
        public int WordCount { get; set; }
        public int MaxReactionTime { get; set; }
        public CalculationMode CalculationMode { get; set; }
        public int DominantPercent { get; set; }
        public int CongruencePercent { get; set; }
        public int? SwitchPercent { get; set; }
        public string TaskLanguage { get; set; }

        public void UpdateDerivedValues()
        {
            WordDuration = MaxReactionTime + FixationDuration + VisualCueDuration;
            if (CalculationMode == CalculationMode.TaskDuration)
            {
                TaskDuration = ((Hours * 3600) + (Minutes * 60) + Seconds) * 1000;
                if (WordDuration > 0)
                {
                    WordCount = TaskDuration / WordDuration;
                }
            }
            else if (CalculationMode == CalculationMode.WordCount)
            {
                TaskDuration = WordCount * WordDuration;
                Hours = TaskDuration / 3600000;
                Minutes = (TaskDuration % 3600000) / 60000;
                Seconds = (TaskDuration % 60000) / 1000;
            }
        }

        public ExperimentProfile CloneProfile()
        {
            return new ExperimentProfile()
            {
                Id = this.Id,
                ProfileName = this.ProfileName,
                CalculationMode = this.CalculationMode,
                Hours = this.Hours,
                Minutes = this.Minutes,
                Seconds = this.Seconds,
                TaskDuration = this.TaskDuration,
                WordDuration = this.WordDuration,
                MaxReactionTime = this.MaxReactionTime,
                GroupSize = this.GroupSize,
                VisualCueDuration = this.VisualCueDuration,
                FixationDuration = this.FixationDuration,
                WordCount = this.WordCount,
                HasVisualCue = this.HasVisualCue,
                DominantPercent = this.DominantPercent,
                CongruencePercent = this.CongruencePercent,
                SwitchPercent = this.SwitchPercent,
                TaskLanguage = this.TaskLanguage
            };
        }
    }
}
namespace StroopApp.Models.Simon
{
    public class SimonProfile : ExperimentProfile
    {
        public SimonStimulusPositionMode StimulusPositionMode { get; set; }
        public SimonAnswerMode AnswerMode { get; set; }
        public SimonProfile() : base()
        {
            TaskType = TaskType.Simon;
            StimulusPositionMode = SimonStimulusPositionMode.LeftRight;
            AnswerMode = SimonAnswerMode.LeftRight;
        }

        public override void UpdateFrom(ExperimentProfile profile)
        {
            base.UpdateFrom(profile);
            if (profile is SimonProfile simonProfile)
            {
                StimulusPositionMode = simonProfile.StimulusPositionMode;
                AnswerMode = simonProfile.AnswerMode;
            }
        }
        public override SimonProfile CloneProfile()
        {
            return new SimonProfile()
            {
                Id = this.Id,
                ProfileName = this.ProfileName,
                TaskType = this.TaskType,
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
                TaskLanguage = this.TaskLanguage,
                StimulusPositionMode = this.StimulusPositionMode,
                AnswerMode = this.AnswerMode,
            };
        }
    }
}

namespace StroopApp.Core.Models
{
    public class InstructionStep : IExperimentStep
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Instruction";
        public StepType Type => StepType.Instruction;

        public string ContentText { get; set; } = string.Empty;
        public string ValidationKey { get; set; } = "Space";
        public int? DurationTimeoutMs { get; set; } = null;
    }

    public class TaskStep : IExperimentStep
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Task";
        public StepType Type => StepType.Task;

        public string TaskTypeIdentifier { get; set; } = string.Empty;
        public TaskSettings Settings { get; set; } = null!;
    }
}

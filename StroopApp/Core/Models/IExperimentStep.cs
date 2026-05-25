using System.Text.Json.Serialization;

namespace StroopApp.Core.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$stepType")]
    [JsonDerivedType(typeof(InstructionStep), typeDiscriminator: "Instruction")]
    [JsonDerivedType(typeof(TaskStep), typeDiscriminator: "Task")]
    public interface IExperimentStep
    {
        string Id { get; }
        string Name { get; set; }
        StepType Type { get; }
    }

    public enum StepType
    {
        Instruction,
        Task
    }
}

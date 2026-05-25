using System.Text.Json.Serialization;

namespace StroopApp.Core.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$taskType")]
    [JsonDerivedType(typeof(StroopTaskSettings), typeDiscriminator: "Stroop")]
    public abstract class TaskSettings
    {
        public string TaskName { get; set; } = string.Empty;
    }
}

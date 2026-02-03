/// <summary>
/// Represents a language option with its code and display name for UI selection.
/// </summary>
public class LanguageOption
{
    public string Code { get; set; } = null!;
    public string DisplayName { get; set; } = null!;

    public override string ToString() => DisplayName;
}
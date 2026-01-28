namespace StroopApp.Models
{
	public class LanguageOption
	{
		public string Code { get; set; }       // "fr", "en"
	public string DisplayName { get; set; }

		public override string ToString() => DisplayName;
	}

}

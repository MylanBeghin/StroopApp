namespace StroopApp.Models
{
	public class LanguageOption
	{
		public string Code { get; set; }       // "fr", "en"
		public string DisplayName { get; set; } // "Français", "English"

		public override string ToString() => DisplayName; // Pour affichage dans le ComboBox
	}

}

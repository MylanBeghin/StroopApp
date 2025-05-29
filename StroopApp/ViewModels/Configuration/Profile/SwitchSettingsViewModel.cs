using System.Collections.ObjectModel;
using System.Text;

using StroopApp.Core;

public class SwitchSettingsViewModel : ViewModelBase
{


	private int _switchPercent = 50;
	public int SwitchPercent
	{
		get => _switchPercent;
		set
		{
			if (_switchPercent != value)
			{
				_switchPercent = value;
				OnPropertyChanged(nameof(SwitchPercent));
				OnPropertyChanged(nameof(SwitchPreview));
			}
		}
	}
	private string _dominantForm = "Rond";
	public string DominantForm
	{
		get => _dominantForm;
		set
		{
			if (_dominantForm != value)
			{
				_dominantForm = value;
				OnPropertyChanged(nameof(DominantForm));
				OnPropertyChanged(nameof(SwitchPreview));
			}
		}
	}
	private int _dominantPercent = 50; // 0 = 100% Carré, 100 = 100% Rond
	public int DominantPercent
	{
		get => _dominantPercent;
		set
		{
			if (_dominantPercent != value)
			{
				_dominantPercent = Math.Max(0, Math.Min(100, value));
				OnPropertyChanged(nameof(DominantPercent));
				OnPropertyChanged(nameof(CarrePercent));
				OnPropertyChanged(nameof(RondPercent));
				OnPropertyChanged(nameof(SwitchPreview));
			}
		}
	}

	public int CarrePercent
	{
		get => 100 - DominantPercent;
		set
		{
			int clamped = Math.Max(0, Math.Min(100, value));
			if (CarrePercent != clamped)
			{
				DominantPercent = 100 - clamped;
				// OnPropertyChanged(nameof(CarrePercent)); // Déjà fait dans DominantPercent
			}
		}
	}

	public int RondPercent
	{
		get => DominantPercent;
		set
		{
			int clamped = Math.Max(0, Math.Min(100, value));
			if (RondPercent != clamped)
			{
				DominantPercent = clamped;
				// OnPropertyChanged(nameof(RondPercent)); // Déjà fait dans DominantPercent
			}
		}
	}

	public ObservableCollection<string> DominantForms { get; } = new() { "Carré", "Rond" };

	public string SwitchPreview => GeneratePreview();

	private string GeneratePreview()
	{
		// Génère une séquence qui respecte dominance et pourcentage de switch
		var rnd = new Random();
		int total = 20;
		int dominantTotal = (int)(total * DominantPercent / 100.0);
		int otherTotal = total - dominantTotal;
		string dominantSymbol = DominantForm == "Carré" ? "■" : "●";
		string otherSymbol = DominantForm == "Carré" ? "●" : "■";

		// Démarrage sur la forme dominante ou non (plus naturel)
		string lastForm = DominantForm;
		int currentDominant = 1;
		int currentOther = 0;
		var preview = new StringBuilder();
		preview.Append(lastForm == "Carré" ? "■" : "●").Append(" ");

		for (int i = 1 ; i < total ; i++)
		{
			bool doitSwitch = rnd.Next(100) < SwitchPercent;

			string nextForm;
			if (doitSwitch)
			{
				// Switch à l’autre forme, seulement si quota pas dépassé
				if (lastForm == DominantForm && currentOther < otherTotal)
				{
					nextForm = otherSymbol;
					currentOther++;
					lastForm = otherSymbol == "■" ? "Carré" : "Rond";
				}
				else if (lastForm != DominantForm && currentDominant < dominantTotal)
				{
					nextForm = dominantSymbol;
					currentDominant++;
					lastForm = dominantSymbol == "■" ? "Carré" : "Rond";
				}
				else
				{
					// Pas de switch possible, on reste
					nextForm = lastForm == "Carré" ? "■" : "●";
					if (lastForm == DominantForm)
						currentDominant++;
					else
						currentOther++;
				}
			}
			else
			{
				// Reste sur la même forme (si quota pas dépassé)
				if (lastForm == DominantForm && currentDominant < dominantTotal)
				{
					nextForm = dominantSymbol;
					currentDominant++;
				}
				else if (lastForm != DominantForm && currentOther < otherTotal)
				{
					nextForm = otherSymbol;
					currentOther++;
				}
				else
				{
					// Force switch si quota de la forme actuelle dépassé
					nextForm = lastForm == DominantForm ? otherSymbol : dominantSymbol;
					if (nextForm == dominantSymbol)
						currentDominant++;
					else
						currentOther++;
					lastForm = nextForm == "■" ? "Carré" : "Rond";
				}
			}
			preview.Append(nextForm).Append(" ");
		}
		return preview.ToString().Trim();
	}
}

using CommunityToolkit.Mvvm.ComponentModel;
using StroopApp.Core;
using System.Collections.ObjectModel;
using System.Text;

namespace StroopApp.ViewModels.Configuration.Profile
{
    /// <summary>
    /// ViewModel for configuring visual cue switching behavior and dominant form distribution.
    /// Provides a live preview of the generated sequence with symbols (● for circle, ■ for square).
    /// </summary>
    public partial class SwitchSettingsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int? _switchPercent;

        [ObservableProperty]
        private string _dominantForm = "Circle";

        partial void OnSwitchPercentChanged(int? value)
        {
            OnPropertyChanged(nameof(SwitchPreview));
        }

        partial void OnDominantFormChanged(string value)
        {
            OnPropertyChanged(nameof(SwitchPreview));
        }

        private int _dominantPercent = 50; // 0 = 100% circle, 100 = 100% square
        public int DominantPercent
        {
            get => _dominantPercent;
            set
            {
                if (_dominantPercent != value)
                {
                    _dominantPercent = Math.Max(0, Math.Min(100, value));
                    OnPropertyChanged(nameof(DominantPercent));
                    OnPropertyChanged(nameof(SquarePercent));
                    OnPropertyChanged(nameof(CirclePercent));
                    OnPropertyChanged(nameof(SwitchPreview));
                }
            }
        }

        public int SquarePercent
        {
            get => 100 - DominantPercent;
            set
            {
                int clamped = Math.Max(0, Math.Min(100, value));
                if (SquarePercent != clamped)
                {
                    DominantPercent = 100 - clamped;
                }
            }
        }

        public int CirclePercent
        {
            get => DominantPercent;
            set
            {
                int clamped = Math.Max(0, Math.Min(100, value));
                if (CirclePercent != clamped)
                {
                    DominantPercent = clamped;
                }
            }
        }

        public ObservableCollection<string> DominantForms { get; } = new() { "Square", "Circle" };

        public string SwitchPreview => GeneratePreview();

        private string GeneratePreview()
        {

            if (DominantPercent == 100)
            {
                return string.Join(" ", Enumerable.Repeat("●", 20));
            }
            if (DominantPercent == 0)
            {
                return string.Join(" ", Enumerable.Repeat("■", 20));
            }

            var rnd = new Random();
            int total = 20;
            int dominantTotal = (int)(total * DominantPercent / 100.0);
            int otherTotal = total - dominantTotal;
            string dominantSymbol = DominantForm == "Square" ? "■" : "●";
            string otherSymbol = DominantForm == "Square" ? "●" : "■";

            string lastForm = DominantForm;
            int currentDominant = 1;
            int currentOther = 0;
            var preview = new StringBuilder();
            preview.Append(lastForm == "Square" ? "■" : "●").Append(" ");

            for (int i = 1; i < total; i++)
            {
                bool doitSwitch = SwitchPercent.HasValue && SwitchPercent.Value > 0 && rnd.Next(100) < SwitchPercent.Value;

                string nextForm;
                if (doitSwitch)
                {
                    if (lastForm == DominantForm && currentOther < otherTotal)
                    {
                        nextForm = otherSymbol;
                        currentOther++;
                        lastForm = otherSymbol == "■" ? "Square" : "Circle";
                    }
                    else if (lastForm != DominantForm && currentDominant < dominantTotal)
                    {
                        nextForm = dominantSymbol;
                        currentDominant++;
                        lastForm = dominantSymbol == "■" ? "Square" : "Circle";
                    }
                    else
                    {
                        nextForm = lastForm == "Square" ? "■" : "●";
                        if (lastForm == DominantForm)
                            currentDominant++;
                        else
                            currentOther++;
                    }
                }
                else
                {
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
                        nextForm = lastForm == DominantForm ? otherSymbol : dominantSymbol;
                        if (nextForm == dominantSymbol)
                            currentDominant++;
                        else
                            currentOther++;
                        lastForm = nextForm == "■" ? "Square" : "Circle";
                    }
                }
                preview.Append(nextForm).Append(" ");
            }
            return preview.ToString().Trim();
        }
    }
}
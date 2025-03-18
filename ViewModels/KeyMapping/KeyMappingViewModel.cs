using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.KeyMapping;
using ModernWpf.Controls;

using KeyMappingModel = StroopApp.Models.KeyMapping;

namespace StroopApp.ViewModels
{
    public class KeyMappingViewModel : INotifyPropertyChanged
    {
        private readonly IKeyMappingService _keyMappingService;
        public KeyMappings Mappings { get; set; }

        private KeyMappingModel _editingMapping;
        public KeyMappingModel EditingMapping
        {
            get => _editingMapping;
            set { _editingMapping = value; OnPropertyChanged(); }
        }

        public ICommand EditMappingCommand { get; }
        public ICommand KeyPressedCommand { get; }
        public ICommand OpenKeyMappingEditorCommand { get; }

        public KeyMappingViewModel(IKeyMappingService keyMappingService)
        {
            _keyMappingService = keyMappingService;
            LoadMappings();

            EditMappingCommand = new RelayCommand<string>(StartEditing);
            KeyPressedCommand = new RelayCommand<Key>(OnKeyPressed);
            OpenKeyMappingEditorCommand = new RelayCommand<string>(OpenKeyMappingEditor);
        }

        private async void LoadMappings()
        {
            Mappings = await _keyMappingService.LoadKeyMappingsAsync();
            OnPropertyChanged(nameof(Mappings));
        }

        private void StartEditing(string color)
        {
            switch (color)
            {
                case "Rouge":
                    EditingMapping = Mappings.Red;
                    break;
                case "Bleu":
                    EditingMapping = Mappings.Blue;
                    break;
                case "Vert":
                    EditingMapping = Mappings.Green;
                    break;
                case "Jaune":
                    EditingMapping = Mappings.Yellow;
                    break;
                default:
                    EditingMapping = null;
                    break;
            }
        }

        private async void OpenKeyMappingEditor(string color)
        {
            StartEditing(color);

            // Création d'un ContentDialog ressemblant à un ContextDialog
            var dialog = new ContentDialog
            {
                Title = $"Modification du mapping pour {color}",
                Content = "Appuyez sur une touche pour définir le mapping. Appuyez sur Échap pour annuler.",
                PrimaryButtonText = string.Empty,
                SecondaryButtonText = string.Empty
            };

            dialog.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    // Annulation
                    EditingMapping = null;
                    dialog.Hide();
                    e.Handled = true;
                }
                else
                {
                    // Affectation de la touche et fermeture du dialog
                    EditingMapping.Key = e.Key;
                    dialog.Hide();
                    e.Handled = true;
                }
            };

            dialog.Loaded += (s, e) => dialog.Focus();

            await dialog.ShowAsync();
        }

        private void OnKeyPressed(Key key)
        {
            if (EditingMapping != null)
            {
                if (key == Key.Escape)
                {
                    EditingMapping = null;
                    return;
                }

                EditingMapping.Key = key;
                EditingMapping = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}

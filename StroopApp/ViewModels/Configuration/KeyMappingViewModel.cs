using ModernWpf.Controls;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services.KeyMapping;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace StroopApp.ViewModels.Configuration
{
    public class KeyMappingViewModel : INotifyPropertyChanged
    {
        private readonly IKeyMappingService _keyMappingService;
        public KeyMappings Mappings
        {
            get; set;
        }

        private KeyMapping _editingMapping;
        public KeyMapping EditingMapping
        {
            get => _editingMapping;
            set
            {
                _editingMapping = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditMappingCommand
        {
            get;
        }
        public ICommand KeyPressedCommand
        {
            get;
        }
        public ICommand OpenKeyMappingEditorCommand
        {
            get;
        }

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
            Mappings = await _keyMappingService.LoadKeyMappings();
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

            string originalMessage = "Appuyez sur une touche pour définir le mapping. Appuyez sur Échap pour annuler.";
            var originalText = new TextBlock
            {
                Text = originalMessage,
                TextWrapping = TextWrapping.Wrap
            };

            var errorPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Opacity = 0,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var errorIcon = new FontIcon
            {
                Glyph = "\uE7BA", // Code unicode pour une icône d'avertissement
                Foreground = new SolidColorBrush(Colors.Red),
                FontSize = 24,
                VerticalAlignment = VerticalAlignment.Center
            };
            var errorText = new TextBlock
            {
                Text = "Cette touche est déjà utilisée. Veuillez choisir une autre touche.",
                Foreground = new SolidColorBrush(Colors.Red),
                Margin = new Thickness(8, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            errorPanel.Children.Add(errorIcon);
            errorPanel.Children.Add(errorText);

            var grid = new Grid();
            grid.Children.Add(originalText);
            grid.Children.Add(errorPanel);

            var dialog = new ContentDialog
            {
                Title = $"Modification du mapping pour {color}",
                Content = grid,
                PrimaryButtonText = string.Empty,
                SecondaryButtonText = string.Empty
            };

            dialog.KeyDown += async (s, e) =>
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
                    bool keyAlreadyUsed =
                        Mappings.Red.Key == e.Key && EditingMapping != Mappings.Red ||
                        Mappings.Blue.Key == e.Key && EditingMapping != Mappings.Blue ||
                        Mappings.Green.Key == e.Key && EditingMapping != Mappings.Green ||
                        Mappings.Yellow.Key == e.Key && EditingMapping != Mappings.Yellow;

                    if (keyAlreadyUsed)
                    {
                        // Animation d'erreur : faire apparaître puis disparaître errorPanel
                        var fadeIn = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = TimeSpan.FromMilliseconds(300)
                        };
                        var fadeOut = new DoubleAnimation
                        {
                            From = 1,
                            To = 0,
                            BeginTime = TimeSpan.FromMilliseconds(1500),
                            Duration = TimeSpan.FromMilliseconds(300)
                        };

                        var sb = new Storyboard();
                        sb.Children.Add(fadeIn);
                        sb.Children.Add(fadeOut);
                        Storyboard.SetTarget(fadeIn, errorPanel);
                        Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
                        Storyboard.SetTarget(fadeOut, errorPanel);
                        Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));
                        sb.Begin();

                        // Laisser le dialog ouvert pour une nouvelle saisie
                        dialog.Focus();
                        e.Handled = true;
                    }
                    else
                    {
                        EditingMapping.Key = e.Key;
                        await _keyMappingService.SaveKeyMappings(Mappings);
                        dialog.Hide();
                        e.Handled = true;
                    }
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

using StroopApp.Commands;
using StroopApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StroopApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using ModernWpf.Controls;

namespace StroopApp.ViewModels
{
    public class ProfileEditorViewModel : INotifyPropertyChanged
    {
        public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            Profile = profile;
            _IprofileService = profileService;
            Profiles = profiles;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            Profile.PropertyChanged += Profile_PropertyChanged;
        }

        private void Profile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Profile.WordCount) || e.PropertyName == nameof(Profile.WordDuration) || e.PropertyName == nameof(Profile.Hours) || e.PropertyName == nameof(Profile.Minutes) || e.PropertyName == nameof(Profile.Seconds))
            {
                Profile.UpdateDerivedValues();
            }
            else if (e.PropertyName == nameof(Profile.StroopType))
            {
                if (Profile.StroopType != "Amorce")
                {
                    Profile.AmorceDuration = 0;
                }
            }
        }

        private ExperimentProfile _profile;

        public ExperimentProfile Profile
        {
            get => _profile;
            set
            {
                if (_profile != value)
                {
                    _profile = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly IProfileService _IprofileService;
        public ObservableCollection<ExperimentProfile> Profiles { get; set; }
        public bool? DialogResult { get; private set; }
        public Action? CloseAction { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public void Save()
        {
            // Vérification du nom de profil
            if (string.IsNullOrWhiteSpace(Profile.ProfileName))
            {
                ShowErrorDialog("Le nom du profil ne peut pas être vide ou contenir uniquement des espaces.");
                return;
            }
            if (Profiles.Any(p => p != Profile && p.ProfileName.Equals(Profile.ProfileName, StringComparison.OrdinalIgnoreCase)))
            {
                ShowErrorDialog("Un profil avec ce nom existe déjà. Veuillez choisir un autre nom.");
                return;
            }

            // Vérification de TaskDuration/WordDuration
            if (Profile.WordDuration <= 0 || Profile.TaskDuration % Profile.WordDuration != 0)
            {
                ShowErrorDialog("La durée du mot doit être positive et TaskDuration doit être divisible par WordDuration.");
                return;
            }
            int wordNumber = Profile.TaskDuration / Profile.WordDuration;

            // Vérification de GroupSize
            if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
            {
                ShowErrorDialog("La taille du groupe doit être positive et diviser WordNumber.");
                return;
            }

            // Vérification du type de Stroop
            if (string.IsNullOrEmpty(Profile.StroopType))
            {
                ShowErrorDialog("Le type de Stroop ne peut pas être nul.");
                return;
            }

            // Vérification du temps de réaction maximum
            if (Profile.MaxReactionTime <= 0)
            {
                ShowErrorDialog("Le temps de réaction maximum doit être positif.");
                return;
            }

            DialogResult = true;
            CloseAction?.Invoke();
        }
        public void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }
        private async void ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Erreur",
                Content = message,
                CloseButtonText = "OK"
            };

            await dialog.ShowAsync();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

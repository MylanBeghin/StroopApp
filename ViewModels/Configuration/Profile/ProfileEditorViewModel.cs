using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ModernWpf.Controls;
using StroopApp.Core;
using StroopApp.Models;
using StroopApp.Services;
using StroopApp.Services.Profile;

namespace StroopApp.ViewModels.Configuration.Profile
{
    public class ProfileEditorViewModel : INotifyPropertyChanged
    {
        private ExperimentProfile _profile;
        public ExperimentProfile Profile
        {
            get => _profile;
            set { _profile = value; OnPropertyChanged(); }
        }
        private ExperimentProfile _originalProfile;

        public ObservableCollection<ExperimentProfile> Profiles { get; }
        public bool? DialogResult { get; private set; }
        public Action? CloseAction { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IProfileService _IprofileService;

        public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            _IprofileService = profileService;
            Profiles = profiles;

            if (Profiles.Contains(profile))
            {
                _originalProfile = profile;
                Profile = CloneProfile(profile);
            }
            else
            {
                Profile = profile;
            }
            Profile.UpdateDerivedValues();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            Profile.PropertyChanged += Profile_PropertyChanged;
        }

        private void Profile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Profile.WordCount) ||
                e.PropertyName == nameof(Profile.WordDuration) ||
                e.PropertyName == nameof(Profile.Hours) ||
                e.PropertyName == nameof(Profile.Minutes) ||
                e.PropertyName == nameof(Profile.Seconds) ||
                e.PropertyName == nameof(Profile.CalculationMode))
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

        private ExperimentProfile CloneProfile(ExperimentProfile profile)
        {
            return new ExperimentProfile
            {
                ProfileName = profile.ProfileName,
                CalculationMode = profile.CalculationMode,
                Hours = profile.Hours,
                Minutes = profile.Minutes,
                Seconds = profile.Seconds,
                TaskDuration = profile.TaskDuration,
                WordDuration = profile.WordDuration,
                MaxReactionTime = profile.MaxReactionTime,
                GroupSize = profile.GroupSize,
                StroopType = profile.StroopType,
                AmorceDuration = profile.AmorceDuration,
                FixationDuration = profile.FixationDuration,
                WordCount = profile.WordCount,
                StroopTypes = profile.StroopTypes
            };
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Profile.ProfileName))
            {
                ShowErrorDialog("Le nom du profil ne peut pas être vide ou contenir uniquement des espaces.");
                return;
            }
            if (Profiles.Any(p => p != Profile && p != _originalProfile && p.ProfileName==Profile.ProfileName ))
            {
                ShowErrorDialog("Un profil avec ce nom existe déjà. Veuillez choisir un autre nom.");
                return;
            }
            if (Profile.WordDuration <= 0 || Profile.TaskDuration % Profile.WordDuration != 0)
            {
                ShowErrorDialog("La durée du mot doit être positive et TaskDuration doit être divisible par WordDuration.");
                return;
            }
            int wordNumber = Profile.TaskDuration / Profile.WordDuration;
            if (Profile.GroupSize <= 0 || wordNumber % Profile.GroupSize != 0)
            {
                ShowErrorDialog("La taille du groupe doit être positive et diviser le nombre de mots.");
                return;
            }
            if (string.IsNullOrEmpty(Profile.StroopType))
            {
                ShowErrorDialog("Le type de Stroop ne peut pas être nul.");
                return;
            }
            if (Profile.AmorceDuration == 0 && Profile.StroopType == "Amorce")
            {
                ShowErrorDialog("Le temps d'amorce est doit être supérieur à 0 !");
                return;
            }
            if (Profile.MaxReactionTime <= 0)
            {
                ShowErrorDialog("Le temps de réaction maximum doit être positif.");
                return;
            }
            if (_originalProfile != null)
            {
                _originalProfile.ProfileName = Profile.ProfileName;
                _originalProfile.CalculationMode = Profile.CalculationMode;
                _originalProfile.Hours = Profile.Hours;
                _originalProfile.Minutes = Profile.Minutes;
                _originalProfile.Seconds = Profile.Seconds;
                _originalProfile.TaskDuration = Profile.TaskDuration;
                _originalProfile.WordDuration = Profile.WordDuration;
                _originalProfile.MaxReactionTime = Profile.MaxReactionTime;
                _originalProfile.GroupSize = Profile.GroupSize;
                _originalProfile.StroopType = Profile.StroopType;
                _originalProfile.AmorceDuration = Profile.AmorceDuration;
                _originalProfile.FixationDuration = Profile.FixationDuration;
                _originalProfile.WordCount = Profile.WordCount;
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

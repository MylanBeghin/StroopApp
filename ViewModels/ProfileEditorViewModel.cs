using StroopApp.Commands;
using StroopApp.Models;
using StroopApp.Services.Profile;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using StroopApp.Services.Profile;
using StroopApp.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace StroopApp.ViewModels
{
    public class ProfileEditorViewModel
    {
        public ProfileEditorViewModel(ExperimentProfile profile, ObservableCollection<ExperimentProfile> profiles, IProfileService profileService)
        {
            Profile = profile;
            _IprofileService = profileService;
            Profiles = profiles;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
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
            _IprofileService.AddProfile(Profile, Profiles);
            DialogResult = true;
            CloseAction?.Invoke();
        }
        public void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}




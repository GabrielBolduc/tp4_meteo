using System;
using System.Windows.Input;
using tp4_meteo.Services;
using tp4_meteo.ViewModels.Commands;

namespace tp4_meteo.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigService _configService;
        public event EventHandler RequestClose;

        public string ApiKey
        {
            get => _configService.ApiKey;
            set { _configService.ApiKey = value; OnPropertyChanged(); }
        }
        public string Language
        {
            get => _configService.Language;
            set { _configService.Language = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        public SettingsViewModel(IConfigService configService)
        {
            _configService = configService;
            SaveCommand = new RelayCommand(_ => {
                _configService.Save();
                RequestClose?.Invoke(this, EventArgs.Empty);
            });
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke(this, EventArgs.Empty));
        }
    }
}
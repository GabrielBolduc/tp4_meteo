using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using tp4_meteo.Services;
using tp4_meteo.Properties;
using tp4_meteo.ViewModels.Commands;

namespace tp4_meteo.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IConfigService _configService;
        private readonly string _initialLanguage;

        public event EventHandler RequestClose;

        private string _apiKey;
        public string ApiKey
        {
            get => _apiKey;
            set => Set(ref _apiKey, value);
        }

        private string _language;
        public string Language
        {
            get => _language;
            set => Set(ref _language, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        public SettingsViewModel(IConfigService configService)
        {
            _configService = configService;

            ApiKey = _configService.ApiKey;
            Language = _configService.Language;
            _initialLanguage = _configService.Language;

            SaveCommand = new RelayCommand(_ =>
            {
                _configService.ApiKey = ApiKey;
                _configService.Language = Language;
                _configService.Save();

                if (_initialLanguage != Language)
                {
                    var result = MessageBox.Show(Resources.MsgRestart, Resources.AppTitle,
                                                 MessageBoxButton.OKCancel, MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
                    {
                        Process.Start(Environment.ProcessPath);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        RequestClose?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    RequestClose?.Invoke(this, EventArgs.Empty);
                }
            });

            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke(this, EventArgs.Empty));
        }
    }
}
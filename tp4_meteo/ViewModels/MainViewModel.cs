using Autofac;
using System.Windows.Input;
using tp4_meteo.Services;
using tp4_meteo.ViewModels.Commands;
namespace tp4_meteo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IWindowService _windowService;
        public WeatherViewModel WeatherVM { get; }
        public SettingsViewModel SettingsVM { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainViewModel() : this(
            Bootstrapper.Container.Resolve<IWindowService>(),
            Bootstrapper.Container.Resolve<WeatherViewModel>(),
            Bootstrapper.Container.Resolve<SettingsViewModel>())
        { }

        public MainViewModel(IWindowService windowService, WeatherViewModel weatherVM, SettingsViewModel settingsVM)
        {
            _windowService = windowService;
            WeatherVM = weatherVM;
            SettingsVM = settingsVM;
            OpenSettingsCommand = new RelayCommand(_ => _windowService.ShowSettingsWindow(SettingsVM));
        }
    }
}
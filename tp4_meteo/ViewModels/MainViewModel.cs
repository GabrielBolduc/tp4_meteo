using Autofac;
using System.Windows.Input;
using tp4_meteo.ViewModels.Commands;
using tp4_meteo.Views;

namespace tp4_meteo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public WeatherViewModel WeatherVM { get; }

        public ICommand OpenSettingsCommand { get; }

        public MainViewModel(WeatherViewModel weatherVM)
        {
            WeatherVM = weatherVM;

            OpenSettingsCommand = new RelayCommand(_ =>
            {
                var settingsVM = Bootstrapper.Container.Resolve<SettingsViewModel>();

                var settingsWindow = new SettingsView();
                settingsWindow.DataContext = settingsVM;

                settingsVM.RequestClose += (s, e) => settingsWindow.Close();

                settingsWindow.Owner = System.Windows.Application.Current.MainWindow;
                settingsWindow.ShowDialog();
            });
        }
    }
}
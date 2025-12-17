using System.Windows;
using tp4_meteo.Views;

namespace tp4_meteo.Services
{
    public class WindowService : IWindowService
    {
        public void ShowSettingsWindow(object viewModel)
        {
            var win = new SettingsView
            {
                Owner = Application.Current.MainWindow,
                DataContext = viewModel
            };
            win.ShowDialog();
        }
    }
}
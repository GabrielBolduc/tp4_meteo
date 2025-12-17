using System.Windows;
using tp4_meteo.ViewModels;

namespace tp4_meteo.Views
{
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => {
                if (e.NewValue is SettingsViewModel vm) vm.RequestClose += (sender, args) => Close();
            };
        }
    }
}
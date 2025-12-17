using System.Windows;

namespace tp4_meteo
{
    public partial class App : Application
    {
        public App()
        {
            // On applique la langue AVANT que le XAML ne charge
            Bootstrapper.ApplyCulture();
        }
    }
}
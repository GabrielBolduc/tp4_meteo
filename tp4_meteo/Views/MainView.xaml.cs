using Autofac;
using System.Windows;
using tp4_meteo.ViewModels;
namespace tp4_meteo.Views 
{ 
    public partial class MainView : Window 
    { 
        public MainView() 
        { 
            InitializeComponent();

            this.DataContext = Bootstrapper.Container.Resolve<MainViewModel>();
        } 
    } 
}
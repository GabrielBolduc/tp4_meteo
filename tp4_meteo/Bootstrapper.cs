using Autofac;
using System.Globalization;
using System.Reflection;
using System.Threading; // <--- Ajout
using tp4_meteo.Data;
using tp4_meteo.Services;
using System.Linq;

namespace tp4_meteo
{
    public static class Bootstrapper
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get
            {
                if (_container == null) BuildContainer();
                return _container;
            }
        }

        private static void BuildContainer()
        {
            // (Ton code existant pour la BD et le Builder ne change pas)
            using (var context = new MeteoDbContext()) { context.Database.EnsureCreated(); }
            var builder = new ContainerBuilder();
            builder.RegisterType<ConfigService>().As<IConfigService>().SingleInstance();
            builder.RegisterType<WindowService>().As<IWindowService>();
            builder.RegisterType<MeteoDbContext>().AsSelf();
            builder.RegisterType<MeteoRepository>().As<IMeteoRepository>();
            builder.RegisterType<WeatherBitService>().As<IMeteoService>();
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("ViewModel")).AsSelf();
            _container = builder.Build();
        }

        public static void ApplyCulture()
        {
            var config = Container.Resolve<IConfigService>();
            var lang = config.Language;

            if (!string.IsNullOrEmpty(lang))
            {
                try
                {
                    var culture = new CultureInfo(lang);
                    // Applique la culture aux Threads
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;

                    // AJOUT IMPORTANT POUR LE FICHIER PROPERTIES STANDARD
                    // Il faut dire explicitement au gestionnaire de ressources de changer sa culture
                    tp4_meteo.Properties.Resources.Culture = culture;
                }
                catch { }
            }
        }
    }
}
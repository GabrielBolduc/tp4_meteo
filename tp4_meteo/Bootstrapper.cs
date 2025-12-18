using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Threading;
using tp4_meteo.Data;
using tp4_meteo.Services;

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
            // initalisation bd
            using (var context = new MeteoDbContext())
            {
                context.Database.EnsureCreated();
            }

            var builder = new ContainerBuilder();

            // config pour lire le fichier JSON autofac
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("autofac.json", optional: false, reloadOnChange: true);

            var configuration = configBuilder.Build();
            var configModule = new ConfigurationModule(configuration);

            builder.RegisterModule(configModule);

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

                    // appliquer culture 
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;

                    tp4_meteo.Properties.Resources.Culture = culture;
                }
                catch { }
            }
        }
    }
}
using Autofac;
using System.Reflection;
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
            using (var context = new MeteoDbContext()) context.Database.EnsureCreated();
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
    }
}
using _2.Application.Extensions.WatchDog;
using _2.Application.Interfaces;
using _2.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace _2.Application.Extension
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            // Automapper de manera global en la capa
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Inyección de servicios de la capa aplicación
            services.AddScoped<IMantenimientoApplication, MantenimientoApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();

            services.AddWatchDog(configuration);

            return services;
        }
    }
}

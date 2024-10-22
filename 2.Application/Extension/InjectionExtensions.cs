using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace _2.Application.Extension
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //    services.AddSingleton(configuration);

            //    // Automapper de manera global en la capa
            //    //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

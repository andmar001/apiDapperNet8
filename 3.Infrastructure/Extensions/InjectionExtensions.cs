using _3.Infrastructure.Persistence.Interfaces.Mantenimiento;
using _3.Infrastructure.Persistence.Repositories.Mantenimiento;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMantenimientoRepository, MantenimientoRepository>();

            return services;
        }
    }
}

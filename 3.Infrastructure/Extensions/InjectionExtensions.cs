using _3.Infrastructure.Persistence.Interfaces.Auth;
using _3.Infrastructure.Persistence.Interfaces.JWT;
using _3.Infrastructure.Persistence.Interfaces.Mantenimiento;
using _3.Infrastructure.Persistence.Interfaces.Token;
using _3.Infrastructure.Persistence.Repositories.Auth;
using _3.Infrastructure.Persistence.Repositories.JWT;
using _3.Infrastructure.Persistence.Repositories.Mantenimiento;
using _3.Infrastructure.Persistence.Repositories.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _3.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IDatosJWTRepository, DatosJWTRepository>();

            return services;
        }
    }
}

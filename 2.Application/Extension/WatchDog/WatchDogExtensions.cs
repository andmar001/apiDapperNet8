using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _2.Application.Extension.WatchDog
{
    public static class WatchDogExtensions
    {
        public static IServiceCollection AddWatchDog(this IServiceCollection services, IConfiguration configuration)
        {
            string conexion = ConnectionStringCustom.GetConnectionString(configuration);


        }
    }
}

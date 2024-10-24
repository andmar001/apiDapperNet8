using _3.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatchDog;
using WatchDog.src.Enums;

namespace _2.Application.Extensions.WatchDog
{
    public static class WatchDogExtensions
    {
        public static IServiceCollection AddWatchDog(this IServiceCollection services, IConfiguration configuration)
        {
            string conexion = ConnectionStringCustom.GetConnectionString(configuration);

            services.AddWatchDogServices(options =>
            {
                options.SetExternalDbConnString = conexion;                             //==> Set the connection string to the database
                options.DbDriverOption = WatchDogDbDriverEnum.MSSQL;                    //==> Set the database provider
                options.IsAutoClear = true;
                options.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;      //==> Clear every month
            });

            return services;
        }
    }
}

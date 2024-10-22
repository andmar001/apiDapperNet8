namespace _1.API.Extensions
{
    public static class CorsExtensionsCustom
    {
        public static IServiceCollection AddInjectionCorsCustom(this IServiceCollection services,IConfiguration configuration, string cors)
        {
            services.AddSingleton(configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(name: cors,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "https://sfia-didsqa.guanajuato.gob.mx")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            return services;
        }
    }
}

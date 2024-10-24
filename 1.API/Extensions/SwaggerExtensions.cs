using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace _1.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var openApi = new OpenApiInfo
            {
                Title = "Dapper Micro ORM API - Clean Arquitecture",
                Version = "v1",
                Description = "API Desarrollada con Net Core 8 - 2024",
                TermsOfService = new Uri("https://opensource.org/licenses/NIT"),
                Contact = new OpenApiContact
                {
                    Name = "TEST DGTIT S.A.C",
                    Email = "test@gmail.com",
                    Url = new Uri("https://es.lipsum.com/")
                },
                License = new OpenApiLicense
                {
                    Name = "User under LICX",
                    Url = new Uri("https://opensource.org/licenses")
                }
            };

            services.AddSwaggerGen(x =>
            {
                openApi.Version = "v1";
                x.SwaggerDoc("v1", openApi);

                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "JWT Bearer Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                x.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new string[] { } }
                });
            });

            return services;
        }
    }
}

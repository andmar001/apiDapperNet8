using _1.API.Extensions;
using _2.Application.Extension;
using _3.Infrastructure.Extensions;
using _3.Infrastructure.Security;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

var Cors = "Cors";

// Add services to the container.
builder.Services.AddInjectionCorsCustom(Configuration, Cors);   // Cors
builder.Services.AddInjectionInfrastructure(Configuration);     // Infrastructure
builder.Services.AddInjectionApplication(Configuration);        // Application
builder.Services.AddAuthentication(Configuration);              // Authentication

// Si es valor NULL no enviarlo
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();                     // API Explorer
builder.Services.AddSwagger();                                  // Swagger

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())                            // WatchDog 
{
    app.UseWatchDog(configuration =>
    {
        configuration.WatchPageUsername = FuncionesEncriptacionLogic.DecryptValue(Configuration.GetSection("WatchDog:Username").Value!);
        configuration.WatchPagePassword = FuncionesEncriptacionLogic.DecryptValue(Configuration.GetSection("WatchDog:Password").Value!);
    });
}

app.Run();
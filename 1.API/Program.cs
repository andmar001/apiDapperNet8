using _1.API.Extensions;
using _2.Application.Extension;
using _3.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

var Cors = "Cors";

builder.Services.AddInjectionCorsCustom(Configuration, Cors);

// Add services to the container.
builder.Services.AddInjectionInfrastructure(Configuration); // Infrastructure
builder.Services.AddInjectionApplication(Configuration);    // Application

// Si es valor NULL no enviarlo
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Agregar autenticaci�n JWT Bearer
builder.Services.AddAuthentication(Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();